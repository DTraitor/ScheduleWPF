using System.ComponentModel.DataAnnotations;

namespace Main.ScheduleClasses;

public class Teacher
{
    [Key]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? Patronymic { get; private set; }
    public string FullName => Patronymic is null ? $"{Name} {Surname}" : $"{Name} {Patronymic} {Surname}";
    public string Title { get; private set; }
}