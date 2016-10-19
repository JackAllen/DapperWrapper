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

## Tvp Extensions
```csharp
public static SqlMapper.ICustomQueryParameter ToTvp<T>(this T obj, string name)
```
```csharp
public static SqlMapper.ICustomQueryParameter ToTvp<T>(this List<T> objects, string name)
```
```csharp
public static SqlMapper.ICustomQueryParameter ToSingleTypeTvp<T>(this List<T> objects, string name, string columnName)
```

```csharp 
using Dapper.Wrapper;

public class Employee {
    public long Id { get; set; }
    
    [TvpProperty(Name = "Forename")]
    public string Firstname { get; set; }
    
    [TvpProperty]
    public string Surname { get; set; }
}
```

Example Usage
```csharp
using Dapper.Wrapper;

public long AddEmployee(long companyId, Employee employee) {
    const string Sql = "EXEC [AddEmployee] @CompanyId, @Employee"; // RETURNS EMPLOYEE ID

    var parameters = new {
        CompanyId = companyId,
        Employee = employee.ToTvp("EmployeeTvp")
    };
    
    return this.DapperWrapper.Get<long>(Sql, parameters);
}

public List<long> AddEmployees(long companyId, List<Employee> employees) {
    const string Sql = "EXEC [AddEmployees] @CompanyId, @Employees"; // RETURNS EMPLOYEE IDs

    var parameters = new {
        CompanyId = companyId,
        Employees = employees.ToTvp("EmployeeTvp")
    };
    
    return this.DapperWrapper.GetList<long>(Sql, parameters);
}

public void DeleteEmployees(long companyId, List<long> employeeIds) {
    const string Sql = "EXEC [DeleteEmployees] @CompanyId, @EmployeeIds";

    var parameters = new {
        CompanyId = companyId,
        EmployeeIds = employeeIds.ToTvp("IDArrayTvp", "ID")
    };
    
    this.DapperWrapper.Put(Sql, parameters);
}
```
