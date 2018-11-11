using FileImportApp.API.DAO;
using FileImportApp.API.Logger;
using FileImportApp.API.Models.Client;
using FileImportApp.API.Models.Conf;
using FileImportApp.API.Models.DB;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileImportApp.API.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IOptions<Settings> _settings;
        private readonly ILoggerManager _logger;

        public FileService(IFileRepository fileRepository, IOptions<Settings> settings, ILoggerManager logger)
        {
            _fileRepository = fileRepository;
            _settings = settings;
            _logger = logger;
        }

        /* Returns data from DB */
        public async Task<DataResponse> GetData(string session, int pageSize, int pageNum)
        {
            List<StoreItem> data = await _fileRepository.GetData(session, pageSize, pageNum);
            List<StoreItemDto> respData = new List<StoreItemDto>();

            foreach (StoreItem item in data)
            {
                respData.Add(new StoreItemDto(item));
            }

            FileInformation fileInfo = await GetFileInfo(session);

            DataResponse response = new DataResponse();
            response.data = respData;
            response.totalCount = fileInfo.RecordCount;

            return response;
        }

        /* Saves data into DB and json file */
        public async Task SaveData(string session, List<StoreItemDto> data, string filename)
        {
            // file name that will be created as a json file
            string newfilename = filename.Substring(0, filename.Length - 4) + "_" + session + ".json";

            await SaveDataToDB(session, data, filename, newfilename);
            await SaveDataToFile(session, data, newfilename);
        }

        /* Saves data into DB */
        private async Task SaveDataToDB(string session, List<StoreItemDto> data, string filename, string newfilename)
        {
            StoreItem itemDB;
            int index = 0;

            foreach (StoreItemDto item in data) 
            {
                itemDB = new StoreItem(item, filename, newfilename);
                itemDB.Session = session;
                await _fileRepository.InsertToDB(itemDB);
                ImportStateService.SetProgress(session, ImportState.DBImporting, CalculatePercentage(++index, data.Count));
            }
        }

        /* Saves data into json file */
        private async Task SaveDataToFile(string session, List<StoreItemDto> data, string newfilename)
        {
            ImportStateService.SetProgress(session, ImportState.DBImporting, 0);

            if (!Directory.Exists(_settings.Value.SavingDirectory))
            {
                Directory.CreateDirectory(_settings.Value.SavingDirectory);
            }

            string path = _settings.Value.SavingDirectory + newfilename;
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(data));

            ImportStateService.SetProgress(session, ImportState.DBImporting, 100);
            _logger.LogInfo("Data is saved as a csv file : " + path);
        }

        /* Calculates percentage for the given total and current index */
        private int CalculatePercentage(int current, int total)
        {
            return Convert.ToInt32(Math.Ceiling((decimal)(((decimal)current / (decimal)total) * 100)));
        }

        /* Saves general file info in FileInfo collection */
        public async Task SaveFileInfo(string session, int count, string filename, ImportState state, string description)
        {
            FileInformation fileInfo = new FileInformation
            {
                Session = session,
                UploadedFileName = filename,
                FinalState = state,
                Description = description,
                RecordCount = count
            };

            await _fileRepository.InsertFileInfoDetails(fileInfo);
        }

        /* Returns general file information */
        public async Task<FileInformation> GetFileInfo(string session)
        {
            return await _fileRepository.GetFileInformation(session);
        }

        /* Returns directory for file saving */
        public string GetSavingDirectory() {
            return _settings.Value.SavingDirectory;
        }
    }
}
