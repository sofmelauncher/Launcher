using System.Data.SQLite;

namespace DataConverterFromDB.DataRecorder{
	public interface IDataRecorder{
		void AddMember(SQLiteDataReader sdr);
	}
}