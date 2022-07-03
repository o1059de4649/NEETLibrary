using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSELibrary.Cse.File
{
    /// <summary>
    /// ファイルの管理を容易にする
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// バイト配列を返す
        /// </summary>
        /// <param name="exportPath">読み込み先の先パス</param>
        /// <param name="isDelete">読み込み後、削除するかどうか</param>
        /// <returns></returns>
        public static byte[] GetBinary(string exportPath,bool isDelete = false)
        {
            using (FileStream fileStream = new FileStream(exportPath, FileMode.Open))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                //ファイルを読み込むバイト型配列を作成する
                byte[] bs = new byte[fileStream.Length];
                //ファイルの内容をすべて読み込む
                fileStream.Read(bs, 0, bs.Length);
                //閉じる
                fileStream.Close();
                if(isDelete) System.IO.File.Delete(exportPath);
                return bs;
            }
        }
    }
}
