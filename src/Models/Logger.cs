using System;
using System.IO;
using System.Text;
using meGaton.src.Models;

namespace meGaton.Models{
    public enum LogLevel{
        Log,Warning,Error
    }
    /// <summary>
    /// シンプルなロガー。
    /// 超絶適当に作ったので色々とよろしくないと思う、毎回ストリーム開くのとか。インタフェースはそのままでいいから中身あとで直したい
    /// </summary>
    public class Logger{
        public static Logger Inst { get; }=new Logger();

        private StreamWriter streamWriter;

        private  Logger(){
            
        }

        public void Log(string str,LogLevel logLevel=LogLevel.Log){
            using (streamWriter = new StreamWriter(PathManage.MY_BIN_PATH + "\\meGaton.log", true, Encoding.UTF8))
            {
                var date = DateTime.Now;
                var mess = "[" + (logLevel.ToString()).ToUpper() + "]:" + date + ":" + str;
                streamWriter.WriteLine(mess);
                Console.WriteLine(mess);
            }
        }
    }
}
