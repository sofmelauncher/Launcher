using System;
using System.Diagnostics;

namespace meGaton.Models {
    public class GameProcessControll {
        public static GameProcessControll GetInstance { get; } = new GameProcessControll();
        private Process currentProcess;
        public bool IsRunning => currentProcess != null;

        private DateTime startTime=new DateTime();

        private GameProcessControll() {
        }

        public void GameLaunch(string path) {
            if (IsRunning) return;

            try{
                System.IO.Directory.SetCurrentDirectory(path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal)));

            } catch (Exception e){
                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
                Logger.Inst.Log(e+". I didn't change currentDirectory.",LogLevel.Error);
                throw;
            }

            try{
                currentProcess = new Process();
                currentProcess.StartInfo.FileName = path;
                currentProcess.EnableRaisingEvents = true;
                currentProcess.Exited += (sender, e) =>{
                    Logger.Inst.Log("-Finish- " + currentProcess.ProcessName +" -Running Time- "+ (DateTime.Now - startTime).TotalSeconds+"seconds");
                    currentProcess = null;
                    System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
                };
                currentProcess.Start();
                startTime = DateTime.Now;
                Logger.Inst.Log("-Launch- "+currentProcess.ProcessName);
            }catch (Exception e){
                currentProcess = null;
                Logger.Inst.Log(e+".I didn't launch game that is "+path,LogLevel.Error);
                throw;
            }
        }
     }
}
