using System;
using System.Collections.Generic;
using System.Windows.Controls;
using meGaton.Util;

namespace meGaton.Models {
    /// <summary>
    /// 紹介動画/画像を再生する
    /// </summary>
    public class MediaDisplay {
        private enum ContentType {
            video,image
        }
        private struct Content {
            public Uri uri;
            public ContentType contentType;
            public Content(Uri uri, ContentType contentType) {
                this.uri = uri;
                this.contentType = contentType;
            }
        }

        private MediaElement displayMediaElement;

        private readonly List<Content> currentContents=new List<Content>();
        private Content current;
        private int contentCoumter;

        private InstantTimer timer;

        private const int NEXT_IMAGE_TIME=3;

        public MediaDisplay(MediaElement media_element) {
            displayMediaElement = media_element;

            displayMediaElement.MediaEnded += (e, sender) => { if(current.contentType==ContentType.video)PlayContent();};
        }

        public void SetMedia(string[]panel_paths,string video_path) {
            if (panel_paths == null) {
                throw new ArgumentException();
            }
            displayMediaElement.Source = null;
            timer?.Close();
            timer = null;

            currentContents.Clear();
            contentCoumter = 0;

            if (video_path != "") {
                currentContents.Add(new Content(new Uri(PathManage.GAMES_ROOT_PATH+"\\"+video_path, UriKind.Relative),ContentType.video));
            }

            foreach (var item in panel_paths) {
                if(item=="")continue;
                Uri temp;
                try {
                    temp = new Uri(PathManage.GAMES_ROOT_PATH + "\\" + item, UriKind.Relative);
                } catch (Exception e) {
                    Logger.Inst.Log(e+"Panel not found.");
                    continue;
                }
                currentContents.Add(new Content(temp,ContentType.image));
            }
            PlayContent();
        }

        public void Pause() {
            if (current.contentType == ContentType.video) {
                displayMediaElement.Pause();
            } else {
                timer?.Close();
            }
        }

        public void ReStart() {
            if (current.contentType == ContentType.video) {
                displayMediaElement.Dispatcher.BeginInvoke(new Action(() => { displayMediaElement.Play(); }));
            } else {
                PlayContent();
            }
        }

        private void PlayContent() {
            if (currentContents.Count==0)return;
            current = currentContents[contentCoumter];
            contentCoumter = contentCoumter == currentContents.Count - 1 ? 0 : contentCoumter + 1;
            displayMediaElement.Dispatcher.BeginInvoke(new Action(() => {
                displayMediaElement.Source = current.uri;
                displayMediaElement.Play();
            }));
            
            if (current.contentType == ContentType.image) {
                timer=new InstantTimer(NEXT_IMAGE_TIME,PlayContent);
            }
        }
    }
}
