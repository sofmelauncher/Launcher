using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace meGaton.Util
{
    public class ServerConnector
    {

        private static ServerConnector instance;

        private System.Net.Http.HttpClient client;

        private readonly string SERVER_URL;
        private readonly string KEY_NAME;
        private readonly bool canPost = true;

        public static ServerConnector Inst {
            get {
                if (instance == null)
                {
                    instance = new ServerConnector();
                }
                return instance;
            }
        }

        private ServerConnector()
        {
            this.client = new System.Net.Http.HttpClient();

            try
            {
                this.KEY_NAME = ConfigurationManager.AppSettings["key"];
                this.SERVER_URL = ConfigurationManager.AppSettings["serverUrl"];
            }
            catch (ConfigurationErrorsException ex)
            {
                this.canPost = false;
                Logger.Inst.Log(ex.ToString(), LogLevel.Error);
            }
        }

        /// <summary>
        /// 外部から呼ばれるPostメソッド。
        /// 設定読み込みに失敗した場合は、オンライン送信はしない。
        /// </summary>
        /// <param name="post">送信される文字列</param>
        public async void Post(string post)
        {
            if (this.canPost)
            {
                var t = await Task.Run(() =>
                {
                    return this.OnlinePost(post);
                });

            }

        }

        /// <summary>
        /// サーバーにデータを送信するメソッド。
        /// </summary>
        /// <param name="post">送信される文字列</param>
        /// <returns>サーバーから返ってきた値。</returns>
        private async Task<string> OnlinePost(string post)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
                {
                    { KEY_NAME, post },
                };
            var content = new System.Net.Http.FormUrlEncodedContent(data);
            var response = await client.PostAsync(this.SERVER_URL, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
