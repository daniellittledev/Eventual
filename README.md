Eventual
=================
[Build status](https://ci.appveyor.com/project/Daniel45729/eventual/branch/master)

Eventual is a Event Sourcing framework for .NET

> PM> Install-Package [Eventual](https://www.nuget.org/packages/Eventual/)

I wanted a functional easy to get started event sourcing library for .NET so I made Eventual. 

## Rules of Eventual

1. Entities should never expose state
2. Behavious should only return events, and never throw exceptions
