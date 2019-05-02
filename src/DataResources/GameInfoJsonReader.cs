using System;
using System.Collections.Generic;
using System.IO;
using meGaton.Util;
using Newtonsoft.Json;

namespace meGaton.DataResources {
    /// <summary>
    /// Jsonを読み込むDataConnector。meGaton v2.0ではJsonを使用する
    /// </summary>
    class GameInfoJsonReader :IGamesDataConnector {
        private readonly List<GameInfo> gamesInfo;//デシリアライズのオーバーヘッドを避けるためコンストラクタでキャッシュしておく

        public GameInfoJsonReader() {
            var data="";
            try {
                using (var sr = new StreamReader(PathManage.GAMES_ROOT_PATH + "\\gameinfo.json")) {
                    data = sr.ReadToEnd();
                }
            } catch (FileNotFoundException e) {
                Logger.Inst.Log("gameinfo.json is not found.",LogLevel.Warning);
                gamesInfo = null;
                return;
            }
            try {
                gamesInfo=JsonConvert.DeserializeObject<List<GameInfo>>(data);
            } catch (Exception e) {
                Logger.Inst.Log(e.ToString(), LogLevel.Error);
                gamesInfo = null;
            }
        }

        public List<GameInfo> GetGamesInfo() {
            return gamesInfo;
        }
    }
}
