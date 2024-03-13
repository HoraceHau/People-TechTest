using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]

public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List()
    {
        var logs = _logService.GetAll().Select(p => new LogInfoItemViewModel
        {
            LogId = p.LogId,
            Action = p.Action,
            LogDatetime = p.LogDatetime
        });

        var model = new LogInfoViewModel
        {
            Logs = logs.OrderByDescending(x=>x.LogId).ToList()
        };
        return View(model);
    }

    [HttpGet]
    [Route("CreateLogs")]
    public bool CreateLogs(string message)
    {   
        long id = _logService.GetAll().OrderBy(x=>x.LogId).Last().LogId + 1;
        var log = new LogInfo {LogId = id, Action = message, LogDatetime = DateTime.Now + ""};
        _logService.Create(log);

        return true; 
    }
}
