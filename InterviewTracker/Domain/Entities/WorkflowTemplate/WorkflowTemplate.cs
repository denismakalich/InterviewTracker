using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities.Requests;

namespace Domain.Entities.WorkflowTemplate;

public class WorkflowTemplate
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    private List<WorkflowStepTemplate> Steps { get; }
    public ReadOnlyCollection<WorkflowStepTemplate>? ReadSteps => Steps.AsReadOnly();

    public WorkflowTemplate(Guid id, string name, List<WorkflowStepTemplate> steps)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("id cannot be empty or null.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(steps);

        Id = id;
        SetName(name);
        Steps = steps;
    }

    public static WorkflowTemplate Create(string name)
    {
        return new WorkflowTemplate(Guid.NewGuid(), name, new List<WorkflowStepTemplate>());
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("The name cannot be empty or null.", nameof(name));
        }

        Name = name;
    }

    public Request CreateRequest(User user, Document document)
    {
        return Request.Create(
            user,
            document,
            new Workflow(Guid.NewGuid(),
                Name,
                Id,
                Steps.Select(stepTemplate =>
                    stepTemplate.UserId is not null
                        ? WorkflowStep.Create(
                            stepTemplate.Name,
                            Status.Pending,
                            user,
                            "Add step")
                        : WorkflowStep.Create(
                            stepTemplate.Name,
                            Status.Pending,
                            stepTemplate.RoleId,
                            "Add step"
                        )).ToList()));
    }
}