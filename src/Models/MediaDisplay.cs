using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using meGaton.Util;

namespace meGaton.Models {
    /// <summary>
    /// 紹介動画/画像を再生する
    /// </summary>
    public class MediaDisplay {
        
        // 動画と画像を識別する
        private enum ContentType {
            Video,Image
        }
        //コンテンツを動画と画像区別なく扱うためのパッケージ
        private struct Content {
            public readonly Uri uri;
            public readonly ContentType contentType;
            public Content(Uri uri, ContentType content_type) {
                this.uri = uri;
                this.contentType = content_type;
            }
        }

        
        private int contentCounter;

        private readonly MediaElement displayMediaElement;
        private readonly List<Content> currentContents=new List<Content>();
        private Content current;
        private InstantTimer timer;

        
        private const int NEXT_IMAGE_TIME=3;

        
        /// <param name="media_element">表示に使用するMediaElement</param>
        public MediaDisplay(MediaElement media_element) {
            displayMediaElement = media_element;

            displayMediaElement.MediaEnded += (e, sender) => {
                if(current.contentType==ContentType.Video)PlayContent();
            };
        }


        /// <summary>
        /// 動画、画像の順でループして表示させます
        /// </summary>
        /// <param name="video_path">表示する動画のGamesRootからの相対パス</param>
        /// <param name="panel_paths">表示する画像のGamesRootからの相対パス</param>
        /// <exception cref="ArgumentException">パスがnull</exception>
        public void SetMedia(string video_path,string[]panel_paths) {
            if (video_path==null||panel_paths == null) {
                throw new ArgumentException();
            }
            displayMediaElement.Source = null;
            timer?.Close();
            timer = null;

            currentContents.Clear();
            contentCounter = 0;

            if (video_path != "") {
                currentContents.Add(new Content(new Uri(PathManage.GAMES_ROOT_PATH+"\\"+video_path, UriKind.Relative),ContentType.Video));
            }

            foreach (var item in panel_paths) {
                if(item=="")continue;
                Uri temp;
                try {
                    temp = new Uri(PathManage.GAMES_ROOT_PATH + "\\" + item, UriKind.Relative);
                } catch (FileNotFoundException e) {
                    Logger.Inst.Log(e+"Panel not found.",LogLevel.Error);
                    continue;
                }
                currentContents.Add(new Content(temp,ContentType.Image));
            }
            PlayContent();
        }

        /// <summary>
        /// 動画の再生とコンテンツの切り替えを止めます
        /// </summary>
        public void Pause() {
            if (current.contentType == ContentType.Video) {
                displayMediaElement.Pause();
            } else {
                timer?.Close();
            }
        }

        /// <summary>
        /// 動画の再生とコンテンツの切り替えを再開します
        /// </summary>
        public void ReStart() {
            if (current.contentType == ContentType.Video) {
                displayMediaElement.Dispatcher.BeginInvoke(new Action(() => { displayMediaElement.Play(); }));
            } else {
                //PlayContent();
            }
        }

        private void PlayContent() {
            if (currentContents.Count==0)return;
            current = currentContents[contentCounter];
            contentCounter = contentCounter == currentContents.Count - 1 ? 0 : contentCounter + 1;
            displayMediaElement.Dispatcher.BeginInvoke(new Action(() => {
                displayMediaElement.Source = current.uri;
                displayMediaElement.Play();
            }));
            
            if (current.contentType == ContentType.Image) {
                timer=new InstantTimer(NEXT_IMAGE_TIME,PlayContent);
            }
        }
    }
}
