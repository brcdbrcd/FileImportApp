using FileImportApp.API.Logger;
using FileImportApp.API.Models.Client;
using FileImportApp.API.Models.DB;
using FileImportApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileImportApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _service;
        private readonly ILoggerManager _logger;

        public FileController(IFileService service, ILoggerManager logger)
        {
            _service = service;
            _logger = logger;
        }

        /* Returns file import process status */
        // GET api/file/import/status?session=20181111142940
        [HttpGet("import/status")]
        public async Task<IActionResult> GetImportStatus(string session)
        {
            ImportStateResponse resp = ImportStateService.Get(session); // cache
            if (resp == null)
            {
                FileInformation fileInfo = await _service.GetFileInfo(session); // DB
                if (fileInfo == null)
                {
                    return NotFound();
                }
                return Ok(new ImportStateResponse(fileInfo.FinalState));
                
            }
            return Ok(resp);
        }

        /* Returns paginated data related to the session */
        // GET api/file/data?session=20181111142940&pageSize=10&pageNum=2
        [HttpGet("data")]
        public async Task<IActionResult> GetData(string session, int pageSize, int pageNum)
        {
            DataResponse resp = await _service.GetData(session, pageSize, pageNum);
            return Ok(resp);
        }

        /* Returns immediately and triggers async processing of the file */
        // POST api/file/import
        [HttpPost("import"), RequestSizeLimit(100_000_000)]
        public IActionResult ImportFile(IFormFile file)
        {
            _logger.LogInfo("New file import request accepted");
            if (file == null || file.Length == 0)
            {
                _logger.LogInfo("Invalid file (null or empty)");
                return BadRequest(new ImportResponse(null,"File is invalid."));
            }

            string session = DateTime.Now.ToString("yyyyMMddHHmmss");
            ImportStateService.SetInitialized(session);
            ImportResponse resp = new ImportResponse(session,"OK");

            // 
            string dir = _service.GetSavingDirectory() + session + "/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string path = dir + file.FileName;
            using (var fileStream = new FileStream(path, FileMode.Create)) {
                file.CopyTo(fileStream);
            }
            //

            Task.Run(() => ImportAsync(session, dir, file.FileName));
            
            return Ok(resp);
        }
        
        /* Reads file from temp dir and imports to disk and DB  */
        private async Task ImportAsync(string session, string dir, string fileName)
        {
            List<StoreItemDto> fileContent = new List<StoreItemDto>();
            StoreItemDto item;
            string[] line;

            try
            {
                FileStream fileStream = new FileStream(dir + fileName, FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    ImportStateService.SetReading(session);
                    await reader.ReadLineAsync();
                    while (reader.Peek() >= 0)
                    {
                        line = (await reader.ReadLineAsync()).Split(',');

                        item = new StoreItemDto
                        {
                            Key = line[0],
                            ArtikelCode = line[1],
                            ColorCode = line[2],
                            Description = line[3],
                            Price = decimal.Parse(line[4]),
                            DiscountPrice = decimal.Parse(line[5]),
                            DeliveredIn = line[6],
                            Q1 = line[7],
                            Size = line[8],
                            Color = line[9]
                        };

                        fileContent.Add(item);
                    }
                }

                // Save data to DB and disk
                await _service.SaveData(session, fileContent, fileName);

                await _service.SaveFileInfo(session, fileContent.Count, fileName, ImportState.Completed, "save successed");
                
                // delete directory which csv file is in it
                DirectoryInfo di = new DirectoryInfo(dir);
                foreach (FileInfo file in di.EnumerateFiles()) 
                {
                    file.Delete();
                }
                Directory.Delete(dir);

                ImportStateService.SetProgress(session, ImportState.Completed, 100);
                _logger.LogInfo("File Imported Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR! : " + ex.StackTrace);
                await _service.SaveFileInfo(session, fileContent.Count, fileName, ImportState.Failed, ex.Message);
                ImportStateService.SetFailed(session, ex.Message);
            }
        }
    }
}
