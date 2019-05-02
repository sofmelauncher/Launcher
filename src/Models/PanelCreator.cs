using System.Data;
using System.Windows.Controls;
using meGaton.DataResources;
using meGaton.Util;
using meGaton.ViewModels;
using meGaton.Views;

namespace meGaton.Models{
    /// <summary>
    /// パネルファクトリ
    /// </summary>
    public class PanelCreator {
  
        private readonly IGamesDataConnector iGamesDataConnector;//データベース

        
        //DataConnectorへの依存は抽入する
        public PanelCreator(IGamesDataConnector i_games_data_connector) {
            iGamesDataConnector = i_games_data_connector;
        }

        /// <summary>
        /// パネルを作成しViewModelを抽入します
        /// </summary>
        /// <param name="parent_panel"></param>
        public void Launch(Panel parent_panel){
            var games_info = iGamesDataConnector.GetGamesInfo();
            if (games_info==null||games_info.Count == 0){
                Logger.Inst.Log("GamesList is Empty.I don't create panels.",LogLevel.Warning);
                throw new DataException();
            }

            foreach (var item in games_info){
                parent_panel.Children.Add(new GamePanel(new GamePanelViewModel(item)));
            }
        }
    }
}
