using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportSys.Database.Context;

public partial class SportSysDbContext(DbContextOptions<SportSysDbContext> options) : DbContext(options)
{
  public DbSet<Models.Emr.Task> Tasks => Set<Models.Emr.Task>();


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
  }

}