using System;
using System.Diagnostics;

namespace meGaton.Models {
    public class GameProcessControll {
        private Process currentProcess;
        public bool IsRunning => currentProcess != null;

        public void GameLaunch(string path) {
            if (IsRunning) return;
            currentProcess = new Process();
            currentProcess.StartInfo.FileName = path;
            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += ProcessExited;
            currentProcess.Start();
        }

        void ProcessExited(object sender, EventArgs e) {
            currentProcess = null;
        }
    }
}
