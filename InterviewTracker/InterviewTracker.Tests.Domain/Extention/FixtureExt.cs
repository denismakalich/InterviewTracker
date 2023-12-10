using System.Net.Mail;
using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;

namespace InterviewTracker.Tests.Domain.Extention;

public static class FixtureExt
{
    public static void ExecuteAllCustomizations(this IFixture fixture)
    {
        fixture.Customize<Email>(x =>
            x.FromFactory(() => new Email("test@example.com")));

        fixture.Customize<Request>(x =>
            x.FromFactory(() =>
                Request.Create(fixture.Create<User>(), fixture.Create<Document>(), fixture.Create<Workflow>())));
    }
}