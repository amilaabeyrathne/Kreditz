namespace KreditzBankData.IngestionService.Options
{
    public class IngestionOptions
    {
        public string FilesBasePath { get; set; } = "";
        public string ArrivedFolder { get; set; } = "Files/Arrived";
        public string ProcessedFolder { get; set; } = "Files/Processed";
        public string ProcessingFolder { get; set; } = "Files/Processing";
        public string DuplicatesFolder { get; set; } = "Files/Duplicates";
        public string ErrorFolder { get; set; } = "Files/Error";

        public int PollingIntervalSeconds { get; set; } = 200;
    }
}
