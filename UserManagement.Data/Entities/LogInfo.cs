using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class LogInfo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LogId { get; set; }
    public string? Action { get; set; }
    public string? LogDatetime { get; set; } = default!;
}
