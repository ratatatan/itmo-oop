namespace UniSystem;

public abstract class Course
{
    public uint ID { get; }
    public string Title { get; }
    public bool Graded { get; }
    public bool Offline { get; }

    private readonly List<Educator> _educators;
    private readonly List<Student> _students;

    public IReadOnlyCollection<Educator> Educators => _educators.AsReadOnly();
    public IReadOnlyCollection<Student> Students => _students.AsReadOnly();

    private static uint _idCounter = 0;

    protected Course(
        string title,
        bool graded,
        bool offline,
        IEnumerable<Educator>? educators = null,
        IEnumerable<Student>? students = null)
    {
        ID = _idCounter++;
        Title = title;
        Graded = graded;
        Offline = offline;

        _educators = educators?.ToList() ?? new List<Educator>();
        _students = students?.ToList() ?? new List<Student>();
    }

    public void AssignStudent(Student student)
    {
        if (!_students.Contains(student))
        {
            _students.Add(student);
            Console.WriteLine($"Студент {student.Name} был зачислен на курс \"{Title}\"");
        }
    }

    public void AssignStudents(IEnumerable<Student> students)
    {
        foreach (var student in students)
            AssignStudent(student);
    }

    public void AssignEducator(Educator educator)
    {
        if (!_educators.Contains(educator))
        {
            _educators.Add(educator);
            Console.WriteLine($"Преподаватель {educator.Name} был назначен на курс \"{Title}\"");
        }
    }

    public void AssignEducators(IEnumerable<Educator> educators)
    {
        foreach (var educator in educators)
            AssignEducator(educator);
    }

    public override string ToString()
    {
        var type = Offline ? "Очный" : "Онлайн";
        return $"{type} курс #{ID}: {Title} (Преподаватели: {Educators.Count}, Студенты: {Students.Count})";
    }

    // ====== Builder для базового курса ======
    public class Builder
    {
        private string _title = "Неизвестно";
        private bool _graded = false;
        private bool _offline = false;

        private readonly List<Educator> _professors = new();
        private readonly List<Student> _students = new();

        // специальные поля для онлайн/офлайн
        private string _classroom = "Аудитория 101";
        private string _platform = "Moodle";
        private string _meetingLink = "https://example.com/meet";

        public Builder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public Builder WithGraded(bool graded)
        {
            _graded = graded;
            return this;
        }

        public Builder WithOffline(bool offline)
        {
            _offline = offline;
            return this;
        }

        public Builder WithProfessors(IEnumerable<Educator> professors)
        {
            _professors.Clear();
            _professors.AddRange(professors);
            return this;
        }

        public Builder AddProfessor(Educator educator)
        {
            _professors.Add(educator);
            return this;
        }

        public Builder WithStudents(IEnumerable<Student> students)
        {
            _students.Clear();
            _students.AddRange(students);
            return this;
        }

        public Builder AddStudent(Student student)
        {
            _students.Add(student);
            return this;
        }

        // === новые методы ===

        public Builder WithClassroom(string classroom)
        {
            _classroom = classroom;
            return this;
        }

        public Builder WithPlatform(string platform)
        {
            _platform = platform;
            return this;
        }

        public Builder WithMeetingLink(string meetingLink)
        {
            _meetingLink = meetingLink;
            return this;
        }

        public Course Build()
        {
            if (_offline)
            {
                return new OfflineCourse(
                    _title,
                    _graded,
                    _professors,
                    _students,
                    _classroom
                );
            }
            else
            {
                return new OnlineCourse(
                    _title,
                    _graded,
                    _professors,
                    _students,
                    _platform,
                    _meetingLink
                );
            }
        }
    }
}

// Онлайн‑курс
public class OnlineCourse : Course
{
    public string Platform { get; }
    public string MeetingLink { get; }

    public OnlineCourse(
        string title,
        bool graded,
        IEnumerable<Educator>? professors,
        IEnumerable<Student>? students,
        string platform,
        string meetingLink)
        : base(title, graded, offline: false, educators: professors, students: students)
    {
        Platform = platform;
        MeetingLink = meetingLink;
    }

    public override string ToString()
    {
        return base.ToString() + $", Платформа: {Platform}, Ссылка: {MeetingLink}";
    }
}

// Офлайн‑курс
public class OfflineCourse : Course
{
    public string Classroom { get; }

    public OfflineCourse(
        string title,
        bool graded,
        IEnumerable<Educator>? professors,
        IEnumerable<Student>? students,
        string classroom)
        : base(title, graded, offline: true, educators: professors, students: students)
    {
        Classroom = classroom;
    }

    public override string ToString()
    {
        return base.ToString() + $", Аудитория: {Classroom}";
    }
}
