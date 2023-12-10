using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities.Templates;

namespace Domain.Entities.Requests;

public class Workflow
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public Guid WorkflowTemplateId { get; private set; }

    private List<WorkflowStep> Steps { get; set; }
    public ReadOnlyCollection<WorkflowStep> ReadSteps => Steps.AsReadOnly();

    public Workflow(Guid id, string name, Guid workflowTemplateId, List<WorkflowStep> steps)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("id cannot be empty or null.", nameof(id));
        }

        if (workflowTemplateId == Guid.Empty)
        {
            throw new ArgumentException("workflowTemplateId cannot be empty or null.",
                nameof(workflowTemplateId));
        }

        ArgumentNullException.ThrowIfNull(steps);

        Id = id;
        SetName(name);
        WorkflowTemplateId = workflowTemplateId;
        Steps = steps;
    }

    public static Workflow Create(string name, WorkflowTemplate workflowTemplate)
    {
        return new Workflow(Guid.NewGuid(), name, workflowTemplate.Id, new List<WorkflowStep>());
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("The name cannot be empty or null", nameof(name));
        }

        Name = name;
    }

    public void AddStep(string name, Status status, User? user, Guid roleId, string comment)
    {
        if (user is null && roleId != Guid.Empty)
        {
            Steps.Add(WorkflowStep.Create(name, status, roleId, comment));
        }
        else if (roleId == Guid.Empty && user is not null)
        {
            Steps.Add(WorkflowStep.Create(name, status, user, comment));
        }
    }

    public void Restart()
    {
        Steps = new();
    }

    public bool IsApprove()
    {
        return ReadSteps.Any() && ReadSteps.LastOrDefault()!.Status == Status.Approve
                               && !ReadSteps.Any(step => step.Status is Status.Reject or Status.Pending);
    }

    public bool IsReject()
    {
        return ReadSteps.Any(step => step.Status == Status.Reject)
               && !ReadSteps.Any(step => step.Status is Status.Approve or Status.Pending);
    }
}