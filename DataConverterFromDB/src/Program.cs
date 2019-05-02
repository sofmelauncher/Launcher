using System;

namespace DataConverterFromDB {
    public class Program {
        
        static void Main(string[] args) {
            Console.WriteLine("Convert Start.");
            var data = new DatabaseConnector().GetGamesInfo();
            foreach (var item in data) {
                Console.WriteLine("["+item.GameId+","+item.BinPath+"]");
            }
            new GameInfoJsonWriter().Write(data);
            Console.WriteLine("Convert Finished.");
            Console.ReadKey();
        }
    }
}
