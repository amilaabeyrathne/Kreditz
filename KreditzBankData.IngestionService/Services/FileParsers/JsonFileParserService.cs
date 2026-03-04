using KreditzBankData.IngestionService.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KreditzBankData.IngestionService.Services.FileParsers
{
    public class JsonFileParserService : FileParserService
    {
        public JsonFileParserService(
            ILogger<FileParserService> logger,
            IOptions<IngestionOptions> options,
            IHostEnvironment env)
            : base(logger, options, env)
        {
        }
    }
}
