using System;
using System.IO;
using System.Linq;

namespace meGatonDR{
	public class BinarySercher{

		private readonly string GAMES_ROOT_PATH;

		public BinarySercher(){
			GAMES_ROOT_PATH = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Games";
		}

		public string Serch(int game_id){
			var files = System.IO.Directory.GetFiles(GAMES_ROOT_PATH + "\\" + game_id,"*.exe",SearchOption.AllDirectories);
			if (files.Length == 0){
				return "";
			}
			var res = files.First().Replace(GAMES_ROOT_PATH, "").Trim('\\');
			//Console.WriteLine(res);
			return files.First();
		}
	}
}