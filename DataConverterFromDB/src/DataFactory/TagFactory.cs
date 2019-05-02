using System.Collections.Generic;
using System.Linq;
using DataConverterFromDB.DataRecorder;
using meGaton.DataResources;

namespace DataConverterFromDB.DataFactory{
	public class TagFactory{
		private readonly List<TempTagInfo> infos;
		private readonly List<TempTagData> data;
		
		
		public TagFactory(TagRecorder tag_recorder,TagDataRecorder tag_data_recorder){
			infos = tag_recorder.tempTagInfos;
			data = tag_data_recorder.tempTagData;
		}

		public Tag[] GetTags(int game_id){
			var res = new List<Tag>();
			var target_tag_infos = infos.Where(n => n.gameInfoId == game_id);
			foreach (var item in target_tag_infos){
				var temp = data.Find(n=>n.id==item.tagId);
				res.Add(new Tag(temp.tagText,temp.color));
			}
			return res.ToArray();
		}
	}
}