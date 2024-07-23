using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Worker
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly DataAccess _dataAccess;
        private readonly ConsistentHashing _hashing;

        public WorkerService(ILogger<WorkerService> logger, DataAccess dataAccess, ConsistentHashing hashing)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _hashing = hashing;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workerName = Environment.GetEnvironmentVariable("WORKER_NAME");
            if (string.IsNullOrEmpty(workerName))
            {
                _logger.LogError("WORKER_NAME environment variable is not set.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                // Fetch and process items assigned to this worker
                var items = _dataAccess.GetItemsForWorker(workerName);
                foreach (var item in items)
                {
                    _dataAccess.UpdateItemValue(item.Id);
                }

                // Log current items
                _logger.LogInformation($"Worker {workerName} is processing items: {string.Join(", ", items.Select(i => i.Id))}");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
