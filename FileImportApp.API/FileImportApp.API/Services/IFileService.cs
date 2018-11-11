using FileImportApp.API.Models.Client;
using FileImportApp.API.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImportApp.API.Services
{
    public interface IFileService
    {
        Task<DataResponse> GetData(string session, int pageSize, int pageNum);

        Task SaveData(string session, List<StoreItemDto> data, string filename);

        Task SaveFileInfo(string session, int count, string filename, ImportState state, string description);

        Task<FileInformation> GetFileInfo(string session);

        string GetSavingDirectory();
    }
}
