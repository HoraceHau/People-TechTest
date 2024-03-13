using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService 
{
    IEnumerable<LogInfo> GetAll();
    void Create(LogInfo log);
}
