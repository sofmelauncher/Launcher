using System.Collections.Generic;
using DataConverterFromDB.CalamFilter;
using DataConverterFromDB.DataFactory;
using DataConverterFromDB.DataRecorder;
using meGaton.DataResources;

namespace DataConverterFromDB{
	public class DatabaseConnector{
		
		public List<GameInfo> GetGamesInfo(){
			var loader=new Loader();
			
			var tag_recorder=new TagRecorder();
			loader.Load("gameregister_gameinfo_tag",tag_recorder);
			var tag_data_recorder=new TagDataRecorder();
			loader.Load("gameregister_tag",tag_data_recorder);
			
			
			var game_recorder=new GameInfoRecorder(new TagFactory(tag_recorder,tag_data_recorder));
			loader.Load("gameregister_gameinfo", game_recorder,new WindowsFilter());
			return game_recorder.gameInfos;
		}
	}
}