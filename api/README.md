# Centeva Architecture Template

This is a .NET solution template for illustrating some architecture concepts
that should result in cleaner, more testable, and more maintainable code.

This is an implementation of Clean Architecture principles. You can learn more
about Clean Architecture here:

- [The Clean Architecture by Robert C.
  Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
  (blog post)
- [Clean Architecture with .NET Core: Getting Started by Jason
  Taylor](https://jasontaylor.dev/clean-architecture-getting-started/) (blog
  post)
- [Common web application
  architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
  (Microsoft Docs)
- [Clean Architecture: Patterns, Practices, and Principles by Matthew
  Renze](https://app.pluralsight.com/library/courses/clean-architecture-patterns-practices-principles)
  (Pluralsight course)

## Technologies

- [ASP.NET
  Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [Docker](https://www.docker.com/)
- [ProblemDetails middleware](https://github.com/khellang/Middleware) - maps
  custom exceptions to Problem Details JSON format (see also
  <https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-6.0#produce-a-problemdetails-payload-for-exceptions>)
- [FluentValidation](https://docs.fluentvalidation.net/) - specify .NET API
  model validation rules
- [Fluent Assertions](https://fluentassertions.com/) - extension methods for
  better .NET test readability

## Architecture Overview

The objective of the layered project structure described here is to separate
concerns through via the Dependency Rule.  This rule states that code in a given
layer can only reference code in the same layer or in a layer below it:

WebApi => BusinessLogic => Core

You are encouraged to organize code within each layer into folders by feature or
area of concern.  This will help you to keep related code together and follows
the [Screaming
Architecture](https://blog.cleancoder.com/uncle-bob/2011/09/30/Screaming-Architecture.html)
principle, which states that the structure of your code should reflect the most
important aspects of your application.

This method of layering encourages a "domain-centric" approach to application
design, in which the domain model is the most important part of the application,
as opposed to a "database-centric" approach, in which the database is the most
important part of the application.

This template uses Domain Driven Design (DDD) tactical patterns to guide the
design of your application.  See the
[Centeva.DomainModeling](https://github.com/Centeva/Centeva.DomainModeling) for
a description of these patterns.  Note that DDD is not required to use this
template, but it is encouraged.

### Core

This project contains classes and logic for implenting your Domain Model:

- Aggregates (Entities and Value Objects)
- Interfaces
- Exceptions
- Domain Services
- Domain Events and Handlers
- Specifications

There should be very few external dependencies in this project.

Place each aggregate into a folder at the top level of this project.  You can
organize components into subfolders (Events, Handlers, Specifications) if needed
to reduce clutter.  Place domain services, which commonly orchestrate between
multiple aggregates, into the Services folder, creating a corresponding
interface in the Interfaces folder so that you can configure Dependency
Injection in your outer layers.

It is encouraged to enforce your business rules at the lowest possible level, in
order to ensure that your aggregates will always be valid.  You can use
techniques such as constructors, private setters, encapsulated collections, and
public methods.  Avoid creating an anemic domain model in which your entities
only have public getters and setters and no logic.

- [Centeva.DomainModeling
  package](https://github.com/Centeva/Centeva.DomainModeling)
- [Domain Modeling - Anemic
  Models](https://ardalis.com/domain-modeling-anemic-models/)
- [Domain Modeling -
  Encapsulation](https://ardalis.com/domain-modeling-encapsulation/)
- [Aggregates](https://ardalis.com/aggregate-responsibility-design/)
- [Value
  Objects](https://enterprisecraftsmanship.com/posts/value-objects-explained/)

### BusinessLogic

This project contains your application's use cases and is dependent only upon
the Core layer. This project also defines interfaces that will be implemented by
outer layers. It should not reference packages or code that is specific to the
details of your user interface or external services.

- Organize use cases into folders by feature at the top level of the project.
  This ensures that files that change together are in the same place.  If you
  have code that is shared between several features, or not directly related to
  a feature, place it in the Common folder.

- The [MediatR](https://github.com/jbogard/MediatR) library is used to define
  requests (Queries and Commands) and their respective handlers. Everything that
  the user can do in your application should be one or the other.

- Keep your handlers simple and focus on the business logic and persistence of
  data. If you need to trigger other actions based on a command, then register
  Domain Events from within your entities, or create a Domain Service from which
  you dispatch Domain Events directly.

- Try to write your Command handlers such that they have no return value, or
  only return metadata about the request, such as the identifier of a newly
  created entity, or information about success or failure.

- Data access should be through `IReadRepository<T>` and `IRepository<T>`. To
  encapsulate custom queries of entities, use the
  [Specification](https://ardalis.github.io/Specification/) pattern.

### WebApi

This project is the ASP.NET Core Web API application. It has dependencies on the
BusinessLogic and Infrastructure layers. The Infrastructure reference is for
configuring dependency injection, which should only be done in the `Program.cs`
file or separate classes that are referenced by it.

Avoid building any application logic in this project. Only logic specific to the
web (such as producing HTTP status codes and configuring authentication) should
be here.

Entities defined in the Core project should not be referenced.  Define DTO
classes in the BusinessLogic project.

- [Web API DTO Considerations](https://ardalis.com/web-api-dto-considerations/)

### Infrastructure

This project provides implementations of the interfaces defined in the inner
layers (Core and BusinessLogic.) This contains classes that use external
resources, such as the database, filesystem, other web services, SMTP, message
queues, system clock, etc.

If you need to use raw SQL in your application (for example, for complex
reporting needs), create a Domain Service in your Core project and implement it
in this project.

## Tests

You can organize tests based on the project they're testing, or by the kind of
test.  This template uses the latter approach, with projects for Unit and
Functional tests.

### Unit Tests

Unit tests should be fast and test a single class or method.  They should not
have external dependencies, such as a database or web service.  They should not
require a running application.

### Functional Tests

Functional tests should test the application as a whole from the perspective of
a user or client application, in this case by exercising the API endpoints
defined in the WebApi project.  

The functional tests set up here use the ASP.NET Core `TestServer` class to
create an in-memory instance of the application.  This allows you to test the
application without having to deploy it to a server.  The `TestServer` class
will automatically start up the application and shut it down when the tests are
complete.  It uses an in-memory date store rather than a SQL database.

## Getting Started

This is a template, and you will need to make changes as you build your own
application.  Existing Entities, Controllers, requests, and tests are samples
and should be removed.

### Docker Services

You can start up external service dependencies using Docker:

```pwsh
docker-compose up -d
```

The template should already be configured to use these services. Your own
application will likely need to make changes to this, especially if you're using
a different database engine.

### Database Migrations

The `dotnet-ef` tool is used to create and run database migrations and must be
[installed as a global
tool](https://docs.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools):

```sh
dotnet tool install --global dotnet-ef
```

To add your first migration, run this command from your terminal, with your
current directory at the root of this repository:

```sh
dotnet ef migrations add "Initial" --project src/AuthSample.Infrastructure --startup-project src/AuthSample.WebApi --output-dir Persistence/Migrations
```

The `--output-dir` option can be eliminated if you have existing migrations.

To update your database by applying missing migrations:

```sh
dotnet ef database update --project src/AuthSample.Infrastructure --startup-project src/AuthSample.WebApi
```

Alternatively, you can use the [Package Manager Console
tools](https://docs.microsoft.com/en-us/ef/core/cli/powershell) within Visual
Studio to create and run migrations.

