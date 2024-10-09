using Main;
using Main.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class DayScheduleTests
{
    private ScheduleDbContext _context;
    private DayScheduleViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _context = new ScheduleDbContext(options);

        // Initialize DayScheduleViewModel
        _viewModel = new DayScheduleViewModel(DayOfWeek.Monday, DateTime.Now, new AppViewModel(_context), _context, new SemaphoreSlim(1));
    }

    [Test]
    public void TestDayOfTheWeek()
    {
        Assert.AreEqual("Monday", _viewModel.DayOfTheWeek);
    }

    [Test]
    public void TestDate()
    {
        var expectedDate = DateTime.Now.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek).Date;
        Assert.AreEqual(expectedDate, _viewModel.Date.Date);
    }

    [Test]
    public void TestSchedule()
    {
        // Assuming there are no lessons in the database, the schedule should be empty
        Assert.IsEmpty(_viewModel.Schedule);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}