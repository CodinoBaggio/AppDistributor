using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDistributor
{
    class Program
    {
        static void Main(string[] args)
        {
            // 更新対象EXEの起動チェック
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.BootExeName)).Length > 0)
            {
                // 起動中は何もしない
                return;
            }

            // アップデートバージョンファイルを取得
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                string uri = System.IO.Path.Combine(Properties.Settings.Default.DownloadUri, Properties.Settings.Default.UpdateVerFileName);
                wc.DownloadFile(uri, Properties.Settings.Default.UpdateVerFileName);
            }

            // アップデートバージョンを取得
            string updateVer = System.IO.File.ReadAllText(Properties.Settings.Default.UpdateVerFileName);

            bool isDownload = false;
            if (System.IO.File.Exists(Properties.Settings.Default.CurrentVerFileName))
            {
                // アップデートバージョンとカレントバージョンが違う場合はzipファイルをダウンロード
                string currentVer = System.IO.File.ReadAllText(Properties.Settings.Default.CurrentVerFileName);
                isDownload = updateVer != currentVer;
            }
            else
            {
                // カレントバージョンファイルが無い場合はzipファイルをダウンロード
                isDownload = true;
            }

            if (isDownload)
            {
                // zipファイルをダウンロード
                string fileName = string.Empty;
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.BootExeName) + ".zip";
                    string uri = System.IO.Path.Combine(Properties.Settings.Default.DownloadUri, fileName);
                    wc.DownloadFile(uri, fileName);
                }

                // zipファイルを解凍
                Zip.Unzip(fileName, ".", "");

                // バージョンを書き込む
                System.IO.File.WriteAllText(Properties.Settings.Default.CurrentVerFileName, updateVer);
            }

            System.Diagnostics.Process.Start(Properties.Settings.Default.BootExeName);
        }
    }
}
