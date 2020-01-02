using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Javirs.Common.IO
{
    /// <summary>
    /// EXCEL表格
    /// </summary>
    public class ExcelSheet : ExcelBase
    {
        private DataTable _table;
        private IEnumerable<ExcelSheetContext> _contexts;
        /// <summary>
        /// EXCEL表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="contexts"></param>
        public ExcelSheet(DataTable table, IEnumerable<ExcelSheetContext> contexts = null)
        {
            this._table = table;
            this._contexts = contexts;
        }
        /// <summary>
        /// EXCEL表格
        /// </summary>
        /// <param name="datalist"></param>
        /// <param name="contexts"></param>
        public ExcelSheet(IEnumerable<object> datalist, IEnumerable<ExcelSheetContext> contexts = null)
        {
            this._table = BuildTable(datalist);
            this._contexts = contexts;
        }
        private static DataTable BuildTable(IEnumerable<object> datalist)
        {
            DataTable table = new DataTable();
            if (datalist == null || datalist.Count() <= 0)
            {
                return table;
            }
            try
            {
                Type t = datalist.First().GetType();
                var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var p in properties)
                {
                    table.Columns.Add(p.Name, p.PropertyType);
                }
                foreach (object item in datalist)
                {
                    object[] itemArray = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        itemArray[i] = properties[i].GetValue(item);
                    }
                    table.Rows.Add(itemArray);
                }
            }
            catch
            {

            }
            return table;
        }
        private ExcelSheetContext[] SheetColumns()
        {
            if (_contexts == null || _contexts.Count() <= 0)
            {
                return null;
            }
            var temp = from x in _contexts where x.Ignore == false select x;
            var columns = new List<ExcelSheetContext>();
            foreach (var item in temp)
            {
                if (_table.Columns.Contains(item.ColumnName))
                {
                    columns.Add(item);
                }
            }
            return columns.ToArray();
        }
        private ExcelSheetContext[] SortByRange(ExcelSheetContext[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    var npoi0 = array[i];
                    var npoi1 = array[j];
                    int range0 = i, range1 = j;
                    if (npoi0 != null && npoi0.Column_Index > 0)
                    {
                        range0 = npoi0.Column_Index;
                    }
                    if (npoi1 != null && npoi1.Column_Index > 0)
                    {
                        range1 = npoi1.Column_Index;
                    }
                    ExcelSheetContext temp = array[i];
                    if (range1 < range0)
                    {
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 导出EXCEL到流
        /// </summary>
        /// <param name="stream"></param>
        public override void Export(Stream stream)
        {
            var columns = SheetColumns();
            if (columns != null)
            {
                columns = SortByRange(columns.ToArray());
            }
            HSSFWorkbook hSSFWorkbook = new HSSFWorkbook();
            ISheet sheet = hSSFWorkbook.CreateSheet(string.IsNullOrEmpty(_table.TableName) ? "sheet1" : _table.TableName);
            IRow head_row = sheet.GetRow(0) ?? sheet.CreateRow(0);
            head_row.HeightInPoints = 20;
            var index = 0;
            List<int> columnWidthlist = new List<int>();
            //表头
            if (columns == null)
            {
                columns = new ExcelSheetContext[_table.Columns.Count];
                for (int i = 0; i < _table.Columns.Count; i++)
                {
                    columns[i] = new ExcelSheetContext
                    {
                        ColumnName = _table.Columns[i].ColumnName,
                        Column_Index = i,
                    };
                }
            }
            foreach (var item in columns)
            {
                string titlename = string.IsNullOrEmpty(item.Cell_Title) ? item.ColumnName : item.Cell_Title;
                if (string.IsNullOrEmpty(titlename))
                {
                    continue;
                }

                ICell cell = head_row.CreateCell(index);
                cell.SetCellValue(titlename);
                cell.CellStyle = GetTitleStyle(hSSFWorkbook);

                index++;
            }
            //单元格
            for (int j = 0; j < _table.Rows.Count; j++)
            {
                IRow row2 = sheet.GetRow(j + 1) ?? sheet.CreateRow(j + 1);
                row2.HeightInPoints = 20;

                for (int k = 0; k < columns.Length; k++)
                {
                    var col = columns[k];

                    ICell cell2 = row2.CreateCell(k);
                    cell2.CellStyle = GetCellStyle(hSSFWorkbook);
                    int num = Encoding.Default.GetByteCount(_table.Rows[j][col.ColumnName].ToString()) > Encoding.Default.GetByteCount(col.ColumnName) ?
                        Encoding.Default.GetByteCount(_table.Rows[j][col.ColumnName].ToString()) :
                        Encoding.Default.GetByteCount(col.ColumnName);

                    sheet.SetColumnWidth(k, (num + 1) * 256);
                    object value = col.Convert(_table.Rows[j][col.ColumnName]);
                    if (value == null)
                    {
                        SetCellValue(cell2, string.Empty, typeof(string));
                    }
                    else
                    {
                        SetCellValue(cell2, value, value.GetType());
                    }
                }
            }
            hSSFWorkbook.Write(stream);
            //hSSFWorkbook.Dispose();
            hSSFWorkbook.Close();
        }
    }
    /// <summary>
    /// 表格上下文对象
    /// </summary>
    public class ExcelSheetContext
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 映射表头名称
        /// </summary>
        public string Cell_Title { get; set; }
        /// <summary>
        /// EXCEL列顺序，从小到大
        /// </summary>
        public int Column_Index { get; set; }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// 转换单元格值
        /// </summary>
        public Func<object, object> Converter;

        internal object Convert(object value)
        {
            if (Converter != null)
            {
                try
                {
                    return Converter(value);
                }
                catch (Exception ex)
                {
                    return value;
                }
            }
            return value;
        }
        /// <summary>
        /// 创建上下文对象
        /// </summary>
        /// <param name="columnName">数据库表列名</param>
        /// <param name="column_title">列名</param>
        /// <param name="column_index">表格列顺序</param>
        /// <param name="ignore">是否忽略</param>
        /// <param name="converter">值转换委托</param>
        /// <returns></returns>
        public static ExcelSheetContext GetContext(string columnName, string column_title, int column_index, bool ignore = false, Func<object, object> converter = null)
        {
            var context = new ExcelSheetContext
            {
                ColumnName = columnName,
                Cell_Title = column_title,
                Column_Index = column_index,
                Ignore = ignore,
                Converter = converter
            };
            return context;
        }
    }
}
