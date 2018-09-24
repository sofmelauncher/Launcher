using System;
using System.Diagnostics;

namespace meGaton.Models {
    public class GameProcessControll {
        private Process currentProcess;

        public void GameLaunch(string path) {
            currentProcess=new Process();
            currentProcess.StartInfo.FileName = path;
            currentProcess.Start();
        }
    }
}
