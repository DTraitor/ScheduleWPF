using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.ScheduleClasses;

public class Lesson
{
    [Key]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public LessonType LType { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public int TeacherId { get; private set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get;  private set; }
    public DateTime BeginDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TimeSpan BeginTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool WeekNumber { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
}