using KreditzBankData.IngestionService.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KreditzBankData.IngestionService.Services
{
    public class FileWatcherBackgroundService : BackgroundService
    {
        private readonly ILogger<FileWatcherBackgroundService> _logger;
        private readonly IngestionOptions _options;

        public FileWatcherBackgroundService(
            ILogger<FileWatcherBackgroundService> logger,
            IOptions<IngestionOptions> options,
            IHostEnvironment env)
        {
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("File watcher started. Polling {Folder} every {Seconds}s",
                _options.ArrivedFolder, _options.PollingIntervalSeconds);

            TimeSpan interval = TimeSpan.FromSeconds(_options.PollingIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ScanArrivedFolderAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while scanning Arrived folder");
                }

                await Task.Delay(interval, stoppingToken);
            }

            _logger.LogInformation("File watcher stopped");
        }

        private async Task ScanArrivedFolderAsync(CancellationToken stoppingToken)
        {
            string path = Path.GetFullPath(_options.ArrivedFolder);

            if (!Directory.Exists(path))
            {
                _logger.LogDebug("Arrived folder does not exist: {Path}", path);
                return;
            }

            string[] files = Directory.GetFiles(path, "*.json");
            if (files.Length == 0)
            {
                await Task.CompletedTask;
                return;
            }

            _logger.LogInformation("Found {Count} file(s) in Arrived folder", files.Length);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string processingPath = Path.Combine(ResolvePath(_options.ProcessingFolder), fileName);

                try
                {
                    // Move FIRST, before anything else
                    // This is the atomic guard: once the file leaves Arrived/,
                    // the watcher will never pick it up again — even after a crash
                    File.Move(file, processingPath, overwrite: false);

                    _logger.LogInformation("Picked up {File} → Processing", fileName);

                    //  Now process 
                }
                catch (IOException) when (File.Exists(processingPath))
                {
                    // Another instance already moved this file — safe to skip
                    _logger.LogWarning("File {File} already in Processing, skipping", fileName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process {File}", fileName);

                    // Move to Error/ so it doesn't block the queue
                    string errorPath = Path.Combine(ResolvePath(_options.ErrorFolder), fileName);
                    if (File.Exists(processingPath))
                        File.Move(processingPath, errorPath, overwrite: true);
                }
            }

            await Task.CompletedTask;
        }

        private string ResolvePath(string relativePath)
        {
            string basePath = string.IsNullOrEmpty(_options.FilesBasePath)
                ? Directory.GetCurrentDirectory()
                : _options.FilesBasePath;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }
    }
}
