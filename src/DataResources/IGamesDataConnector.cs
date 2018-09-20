using System.Collections.Generic;
using meGaton.DataClass;

namespace meGaton.DataResources {
    public interface IGamesDataConnector {
        List<GameInfo> GetGamesInfo();
    }
}
