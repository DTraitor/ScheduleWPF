using Main;
using Main.Models;
using Main.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class AppTests
{
    private AppViewModel _viewModel;
    private ScheduleDbContext _context;

    [SetUp]
    public void Setup()
    {
        var inMemoryOptions = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        // Use the appropriate options based on your requirements
        _context = new ScheduleDbContext(inMemoryOptions);
        _viewModel = new AppViewModel(_context);
    }

    [Test]
    public void AddNewLesson_CreatesNewLesson()
    {
        // Arrange
        _viewModel.Name = "Test Lesson";
        _viewModel.Type = LessonType.Lecture;
        _viewModel.Description = "Test Description";
        _viewModel.Location = "Test Location";
        _viewModel.Teacher = new Teacher { Id = 1, Name = "Test Teacher" };
        _viewModel.Begin = DateTime.Now;
        _viewModel.End = DateTime.Now.AddDays(1);
        _viewModel.BeginTime = DateTime.Now;
        _viewModel.EndTime = DateTime.Now.AddHours(1);
        _viewModel.SelectedWeekNumber = WeekNumber.First;
        _viewModel.DayOfWeek = DayOfWeek.Monday;

        // Act
        _viewModel.AddNewLesson.Execute().Subscribe();

        // Assert
        Assert.AreEqual(1, _context.Lessons.Count());
    }

    [Test]
    public void NextWeek_IncrementsSelectedDateBy7Days()
    {
        // Arrange
        var initialDate = _viewModel.SelectedDate;

        // Act
        _viewModel.NextWeek.Execute().Subscribe();

        // Assert
        Assert.AreEqual(initialDate.AddDays(7), _viewModel.SelectedDate);
    }

    [Test]
    public void PreviousWeek_DecrementsSelectedDateBy7Days()
    {
        // Arrange
        var initialDate = _viewModel.SelectedDate;

        // Act
        _viewModel.PreviousWeek.Execute().Subscribe();

        // Assert
        Assert.AreEqual(initialDate.AddDays(-7), _viewModel.SelectedDate);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}