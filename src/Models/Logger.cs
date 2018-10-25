using System;
using System.IO;
using System.Text;

namespace meGaton.Models{
    public enum LogLevel{
        Log,Warning,Error
    }
    public class Logger{
        public static Logger Inst { get; }=new Logger();

        private StreamWriter streamWriter;

        private  Logger(){
            
        }

        public void Log(string str,LogLevel logLevel=LogLevel.Log){
            var bin_path = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            using (streamWriter = new StreamWriter(bin_path + "\\meGaton.log", true, Encoding.UTF8))
            {
                var date = DateTime.Now;
                var mess = "[" + (logLevel.ToString()).ToUpper() + "]:" + date + ":" + str;
                streamWriter.WriteLine(mess);
                Console.WriteLine(mess);
            }
        }
    }
}
