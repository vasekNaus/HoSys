using Microsoft.EntityFrameworkCore;
using Apollo.Data.Core.Extensions;
using SportSys.Database.Models.dboSchema;
using SportSys.Database.Models.sportSchema;
using SportSys.Database.Models.identity;
using MatchType = SportSys.Database.Models.sportSchema.MatchType;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SportSys.Database.Models;
using SportSys.Database.Models.dbo;
using SportSys.Database.Models.sport;

namespace SportSys.Database.Context;

public class SportSysDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
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

  public virtual DbSet<Team> Teams { get; set; }

  public virtual DbSet<ParticipationType> ParticipationTypes { get; set; }

  public virtual DbSet<Season> Seasons { get; set; }

  public virtual DbSet<SeasonCategory> SeasonCategories { get; set; }

  public virtual DbSet<Training> Training { get; set; }

  public virtual DbSet<TrainingEntitlement> TrainingEntitlements { get; set; }

  public virtual DbSet<TrainingPhase> TrainingPhases { get; set; }

  public virtual DbSet<TrainingPlan> TrainingPlans { get; set; }

  public virtual DbSet<TrainingState> TrainingStates { get; set; }

  public virtual DbSet<TrainingType> TrainingTypes { get; set; }

  //public virtual DbSet<Permission> Permissions { get; set; }

  //public virtual DbSet<BusinessRole> BusinessRoles { get; set; }

  //public virtual DbSet<BusinessRolePermission> BusinessRolePermissions { get; set; }

  //public virtual DbSet<UserBusinessRole> UserBusinessRoles { get; set; }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    //nechceme automaticky indexy na FK
    configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));

    configurationBuilder.Conventions.Remove<TableNameFromDbSetConvention>();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Identity nastaví výchozí konfigurace tabulek (AspNetUsers, AspNetRoles atd.)
    base.OnModelCreating(modelBuilder);

    //zmnena pojmenovani tabulek s daty Identity
    modelBuilder.Entity<User>().ToTable(nameof(User), Schemas.Identity);
    modelBuilder.Entity<Role>().ToTable(nameof(Role), Schemas.Identity);
    modelBuilder.Entity<UserRole>().ToTable(nameof(UserRole), Schemas.Identity);
    modelBuilder.Entity<UserClaim>().ToTable(nameof(UserClaim), Schemas.Identity);
    modelBuilder.Entity<UserLogin>().ToTable(nameof(UserLogin), Schemas.Identity);
    modelBuilder.Entity<UserToken>().ToTable(nameof(UserToken), Schemas.Identity);
    modelBuilder.Entity<RoleClaim>().ToTable(nameof(RoleClaim), Schemas.Identity);

    modelBuilder.Entity<Role>(role =>
    {
      role.HasKey(ur => ur.Id);
      role.Property(r => r.Id).ValueGeneratedNever();
    });

    ////userRole bude mít PK id Identity
    //modelBuilder.Entity<UserRole>(userRole =>
    //{
    //  userRole.HasOne(ur => ur.Role)
    //      .WithMany(r => r.UserRoles)
    //      .HasForeignKey(ur => ur.RoleId);

    //  userRole.HasOne(ur => ur.User)
    //      .WithMany(r => r.UserRoles)
    //      .HasForeignKey(ur => ur.UserId);
    //});

//    modelBuilder.Entity<UserToken>(entity =>
//    {
//      entity.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
////      entity.Property(e => e.IssueDateTime).HasDefaultValueSql("(getdate())");
//    });


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