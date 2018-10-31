using System;
using System.Windows.Controls;
using meGaton.src.Models;

namespace meGaton.Models {
    /// <summary>
    /// 紹介動画/画像を再生する
    /// </summary>
    public class MediaDisplay {
        private MediaElement displayMediaElement;

        public MediaDisplay(MediaElement media_element) {
            displayMediaElement = media_element;
        }

        public void SetMedia(string[]panel_paths,string video_path) {
            displayMediaElement.Source = null;
            if (video_path == "") {
                displayMediaElement.Source = new Uri(PathManage.GAMES_ROOT_PATH+"\\"+panel_paths[0], UriKind.Relative);
            } else {
                displayMediaElement.Source = new Uri(PathManage.GAMES_ROOT_PATH + "\\" + video_path, UriKind.Relative);
            }
            displayMediaElement.Play();
        }
    }
}
