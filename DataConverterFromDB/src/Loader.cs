using System.Data.SQLite;
using meGaton.DataResources;
using meGaton.src.DataResources;

namespace meGatonDR{
	public class Loader{
		public void Load(string db_name,IDataRecorder recorder,ICalamFilter filter=null){
			var bin_path=System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
			using (var con=new SQLiteConnection("Data Source="+bin_path+"\\Games\\db.sqlite3")){
				con.Open();
				var sql = "select * from "+db_name;
				var com = new SQLiteCommand(sql, con);
				var sdr = com.ExecuteReader();
				while (sdr.Read() == true){
                    if(filter!=null&&!filter.Filter(sdr))continue;
				    recorder.AddMember(sdr);
				}
				sdr.Close();
				con.Close();
			}
		}
	}
}