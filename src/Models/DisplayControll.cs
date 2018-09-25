using System;
using System.Windows.Controls;

namespace meGaton.Models {
    class DisplayControll {
        private MediaElement displayMediaElement;
        private Image displayImage;

        public DisplayControll(MediaElement media_element,Image image) {
            displayMediaElement = media_element;
            displayImage = image;
        }

        public void SetMedia(string[]panel_paths,string video_path) {
            displayMediaElement.Source = null;
            if (video_path == "") {
                displayMediaElement.Source = new Uri(panel_paths[0], UriKind.Relative);
            } else {
                displayMediaElement.Source = new Uri(video_path, UriKind.Relative);
            }
            displayMediaElement.Play();
        }
    }
}
