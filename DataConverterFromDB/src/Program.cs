using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConverterFromDB.src;
using meGatonDR;
using Newtonsoft.Json;

namespace meGatonDataConverter {
    class Program {
        static void Main(string[] args) {
            var data = new meGatonDatabaseConnector().GetGamesInfo();
            foreach (var item in data) {
                Console.WriteLine("["+item.GameId+","+item.BinPath+"]");
            }
            new GameInfoJsonWriter().Write(data);
            Console.WriteLine("Convert Finished.");
            Console.ReadKey();
        }
    }
}
