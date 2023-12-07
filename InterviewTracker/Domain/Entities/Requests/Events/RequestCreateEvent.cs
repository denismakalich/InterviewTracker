﻿namespace Domain.Entities.Requests.Events;

public class RequestCreateEvent : IEvent
{
    public Guid Id { get; }
    public string Data { get; }
    public Guid RequestId { get; }

    public RequestCreateEvent(Guid id, string data, Guid requestId)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(requestId);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        if (requestId == Guid.Empty)
        {
            throw new ArgumentException("RequestId cannot be empty", nameof(requestId));
        }

        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or whitespace", nameof(data));
        }

        Id = id;
        Data = data;
        RequestId = requestId;
    }

    public static RequestCreateEvent Create(string data, Guid requestId)
    {
        return new RequestCreateEvent(Guid.NewGuid(), data, requestId);
    }
}