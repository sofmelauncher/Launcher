using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using meGaton.DataResources;
using meGaton.Models;
using meGaton.src.Models;
using Newtonsoft.Json;

namespace meGaton.src.DataResources {
    class GameInfoJsonReader :IGamesDataConnector{
        public List<GameInfo> GetGamesInfo() {
            var data = "";
            try {
                using (var sr=new StreamReader(PathManage.GAMES_ROOT_PATH + "\\gameinfo.json")) {
                    data = sr.ReadToEnd();
                }
            } catch (NotFiniteNumberException e) {
                Logger.Inst.Log("gameinfo.json is not found.");
                return null;
            }

            try {
                return JsonConvert.DeserializeObject<List<GameInfo>>(data);
            } catch (Exception e) {
                Logger.Inst.Log(e.ToString(),LogLevel.Error);
                return null;
            }
        }
    }
}
