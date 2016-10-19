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
