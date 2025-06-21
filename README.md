# Clean Architecture .NET 9 Project

A comprehensive implementation of Clean Architecture principles in .NET 9, featuring a TODO API that demonstrates proper separation of concerns, dependency inversion, and maintainable code structure.

## 🏗️ Architecture Overview

This project follows Uncle Bob's Clean Architecture pattern with the **Dependency Rule**: dependencies point inward toward the business logic.

```
┌─────────────────────────────────────────────────────────┐
│                    🌐 API Layer                         │
│                 (Controllers, DTOs)                     │
└──────────────────────┬──────────────────────────────────┘
                       │
┌─────────────────────────────────────────────────────────┐
│              🔧 Infrastructure Layer                    │
│        (External Services, File System, Email)         │
└──────────────────────┬──────────────────────────────────┘
                       │
┌─────────────────────────────────────────────────────────┐
│               💾 Persistence Layer                      │
│         (Database, Repositories, Data Access)          │
└──────────────────────┬──────────────────────────────────┘
                       │
┌─────────────────────────────────────────────────────────┐
│              ⚡ Application Layer                       │
│         (Use Cases, Commands, Queries, DTOs)           │
└──────────────────────┬──────────────────────────────────┘
                       │
┌─────────────────────────────────────────────────────────┐
│               💼 Domain Layer                           │
│        (Entities, Business Rules, Interfaces)          │
└─────────────────────────────────────────────────────────┘
```

## 📁 Project Structure

```
CleanArchitecture.sln
├── src/
│   ├── Core/                           # Business Logic (Inner Layers)
│   │   ├── CleanArchitecture.Domain/   # 💼 Pure Business Logic
│   │   └── CleanArchitecture.Application/ # ⚡ Use Cases & Orchestration
│   ├── Infrastructure/                 # External Concerns (Outer Layers)
│   │   ├── CleanArchitecture.Infrastructure/ # 🔧 External Services
│   │   └── CleanArchitecture.Persistence/    # 💾 Data Access
│   └── CleanArchitecture.API/          # 🌐 Entry Point
├── tests/
│   ├── CleanArchitecture.Unit/         # Unit Tests
│   └── CleanArchitecture.Integration/  # Integration Tests
└── docs/
    └── README.md
```

## 🎯 Layer Responsibilities

### 💼 Domain Layer (Core Business Logic)
**📍 Location:** `src/Core/CleanArchitecture.Domain/`
**🎯 Purpose:** Contains the heart of your business - pure business logic with zero external dependencies.

**📦 Contains:**
- **Entities:** Business objects with identity and behavior (`TodoItem`)
- **Value Objects:** Objects without identity (`Email`, `Money` - when needed)
- **Enums:** Business constants (`Priority`)
- **Repository Interfaces:** Contracts for data access (`ITodoItemRepository`)
- **Domain Events:** Events that occur within business operations
- **Domain Exceptions:** Business rule violations (`DomainException`)
- **Base Classes:** Common functionality (`BaseEntity`, `ValueObject`)

**✅ Responsibilities:**
- Enforce business rules and invariants
- Define domain entities with behavior
- Specify contracts for data access
- Raise domain events when important business events occur

**❌ What it CANNOT do:**
- Reference any external libraries (except .NET primitives)
- Know about databases, APIs, or UI
- Contain any infrastructure concerns

**📄 Example:**
```csharp
public class TodoItem : BaseEntity<int>
{
    public string Title { get; private set; }
    
    public static TodoItem Create(string title, Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item must have a title");
            
        return new TodoItem(title, priority);
    }
}
```

---

### ⚡ Application Layer (Use Cases)
**📍 Location:** `src/Core/CleanArchitecture.Application/`
**🎯 Purpose:** Orchestrates domain objects to fulfill specific use cases and application workflows.

**📦 Contains:**
- **Command Services:** Handle write operations (`ITodoItemCommandService`)
- **Query Services:** Handle read operations (`ITodoItemQueryService`)
- **DTOs:** Data transfer between layers
- **Interface Definitions:** Contracts for external services (`IEmailService`, `IFileService`)
- **Application Logic:** Coordinate domain objects for specific use cases

**✅ Responsibilities:**
- Coordinate domain entities to fulfill use cases
- Define interfaces for external dependencies
- Handle application-specific business flows
- Transform data between domain and external layers

**❌ What it CANNOT do:**
- Contain business rules (those belong in Domain)
- Know about specific external implementations
- Reference Infrastructure or Persistence layers directly

