using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDistributor
{
    class Zip
    {
        public static void Compress(string targetFileName, string saveFileName, string password)
        {
            using (ZipFile zip = new ZipFile(System.Text.Encoding.GetEncoding(932)))
            {
                zip.Password = password;
                zip.CompressionLevel = CompressionLevel.BestCompression;

                zip.AddFile(targetFileName, "");

                //ディレクトリを追加する場合
                //zip.AddDirectory(@"C:\追加したいフォルダ");

                zip.Save(saveFileName);
            }
        }

        public static void Unzip(string zipFileName, string extractFolder, string password)
        {
            using (ZipFile zip = ZipFile.Read(zipFileName, new ReadOptions() { Encoding = Encoding.GetEncoding(932) }))
            {
                zip.Password = password;
                zip.ExtractAll(extractFolder, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
