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
        private readonly IHostEnvironment _env;

        public FileWatcherBackgroundService(
            ILogger<FileWatcherBackgroundService> logger,
            IOptions<IngestionOptions> options,
            IHostEnvironment env)
        {
            _logger = logger;
            _options = options.Value;
            _env = env;
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
            string path = ResolvePath(_options.ArrivedFolder);

            if (!Directory.Exists(path))
            {
                _logger.LogInformation("Arrived folder does not exist: {Path}", path);
                return;
            }

            string[] files = Directory.GetFiles(path, "*.json");
            if (files.Length == 0)
            {
                await Task.CompletedTask;
                return;
            }

            foreach (string file in files)
            {
                _logger.LogInformation("  {File}", Path.GetFileName(file));
                // TODO: process file via IFileParserService / orchestrator
            }

            await Task.CompletedTask;
        }

        private string ResolvePath(string relativePath)
        {
            string basePath = string.IsNullOrEmpty(_options.FilesBasePath)
                ? _env.ContentRootPath
                : _options.FilesBasePath;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }
    }
}
