using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using meGaton.DataResources;
using Newtonsoft.Json;

namespace DataConverterFromDB.src {
    class GameInfoJsonWriter {
        public void Write(List<GameInfo> infos) {
            var json_string = JsonConvert.SerializeObject(infos, Formatting.Indented);
            var write_path = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')+"\\Games\\";

            //BOM無しUTF-8書き込みなのでエンコード指定をしない
            using (var sw=new StreamWriter(write_path+"gameinfo.json",false)) {
                sw.Write(json_string);
            }
        }
    }
}
