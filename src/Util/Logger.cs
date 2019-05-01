using System;
using System.IO;
using System.Text;

namespace meGaton.Util{
    public enum LogLevel{
        Log,Warning,Error
    }
    /// <summary>
    /// シンプルなロガー。
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
