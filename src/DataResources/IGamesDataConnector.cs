using System.Collections.Generic;

namespace meGaton.DataResources {
    public interface IGamesDataConnector {
        List<GameInfo> GetGamesInfo();
    }
}
