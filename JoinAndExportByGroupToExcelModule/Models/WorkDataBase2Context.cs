using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace JoinAndExportByGroupToExcelModule.Models;

public partial class WorkDataBase2Context : DbContext
{
    public WorkDataBase2Context()
    {
    }

    public WorkDataBase2Context(DbContextOptions<WorkDataBase2Context> options)
        : base(options)
    {
    }
    private string connectionString = ConfigurationManager.ConnectionStrings["ESP-WorkDataBase2"].ConnectionString;
    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<MapSource> MapSources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MappingTemplates");

            entity.ToTable("Maps ");
        });

        modelBuilder.Entity<MapSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MappingTemplateCodes");

            entity.ToTable("MapSource");

            entity.HasIndex(e => e.MapId, "IX_MappingTemplateId");

            entity.HasOne(d => d.Map).WithMany(p => p.MapSources)
                .HasForeignKey(d => d.MapId)
                .HasConstraintName("FK_dbo.MappingTemplateCodes_dbo.MappingTemplates_MappingTemplateId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
