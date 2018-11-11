using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FileImportApp.API.Models.Client
{
    public class ImportStateResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ImportState ImportState { get; set; }
        public int Percentage { get; set; }
        public string Description { get; set; }

        public ImportStateResponse(ImportState _ImportState)
        {
            ImportState = _ImportState;
        }

        public ImportStateResponse(ImportState _ImportState, int _Percentage)
        {
            ImportState = _ImportState;
            Percentage = _Percentage;
        }

        public ImportStateResponse(ImportState _ImportState, string _Description)
        {
            ImportState = _ImportState;
            Description = _Description;
        }
    }
}