**📄 Example:**
```csharp
public class TodoItemCommandService : ITodoItemCommandService
{
    private readonly ITodoItemRepository _repository;
    
    public async Task<TodoItem> CreateTodoItemAsync(string title, Priority priority)
    {
        var todoItem = TodoItem.Create(title, priority); // Domain logic
        return await _repository.AddAsync(todoItem);     // Persistence abstraction
    }
}
```

---

### 💾 Persistence Layer (Data Access)
**📍 Location:** `src/Infrastructure/CleanArchitecture.Persistence/`
**🎯 Purpose:** Implements data access and storage concerns defined by the Application layer.

**📦 Contains:**
- **DbContext:** Database context implementation (`ApplicationDbContext`)
- **Repository Implementations:** Concrete data access (`TodoItemRepository`)
- **Entity Configurations:** Database mappings and relationships
- **Migrations:** Database schema changes
- **Unit of Work:** Transaction management (`UnitOfWork`)

**✅ Responsibilities:**
- Implement repository interfaces from Domain
- Handle database operations and transactions
- Manage data persistence and retrieval
- Configure entity mappings

**📄 Example:**
```csharp
public class TodoItemRepository : ITodoItemRepository
{
    private readonly IApplicationDbContext _context;
    
    public async Task<TodoItem> AddAsync(TodoItem todoItem)
    {
        _context.Add(todoItem);
        await _context.SaveChangesAsync();
        return todoItem;
    }
}
```

---

### 🔧 Infrastructure Layer (External Services)
**📍 Location:** `src/Infrastructure/CleanArchitecture.Infrastructure/`
**🎯 Purpose:** Implements external service interfaces defined by the Application layer.

**📦 Contains:**
- **External Service Implementations:** Email, file storage, notifications
- **Third-party Integrations:** APIs, cloud services
- **Cross-cutting Concerns:** Caching, logging, date/time providers
- **Security Services:** Authentication, authorization helpers

**✅ Responsibilities:**
- Implement external service interfaces from Application
- Handle third-party API integrations
- Provide cross-cutting concerns like caching
- Manage external dependencies

**📄 Example:**
```csharp
public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Implementation using SendGrid, SMTP, etc.
    }
}
```

---

### 🌐 API Layer (Presentation)
**📍 Location:** `src/CleanArchitecture.API/`
**🎯 Purpose:** Provides the entry point for external interactions and handles HTTP concerns.

**📦 Contains:**
- **Controllers:** API endpoints (`TodoItemsController`)
- **DTOs:** Request/Response models (`TodoItemCreateRequest`, `TodoItemResponse`)
- **Validators:** Input validation (`TodoItemCreateRequestValidator`)
- **Middleware:** Cross-cutting HTTP concerns
- **Configuration:** DI setup, pipeline configuration

**✅ Responsibilities:**
- Handle HTTP requests and responses
- Validate input from external sources
- Transform between API DTOs and Application models
- Configure dependency injection and middleware pipeline

