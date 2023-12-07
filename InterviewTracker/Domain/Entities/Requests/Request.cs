using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities.Requests.Events;

namespace Domain.Entities.Requests;

public class Request
{
    public Guid Id { get; private init; }
    public User User { get; private set; }
    public Document Document { get; private set; }
    public Workflow Workflow { get; private set; }
    private List<IEvent> _events = new List<IEvent>();
    public ReadOnlyCollection<IEvent> ReadEvents => _events.AsReadOnly();

    public Request(Guid id, User user, Document document, Workflow workflow)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("id cannot be empty or null.", nameof(id));
        }

        Id = id;
        SetUser(user);
        SetDocument(document);
        SetWorkflow(workflow);
    }

    public static Request Create(User user, Document document, Workflow workflow)
    {
        var requestId = Guid.NewGuid();

        var request = new Request(requestId, user, document, workflow);

        request._events.Add(new RequestCreateEvent(Guid.NewGuid(), $"{DateTime.UtcNow}: create event added",
            requestId));

        return request;
    }

    [MemberNotNull(nameof(User))]
    public void SetUser(User? user)
    {
        ArgumentNullException.ThrowIfNull(user);

        User = user;
    }

    [MemberNotNull(nameof(Workflow))]
    public void SetWorkflow(Workflow workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        Workflow = workflow;
    }

    [MemberNotNull(nameof(Document))]
    public void SetDocument(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        Document = document;
    }

    [MemberNotNull(nameof(User))]
    public void Approve(User user)
    {
        var statusPending = Workflow?.ReadSteps.FirstOrDefault(step => step.Status == Status.Pending);

        if (statusPending == null)
        {
            throw new ArgumentException("No standby statuses found");
        }

        statusPending.SetStatus(user, Status.Approve);

        _events.Add(new RequestApproveEvent(Guid.NewGuid(), $"{DateTime.UtcNow}: approve event added", Id));
    }

    [MemberNotNull(nameof(User))]
    public void Reject(User user)
    {
        var statusPending = Workflow?.ReadSteps.FirstOrDefault(step => step.Status == Status.Pending);

        if (statusPending == null)
        {
            throw new ArgumentException("No stand by statuses found");
        }

        statusPending.SetStatus(user, Status.Reject);

        _events.Add(new RequestRejectEvent(Guid.NewGuid(), $"{DateTime.UtcNow}: reject event edded", Id));
    }

    public void Restart()
    {
        if (Workflow != null)
        {
            foreach (var step in Workflow.ReadSteps)
            {
                step.SetStatus(User, Status.Pending);
            }
        }
    }
}