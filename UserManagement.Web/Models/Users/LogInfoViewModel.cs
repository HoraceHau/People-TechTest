namespace UserManagement.Web.Models.Users;

public class LogInfoViewModel
{
    public List<LogInfoItemViewModel> Logs { get; set; } = new();
}

public class LogInfoItemViewModel
{
    public long LogId { get; set; }
    public string? Action { get; set; }
    public string? LogDatetime { get; set; }
}
