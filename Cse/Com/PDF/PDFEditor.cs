using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSELibrary.Cse.Com.PDF
{
    public class PDFEditor
    {
        /// <summary>
        /// PDF結合関数
        /// </summary>
        /// <param name="pdfPathList">PDFを結合するリスト</param>
        /// <param name="destPDFPath">出力先のパス</param>
        /// <param name="document">ドキュメントオブジェクト</param>
        public static void PDFMerge(List<string> pdfPathList, string destPDFPath, PdfDocument document = null)
        {
            if (document == null) document = new PdfDocument();
            foreach (var callPath in pdfPathList)
            {
                // 結合するPDFオブジェクトを作成
                PdfDocument inputDocument = PdfReader.Open(callPath, PdfDocumentOpenMode.Import);
                // 頁全件ループ
                foreach (PdfPage page in inputDocument.Pages)
                {
                    // PDF頁を追加
                    document.AddPage(page);
                }
            }
            // PDF保存
            document.Save(destPDFPath);
            // PDFを閉じる
            document.Close();
        }
        /// <summary>
        /// PDF結合関数
        /// </summary>
        /// <param name="pdfStreamList">PDFを結合するリスト</param>
        /// <param name="destPDFPath">出力先のパス</param>
        /// <param name="document">ドキュメントオブジェクト</param>
        public static void PDFMerge(List<MemoryStream> pdfStreamList, string destPDFPath, PdfDocument document = null)
        {
            if (document == null) document = new PdfDocument();
            foreach (var stream in pdfStreamList)
            {
                // 結合するPDFオブジェクトを作成
                PdfDocument inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                // 頁全件ループ
                foreach (PdfPage page in inputDocument.Pages)
                {
                    // PDF頁を追加
                    document.AddPage(page);
                }
            }
            // PDF保存
            document.Save(destPDFPath);
            // PDFを閉じる
            document.Close();
        }
    }

    public class PDFMergeDto
    {
        /// <summary>
        /// パスリスト
        /// </summary>
        public List<string> PathList { get; set; }
        /// <summary>
        /// バイナリリスト
        /// </summary>
        public List<List<byte>> ByteList { get; set; }
        /// <summary>
        /// 出力先パス
        /// </summary>
        public string ExportPath { get; set; }
    }
}
