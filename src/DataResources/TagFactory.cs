using System.Collections.Generic;
using System.Linq;
using meGaton.DataResources;

namespace meGatonDR{
	public class TagFactory{
		private List<TempTagInfo> infos;
		private List<TempTagData> data;
		
		public TagFactory(TagRecorder tag_recorder,TagDataRecorder tag_data_recorder){
			infos = tag_recorder.TempTagInfos;
			data = tag_data_recorder.tempTagDatas;
		}

		public Tag[] GetTags(int game_id){
			var res = new List<Tag>();
			var target_tag_infos = infos.Where(n => n.gameInfoID == game_id);
			foreach (var item in target_tag_infos){
				var temp = data.Find(n=>n.id==item.tagID);
				res.Add(new Tag(temp.tagText,temp.color));
			}
			return res.ToArray();
		}
	}
}