using NLog;
using NLog.LiteDB;
using System;
using System.Linq;
using System.Threading;

namespace ExampleApp
{
    class Program
    {
        private static LiteDBTarget _specialTarget;
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Info($"Start of application at {DateTime.Now.ToString("HH.mm.ss")}");

            DisplayLog();

            Console.ReadLine();
        }

        static void DisplayLog()
        {
            var targets = LogManager.Configuration.AllTargets;
            foreach (var target in targets)
            {
                if (target.Name == "liteDB")
                {
                    _specialTarget = target as LiteDBTarget;
                }
            }

            var db = new LiteDB.LiteDatabase(_specialTarget.ConnectionString);

            var collection = db.GetCollection<DefaultLog>("DefaultLog");
            Console.WriteLine($"There are {collection.Count()} log entries in special collection\n");

            var entries = collection.FindAll().ToList();

            foreach (var item in entries)
            {
                Console.WriteLine($"{item.Level}  {item.Message}");
            }

        }
    }
}
