using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.ScheduleClasses;

public class Lesson
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public LessonType LType { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public int TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get;  set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan BeginTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public WeekNumber WeekNumber { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}