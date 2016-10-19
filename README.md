Dapper Wrapper
==
Example Usage
```csharp
using Dapper.Wrapper;

public CompanyInformation GetCompanyInformation(long companyId) {
    const string Sql = "EXEC [GetCompanyInformation] @CompanyId";

    var parameters = new {
        CompanyId = companyId
    };
    
    return this.DapperWrapper.Get<CompanyInformation>(Sql, parameters);
}
```

Example Usage
```csharp
using Dapper.Wrapper;

public Company GetCompany(long companyId) {
    const string Sql = "EXEC [GetCompany] @CompanyId";

    var parameters = new {
        CompanyId = companyId
    };
    
    return this.DapperWrapper.Get(Sql, parameters, multi => new Company {
        Information = multi.Read<CompanyInformation>().FirstOrDefault(),
        Employees = multi.Read<Employee>().ToList()
    });
}
```

Example Usage
```csharp
using Dapper.Wrapper;

public class Employee {
    [TvpProperty]
    public long Id { get; set; }
    
    [TvpProperty(Name = "Forename")]
    public string Firstname { get; set; }
    
    [TvpProperty]
    public string Surname { get; set; }
}

public long AddEmployee(long companyId, Employee employee) {
    const string Sql = "EXEC [AddEmployee] @CompanyId, @Employee"; // RETURNS EMPLOYEE ID

    var parameters = new {
        CompanyId = companyId,
        Employee = emplyoee.ToTvp("EmployeeTvp")
    };
    
    return this.DapperWrapper.Get<long>(Sql, parameters);
}
```
