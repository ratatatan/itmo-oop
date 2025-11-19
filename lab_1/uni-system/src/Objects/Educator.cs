namespace UniSystem;

public class Educator : Person
{
    public AcademicRank Rank { get; private set; }
    public string FieldOfStudy { get; private set; }

    public Educator(string name, AcademicRank rank, string fof)
        : base(name)
    {
        Rank = rank;
        FieldOfStudy = fof;
    }

    public void UpdateProfessor(string? newName, AcademicRank? newRank, string? newFoF)
    {
        var updateString = "";

        if (!string.IsNullOrWhiteSpace(newName))
        {
            updateString += $"- Имя: {Name} -> {newName}\n";
            Name = newName;
        }

        if (newRank != null)
        {
            updateString += $"- Учёное звание: {Rank} -> {newRank}\n";
            Rank = newRank.Value;
        }

        if (!string.IsNullOrWhiteSpace(newFoF))
        {
            updateString += $"- Специальность: {FieldOfStudy} -> {newFoF}\n";
            FieldOfStudy = newFoF;
        }

        if (updateString != "")
        {
            Console.WriteLine($"Преподаватель {Name} был обновлен:\n{updateString}");
        }
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Rank: {Rank}, Field: {FieldOfStudy}";
    }

    public enum AcademicRank
    {
        Unknown,
        Docent,
        Professor
    }

    // Builder
    public class Builder
    {
        private string _name = "Неизвестно";
        private AcademicRank _rank = AcademicRank.Unknown;
        private string _fieldOfStudy = "Неизвестно";

        public Builder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Builder WithRank(AcademicRank rank)
        {
            _rank = rank;
            return this;
        }

        public Builder WithFieldOfStudy(string fof)
        {
            _fieldOfStudy = fof;
            return this;
        }

        public Educator Build()
        {
            return new Educator(_name, _rank, _fieldOfStudy);
        }
    }
}
