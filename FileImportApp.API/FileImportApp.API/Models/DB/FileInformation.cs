using FileImportApp.API.Models.Client;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileImportApp.API.Models.DB
{
    public class FileInformation
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string Session { get; set; }
        public string UploadedFileName { get; set; }
        public ImportState FinalState { get; set; }
        public string Description { get; set; }
        public int RecordCount { get; set; }
    }
}
