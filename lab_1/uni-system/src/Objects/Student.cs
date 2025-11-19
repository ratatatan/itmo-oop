namespace UniSystem;

public class Student : Person
{
    // Name, Semester, Faculty, FieldOfStudy - теоретически изменяемые поля
    // Education - изменение вида обучения подразумевает перезаключение контракта
    // (создание "нового" студента)

    public uint Semester { get; private set; }
    public string Faculty { get; private set; }
    public string FieldOfStudy { get; private set; }
    public bool Graduated { get; private set; }
    public readonly EducationType Education;

    private readonly uint _maxSemester;

    public Student(string name, uint sem, string faculty, string fieldOfStudy, EducationType edu)
        : base(name)
    {
        Semester = sem;
        Faculty = faculty;
        FieldOfStudy = fieldOfStudy;
        Education = edu;

        switch (edu)
        {
            case EducationType.Bachelor:
                _maxSemester = 8;
                break;
            case EducationType.Master:
                _maxSemester = 4;
                break;
            case EducationType.Specialist:
                _maxSemester = 12;
                break;
            default:
                _maxSemester = 0;
                break;
        }
    }

    public void UpdateInfo(string? newName, uint? newSem, string? newFac, string? newFoF)
    {
        string updateString = "";

        if (newName != null)
        {
            updateString += $"\n- Имя: {Name} -> {newName}";
            Name = newName;
        }

        if (newSem != null)
        {
            updateString += $"\n- Семестр: {Semester} -> {newSem}";
            UpdateSemester((uint)newSem);
        }

        if (newFac != null)
        {
            updateString += $"\n- Факультет: {Faculty} -> {newFac}";
            Faculty = newFac;
        }

        if (newFoF != null)
        {
            updateString += $"\n- Специальность: {FieldOfStudy} -> {newFoF}";
            FieldOfStudy = newFoF;
        }

        if (updateString != "")
        {
            Console.WriteLine($"Студент {Name} был обновлен: {updateString}");
        }
    }

    // Оставляем эту функцию публичной для массовых обновлений базы в конце каждого семестра
    // Это позволяет нам не передавать кучу параметров в функцию UpdateInfo
    // И просто обновить семестр
    public void UpdateSemester()
    {
        try
        {
            UpdateSemester(Semester + 1);
        }
        catch
        {
            Graduated = true;
        }
    }

    private void UpdateSemester(uint sem)
    {
        if (Graduated) return;

        if (_maxSemester != 0 && sem > _maxSemester)
            throw new StudentUpdateException(
                $"Попытка изменить семестр студента {Name} на более, чем максимальный");

        Semester = sem;
    }

    public override string ToString()
    {
        return $"{Name} ({Semester} семестр)";
    }

    public enum EducationType
    {
        Unknown,
        Bachelor,
        Master,
        Specialist,
    }

    public class Builder
    {
        private string _name = "Неизвестно";
        private uint _semester = 1;
        private EducationType _educationType = EducationType.Unknown;
        private string _faculty = "Неизвестно";
        private string _fieldOfStudy = "Неизвестно";

        public Builder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Builder WithSemester(uint semester)
        {
            _semester = semester;
            return this;
        }

        public Builder WithEducationType(EducationType educationType)
        {
            _educationType = educationType;
            return this;
        }

        public Builder WithFaculty(string faculty)
        {
            _faculty = faculty;
            return this;
        }

        public Builder WithFieldOfStudy(string fieldOfStudy)
        {
            _fieldOfStudy = fieldOfStudy;
            return this;
        }

        public Student Build()
        {
            return new Student(_name, _semester, _faculty, _fieldOfStudy, _educationType);
        }
    }
}

public class StudentUpdateException : ArgumentException
{
    public StudentUpdateException()
    { }

    public StudentUpdateException(string message) : base(message)
    { }

    public StudentUpdateException(string message, Exception innerException) : base(message, innerException)
    { }
}
