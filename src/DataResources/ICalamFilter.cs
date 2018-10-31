using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meGaton.src.DataResources
{
    public interface ICalamFilter{
        bool Filter(SQLiteDataReader sdr);
    }
}
