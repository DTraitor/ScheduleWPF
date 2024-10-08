using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Main.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace Main.ViewModels;

public class AppViewModel : ReactiveObject
{
    private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);  // Semaphore to control access;
    public ScheduleDbContext _context = new();
    private DateTime _selectedDate = DateTime.Now;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => this.RaiseAndSetIfChanged(ref _selectedDate, value);
    }
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

    //observable list of DayScheduleViewModels
    public IEnumerable<DayScheduleViewModel> DayScheduleViewModels { get; }

    public AppViewModel()
    {
        _context.Teachers.Load();
        Teachers = _context.Teachers.Local.ToObservableCollection();

        DayScheduleViewModels = GetDayScheduleViewModels(DateTime.Now);

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

            SelectedDate = SelectedDate;
        });

        NextWeek = ReactiveCommand.Create(() =>
        {
            SelectedDate = SelectedDate.AddDays(7);
        });

        PreviousWeek = ReactiveCommand.Create(() =>
        {
            SelectedDate = SelectedDate.AddDays(-7);
        });

        SelectedDate = DateTime.Now;
    }

    private IEnumerable<DayScheduleViewModel> GetDayScheduleViewModels(DateTime date)
    {
        var dayScheduleViewModels = new List<DayScheduleViewModel>();
        foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
        {
            DateTime dateAtDayOfWeek = date.AddDays((int)day - (int)date.DayOfWeek);
            dayScheduleViewModels.Add(new DayScheduleViewModel(day, dateAtDayOfWeek, this, _context, _semaphore));
        }

        return dayScheduleViewModels;
    }

    public ReactiveCommand<Unit, Unit> AddNewLesson { get; }
    public ReactiveCommand<Unit, Unit> NextWeek { get; }
    public ReactiveCommand<Unit, Unit> PreviousWeek { get; }
}