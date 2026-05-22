using System.ComponentModel.DataAnnotations;

namespace SportSys.Database.Enums;

public enum EParticipationType
{
    [Display(Name = "Attended", ResourceType = typeof(SportSys.Database.Resources.EParticipationType))]
    Attended = 1,

    [Display(Name = "NoResponse", ResourceType = typeof(SportSys.Database.Resources.EParticipationType))]
    NoResponse = 2,

    [Display(Name = "Absent", ResourceType = typeof(SportSys.Database.Resources.EParticipationType))]
    Absent = 3,

    [Display(Name = "JointTraining", ResourceType = typeof(SportSys.Database.Resources.EParticipationType))]
    JointTraining = 4,

    [Display(Name = "AbsentOtherTraining", ResourceType = typeof(SportSys.Database.Resources.EParticipationType))]
    AbsentOtherTraining = 5
}
