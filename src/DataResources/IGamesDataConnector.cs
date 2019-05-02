using System.Collections.Generic;

namespace meGaton.DataResources {
    /// <summary>
    /// 表示に使用するゲーム情報を提供する
    /// </summary>
    public interface IGamesDataConnector {
        List<GameInfo> GetGamesInfo();
    }
}
