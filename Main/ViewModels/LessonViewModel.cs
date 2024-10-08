using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Main.Models;
using ReactiveUI;

namespace Main.ViewModels;

public class LessonViewModel : ReactiveObject
{
    private Lesson? _lesson;
    private readonly ScheduleDbContext _context;
    private SemaphoreSlim _semaphore;

    public LessonViewModel(Lesson lesson, ScheduleDbContext context, SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
        _context = context;
        _lesson = lesson;

        DeleteLesson = ReactiveCommand.CreateFromTask(async () =>
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Lessons.Remove(_lesson);
            }
            finally
            {
                _semaphore.Release();
            }

            await _semaphore.WaitAsync();
            try
            {
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        });
    }

    public string Name => _lesson.Name;
    public string Description => _lesson.Description;
    public string Location => _lesson.Location;
    public string TeacherName => _lesson.Teacher.FullName;
    public string LessonTime => $"{_lesson.BeginTime:hh\\:mm} - {_lesson.EndTime:hh\\:mm}";
    public string LessonType => _lesson.LType.ToString();

    public ReactiveCommand<Unit, Unit> DeleteLesson { get; }
}