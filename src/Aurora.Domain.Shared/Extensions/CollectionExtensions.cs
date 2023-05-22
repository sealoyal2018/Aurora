using Aurora.Domain.Shared.Attributes;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Aurora.Domain.Shared.Extensions;
public static class CollectionExtensions {

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection != null && collection.Any();
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection is null || !collection.Any();
    }


    /// <summary>
    /// excel export
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="savePath"></param>
    /// <param name="title"></param>
    /// <param name="isOpenNo"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static async Task ExportExcel<T>(this IEnumerable<T> collection, string savePath, string title, bool isOpenNo, params string[] columnNames) {
        IWorkbook workbook = new HSSFWorkbook();

        var sheet = workbook.CreateSheet(title);
        var properties = typeof(T).GetProperties();

        sheet.DefaultColumnWidth = 20;

        var sheetName = typeof(T).Name;
        var validProperties = properties.Where(prop => prop.GetCustomAttributes<ExcelSheetAttribute>().Any(t => t.Sheet == sheetName)).ToList();

        if (columnNames.Length > 0) {
            validProperties = validProperties.Where(t => columnNames.Contains(t.Name)).ToList();
        }
        
        
        var rowIndex = 0;

        var titleRow = sheet.CreateRow(rowIndex++);

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 22;
        titleStyle.Alignment = HorizontalAlignment.Center;
        titleStyle.VerticalAlignment = VerticalAlignment.Center;
        titleStyle.SetFont(titleFont);
        titleStyle.BorderBottom = BorderStyle.Thin;
        titleStyle.BorderLeft = BorderStyle.Thin;
        titleStyle.BorderRight = BorderStyle.Thin;
        titleStyle.BorderTop = BorderStyle.Thin;

        var cell = titleRow.CreateCell(0);
        cell.SetCellValue(title);
        cell.CellStyle = titleStyle;
        titleRow.Height = 47*20;
        var region = new CellRangeAddress(0, 0, 0, validProperties.Count);
        _ = sheet.AddMergedRegion(region);

        var headRow = sheet.CreateRow(rowIndex++);
        var headStyle = workbook.CreateCellStyle();
        var headFont = workbook.CreateFont();
        headFont.IsBold = true;
        headFont.FontHeightInPoints = 12;
        headStyle.SetFont(headFont);
        headStyle.Alignment = HorizontalAlignment.Center;
        headStyle.VerticalAlignment = VerticalAlignment.Center;
        headStyle.WrapText = true;
        headRow.Height = 100 * 20;
        headStyle.BorderBottom = BorderStyle.Thin;
        headStyle.BorderLeft = BorderStyle.Thin;
        headStyle.BorderRight = BorderStyle.Thin;
        headStyle.BorderTop = BorderStyle.Thin;
        var columnOffset = isOpenNo ? -1 : 0;
        foreach (var cellIndex in Enumerable.Range(0, validProperties.Count + (-1 * columnOffset))) {
            cell = headRow.CreateCell(cellIndex);
            cell.CellStyle = headStyle;
            if (cellIndex == 0 && isOpenNo) {
                cell.SetCellValue("序号");
                continue;
            }
            var prop = validProperties[cellIndex + columnOffset];
            var attr = prop.GetCustomAttributes<ExcelSheetAttribute>(true)
               .FirstOrDefault(t => t.Sheet == typeof(T).Name);

            if (attr is null) {
                continue;
            }

            cell.SetCellValue(attr.ColumnName ?? prop.Name);
        }

        var bodyStyle = workbook.CreateCellStyle();
        bodyStyle.BorderBottom = BorderStyle.Thin;
        bodyStyle.BorderLeft = BorderStyle.Thin;
        bodyStyle.BorderRight = BorderStyle.Thin;
        bodyStyle.BorderTop = BorderStyle.Thin;

        if(collection.IsNotNullOrEmpty()) {
            foreach (var item in collection) {
                var row = sheet.CreateRow(rowIndex++);
                row.Height = 25 * 20;
                foreach (var cellIndex in Enumerable.Range(0, validProperties.Count + (-1 * columnOffset))) {
                    cell = row.CreateCell(cellIndex);
                    cell.CellStyle = bodyStyle;
                    if (cellIndex == 0 && isOpenNo) {
                        cell.SetCellValue(rowIndex - 2);
                        continue;
                    }
                    var prop = validProperties[cellIndex + columnOffset];

                    if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?)) {
                        var attr = prop.GetCustomAttributes<ExcelSheetAttribute>(true)
                           .FirstOrDefault(t => t.Sheet == typeof(T).Name);
                        if (attr.BooleanValues.IsNotNullOrEmpty() && bool.TryParse(prop.GetValue(item)?.ToString(), out var ret)) {
                            var values = attr.BooleanValues.Split('|');
                            Debug.Assert(values.Length == 2);
                            cell.SetCellValue(values[ret ? 0 : 1]);
                            continue;
                        }
                    }

                    cell.SetCellValue(prop.GetValue(item)?.ToString());
                }
            }

        }
        var dir = Directory.GetParent(savePath);
        if (!dir.Exists) {
            dir.Create();
        }

        using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        workbook.Write(fs, false);
        await Task.Delay(5);
    }




    private static ICell SetStyles(this ICell cell, ICellStyle style) {
        cell.CellStyle = style;
        return cell;
    }
}
