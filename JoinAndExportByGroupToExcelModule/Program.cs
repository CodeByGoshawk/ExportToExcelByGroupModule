using JoinAndExportByGroupToExcelModule.Models;
using JoinAndExportToExcelModule.ProcessData;

namespace JoinAndExportByGroupToExcelModule;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("# Export Data Module #\n");
        Console.Write("Enter directory to save output : ");
        var directory = Console.ReadLine();
        Console.Write("Select export format\n\n" +
                          "* xlsx (Excel 2007+)\n" +
                          "* xlsm \n" +
                          "* xltx \n" +
                          "* xltm \n\n" +
                          ">> ");
        var userSelectedFormat = Console.ReadLine();
        var outputFormat = String.IsNullOrEmpty(userSelectedFormat) ? "" : userSelectedFormat.Trim().ToLower();
        var dataTable = QueryOutput();

        var exportModule = new ExportModule(dataTable, directory, outputFormat);
        var response = exportModule.ExportData();
        Console.WriteLine(response.Message);
    }

    static IEnumerable<object> QueryOutput()
    {
        var workDataBase1Context = new WorkDataBase1Context();
        var workDataBase2Context = new WorkDataBase2Context();
        List<Map> maps = workDataBase1Context.Maps.Take(50000).ToList();
        List<MapSource> mapSources = workDataBase2Context.MapSources.Take(50000).ToList();
        var query =
            from mapSource in mapSources
            join map in maps on mapSource.MapId equals map.Id
            select new
            {
                Id = mapSource.Id,
                TreeId = map.TreeId,
                NodeId = map.NodeId,
                CreatedByCategoryId = map.CreatedByCategoryId,
                Code = mapSource.Code
            };
        return query;
    }
}