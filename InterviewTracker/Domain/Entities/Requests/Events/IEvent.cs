namespace Domain.Entities.Requests.Events;

public interface IEvent
{
    public Guid Id { get; }
    public string Data { get; }
    public Guid RequestId { get; }
}