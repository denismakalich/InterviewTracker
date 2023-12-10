namespace Domain.Entities.Templates;

public class WorkflowStepTemplate
{
    public string Name { get; private set; }
    public int Order { get; } = 0;
    public Guid? UserId { get; private set; }
    public Guid RoleId { get; private set; }

    public WorkflowStepTemplate(string name, Guid userId, Guid roleId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The name cannot be empty or null or white space.", nameof(name));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(userId));
        }

        if (roleId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty or null.", nameof(roleId));
        }

        RoleId = roleId;
        UserId = userId;
        Name = name;
    }

    public static WorkflowStepTemplate Create(string name, Guid userId, Guid roleId)
    {
        return new WorkflowStepTemplate(name, userId, roleId);
    }
}