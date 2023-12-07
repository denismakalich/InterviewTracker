using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.WorkflowTemplate;

public class WorkflowStepTemplate
{
    public string Name { get; private set; }
    public int Order { get; private set; } = 0;
    public Guid? UserId { get; private set; }
    public Guid RoleId { get; private set; }

    public WorkflowStepTemplate(string name, Guid userId, Guid roleId)
    {
        SetName(name);
        SetUser(userId);
        SetRole(roleId);
    }

    public static WorkflowStepTemplate Create(string name, Guid userId, Guid roleId)
    {
        return new WorkflowStepTemplate(name, userId, roleId);
    }

    [MemberNotNull(nameof(Name))]
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty or null or white space.", nameof(name));
        }

        Name = name;
    }

    private void SetOrder()
    {
        Order++;
    }

    [MemberNotNull(nameof(UserId))]
    private void SetUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(userId));
        }

        UserId = userId;
    }

    [MemberNotNull(nameof(RoleId))]
    private void SetRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(roleId));
        }

        RoleId = roleId;
    }
}