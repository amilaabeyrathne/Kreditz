namespace KreditzBankData.IngestionService.Options
{
    public class IngestionOptions
    {
        /// <summary>Base path for folder paths. If empty, uses app content root. Use absolute path to avoid bin/Debug resolution.</summary>
        public string FilesBasePath { get; set; } = "";

        public string ArrivedFolder { get; set; } = "Files/Arrived";
        public string ProcessedFolder { get; set; } = "Files/Processed";
        public string DuplicatesFolder { get; set; } = "Files/Duplicates";
        public string ErrorFolder { get; set; } = "Files/Error";

        /// <summary>How often to poll the Arrived folder for new files (seconds).</summary>
        public int PollingIntervalSeconds { get; set; } = 200;
    }
}
