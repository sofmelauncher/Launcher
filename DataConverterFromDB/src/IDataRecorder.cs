using System.Data.SQLite;

namespace meGatonDR{
	public interface IDataRecorder{
		void AddMember(SQLiteDataReader sdr);
	}
}