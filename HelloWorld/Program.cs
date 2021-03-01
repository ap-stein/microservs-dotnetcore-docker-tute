using System;
using Serilog;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            LogData firstLog = new LogData(){ Message = "Hello World", Number = 123 };

            Log.Information("{Message}, {Number}", firstLog.Message, firstLog.Number);
            Log.Information("{@log}", firstLog);

            // int number = 123;
            // Log.Information("Hello World, Test {number}!", number);
            // Log.Warning("Hello World, Test {number}!", number);
            // Log.Error(new ArgumentOutOfRangeException(), "Hello World, Test 123!");
            // Console.WriteLine("Hello World, Test 123!");
        }
    }

    class LogData
    {
        public string Message { get; set; }
        public int Number { get; set; }
    }
}
