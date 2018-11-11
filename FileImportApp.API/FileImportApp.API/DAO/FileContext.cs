using FileImportApp.API.Models.Conf;
using FileImportApp.API.Models.DB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;

namespace FileImportApp.API.DAO
{
    public class FileContext
    {
        private readonly IMongoDatabase _database = null;

        public FileContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        /* Collection for file content  */
        public IMongoCollection<StoreItem> FileData
        {
            get
            {
                bool collectionFound = _database.ListCollectionNames().ToEnumerable().Contains("FileData");
                IMongoCollection<StoreItem> collection = _database.GetCollection<StoreItem>("FileData");
                return collection;
            }
        }

        /* Collection for file information */
        public IMongoCollection<FileInformation> FileInfo
        {
            get
            {
                bool collectionFound = _database.ListCollectionNames().ToEnumerable().Contains("FileInfo");
                IMongoCollection<FileInformation> collection = _database.GetCollection<FileInformation>("FileInfo");
                return collection;
            }
        }

    }
}
