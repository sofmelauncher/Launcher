using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace DataConverterFromDB.DataRecorder{
	public struct TempTagData{
		public readonly int id;
		public readonly string tagText;
		public Color color;

		public TempTagData(int id, string tag_text, Color color){
			this.id = id;
			tagText = tag_text;
			this.color = color;
		}
	}
	
	public class TagDataRecorder:IDataRecorder{
		public readonly List<TempTagData> tempTagData=new List<TempTagData>();
		
		public void AddMember(SQLiteDataReader sdr){
			var id = (int) ((long) sdr["tag_id"]);
			var txt = (string) sdr["tag_name"];
			var co = (string) sdr["color"];
		    var draw_color = ColorTranslator.FromHtml(co);
		    var media_color = Color.FromArgb(draw_color.A,draw_color.R,draw_color.G,draw_color.B);
			tempTagData.Add(new TempTagData(id,txt,media_color));
		}
	}
}