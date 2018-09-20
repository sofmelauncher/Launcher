using System.IO;

namespace meGaton.DataClass
{
    public class GameInfo{
        public string GameName {get; private set; }
        public string GameDescription { get; private set; }
        public int GameId { get; private set; }
        public string BinPath { get; private set; }
        public string[] PanelsPath { get; private set; }

        public GameInfo(string game_name, string game_description, int game_id,string bin_path,string[] panels_path) {
            GameName = game_name;
            GameDescription = game_description;
            GameId = game_id;
            BinPath = bin_path;

        }
    }
}
