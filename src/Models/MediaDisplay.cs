using System;
using System.Windows.Controls;

namespace meGaton.Models {
    class MediaDisplay {
        private MediaElement displayMediaElement;

        public MediaDisplay(MediaElement media_element) {
            displayMediaElement = media_element;
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
