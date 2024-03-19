# Income Tax Calculator
UK Income tax is calculated according to tax bands. Tax within each band is 
calculated based on the amount of the salary falling within a band. The total tax is 
the sum of the tax paid within all bands. Each band has an optional upper and 
mandatory lower limit and a percentage rate of tax. 

Each band takes its upper limit to be the lower limit of the next band. The tax band 
covering the upper part of the salary never has an upper limit. The 
the uppermost tax band has a tax rate of 40%; this allows tax to be capped. 

Examples of tax calculations 
For salary: 10000 p.a. 
 Salary in Band A = 5000 => Tax paid = 5000 x 0% = 0 
 Salary in Band B = 5000 => Tax paid = 5000 x 20% = 1000 
 Salary in Band C = 0 => Tax paid = 0 x 40% = 0 
 => Annual tax paid = 1000 
 
For salary: 40000 p.a. 
 Salary in Band A = 5000 => Tax paid = 5000 x 0% = 0 
 Salary in Band B = 15000 => Tax paid = 15000 x 20% = 3000 
 Salary in Band C = 20000 => Tax paid = 20000 x 40% = 8000 
 => Annual tax paid = 11000

## Backend part: 
* .NET 8, ASP.NET REST API
* 3-layer architecture with low coupled API, Domain, Persistence layers
* ORM Entity Framework (Code first with migrations, Microsoft SQL Server)
* Repository, Unit Of Work, Strategy patterns
* Automapper, Fluent Validation
* Swashbuckle OpenAPI
* Logging and Exception handling middlewares

## Frontend part:
* Angular SPA

## Testing:
* xUnit, Moq, FluentAssertions
