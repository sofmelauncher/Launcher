using System.Runtime.Serialization;
using System.Windows.Media;

namespace meGaton.DataResources {
    /// <summary>
    /// ゲームで使用するコントローラ
    /// </summary>
    public enum GameController {
        Xbox, Mouse, Keyboard
    }

    /// <summary>
    /// タグ名とカラーを紐づける
    /// </summary>
    public struct Tag {
        public string Category;
        public Color BgColor;

        public Tag(string category, Color bg_color) {
            this.Category = category;
            this.BgColor = bg_color;
        }
    }

    /// <summary>
    /// ゲーム情報を格納する
    /// </summary>
    [DataContract]
    public class GameInfo {
        [DataMember] public string GameName { get; private set; }
        [DataMember] public string GameDescription { get; private set; }
        [DataMember] public int GameId { get; private set; }
        [DataMember] public int DisplayId { get; private set; }
        [DataMember] public string BinPath { get; private set; }
        [DataMember] public string IconPath { get; private set; }
        [DataMember] public string[] PanelsPath { get; private set; }
        [DataMember] public string VideoPath { get; private set; }
        [DataMember] public GameController[] UseControllers { get; set; }
        [DataMember] public Tag[] Tags{get;set;}

        public GameInfo(string game_name, string game_description, int game_id,int display_id,
            string bin_path,string icon_path,string[] panels_path,
            string video_path,GameController[]use_controllers,Tag[]tags
            ) {
            GameName = game_name;
            GameDescription = game_description;
            GameId = game_id;
            DisplayId = display_id;
            BinPath = bin_path;
            IconPath = icon_path;
            PanelsPath = panels_path;
            VideoPath = video_path;
            UseControllers = use_controllers;
            Tags = tags;
        }
    }
}
