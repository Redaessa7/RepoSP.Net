# RepoSP.Net

RepoSP.Net is a lightweight .NET library designed to simplify working with **Stored Procedures** in SQL Server. By implementing the **Repository Pattern**, it abstracts the complexity of executing stored procedures, handling parameters, and mapping results. This allows developers to focus on writing clean, maintainable code without repetitive boilerplate.

## Introduction

Many enterprise applications rely on stored procedures for optimized data access and security. However, managing stored procedure calls directly can be error-prone and tedious. **RepoSP.Net** addresses this by:

- Providing a consistent API for CRUD operations.
- Simplifying parameter management.
- Supporting both synchronous and asynchronous data operations.
- Enhancing code readability and maintainability.

## Getting Started

### Requirements

- .NET 8.0+.
- Microsoft.Data.SqlClient 6.0.1
- SQL Server database with stored procedures.
- Basic knowledge of C# and repository patterns.

### Installation

You can install **RepoSP.Net** via NuGet:

**Using .NET CLI:**

```bash
 dotnet add package RepoSP.Net
```

**Using Package Manager Console:**

```powershell
 Install-Package RepoSP.Net
```

## Usage

### 1. Create a Stored Procedure Configuration

```csharp
using Microsoft.Data.SqlClient;
using RepoSP.Net.Interfaces;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CustomerStoredProcConfig : IStoredProcConfiguration<Customer>
{
    public string IdParameterName => "CustomerId";
    public string Id_Output_ParameterName => "OutputId";
    public string GetByIdProcedure => "sp_GetCustomerById";
    public string GetAllProcedure => "sp_GetAllCustomers";
    public string InsertProcedure => "sp_InsertCustomer";
    public string UpdateProcedure => "sp_UpdateCustomer";
    public string DeleteProcedure => "sp_DeleteCustomer";
    public string IsExistsByIdProcedure => "sp_IsCustomerExists";

    public void SetInsertParameters(SqlCommand command, Customer entity)
    {
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.Add(new SqlParameter("@OutputId", System.Data.SqlDbType.Int)
        { Direction = System.Data.ParameterDirection.Output });
    }

    public void SetUpdateParameters(SqlCommand command, Customer entity)
    {
        command.Parameters.AddWithValue("@CustomerId", entity.Id);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);
    }

    public Customer MapEntity(SqlDataReader reader)
    {
        return new Customer
        {
            Id = Convert.ToInt32(reader["Id"]),
            Name = reader["Name"].ToString(),
            Email = reader["Email"].ToString()
        };
    }
}
```

### 2. Initialize the Repository

```csharp
using RepoSP.Net.Services;

string connectionString = "YourConnectionStringHere";
var customerConfig = new CustomerStoredProcConfig();
var customerRepository = new StoredProcedureRepository<Customer>(connectionString, customerConfig);
```

### 3. Perform CRUD Operations

**Insert a new customer:**

```csharp
var newCustomer = new Customer { Name = "John Doe", Email = "john@example.com" };
var insertedCustomer = await customerRepository.InsertAsync(newCustomer);
Console.WriteLine($"Inserted Customer ID: {insertedCustomer?.Id}");
```

**Retrieve all customers:**

```csharp
var allCustomers = await customerRepository.GetAllAsync();
foreach (var customer in allCustomers)
{
    Console.WriteLine($"Name: {customer.Name}, Email: {customer.Email}");
}
```

**Update a customer:**

```csharp
customer.Name = "Jane Doe";
bool isUpdated = await customerRepository.UpdateAsync(customer);
Console.WriteLine(isUpdated ? "Updated successfully!" : "Update failed.");
```

**Delete a customer:**

```csharp
bool isDeleted = await customerRepository.Delete(1);
Console.WriteLine(isDeleted ? "Deleted successfully!" : "Delete failed.");
```

## Documentation

For detailed documentation, visit the [Wiki](https://github.com/Redaessa7/RepoSP.Net/wiki) section of the repository.

## Feedback and Issues

We welcome feedback and contributions! If you encounter any issues or have feature requests:

- [Open an issue](https://github.com/Redaessa7/RepoSP.Net/issues) on GitHub.
- Contact us via [Linkedin](https://www.linkedin.com/in/redaessa7)

## Contributing

Contributions are highly encouraged. To contribute:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Submit a pull request with a clear description of your changes.

For more details, refer to the [CONTRIBUTING.md](https://github.com/Redaessa7/RepoSP.Net/CONTRIBUTING.md).

## License

RepoSP.Net is licensed under the [MIT License](LICENSE).

---

Thank you for using RepoSP.Net! We hope this library makes integrating stored procedures into your .NET applications simpler and more efficient.

