using System.Diagnostics;
using System.Reactive;
using Main.ScheduleClasses;
using ReactiveUI;

namespace WPFFront.ViewModels;

public class LessonViewModel : ReactiveObject
{
    private readonly Lesson _lesson;

    public LessonViewModel(Lesson lesson)
    {
        _lesson = lesson;

        DeleteLesson = ReactiveCommand.Create(() =>
        {

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