using Main;
using Main.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading;

namespace UnitTests;

public class LessonTests
{
    private ScheduleDbContext _context;
    private Lesson _lesson;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _context = new ScheduleDbContext(options);

        // Initialize Lesson
        _lesson = new Lesson
        {
            Name = "Test Lesson",
            Description = "Test Description",
            Location = "Test Location",
            Teacher = new Teacher
            {
                Name = "Test",
                Surname = "Teacher",
                Patronymic = "Doe"
            },
            BeginTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(10, 0, 0),
            LType = LessonType.Lecture
        };
        _context.Lessons.Add(_lesson);
    }

    [Test]
    public void TestName()
    {
        Assert.AreEqual("Test Lesson", _lesson.Name);
    }

    [Test]
    public void TestDescription()
    {
        Assert.AreEqual("Test Description", _lesson.Description);
    }

    [Test]
    public void TestLocation()
    {
        Assert.AreEqual("Test Location", _lesson.Location);
    }

    [Test]
    public void TestTeacherName()
    {
        Assert.AreEqual("Test Doe Teacher", _lesson.Teacher.FullName);
    }

    [Test]
    public void TestLessonTime()
    {
        var expectedTime = "09:00 - 10:00";
        var actualTime = $"{_lesson.BeginTime:hh\\:mm} - {_lesson.EndTime:hh\\:mm}";
        Assert.AreEqual(expectedTime, actualTime);
    }

    [Test]
    public void TestLessonType()
    {
        Assert.AreEqual(LessonType.Lecture, _lesson.LType);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}