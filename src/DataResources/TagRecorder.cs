using System.Collections.Generic;
using System.Data.SQLite;

namespace meGatonDR{
	public struct TempTagInfo{
		public int gameInfoID;
		public int tagID;

		public TempTagInfo(int game_info_id, int tag_id){
			gameInfoID = game_info_id;
			tagID = tag_id;
		}
	}
	public class TagRecorder:IDataRecorder{
		public List<TempTagInfo> TempTagInfos=new List<TempTagInfo>();
		
		public void AddMember(SQLiteDataReader sdr){
			TempTagInfos.Add(new TempTagInfo((int)((long)sdr["gameinfo_id"]),(int)((long)sdr["tag_id"])));
		}
	}
}