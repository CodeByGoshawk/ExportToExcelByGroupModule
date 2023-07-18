using System;
using System.Collections.Generic;

namespace JoinAndExportByGroupToExcelModule.Models;

public partial class Map
{
    public int Id { get; set; }

    public int TreeId { get; set; }

    public int NodeId { get; set; }

    public int CreatedByCategoryId { get; set; }

    public int SubstituteNodeId { get; set; }

    public virtual ICollection<MapSource> MapSources { get; set; } = new List<MapSource>();
}
