using System.Reactive.Linq;
using Main;
using Main.ScheduleClasses;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class AppViewModel : ReactiveObject
{
    private DateTime _selectedDate;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => this.RaiseAndSetIfChanged(ref _selectedDate, value);
    }

    private readonly ObservableAsPropertyHelper<IEnumerable<LessonViewModel>> _selectedSchedule;
    public IEnumerable<LessonViewModel> SelectedSchedule => _selectedSchedule.Value;

    public AppViewModel()
    {
        _selectedSchedule = this
            .WhenAnyValue(x => x.SelectedDate)
            .Throttle(TimeSpan.FromSeconds(3))
            .SelectMany(GetSchedule)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, x => x.SelectedSchedule);

        _selectedSchedule.ThrownExceptions.Subscribe(ex =>
        {
            //log the exception
        });

        SelectedDate = DateTime.Now;
    }

    private readonly ScheduleDbContext _context = new();
    private async Task<IEnumerable<LessonViewModel>> GetSchedule(DateTime date)
    {
        bool weekNumber = (new DateTime(date.Year, 9, 1).DayOfYear / 7 + 1) % 2 == 0;
        return await _context.Lessons
            .Where(x => date >= x.BeginDate && date <= x.EndDate && x.WeekNumber == weekNumber && x.DayOfWeek == date.DayOfWeek)
            .Include(l => l.Teacher)
            .Select(x => new LessonViewModel(x))
            .ToListAsync();
    }
}