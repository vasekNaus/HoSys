using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportSys.Database.Context;

public partial class SportSysDbContext(DbContextOptions<SportSysDbContext> options) : DbContext(options)
{
  public DbSet<Models.Emr.Task> Tasks => Set<Models.Emr.Task>();

  public DbSet<Models.Sport.IceRink> IceRinks => Set<Models.Sport.IceRink>();
  public DbSet<Models.Sport.Opponent> Opponents => Set<Models.Sport.Opponent>();
  public DbSet<Models.Sport.MatchType> MatchTypes => Set<Models.Sport.MatchType>();
  public DbSet<Models.Sport.Match> Matches => Set<Models.Sport.Match>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // sport schéma tabulky jsou spravovány SQL skripty, ne EF migracemi
    modelBuilder.Entity<Models.Sport.IceRink>()
      .ToTable("IceRink", "sport", t => t.ExcludeFromMigrations());

    modelBuilder.Entity<Models.Sport.Opponent>()
      .ToTable("Opponent", "sport", t => t.ExcludeFromMigrations());

    modelBuilder.Entity<Models.Sport.MatchType>()
      .ToTable("MatchType", "sport", t => t.ExcludeFromMigrations());

    modelBuilder.Entity<Models.Sport.Match>(entity =>
    {
      entity.ToTable("Match", "sport", t => t.ExcludeFromMigrations());
      // Id je nastaveno přes DEFAULT (NEXT VALUE FOR sport.SportEventSeq), EF nepoužívá IDENTITY
      entity.Property(e => e.Id).ValueGeneratedNever();
    });
  }

}