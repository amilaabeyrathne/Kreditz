using KreditzBankData.IngestionService.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KreditzBankData.IngestionService.Services.FileParsers
{
    public class FileParserService
    {
        private readonly ILogger<FileParserService> _logger;
        private readonly IngestionOptions _options;
        private readonly IHostEnvironment _env;

        public FileParserService(ILogger<FileParserService> logger,
            IOptions<IngestionOptions> options,
            IHostEnvironment env)
        {
            _logger = logger;
            _options = options.Value;
            _env = env;
        }

        
    }
}
