using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Requests;

public class Document
{
    public string Name { get; private set; }
    public Email Email { get; private set; } = null!;
    public DateTime Age { get; private set; }
    public int Experience { get; private set; }

    public Document(string name, Email email, DateTime age, int experience)
    {
        SetName(name);
        SetEmail(email);
        SetAge(age);
        SetExperience(experience);
    }

    public static Document Create(string name, Email email, DateTime age, int experience)
    {
        return new Document(name, email, age, experience);
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be whitespace or null.", nameof(name));
        }

        Name = name;
    }

    [MemberNotNull(nameof(Name))]
    public void SetEmail(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        Email = email;
    }

    public void SetAge(DateTime age)
    {
        if (age < DateTime.Now)
        {
            throw new ArgumentException("Age is not filled in or filled in incorrectly", nameof(age));
        }

        Age = age;
    }

    public void SetExperience(int experience)
    {
        if (experience < 0)
        {
            throw new ArgumentException("The experience is not filled out or filled out incorrectly",
                nameof(experience));
        }

        Experience = experience;
    }
}