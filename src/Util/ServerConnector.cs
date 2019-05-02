namespace meGaton.Util {
    public class ServerConnector {

        private static ServerConnector instance;
        public static ServerConnector Inst {
            get {
                if (instance == null) {
                    instance=new ServerConnector();
                }
                return instance;
            }
        }

        private ServerConnector() {
        }

        public void Post(string post) {

        }
        
    }
}
