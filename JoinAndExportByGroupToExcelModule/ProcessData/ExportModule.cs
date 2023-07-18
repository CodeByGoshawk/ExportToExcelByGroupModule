using ClosedXML.Excel;
using JoinAndExportByGroupToExcelModule.Models;

namespace JoinAndExportToExcelModule.ProcessData;

public class ExportModule
{
    IEnumerable<dynamic> _dataTable;
    string? _directory;
    string _exportFormat = "xlsx";

    public ExportModule(IEnumerable<dynamic> dataTable, string? directory)
    {
        _dataTable = dataTable;
        _directory = directory; ;
    }
    public ExportModule(IEnumerable<dynamic> dataTable, string? directory, string exportFormat)
    {
        _dataTable = dataTable;
        _directory = directory;
        _exportFormat = exportFormat;
    }

    private Response ValidateData()
    {
        if (_dataTable is null || _dataTable.ToList().Count == 0)
            return new Response(false, "Error : Table in null or empty\n");

        if (_directory is null || _directory.Length == 0)
            return new Response(false, "Error : Directory string in null or empty\n");

        if (_exportFormat is null ||
            !((_exportFormat == "xlsx") || (_exportFormat == "xlsm") || (_exportFormat == "xltx") || (_exportFormat == "xltm")))
            return new Response(false, "Error : ExportFromat is wrong\n");

        return new Response(true, "");
    }

    public Response ExportData()
    {
        var validateDataResult = ValidateData();
        if (!validateDataResult.IsSuccessfull) return validateDataResult;

        var listedDataTable = _dataTable.ToList();
        var dataTableProperties = listedDataTable[0].GetType().GetProperties();
        var groupsList = listedDataTable.DistinctBy(x => x.CreatedByCategoryId).Select(x => x.CreatedByCategoryId);

        foreach (var item in groupsList)
        {
            var categoryDataTable = listedDataTable.Where(d => d.CreatedByCategoryId == item).ToList();
            var workBook = new XLWorkbook();
            var workSheet = workBook.Worksheets.Add("Sheet");
            workSheet.Columns().Width = 30;
            for (int i = 0; i < dataTableProperties.Length; i++)
            {
                workSheet.Row(1).Height = 18;
                workSheet.Cell(1, i + 1).Value = dataTableProperties[i].Name;
                workSheet.Cell(1, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                workSheet.Cell(1, i + 1).Style.Border.OutsideBorderColor = XLColor.Black;
                workSheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.Gray;
                workSheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int j = 0; j < categoryDataTable.Count; j++)
                {
                    workSheet.Row(j + 2).Height = 18;
                    workSheet.Cell(j + 2, i + 1).Value = dataTableProperties[i].GetValue(categoryDataTable[j]);
                    workSheet.Cell(j + 2, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workSheet.Cell(j + 2, i + 1).Style.Border.OutsideBorderColor = XLColor.Black;
                    workSheet.Cell(j + 2, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }
            try
            {
                workBook.SaveAs($@"{_directory}\Category-{item}.{_exportFormat}");
            }
            catch (Exception)
            {
                return new Response(false, "Error : Directory is wrong");
            }
        }
        return new Response(true, "*** Data successfully exported. ***\n");
    }
}


