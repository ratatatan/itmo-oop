using UniSystem;
using static UniSystem.Student;
using static UniSystem.Educator;

internal class Program
{
    private static readonly List<Educator> _educators = new();
    private static readonly List<Student> _students = new();

    private static void Main(string[] args)
    {
        Initialize();

        while (true)
        {
            Console.WriteLine("\n=== кИСУ ===");
            Console.WriteLine("1. Создать курс");
            Console.WriteLine("2. Удалить курс");
            Console.WriteLine("3. Показать все курсы");
            Console.WriteLine("4. Назначить преподавателя на курс");
            Console.WriteLine("5. Зачислить студента на курс");
            Console.WriteLine("6. Курсы по преподавателю");
            Console.WriteLine("7. Студенты по курсу");
            Console.WriteLine("8. Перевести всех студентов на следующий семестр");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    CreateCourse();
                    break;
                case "2":
                    RemoveCourse();
                    break;
                case "3":
                    ShowAllCourses();
                    break;
                case "4":
                    AssignEducatorToCourse();
                    break;
                case "5":
                    EnrollStudentToCourse();
                    break;
                case "6":
                    ShowCoursesByEducator();
                    break;
                case "7":
                    ShowStudentsByCourse();
                    break;
                case "8":
                    UpdateAllStudentsSemester();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    private static void Initialize()
    {
        // Пара преподавателей
        _educators.Add(new Educator.Builder()
            .WithName("Иванов И.И.")
            .WithRank(AcademicRank.Professor)
            .WithFieldOfStudy("Математика")
            .Build());

        _educators.Add(new Educator.Builder()
            .WithName("Петров П.П.")
            .WithRank(AcademicRank.Docent)
            .WithFieldOfStudy("Информатика")
            .Build());

        // Пара студентов
        _students.Add(new Student.Builder()
            .WithName("Алексей")
            .WithEducationType(EducationType.Bachelor)
            .WithFaculty("ФКН")
            .WithFieldOfStudy("Прикладная математика")
            .Build());

        _students.Add(new Student.Builder()
            .WithName("Мария")
            .WithEducationType(EducationType.Master)
            .WithFaculty("ФКН")
            .WithFieldOfStudy("Компьютерные науки")
            .Build());
    }

    private static void CreateCourse()
    {
        Console.Write("Название курса: ");
        var title = Console.ReadLine() ?? "Без названия";

        Console.Write("Оценочный (y/N): ");
        var gradedInput = (Console.ReadLine() ?? "").ToLower();
        var graded = gradedInput == "y" || gradedInput == "д";

        Console.Write("Тип курса (1 - онлайн, 2 - офлайн): ");
        var typeInput = Console.ReadLine();
        var offline = typeInput == "2";

        var builder = new Course.Builder()
            .WithTitle(title)
            .WithGraded(graded)
            .WithOffline(offline);

        if (offline)
        {
            Console.Write("Аудитория (по умолчанию: Аудитория 101): ");
            var classroom = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(classroom))
                builder.WithClassroom(classroom);
        }
        else
        {
            Console.Write("Платформа (по умолчанию: Moodle): ");
            var platform = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(platform))
                builder.WithPlatform(platform);

            Console.Write("Ссылка на встречу (по умолчанию: https://example.com/meet): ");
            var link = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(link))
                builder.WithMeetingLink(link);
        }

        var course = builder.Build();
        CourseSystem.Instance.AddCourse(course);

        Console.WriteLine($"Создан курс: {course}");
    }


    private static void RemoveCourse()
    {
        var course = SelectCourse();
        if (course == null) return;

        CourseSystem.Instance.RemoveCourse(course);
        Console.WriteLine("Курс удалён.");
    }

    private static void ShowAllCourses()
    {
        var schedule = CourseSystem.Instance.Schedule;
        if (schedule.Count == 0)
        {
            Console.WriteLine("Курсов пока нет.");
            return;
        }

        foreach (var c in schedule)
        {
            Console.WriteLine(c);
        }
    }

    private static void AssignEducatorToCourse()
    {
        var educator = SelectEducator();
        if (educator == null) return;

        var course = SelectCourse();
        if (course == null) return;

        CourseSystem.Instance.AssignEducatorToCourse(educator, course);
    }

    private static void EnrollStudentToCourse()
    {
        var student = SelectStudent();
        if (student == null) return;

        var course = SelectCourse();
        if (course == null) return;

        CourseSystem.Instance.EnrollStudentToCourse(student, course);
    }

    private static void ShowCoursesByEducator()
    {
        var educator = SelectEducator();
        if (educator == null) return;

        var courses = CourseSystem.Instance.GetCoursesByEducator(educator);
        if (courses.Length == 0)
        {
            Console.WriteLine("У этого преподавателя пока нет курсов.");
            return;
        }

        Console.WriteLine($"Курсы преподавателя {educator.Name}:");
        foreach (var c in courses)
        {
            Console.WriteLine(c);
        }
    }

    private static void ShowStudentsByCourse()
    {
        var course = SelectCourse();
        if (course == null) return;

        var students = CourseSystem.Instance.GetStudentsByCourse(course);
        if (students.Length == 0)
        {
            Console.WriteLine("На этом курсе пока нет студентов.");
            return;
        }

        Console.WriteLine($"Студенты курса \"{course.Title}\":");
        foreach (var s in students)
        {
            Console.WriteLine(s);
        }
    }

    // Вспомогательные методы выбора сущностей

    private static Educator? SelectEducator()
    {
        if (_educators.Count == 0)
        {
            Console.WriteLine("Преподавателей нет.");
            return null;
        }

        Console.WriteLine("Преподаватели:");
        for (int i = 0; i < _educators.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_educators[i]}");
        }

        Console.Write("Выберите номер: ");
        if (int.TryParse(Console.ReadLine(), out int idx) &&
            idx > 0 && idx <= _educators.Count)
        {
            return _educators[idx - 1];
        }

        Console.WriteLine("Неверный выбор.");
        return null;
    }

    private static Student? SelectStudent()
    {
        if (_students.Count == 0)
        {
            Console.WriteLine("Студентов нет.");
            return null;
        }

        Console.WriteLine("Студенты:");
        for (int i = 0; i < _students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_students[i]}");
        }

        Console.Write("Выберите номер: ");
        if (int.TryParse(Console.ReadLine(), out int idx) &&
            idx > 0 && idx <= _students.Count)
        {
            return _students[idx - 1];
        }

        Console.WriteLine("Неверный выбор.");
        return null;
    }

    private static Course? SelectCourse()
    {
        var schedule = CourseSystem.Instance.Schedule;
        if (schedule.Count == 0)
        {
            Console.WriteLine("Курсов нет.");
            return null;
        }

        var list = new List<Course>(schedule);

        Console.WriteLine("Курсы:");
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {list[i]}");
        }

        Console.Write("Выберите номер: ");
        if (int.TryParse(Console.ReadLine(), out int idx) &&
            idx > 0 && idx <= list.Count)
        {
            return list[idx - 1];
        }

        Console.WriteLine("Неверный выбор.");
        return null;
    }

    private static void UpdateAllStudentsSemester()
    {
        if (_students.Count == 0)
        {
            Console.WriteLine("Студентов нет.");
            return;
        }

        Console.WriteLine("Переводим всех студентов на следующий семестр (где возможно)...");

        foreach (var student in _students)
        {
            student.UpdateSemester();
        }

        Console.WriteLine("Обновление семестров завершено.");
    }

}
