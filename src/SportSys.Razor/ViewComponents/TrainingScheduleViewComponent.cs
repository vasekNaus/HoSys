using Microsoft.AspNetCore.Mvc;
using SportSys.Razor.Models.TrainingSchedule;

namespace SportSys.Razor.ViewComponents;

public class TrainingScheduleViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ITrainingScheduleViewModel model)
    {
        return View(TrainingScheduleComponentModel.Create(model));
    }
}
