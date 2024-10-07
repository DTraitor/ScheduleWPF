using System.Reactive.Linq;
using Main;
using Main.ScheduleClasses;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class DayScheduleViewModel : ReactiveObject
{
    private ScheduleDbContext _context = new();
    private readonly DayOfWeek _dayOfWeek;
    public string DayOfTheWeek => Enum.GetName(_dayOfWeek);
    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref this._date, value);
    }
    private readonly ObservableAsPropertyHelper<IEnumerable<LessonViewModel>> _schedule;
    public IEnumerable<LessonViewModel> Schedule => _schedule.Value;

    public DayScheduleViewModel(DayOfWeek dayOfWeek, DateTime date)
    {
        _dayOfWeek = dayOfWeek;
        Date = date;

        _schedule = this
            .WhenAnyValue(x => x.Date)
            .Throttle(TimeSpan.FromMilliseconds(800))
            .SelectMany(GetSchedule)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, x => x.Schedule);

        _schedule.ThrownExceptions.Subscribe(ex =>
        {
            return;
        });
    }

    private async Task<IEnumerable<LessonViewModel>> GetSchedule(DateTime date)
    {
        int yearToUse = date.Month < 9 ? date.Year - 1 : date.Year;
        int weekNumberInt = (new DateTime(yearToUse, 9, 1) - date).Days / 7;
        WeekNumber currentWeekNum = weekNumberInt % 2 == 0 ? WeekNumber.First : WeekNumber.Second;
        return await _context.Lessons
            .Where(x => date >= x.BeginDate && date <= x.EndDate && x.WeekNumber == currentWeekNum && x.DayOfWeek == date.DayOfWeek)
            .Include(l => l.Teacher)
            .Select(x => new LessonViewModel(x, _context))
            .ToListAsync();
    }
}