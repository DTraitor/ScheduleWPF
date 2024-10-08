using Main;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class LessonTests
{
    private ScheduleDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _context = new ScheduleDbContext(options);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}