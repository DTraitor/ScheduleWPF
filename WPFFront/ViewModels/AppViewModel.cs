using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Main;
using Main.ScheduleClasses;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class AppViewModel : ReactiveObject
{
    public ScheduleDbContext _context = new();

    private DateTime _selectedDate;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => this.RaiseAndSetIfChanged(ref _selectedDate, value);
    }

    private readonly ObservableAsPropertyHelper<IEnumerable<LessonViewModel>> _selectedSchedule;
    public IEnumerable<LessonViewModel> SelectedSchedule => _selectedSchedule.Value;

    public readonly IEnumerable<Teacher> Teachers;

    private string? _name;
    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private LessonType? _type;
    public LessonType? Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private string? _location;
    public string? Location
    {
        get => _location;
        set => this.RaiseAndSetIfChanged(ref _location, value);
    }

    private Teacher? _teacher;
    public Teacher? Teacher
    {
        get => _teacher;
        set => this.RaiseAndSetIfChanged(ref _teacher, value);
    }

    private DateTime? _begin;
    public DateTime? Begin
    {
        get => _begin;
        set => this.RaiseAndSetIfChanged(ref _begin, value);
    }

    private DateTime? _end;
    public DateTime? End
    {
        get => _end;
        set => this.RaiseAndSetIfChanged(ref _end, value);
    }

    private DateTime? _beginTime;
    public DateTime? BeginTime
    {
        get => _beginTime;
        set => this.RaiseAndSetIfChanged(ref _beginTime, value);
    }

    private DateTime? _endTime;
    public DateTime? EndTime
    {
        get => _endTime;
        set => this.RaiseAndSetIfChanged(ref _endTime, value);
    }

    private WeekNumber? _selectedWeekNumber;
    public WeekNumber? SelectedWeekNumber
    {
        get => _selectedWeekNumber;
        set => this.RaiseAndSetIfChanged(ref _selectedWeekNumber, value);
    }

    private DayOfWeek? _dayOfWeek;
    public DayOfWeek? DayOfWeek
    {
        get => _dayOfWeek;
        set => this.RaiseAndSetIfChanged(ref _dayOfWeek, value);
    }

    public AppViewModel()
    {
        _context.Teachers.Load();
        Teachers = _context.Teachers.Local.ToObservableCollection();
        _selectedSchedule = this
            .WhenAnyValue(x => x.SelectedDate)
            .Throttle(TimeSpan.FromMilliseconds(800))
            .SelectMany(GetSchedule)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, x => x.SelectedSchedule);

        AddNewLesson = ReactiveCommand.CreateFromTask(
            async () =>
        {
            if (_name == null || _type == null || _description == null || _location == null || _teacher == null || _begin == null || _end == null || _beginTime == null || _endTime == null || _selectedWeekNumber == null || _dayOfWeek == null)
            {
                return;
            }

            var lesson = new Lesson
            {
                Name = _name,
                LType = _type.Value,
                Description = _description,
                Location = _location,
                TeacherId = _teacher.Id,
                BeginDate = _begin.Value,
                EndDate = _end.Value,
                BeginTime = _beginTime.Value.TimeOfDay,
                EndTime = _endTime.Value.TimeOfDay,
                WeekNumber = _selectedWeekNumber.Value,
                DayOfWeek = _dayOfWeek.Value
            };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
        });

        _selectedSchedule.ThrownExceptions.Subscribe(ex =>
        {
            Debug.WriteLine(ex);
        });

        SelectedDate = DateTime.Now;
    }

    private async Task<IEnumerable<LessonViewModel>> GetSchedule(DateTime date)
    {
        int yearToUse = date.Month < 9 ? date.Year - 1 : date.Year;
        int weekNumberInt = (new DateTime(yearToUse, 9, 1) - date).Days / 7;
        WeekNumber currentWeekNum = weekNumberInt % 2 == 0 ? Main.ScheduleClasses.WeekNumber.First : Main.ScheduleClasses.WeekNumber.Second;
        return await _context.Lessons
            .Where(x => date >= x.BeginDate && date <= x.EndDate && x.WeekNumber == currentWeekNum && x.DayOfWeek == date.DayOfWeek)
            .Include(l => l.Teacher)
            .Select(x => new LessonViewModel(x, _context))
            .ToListAsync();
    }

    public ReactiveCommand<Unit, Unit> AddNewLesson { get; }
}