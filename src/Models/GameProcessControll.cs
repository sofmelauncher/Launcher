using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Subjects;
using meGaton.Util;

namespace meGaton.Models {
    /// <summary>
    /// 実行ファイルの実行や終了検知などの管理を行う。
    /// 多重起動を防いでかつ複数クラスからアクセスできるようにするためシングルトンにしている
    /// </summary>
    public class GameProcessControll {
        public static GameProcessControll GetInstance { get; } = new GameProcessControll();

        private Process currentProcess;

        public bool IsRunning => currentProcess != null;//実行中かどうか以外のプロセスの情報は公開しない

        private DateTime startTime=new DateTime();//起動時間計測用

        private Subject<Unit> gameStartStream=new Subject<Unit>();
        public IObservable<Unit> OnGameStart=>gameStartStream;
        private Subject<Unit> gameEndStream = new Subject<Unit>();
        public IObservable<Unit> OnGameEnd => gameEndStream;


        private GameProcessControll() {
        }

        public void GameLaunch(string path) {
            if (IsRunning) return;
            
            path= PathManage.GAMES_ROOT_PATH + "\\" + path;//受け取った相対パスを絶対パスに変換

            //ファイルがあるかチェック
            if (path==""||!System.IO.File.Exists(path)) {
                Logger.Inst.Log("I didn't found binary file." + path, LogLevel.Error);
                Logger.Inst.Log(new FileNotFoundException()+"bin not found",LogLevel.Error);
            }

            //カレントディレクトリを実行ファイルのディレクトリまで移動する
            //素材をパッケージングしていないDxLib製ファイルなど、実行時にカレントディレクトリを基にファイルを検索する実行ファイルが存在するため
            try {
                System.IO.Directory.SetCurrentDirectory(path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal)));
            } catch (Exception e){
                ReturnCurrentDirectory();
                Logger.Inst.Log(e+". I didn't change currentDirectory.",LogLevel.Error);
                throw;
            }

            try {
                currentProcess = new Process();
                currentProcess.StartInfo.FileName = path;
                currentProcess.EnableRaisingEvents = true;
                currentProcess.Exited += (sender, e) =>{//プロセス終了イベントに終了処理登録
                    gameEndStream.OnNext(Unit.Default);
                    Logger.Inst.Log("-Finish- " + currentProcess.ProcessName +" -Running Time- "+ (DateTime.Now - startTime).TotalSeconds+"seconds");
                    currentProcess = null;
                    ReturnCurrentDirectory();
                };
                currentProcess.Start();
                gameStartStream.OnNext(Unit.Default);
                startTime = DateTime.Now;
                Logger.Inst.Log("-Launch- "+currentProcess.ProcessName);
            }catch (Exception e){
                currentProcess = null;
                ReturnCurrentDirectory();
                throw;
            }
        }

        //DRYに則り
        private void ReturnCurrentDirectory(){
            System.IO.Directory.SetCurrentDirectory(PathManage.MY_BIN_PATH);
        }
    }
}
