namespace UniSystem;

public sealed class CourseSystem
{
    private static readonly Lazy<CourseSystem> _instance =
        new(() => new CourseSystem());

    public static CourseSystem Instance => _instance.Value;

    private readonly List<Course> _courses = new();

    private CourseSystem() { }

    public IReadOnlyCollection<Course> Schedule => _courses.AsReadOnly();

    public void AddCourse(Course course)
    {
        if (!_courses.Contains(course))
            _courses.Add(course);
    }

    public void RemoveCourse(Course course)
    {
        _courses.Remove(course);
    }

    public Course[] GetCoursesByEducator(Educator educator)
    {
        return _courses
            .Where(c => c.Educators.Contains(educator))
            .ToArray();
    }

    public Student[] GetStudentsByCourse(Course course)
    {
        if (!_courses.Contains(course))
            throw new InvalidOperationException("Course is not in system.");

        return course.Students.ToArray();
    }

    public void AssignEducatorToCourse(Educator educator, Course course)
    {
        if (!_courses.Contains(course))
            throw new InvalidOperationException("Course is not in system.");

        course.AssignEducator(educator);
    }

    public void EnrollStudentToCourse(Student student, Course course)
    {
        if (!_courses.Contains(course))
            throw new InvalidOperationException("Course is not in system.");

        course.AssignStudent(student);
    }
}
