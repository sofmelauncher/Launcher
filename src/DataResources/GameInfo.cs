using System.Windows.Media;

namespace meGaton.DataResources {
    public enum GameController {
        Xbox, Mouse, Keyboard
    }

    public struct Tag {
        public string category;
        public Color bgColor;

        public Tag(string category, Color bgColor) {
            this.category = category;
            this.bgColor = bgColor;
        }
    }
    public class GameInfo {
        public string GameName { get; private set; }
        public string GameDescription { get; private set; }
        public int GameId { get; private set; }
        public string BinPath { get; private set; }
        public string IconPath { get; private set; }
        public string[] PanelsPath { get; private set; }
        public string VideoPath { get; private set; }
        public GameController[] UseControllers { get; set; }
        public Tag[] Tags{get;set;}

        public GameInfo(string game_name, string game_description, int game_id,
            string bin_path,string icon_path,string[] panels_path,
            string video_path,GameController[]use_controllers,Tag[]tags
            ) {
            GameName = game_name;
            GameDescription = game_description;
            GameId = game_id;
            BinPath = bin_path;
            IconPath = icon_path;
            PanelsPath = panels_path;
            VideoPath = video_path;
            UseControllers = use_controllers;
            Tags = tags;
        }
    }
}
