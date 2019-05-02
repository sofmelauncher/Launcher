using System.Data.SQLite;

namespace DataConverterFromDB.CalamFilter{
    public interface ICalamFilter{
        bool Filter(SQLiteDataReader sdr);
    }
}
