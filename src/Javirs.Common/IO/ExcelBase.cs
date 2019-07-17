using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if net40
using System.Web;
using System.Web.Mvc;
#endif
#if netstandard2_0
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#endif
namespace Javirs.Common.IO
{
    /// <summary>
    /// 导出Excel基类
    /// </summary>
    public abstract class ExcelBase
    {
        private ICellStyle _cellstyle;
        private ICellStyle _titlestyle;
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        protected static void SetCellValue(ICell cell, object obj, Type type)
        {
            if (obj == null)
            {
                cell.SetCellValue("");
                return;
            }
            if (obj.GetType() == typeof(DBNull))
            {
                cell.SetCellValue("");
                return;
            }
            if (type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                cell.SetCellValue(Convert.ToDouble(obj));
                return;
            }
            if (type == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(obj));
                return;
            }
            if (type == typeof(DateTime))
            {
                cell.SetCellValue(obj.ToString());
                return;
            }
            cell.SetCellValue(obj.ToString());
        }
       
        /// <summary>
        /// 导出Excel到流
        /// </summary>
        /// <param name="stream"></param>
        public abstract void Export(Stream stream);

#if net40

        /// <summary>
        /// 导出到Web客户端
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ExportToWebClient(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName);
            }
            bool result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Export(memoryStream);
                BinaryReader br = new BinaryReader(memoryStream);
                result = ExportExcelToWebClient(br, memoryStream.Length, fileName);
            }
            return result;
        }
         /// <summary>
        /// 导出到Web客户端，支持断点续传
        /// </summary>
        /// <param name="br"></param>
        /// <param name="fileLength"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected static bool ExportExcelToWebClient(BinaryReader br, long fileLength, string fileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            HttpRequest request = HttpContext.Current.Request;
            if (fileName.LastIndexOf(".xls") <= 0)
            {
                fileName += ".xls";
            }
            bool result;
            try
            {
                response.AddHeader("Accept-Ranges", "bytes");
                response.Buffer = false;
                long beginPosition = 0L;
                double blocksize = 10240d;//分块大小
                if (request.Headers["Range"] != null)
                {
                    response.StatusCode = 206;
                    string[] array = request.Headers["Range"].Split(new char[] { '=', '-' });
                    beginPosition = Convert.ToInt64(array[1]);
                }
                response.AddHeader("Content-Length", (fileLength - beginPosition).ToString());
                response.AddHeader("Connection", "Keep-Alive");
                response.ContentType = "application/octet-stream";
                response.AddHeader("Content-Disposition", "attachment;filename=" + fileName.GetWebEncodeFileName());
                br.BaseStream.Seek(beginPosition, SeekOrigin.Begin);
                int blockCount = (int)Math.Floor((double)(fileLength - beginPosition) / blocksize) + 1;//块数，按10M分块
                for (int i = 0; i < blockCount; i++)
                {
                    if (response.IsClientConnected)
                    {
                        response.BinaryWrite(br.ReadBytes(int.Parse(blocksize.ToString())));
                    }
                    else
                    {
                        i = blockCount;
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
#endif
        /// <summary>
        /// MVC导出到浏览器
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public FileStreamResult Export(string fileName)
        {
            MemoryStream ms = new MemoryStream();
            Export(ms);
            ms.Seek(0, SeekOrigin.Begin);
            FileStreamResult result = new FileStreamResult(ms, "application/octet-stream");
            result.FileDownloadName = fileName;
            return result;
        }

        /// <summary>
        /// 获取单元格样式
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        protected ICellStyle GetCellStyle(HSSFWorkbook book)
        {
            if (_cellstyle == null)
            {
                _cellstyle = book.CreateCellStyle();
                _cellstyle.Alignment = HorizontalAlignment.Center;
                _cellstyle.VerticalAlignment = VerticalAlignment.Center;
                IFont cellfont = book.CreateFont();
                cellfont.FontName = "宋体";
                cellfont.FontHeightInPoints = 12;
                cellfont.Color = HSSFColor.Black.Index;
                _cellstyle.SetFont(cellfont);
            }
            return _cellstyle;
        }
        /// <summary>
        /// 获取表头单元格样式
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        protected ICellStyle GetTitleStyle(HSSFWorkbook book)
        {
            if (_titlestyle == null)
            {
                _titlestyle = book.CreateCellStyle();
                _titlestyle.Alignment = HorizontalAlignment.Center;
                _titlestyle.VerticalAlignment = VerticalAlignment.Center;
                IFont font = book.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 12;
                font.Color = HSSFColor.Black.Index;
                font.Boldweight = 700;
                _titlestyle.SetFont(font);
            }
            return _titlestyle;
        }
        /// <summary>
        /// 设置表头样式
        /// </summary>
        /// <param name="style"></param>
        public void SetTitleStyle(ICellStyle style)
        {
            _titlestyle = style;
        }
        /// <summary>
        /// 设置除表头外的单元格样式
        /// </summary>
        /// <param name="style"></param>
        public void SetCellStyle(ICellStyle style)
        {
            _cellstyle = style;
        }
    }
}
