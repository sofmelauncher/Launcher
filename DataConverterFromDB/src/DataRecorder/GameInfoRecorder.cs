using System;
using System.Collections.Generic;
using System.Data.SQLite;
using DataConverterFromDB.DataFactory;
using meGaton.DataResources;

namespace DataConverterFromDB.DataRecorder{
	public class GameInfoRecorder:IDataRecorder{
		
		public readonly List<GameInfo> gameInfos=new List<GameInfo>();
		
		
		private readonly TagFactory tagFactory;
		private readonly BinarySearcher binarySearcher=new BinarySearcher();
		private readonly ControllerDataFactory controllerDataFactory=new ControllerDataFactory();
		
		
		public GameInfoRecorder(TagFactory tag_factory){
			tagFactory = tag_factory;
		}
		
		public void AddMember(SQLiteDataReader sdr) {
            var id = (int) ((long) sdr["game_id"]);
            try{
	            var info=new GameInfo(
		            game_name:sdr["name"].ToString(),
		            game_description:sdr["launcher_description"].ToString(),
		            game_id:id,
		            display_id: (int)((sdr["display_id"] == DBNull.Value ? 0 : sdr["display_id"])),
		            bin_path:binarySearcher.Search(id),
		            icon_path:sdr["panel"].ToString(),
		            panels_path:new string[]{sdr["picture_1"]as string,sdr["picture_2"]as string,sdr["picture_3"]as string,}, 
		            video_path:(string)sdr["movie"],
		            use_controllers:controllerDataFactory.GetControllerEnable(
			            mouse:(bool)sdr["is_mouse"],
			            keyborad:(bool)sdr["is_keyboard"],
			            gamepad:(bool)sdr["is_gamepad"]),
		            tags:tagFactory.GetTags(id)
		            
	            );
	            if(info.BinPath=="")return;
	            gameInfos.Add(info);
            }
            catch (Exception e){
	            Console.WriteLine(e);
            }
		}
	}
}