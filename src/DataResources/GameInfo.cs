using System.Runtime.Serialization;
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
    [DataContract]
    public class GameInfo {
        [DataMember] public string GameName { get; private set; }
        [DataMember] public string GameDescription { get; private set; }
        [DataMember] public int GameId { get; private set; }
        [DataMember] public string BinPath { get; private set; }
        [DataMember] public string IconPath { get; private set; }
        [DataMember] public string[] PanelsPath { get; private set; }
        [DataMember] public string VideoPath { get; private set; }
        [DataMember] public GameController[] UseControllers { get; set; }
        [DataMember] public Tag[] Tags{get;set;}

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
