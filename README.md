# Ezy SQB (Sql Query Builder) #

> UNDER DEVELOPMENT

> A simple and small library to create SQL queries with fluent interface. No drivers, connections, ORMs - simply assembled to string.

> Only for .NET Core.


## How to Install ##
```shell
Install-Package EzySQB
```

## Example ##
```csharp
using EzySQB;
using EzySQB.Formatters;

public class Test
{
    public QueryBuilder QueryBuilder { get; set; };

    public Test()
    {
        // Inject standard statement formatter.
        QueryBuilder = new QueryBuilder(new StandardStatementFormatter());
    }

    public string Run()
    {
        return QueryBuilder
            .Select("tests")
            .Where("id = @id")
            .OrWhere("name like @name");
    }
}
```

## Documentation ##
Check full documentation here.

## Release Notes ##
Check the release notes here.