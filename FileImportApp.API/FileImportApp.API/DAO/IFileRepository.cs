using FileImportApp.API.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImportApp.API.DAO
{
    public interface IFileRepository
    {
        Task<List<StoreItem>> GetData(string session, int pageSize, int pageNum);

        Task InsertToDB(StoreItem data);

        Task InsertFileInfoDetails(FileInformation fileInfo);

        Task<FileInformation> GetFileInformation(string session);
    }
}
