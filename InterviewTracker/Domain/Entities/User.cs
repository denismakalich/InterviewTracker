using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Guid RoleId { get; private set; }

    public User(Guid id, string name, Email email, Guid roleId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("id cannot be empty or null.", nameof(name));
        }

        Id = id;
        SetName(name);
        SetEmail(email);
        Role(roleId);
    }

    public static User Create(string name, Email email, Guid roleId)
    {
        return new User(Guid.NewGuid(), name, email, roleId);
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

    [MemberNotNull(nameof(Email))]
    public void SetEmail(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        Email = email;
    }

    [MemberNotNull(nameof(RoleId))]
    public void Role(Guid roleId)
    {
        if (roleId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(roleId));
        }

        RoleId = roleId;
    }
}