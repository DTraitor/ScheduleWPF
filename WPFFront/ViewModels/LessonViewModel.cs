using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Main;
using Main.ScheduleClasses;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class LessonViewModel : ReactiveObject
{
    private Lesson? _lesson;
    private readonly ScheduleDbContext _context;
    public bool IsActive => _lesson != null;

    public LessonViewModel(Lesson lesson, ScheduleDbContext context)
    {
        _context = context;
        _lesson = lesson;

        DeleteLesson = ReactiveCommand.Create(() =>
        {
            _context.Lessons.Remove(_lesson);
            _context.SaveChanges();
            _lesson = null;
        });
    }

    public string Name => _lesson.Name;
    public string Description => _lesson.Description;
    public string Location => _lesson.Location;
    public string TeacherName => _lesson.Teacher.FullName;
    public string LessonTime => $"{_lesson.BeginTime} - {_lesson.EndTime}";
    public string LessonType => _lesson.LType.ToString();

    public ReactiveCommand<Unit, Unit> DeleteLesson { get; }
}