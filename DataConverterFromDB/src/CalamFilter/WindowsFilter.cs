using System.Data.SQLite;

namespace DataConverterFromDB.CalamFilter{
    public class WindowsFilter:ICalamFilter{
        
        public bool Filter(SQLiteDataReader sdr){
            return !((bool) sdr["android"]||(bool)sdr["vr"]||(bool)sdr["other"]);
        }
    }
}
