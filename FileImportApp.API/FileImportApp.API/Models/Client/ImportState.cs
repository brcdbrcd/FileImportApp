namespace FileImportApp.API.Models.Client
{
    public enum ImportState
    {
        Initialized,
        Reading,
        DBImporting,
        FileImporting,
        Completed,
        Failed
    }
}
