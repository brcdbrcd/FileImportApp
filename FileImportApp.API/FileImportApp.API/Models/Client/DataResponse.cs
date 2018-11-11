using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileImportApp.API.Models.Client
{
    public class DataResponse
    {
        public List<StoreItemDto> data { get; set; }
        public int totalCount { get; set; }
    }
}
