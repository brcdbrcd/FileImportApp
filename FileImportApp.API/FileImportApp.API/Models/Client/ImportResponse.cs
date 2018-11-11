namespace FileImportApp.API.Models.Client
{
    public class ImportResponse
    {
        public string Session { get; set; }
        public string Description { get; set; }

        public ImportResponse(string _Session, string _Description)
        {
            Session = _Session;
            Description = _Description;
        }

    }
}
