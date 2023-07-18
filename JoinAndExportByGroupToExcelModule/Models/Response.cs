namespace JoinAndExportByGroupToExcelModule.Models;

public class Response
{
    public string? Message { get; private set; }
    public bool IsSuccessfull { get; private set; }
    public Response(bool isSuccessfull, string? message)
    {
        Message = message;
        IsSuccessfull = isSuccessfull;
    }
}

