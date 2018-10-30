using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Media;

namespace meGatonDR{
	public struct TempTagData{
		public int id;
		public string tagText;
		public Color color;

		public TempTagData(int id, string tag_text, Color color){
			this.id = id;
			tagText = tag_text;
			this.color = color;
		}
	}
	
	public class TagDataRecorder:IDataRecorder{
		public List<TempTagData> tempTagDatas=new List<TempTagData>();
		
		public void AddMember(SQLiteDataReader sdr){
			var id = (int) ((long) sdr["tag_id"]);
			var txt = (string) sdr["tag_name"];
			var co = (string) sdr["color"];
		    var color = Colors.Aqua;;
			tempTagDatas.Add(new TempTagData(id,txt,color));
		}
	}
}