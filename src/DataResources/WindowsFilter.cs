using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meGaton.src.DataResources
{
    public class WindowsFilter:ICalamFilter{
        public bool Filter(SQLiteDataReader sdr)
        {
            return !((bool) sdr["android"]||(bool)sdr["vr"]||(bool)sdr["other"]);
        }
    }
}
