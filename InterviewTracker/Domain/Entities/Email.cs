﻿namespace Domain.Entities;

public class Email
{
    public string Value { get; private init; }

    public Email(string value)
    {
        if (!value.Contains('@', StringComparison.InvariantCultureIgnoreCase)
            || value[^1] == '@'
            || value[0] == '@')
        {
            throw new ArgumentException("Email value is not valid.", nameof(value));
        }

        Value = value;
    }
}