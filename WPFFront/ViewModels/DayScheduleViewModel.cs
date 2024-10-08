using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Main;
using Main.ScheduleClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class DayScheduleViewModel : ReactiveObject
{
    private ScheduleDbContext _context;
    private readonly SemaphoreSlim _semaphore;
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

    public DayScheduleViewModel(DayOfWeek dayOfWeek, DateTime date, AppViewModel appViewModel, ScheduleDbContext context, SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
        _context = context;
        _dayOfWeek = dayOfWeek;
        Date = date;

        _context.ChangeTracker.StateChanged += OnEntityStateChanged;

        _schedule = this.WhenAnyValue(x => x.Date)
            .SelectMany(GetSchedule)
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, x => x.Schedule, scheduler: RxApp.MainThreadScheduler);
        _schedule.ThrownExceptions.Subscribe(ex =>
        {
            return;
        });


        _schedule.ThrownExceptions.Subscribe(ex =>
        {
            return;
        });

        appViewModel.WhenAnyValue(x => x.SelectedDate)
            .Subscribe(x => Date = x.AddDays((int)_dayOfWeek - (int)x.DayOfWeek));
    }

    private async Task<IEnumerable<LessonViewModel>> GetSchedule(DateTime date)
    {
        await _semaphore.WaitAsync();
        try
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
        finally
        {
            _semaphore.Release();
        }
    }

    private void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is Lesson)
        {
            Date = Date.AddMicroseconds(1);
        }
    }

    public void Dispose()
    {
        _context.ChangeTracker.StateChanged -= OnEntityStateChanged;
        _context.Dispose();
    }
}