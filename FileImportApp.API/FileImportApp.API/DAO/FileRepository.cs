using FileImportApp.API.Models.Conf;
using FileImportApp.API.Models.DB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImportApp.API.DAO
{
    public class FileRepository : IFileRepository
    {
        private readonly FileContext context;

        public FileRepository(IOptions<Settings> settings)
        {
            context = new FileContext(settings);
        }

        public async Task<List<StoreItem>> GetData(string session, int pageSize, int pageNum)
        {
            var query = context.FileData.Find(x => x.Session == session)
                .Skip(pageNum > 0 ? ((pageNum - 1) * pageSize) : 0).Limit(pageSize);
            return await query.ToListAsync();
        }

        public async Task InsertToDB(StoreItem data)
        {
            await context.FileData.InsertOneAsync(data);
        }

        public async Task InsertFileInfoDetails(FileInformation fileInfo)
        {
            await context.FileInfo.InsertOneAsync(fileInfo);
        }

        public async Task<FileInformation> GetFileInformation (string session)
        {
            var query = context.FileInfo.Find(x => x.Session == session);
            return await query.SingleOrDefaultAsync();
        }
    }
}
