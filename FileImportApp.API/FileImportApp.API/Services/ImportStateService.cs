using FileImportApp.API.Models.Client;
using System.Collections.Generic;

namespace FileImportApp.API.Services
{
    /* Gets and sets file import process state */
    public class ImportStateService
    {
        private static Dictionary<string, ImportStateResponse> sessionMap = new Dictionary<string, ImportStateResponse>();
        
        public static ImportStateResponse Get(string session)
        {
            return sessionMap.GetValueOrDefault(session);
        }

        public static void SetInitialized(string session)
        {
            ImportStateResponse resp = new ImportStateResponse(ImportState.Initialized);
            sessionMap[session] = resp;
        }

        public static void SetReading(string session)
        {
            ImportStateResponse resp = new ImportStateResponse(ImportState.Reading);
            sessionMap[session] = resp;
        }

        public static void SetFailed(string session, string description)
        {
            ImportStateResponse resp = new ImportStateResponse(ImportState.Failed, description);
            sessionMap[session] = resp;
        }

        public static void SetProgress(string session, ImportState state, int percentage)
        {
            ImportStateResponse resp = new ImportStateResponse(state, percentage);
            sessionMap[session] = resp;
        }
    }
}
