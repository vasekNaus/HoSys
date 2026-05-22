using Microsoft.EntityFrameworkCore;
using Apollo.Data.Core.Extensions;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;
using MatchType = SportSys.Database.Models.sportSchema.MatchType;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SportSys.Database.Models;

namespace SportSys.Database.Context;

public class SportSysDbContext : DbContext
{
  public SportSysDbContext(DbContextOptions<SportSysDbContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Coach> Coaches { get; set; }

  public virtual DbSet<CoachRole> CoachRoles { get; set; }

  public virtual DbSet<CoachTraining> CoachTrainings { get; set; }

  public virtual DbSet<CoachTrainingEntitlement> CoachTrainingEntitlements { get; set; }

  public virtual DbSet<CoachTrainingPlan> CoachTrainingPlans { get; set; }

  public virtual DbSet<IceRink> IceRinks { get; set; }

  public virtual DbSet<Match> Matches { get; set; }

  public virtual DbSet<MatchType> MatchTypes { get; set; }

  public virtual DbSet<Opponent> Opponents { get; set; }

  public virtual DbSet<ParticipationType> ParticipationTypes { get; set; }

  public virtual DbSet<Season> Seasons { get; set; }

  public virtual DbSet<SeasonCategory> SeasonCategories { get; set; }

  public virtual DbSet<Training> Training { get; set; }

  public virtual DbSet<TrainingEntitlement> TrainingEntitlements { get; set; }

  public virtual DbSet<TrainingPhase> TrainingPhases { get; set; }

  public virtual DbSet<TrainingPlan> TrainingPlans { get; set; }

  public virtual DbSet<TrainingState> TrainingStates { get; set; }

  public virtual DbSet<TrainingType> TrainingTypes { get; set; }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    //nechceme automaticky indexy na FK
    configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));

    configurationBuilder.Conventions.Remove<TableNameFromDbSetConvention>();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Sdílená sekvence pro Training.Id a Match.Id (TPC vzor na úrovni DB)
    modelBuilder.HasSequence<int>("SportEventSeq", Schemas.Sport)
      .StartsAt(1)
      .IncrementsBy(1);

    // Veškerá konfigurace entit je v IEntityTypeConfiguration<T> třídách v Configurations/
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(SportSysDbContext).Assembly);

    modelBuilder.IdConvention();
    modelBuilder.InitDatetime2();

    //modelBuilder.AddIsValidDateTime();

  }
}