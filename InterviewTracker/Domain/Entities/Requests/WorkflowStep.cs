using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Domain.Entities.Requests;

public class WorkflowStep
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public int Order { get; private set; } = 0;
    public Status Status { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? RoleId { get; private set; }
    public string? Comment { get; private set; }

    public WorkflowStep(Guid id, string name, Status status, User? user, Guid? roleId, string? comment)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(id));
        }

        Id = id;
        SetName(name);
        SetOrder();
        Status = status;
        UserId = user is null ? Guid.Empty : user.Id;
        RoleId = roleId is null ? Guid.Empty : roleId;
        SetComment(comment);
    }

    public static WorkflowStep Create(string name, Status status, Guid roleId, string? comment)
    {
        return new WorkflowStep(Guid.NewGuid(), name, status, null, roleId, comment);
    }

    public static WorkflowStep Create(string name, Status status, User? user, string? comment)
    {
        return new WorkflowStep(Guid.NewGuid(), name, status, user, null, comment);
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty or null", nameof(name));
        }

        Name = name;
    }

    private void SetOrder()
    {
        Order++;
    }

    [MemberNotNull(nameof(Comment))]
    public void SetComment(string? comment)
    {
        if (string.IsNullOrEmpty(comment))
        {
            throw new ArgumentException("Comment cannot be empty or null.", nameof(comment));
        }

        Comment = comment;
    }

    public void SetStatus(User? user, Status status)
    {
        UserId = user!.Id;
        RoleId = user.RoleId;
        Status = status;
    }
}