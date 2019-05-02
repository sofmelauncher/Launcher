using System.Collections.Generic;
using System.Data.SQLite;

namespace DataConverterFromDB.DataRecorder{
	public struct TempTagInfo{
		public readonly int gameInfoId;
		public readonly int tagId;

		public TempTagInfo(int game_info_id, int tag_id){
			gameInfoId = game_info_id;
			tagId = tag_id;
		}
	}
	public class TagRecorder:IDataRecorder{
		public readonly List<TempTagInfo> tempTagInfos=new List<TempTagInfo>();
		
		public void AddMember(SQLiteDataReader sdr){
			tempTagInfos.Add(new TempTagInfo((int)((long)sdr["gameinfo_id"]),(int)((long)sdr["tag_id"])));
		}
	}
}