**📄 Example:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemCommandService _commandService;
    
    [HttpPost]
    public async Task<ActionResult<TodoItemResponse>> Create([FromBody] TodoItemCreateRequest request)
    {
        var todoItem = await _commandService.CreateTodoItemAsync(request.Title, request.Priority);
        return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, MapToResponse(todoItem));
    }
}
```

## 🚀 Development Workflow: Business-First Approach

When adding new features, always start with the **Domain** and work your way outward. This ensures business logic drives technical decisions.

### Step 1: 💼 Start with Domain (Business First!)

**Begin with:** Business requirements and domain modeling

1. **Identify Business Entities**
   ```bash
   # Example: Adding a TodoList feature
   # Create: src/Core/CleanArchitecture.Domain/Entities/TodoList.cs
   ```

2. **Define Business Rules**
   ```csharp
   public class TodoList : BaseEntity<int>
   {
       public string Name { get; private set; }
       private readonly List<TodoItem> _items = new();
       
       public static TodoList Create(string name)
       {
           if (string.IsNullOrWhiteSpace(name))
               throw new DomainException("TodoList must have a name");
               
           return new TodoList(name);
       }
       
       public void AddItem(TodoItem item)
       {
           if (_items.Count >= 100)
               throw new DomainException("TodoList cannot have more than 100 items");
               
           _items.Add(item);
       }
   }
   ```

3. **Create Repository Interface**
   ```csharp
   // src/Core/CleanArchitecture.Domain/Repositories/ITodoListRepository.cs
   public interface ITodoListRepository
   {
       Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
       Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default);
   }
   ```

4. **Add Domain Events (if needed)**
   ```csharp
   // src/Core/CleanArchitecture.Domain/Events/TodoListCreatedEvent.cs
   public record TodoListCreatedEvent(int TodoListId, string Name) : IDomainEvent
   {
       public DateTime OccurredOn { get; } = DateTime.UtcNow;
   }
   ```

### Step 2: ⚡ Define Application Use Cases

**Focus on:** What the application needs to do with the domain

1. **Create Command/Query Services**
   ```csharp
   // src/Core/CleanArchitecture.Application/Commands/ITodoListCommandService.cs
   public interface ITodoListCommandService
   {
       Task<TodoList> CreateTodoListAsync(string name, CancellationToken cancellationToken = default);
       Task AddItemToListAsync(int listId, string itemTitle, CancellationToken cancellationToken = default);
   }
   ```

2. **Implement Service Logic**
   ```csharp
   // src/Core/CleanArchitecture.Application/Commands/TodoListCommandService.cs
   public class TodoListCommandService : ITodoListCommandService
   {
       private readonly ITodoListRepository _todoListRepository;
       private readonly ITodoItemRepository _todoItemRepository;
       
       public async Task<TodoList> CreateTodoListAsync(string name, CancellationToken cancellationToken = default)
       {
           var todoList = TodoList.Create(name); // Domain logic
           return await _todoListRepository.AddAsync(todoList, cancellationToken);
       }
   }
   ```

3. **Register in DI**
   ```csharp
   // src/Core/CleanArchitecture.Application/DependencyInjection.cs
   services.AddScoped<ITodoListCommandService, TodoListCommandService>();
   ```

### Step 3: 💾 Implement Data Access

**Focus on:** How to persist the domain objects

1. **Create Repository Implementation**
   ```csharp
   // src/Infrastructure/CleanArchitecture.Persistence/Repositories/TodoListRepository.cs
   public class TodoListRepository : ITodoListRepository
   {
       private readonly IApplicationDbContext _context;
       
       public async Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default)
       {
           _context.Add(todoList);
           await _context.SaveChangesAsync(cancellationToken);
           return todoList;
       }
   }
   ```

2. **Update DbContext**
   ```csharp
   // src/Infrastructure/CleanArchitecture.Persistence/Contexts/ApplicationDbContext.cs
   public IQueryable<TodoList> TodoLists => _todoLists.AsQueryable();
   ```

3. **Register in DI**
   ```csharp
   // src/Infrastructure/CleanArchitecture.Persistence/DependencyInjection.cs
   services.AddScoped<ITodoListRepository, TodoListRepository>();
   ```

### Step 4: 🔧 Add Infrastructure Services (if needed)

**Focus on:** External integrations and cross-cutting concerns

1. **Define Interface in Application**
   ```csharp
   // src/Core/CleanArchitecture.Application/Common/Interfaces/ITodoListNotificationService.cs
   public interface ITodoListNotificationService
   {
       Task NotifyListCreatedAsync(int listId, string name, CancellationToken cancellationToken = default);
   }
   ```

2. **Implement in Infrastructure**
   ```csharp
   // src/Infrastructure/CleanArchitecture.Infrastructure/Services/TodoListNotificationService.cs
   public class TodoListNotificationService : ITodoListNotificationService
   {
       private readonly IEmailService _emailService;
       
       public async Task NotifyListCreatedAsync(int listId, string name, CancellationToken cancellationToken = default)
       {
           await _emailService.SendEmailAsync("admin@example.com", "New List Created", $"List '{name}' was created");
       }
   }
   ```

### Step 5: 🌐 Create API Endpoints

**Focus on:** External interface and data contracts

1. **Create DTOs**
   ```csharp
   // src/CleanArchitecture.API/DTOs/TodoListCreateRequest.cs
   public class TodoListCreateRequest
   {
       public string Name { get; set; } = string.Empty;
   }
   
   // src/CleanArchitecture.API/DTOs/TodoListResponse.cs
   public class TodoListResponse
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
   }
   ```

2. **Create Validators**
   ```csharp
   // src/CleanArchitecture.API/Validators/TodoListCreateRequestValidator.cs
   public class TodoListCreateRequestValidator : AbstractValidator<TodoListCreateRequest>
   {
       public TodoListCreateRequestValidator()
       {
           RuleFor(x => x.Name)
               .NotEmpty()
               .Length(1, 100);
       }
   }
   ```

3. **Create Controller**
   ```csharp
   // src/CleanArchitecture.API/Controllers/TodoListsController.cs
   [ApiController]
   [Route("api/[controller]")]
   public class TodoListsController : ControllerBase
   {
       private readonly ITodoListCommandService _commandService;
       
       [HttpPost]
       public async Task<ActionResult<TodoListResponse>> Create([FromBody] TodoListCreateRequest request)
       {
           var todoList = await _commandService.CreateTodoListAsync(request.Name);
           
           var response = new TodoListResponse
           {
               Id = todoList.Id,
               Name = todoList.Name,
               CreatedAt = todoList.CreatedAt
           };
           
           return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
       }
   }
   ```

### Step 6: ✅ Add Tests

**Test from the inside out:**

1. **Domain Tests** (Most Important)
   ```csharp
   // tests/CleanArchitecture.Unit/Domain/TodoListTests.cs
   [Fact]
   public void Create_WithValidName_ShouldCreateTodoList()
   {
       // Act
       var todoList = TodoList.Create("My List");
       
       // Assert
       todoList.Name.Should().Be("My List");
   }
   
   [Fact]
   public void Create_WithEmptyName_ShouldThrowDomainException()
   {
       // Act & Assert
       Assert.Throws<DomainException>(() => TodoList.Create(""));
   }
   ```

2. **Application Tests**
   ```csharp
   // tests/CleanArchitecture.Unit/Application/TodoListCommandServiceTests.cs
   [Fact]
   public async Task CreateTodoListAsync_ValidName_ShouldCreateAndReturnTodoList()
   {
       // Arrange
       var mockRepository = new Mock<ITodoListRepository>();
       var service = new TodoListCommandService(mockRepository.Object);
       
       // Act
       var result = await service.CreateTodoListAsync("Test List");
       
       // Assert
       mockRepository.Verify(x => x.AddAsync(It.IsAny<TodoList>(), It.IsAny<CancellationToken>()), Times.Once);
   }
   ```

3. **Integration Tests**
   ```csharp
   // tests/CleanArchitecture.Integration/TodoListsControllerTests.cs
   [Fact]
   public async Task CreateTodoList_ValidRequest_ShouldReturnCreated()
   {
       // Arrange
       var request = new TodoListCreateRequest { Name = "Test List" };
       
       // Act
       var response = await _client.PostAsJsonAsync("/api/todolists", request);
       
       // Assert
       response.StatusCode.Should().Be(HttpStatusCode.Created);
   }
   ```

## 🔄 Key Development Principles

### 1. **Business First**
Always start with domain modeling. Ask "What does the business need?" before "How do we implement it?"

### 2. **Dependency Direction**
Dependencies always point inward:
- API → Application → Domain
- Infrastructure → Application → Domain
- Persistence → Application → Domain

### 3. **Interface Segregation**
Define interfaces in the layer that uses them:
- Domain defines repository interfaces
- Application defines service interfaces

### 4. **Single Responsibility**
Each layer has one reason to change:
- Domain: Business rules change
- Application: Use cases change
- Infrastructure: External services change
- API: API contracts change

### 5. **Test Strategy**
- **Domain**: 70% of tests - pure unit tests
- **Application**: 20% of tests - service tests with mocks
- **Integration**: 10% of tests - full stack tests

## 🛠️ Common Commands

```bash
# Run the application
dotnet run --project src/CleanArchitecture.API

# Run all tests
dotnet test

# Run only unit tests
dotnet test tests/CleanArchitecture.Unit

# Run only integration tests
dotnet test tests/CleanArchitecture.Integration

# Add a new package to Domain (should be rare!)
dotnet add src/Core/CleanArchitecture.Domain package PackageName

# Add a new package to Application
dotnet add src/Core/CleanArchitecture.Application package PackageName

# Build the solution
dotnet build

# Restore packages
dotnet restore
```

## 📚 Further Reading

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft's Clean Architecture Guide](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [Domain-Driven Design Patterns](https://martinfowler.com/tags/domain%20driven%20design.html)

---

## 🎯 Remember: Start with Business, End with Technology

The power of Clean Architecture lies in putting business logic at the center and building technology around it, not the other way around. Always ask "What does the business need?" before diving into implementation details.