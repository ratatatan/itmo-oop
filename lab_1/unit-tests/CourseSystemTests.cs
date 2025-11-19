using static UniSystem.Student;
using static UniSystem.Educator;
namespace UniSystem.Tests;

public class CourseSystemTests
{
    [Fact]
    public void AddCourse_AddsCourseToSchedule()
    {
        var system = CourseSystem.Instance;
        var course = new Course.Builder()
            .WithTitle("Теория вероятностей")
            .WithOffline(false)
            .Build();

        system.AddCourse(course);

        Assert.Contains(course, system.Schedule);
    }

    [Fact]
    public void RemoveCourse_RemovesCourseFromSchedule()
    {
        var system = CourseSystem.Instance;
        var course = new Course.Builder()
            .WithTitle("Алгебра")
            .Build();

        system.AddCourse(course);
        system.RemoveCourse(course);

        Assert.DoesNotContain(course, system.Schedule);
    }

    [Fact]
    public void AssignEducatorToCourse_AddsEducatorToCourse()
    {
        var system = CourseSystem.Instance;

        var educator = new Educator.Builder()
            .WithName("Иванов")
            .WithRank(AcademicRank.Professor)
            .WithFieldOfStudy("Математика")
            .Build();

        var course = new Course.Builder()
            .WithTitle("Матанализ")
            .Build();

        system.AddCourse(course);
        system.AssignEducatorToCourse(educator, course);

        Assert.Contains(educator, course.Educators);
    }

    [Fact]
    public void GetCoursesByEducator_ReturnsOnlyCoursesOfEducator()
    {
        var system = CourseSystem.Instance;

        var educator = new Educator.Builder()
            .WithName("Петров")
            .WithRank(AcademicRank.Docent)
            .WithFieldOfStudy("Информатика")
            .Build();

        var course1 = new Course.Builder()
            .WithTitle("ООП")
            .Build();

        var course2 = new Course.Builder()
            .WithTitle("Базы данных")
            .Build();

        system.AddCourse(course1);
        system.AddCourse(course2);

        system.AssignEducatorToCourse(educator, course1);

        var result = system.GetCoursesByEducator(educator);

        Assert.Contains(course1, result);
        Assert.DoesNotContain(course2, result);
    }

    [Fact]
    public void GetStudentsByCourse_ReturnsStudents()
    {
        var system = CourseSystem.Instance;

        var student = new Student.Builder()
            .WithName("Студент1")
            .WithEducationType(EducationType.Bachelor)
            .Build();

        var course = new Course.Builder()
            .WithTitle("Физика")
            .Build();

        system.AddCourse(course);
        system.EnrollStudentToCourse(student, course);

        var students = system.GetStudentsByCourse(course);

        Assert.Contains(student, students);
    }

    [Fact]
    public void GetStudentsByCourse_ThrowsIfCourseNotInSystem()
    {
        var system = CourseSystem.Instance;

        var course = new Course.Builder()
            .WithTitle("Неизвестный курс")
            .Build();

        Assert.Throws<InvalidOperationException>(() => system.GetStudentsByCourse(course));
    }

    // ===== новые тесты на билдер =====

    [Fact]
    public void Builder_CreatesOnlineCourseWithPlatformAndLink()
    {
        var course = new Course.Builder()
            .WithTitle("Алгебра онлайн")
            .WithGraded(true)
            .WithOffline(false)
            .WithPlatform("Teams")
            .WithMeetingLink("https://teams.microsoft.com/xyz")
            .Build();

        var online = Assert.IsType<OnlineCourse>(course);
        Assert.Equal("Алгебра онлайн", online.Title);
        Assert.True(online.Graded);
        Assert.False(online.Offline);
        Assert.Equal("Teams", online.Platform);
        Assert.Equal("https://teams.microsoft.com/xyz", online.MeetingLink);
    }

    [Fact]
    public void Builder_CreatesOfflineCourseWithClassroom()
    {
        var course = new Course.Builder()
            .WithTitle("Физика очно")
            .WithGraded(false)
            .WithOffline(true)
            .WithClassroom("Аудитория 305")
            .Build();

        var offline = Assert.IsType<OfflineCourse>(course);
        Assert.Equal("Физика очно", offline.Title);
        Assert.False(offline.Graded);
        Assert.True(offline.Offline);
        Assert.Equal("Аудитория 305", offline.Classroom);
    }
}
