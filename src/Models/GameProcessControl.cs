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
    public class GameProcessControl {
        
        private DateTime startTime;//起動時間計測用
        private Process currentProcess;

        
        public static GameProcessControl Inst { get; } = new GameProcessControl();
        public bool IsRunning => currentProcess != null;//実行中かどうか以外のプロセスの情報は公開しない

        private readonly Subject<Unit> onGameStart=new Subject<Unit>();
        /// <summary>ゲームの開始</summary>
        public IObservable<Unit> OnGameStart=>onGameStart;
        private readonly Subject<Unit> onGameEnd = new Subject<Unit>();
        /// <summary>ゲームの終了</summary>
        public IObservable<Unit> OnGameEnd => onGameEnd;


        private GameProcessControl() {
        }

        /// <summary>
        /// ゲームを起動します
        /// </summary>
        /// <param name="path">実行ファイルのGamesRootからの相対パス</param>
        public void GameLaunch(string path,string game_id) {
            if (IsRunning) return;
            
            path= PathManage.GAMES_ROOT_PATH + "\\" + path;//受け取った相対パスを絶対パスに変換

            //ファイルがあるかチェック
            if (path==""||!File.Exists(path)) {
                Logger.Inst.Log("I didn't found binary file." + path, LogLevel.Error);
                Logger.Inst.Log(new FileNotFoundException()+"bin not found",LogLevel.Error);
            }

            //カレントディレクトリを実行ファイルのディレクトリまで移動する
            //素材をパッケージングしていないDxLib製ファイルなど、実行時にカレントディレクトリを基にファイルを検索する実行ファイルが存在するため
            try {
                Directory.SetCurrentDirectory(
                    path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal)));
            } catch (Exception e){
                ReturnCurrentDirectory();
                Logger.Inst.Log(e+". I didn't change currentDirectory.",LogLevel.Error);
                throw;
            }

            try {
                currentProcess = new Process{
                    StartInfo = {FileName = path}, 
                    EnableRaisingEvents = true
                };
                currentProcess.Exited += (sender, e) =>{//プロセス終了イベントに終了処理登録
                    onGameEnd.OnNext(Unit.Default);
                    Logger.Inst.Log("-FinishGame- ID:" + game_id + ",ProcessName:" + currentProcess.ProcessName +
                                    ",FinishTime:"+ DateTime.Now + ",RunningTime:"+ (DateTime.Now - startTime).TotalSeconds+" sec");
                    currentProcess = null;
                    ReturnCurrentDirectory();
                };
                currentProcess.Start();
                onGameStart.OnNext(Unit.Default);
                startTime = DateTime.Now;
                Logger.Inst.Log("-LaunchGame- ID:"+game_id+",ProcessName:"+currentProcess.ProcessName+",StartTime:"+startTime);
            }catch (Exception e){
                currentProcess = null;
                ReturnCurrentDirectory();
                throw;
            }
        }

        //DRYに則り
        private static void ReturnCurrentDirectory(){
            Directory.SetCurrentDirectory(PathManage.MY_BIN_PATH);
        }
    }
}
