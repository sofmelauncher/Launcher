using System.Collections.Generic;
using System.Data.SQLite;
using meGaton.DataResources;

namespace meGatonDR{
	public class GameInfoRecorder:IDataRecorder{
		public List<GameInfo> GameInfos=new List<GameInfo>();
		private TagFactory tagFactory;
		private BinarySercher binarySercher=new BinarySercher();
		private ControllerDataFactory controllerDataFactory=new ControllerDataFactory();
		
		public GameInfoRecorder(TagFactory tag_factory){
			tagFactory = tag_factory;
		}
		
		public void AddMember(SQLiteDataReader sdr){
			var id = (int) ((long) sdr["game_id"]);
			var info=new GameInfo(
				game_name:sdr["name"].ToString(),
				game_description:sdr["discription"].ToString(),
				game_id:id,
				bin_path:binarySercher.Serch(id),
				icon_path:sdr["panel"].ToString(),
				panels_path:new string[]{sdr["picture_1"]as string,sdr["picture_2"]as string,sdr["picture_3"]as string,}, 
				video_path:(string)sdr["movie"],
				use_controllers:controllerDataFactory.GetContollerEnable(
					mouse:(bool)sdr["is_mouse"],
					keyborad:(bool)sdr["is_keyboard"],
					gamepad:(bool)sdr["is_gamepad"]),
				tags:tagFactory.GetTags(id)
			);
			GameInfos.Add(info);
		}
	}
}