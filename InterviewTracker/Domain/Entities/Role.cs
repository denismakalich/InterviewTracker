using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

public class Role
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }

    public Role(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("id cannot be empty or null.", nameof(id));
        }

        Id = id;
        SetName(name);
    }

    public static Role Create(string name)
    {
        return new Role(Guid.NewGuid(), name);
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be null or whitespace.", nameof(name));
        }

        Name = name;
    }
}