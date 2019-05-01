using meGaton.DataResources;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meGaton.DataResources {
    public class SQLiteConnector : IGamesDataConnector
    {

        
        public SQLiteConnector() {
            var path= System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Games" + "\\db.sqlite3";
            using (var conn = new SQLiteConnection("Data Source=" + path)) {
                conn.Open();
                using (var  context=new DataContext(conn))
                {

                }
                conn.Close();
            }
        }
        public List<GameInfo> GetGamesInfo() {
            throw new NotImplementedException();
        }
    }
}
