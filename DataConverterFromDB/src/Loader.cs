using System;
using System.Data.SQLite;
using System.IO;
using DataConverterFromDB.CalamFilter;
using DataConverterFromDB.DataRecorder;

namespace DataConverterFromDB{
	public class Loader{
		
		public void Load(string db_name,IDataRecorder recorder,ICalamFilter filter=null){
			var bin_path=AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
			try{
				using (var con=new SQLiteConnection("Data Source="+bin_path+"\\db.sqlite3")){
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
			}catch (FileNotFoundException e){
				Console.WriteLine("Database not found");
			}
			
		}
	}
}