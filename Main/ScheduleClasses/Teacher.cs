using System.ComponentModel.DataAnnotations;

namespace Main.ScheduleClasses;

public class Teacher
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string FullName => Patronymic is null ? $"{Name} {Surname}" : $"{Name} {Patronymic} {Surname}";
    public string Title { get; set; }

    public override string ToString()
    {
        return FullName;
    }
}