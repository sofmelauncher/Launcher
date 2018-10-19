using System;
using System.Diagnostics;

namespace meGaton.Models {
    public class GameProcessControll {
        public static GameProcessControll GetInstance { get; } = new GameProcessControll();
        private Process currentProcess;
        public bool IsRunning => currentProcess != null;

        private GameProcessControll() {
        }

        public void GameLaunch(string path) {
            if (IsRunning) return;

            try{
                System.IO.Directory.SetCurrentDirectory(path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal)));

            } catch (Exception e){
                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));

                Console.WriteLine(e);
                throw;
            }

            try{
                currentProcess = new Process();
                currentProcess.StartInfo.FileName = path;
                currentProcess.EnableRaisingEvents = true;
                currentProcess.Exited += (sender, e) =>{
                    currentProcess = null;
                    System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
                };
                    currentProcess.Start();
            }catch (Exception e){
                currentProcess = null;

                Console.WriteLine(e);
                throw;
            }
        }
     }
}
