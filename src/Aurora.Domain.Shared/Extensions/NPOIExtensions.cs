using NPOI.SS.UserModel;
using System.Data;

namespace Aurora.Domain.Shared.Extensions;

public static class NPOIExtensions {
    /// <summary>
    /// 将excel文件内容读取到DataTable数据表中
    /// </summary>
    /// <param name="sheet"> 工作表 </param>
    /// <param name="titleRow">  DataTable的列名行 </param>
    /// <returns> DataTable数据表 </returns>
    public static DataTable ReadExcelToDataTable(this ISheet sheet, int titleRowIndex=1) {
        //定义要返回的datatable对象
        var data = new DataTable();
        var titleRow = sheet.GetRow(titleRowIndex);
        //一行最后一个cell的编号 即总的列数
        int cellCount = titleRow.LastCellNum;
        for (int i = titleRow.FirstCellNum; i < titleRow.LastCellNum; ++i) {
            var cell = titleRow.GetCell(i);
            if (cell != null) {
                var cellValue = cell.StringCellValue;
                if (cellValue != null) {
                    var column = new DataColumn(cellValue);
                    data.Columns.Add(column);
                }
            }
        }
        //数据开始行(排除标题行)
        var startRow = titleRowIndex + 1;

        //最后一列的标号
        var rowCount = sheet.LastRowNum;
        for (var i = startRow; i <= rowCount; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) {
                continue; //没有数据的行默认是null　　　　　　　
            }

            var dataRow = data.NewRow();
            for (int j = row.FirstCellNum; j < cellCount; ++j) {
                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                {
                    dataRow[j] = row.GetCell(j).ToString();
                }
            }

            data.Rows.Add(dataRow);
        }

        return data;
    }
}