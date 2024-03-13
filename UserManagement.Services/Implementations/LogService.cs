using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public IEnumerable<LogInfo> GetAll() => _dataAccess.GetAll<LogInfo>();
    public void Create(LogInfo log) => _dataAccess.Create(log);
}
