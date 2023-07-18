using System;
using System.Collections.Generic;

namespace JoinAndExportByGroupToExcelModule.Models;

public partial class MapSource
{
    public int Id { get; set; }

    public int MapId { get; set; }

    public string? Code { get; set; }

    public virtual Map Map { get; set; } = null!;
}
