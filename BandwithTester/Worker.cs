using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeedTest;
using SpeedTest.Models;

namespace BandwithTester
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SpeedTestClient _client;
        private readonly Settings _settings;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _client = new SpeedTestClient();
            _settings = _client.GetSettings();
        }

        private List<Server> GetTestServers()
        {
            var servers = _settings.Servers.OrderBy(server=>server.Distance).Take(10).ToList();
            foreach(var server in servers)
            {
                server.Latency = _client.TestServerLatency(server);
            }

            return servers.OrderBy(server=>server.Latency).Take(3).ToList();
        }

        private double GetAverageDownloadSpeed(List<Server> servers)
        {
            List<double> downloadSpeeds = new List<double>();

            foreach(var server in servers)
            {
                double speed = _client.TestDownloadSpeed(server, _settings.Download.ThreadsPerUrl);
                downloadSpeeds.Add(speed);
            }
            return downloadSpeeds.Average();
        }

        private double GetAverageUploadSpeed(List<Server> servers)
        {
            List<double> uploadSpeeds = new List<double>();

            foreach(var server in servers)
            {
                double speed = _client.TestUploadSpeed(server, _settings.Upload.ThreadsPerUrl);
                uploadSpeeds.Add(speed);
            }
            return uploadSpeeds.Average();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Testing bandwidth at: {time}", DateTimeOffset.Now);
                //_logger.LogWarning("This is a warning.");

                var servers = GetTestServers();
                double averageDownloadSpeed = GetAverageDownloadSpeed(servers);
                double averageUploadSpeed = GetAverageUploadSpeed(servers);

                _logger.LogInformation("Bandwidth at {time}: Download:{download}, Upload:{upload} ", DateTimeOffset.Now, averageDownloadSpeed, averageUploadSpeed);

                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}
