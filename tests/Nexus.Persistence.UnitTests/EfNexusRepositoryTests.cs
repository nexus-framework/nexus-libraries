using Microsoft.EntityFrameworkCore;
using Nexus.Common.Abstractions;
using Nexus.Persistence.Auditing;
using NSubstitute;

namespace Nexus.Persistence.UnitTests;

[ExcludeFromCodeCoverage]
public class EfNexusRepositoryTests
{
    private readonly TestDbContext _context;
    private readonly TestRepository _repository;

    public EfNexusRepositoryTests()
    {
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor = new (Substitute.For<ICurrentUserService>(),
            Substitute.For<IDateTime>());
        DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new TestDbContext(options, auditableEntitySaveChangesInterceptor);
        _repository = new TestRepository(_context);
    }

    [Fact]
    public async Task AllAsync_Returns_All_Entities()
    {
        List<TestEntity> entities = new()
            { new TestEntity{ Id = 1 }, new TestEntity{ Id = 2 } };
        foreach(TestEntity entity in entities)
        {
            _repository.Add(entity);
            await _context.SaveChangesAsync();
        }

        List<TestEntity> result = await _repository.AllAsync();
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetById_Returns_Entity_With_Given_Id()
    {
        TestEntity entity = new()
            { Id = 1 };
        _repository.Add(entity);

        TestEntity? result = _repository.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Entity_With_Given_Id()
    {
        TestEntity entity = new() { Id = 10 };
        _repository.Add(entity);
        await _context.SaveChangesAsync();
        
        TestEntity? result = await _repository.GetByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }
    
    [Fact]
    public async Task Add_Creates_New_Entity()
    {
        TestEntity entity = new() { Id = 20 };
        _repository.Add(entity);
        await _context.SaveChangesAsync();
        
        TestEntity? result = await _repository.GetByIdAsync(20);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }
    
    [Fact]
    public async Task Update_Modifies_Existing_Entity()
    {
        TestEntity entity = new () { Id = 100, Value = "V1" };
        _repository.Add(entity);
        await _context.SaveChangesAsync();
        
        TestEntity? fetchedEntity = await _repository.GetByIdAsync(100);
        Assert.NotNull(fetchedEntity);

        fetchedEntity.Value = "V1Updated";
        _repository.Update(fetchedEntity);
        await _context.SaveChangesAsync();
        
        TestEntity? result = await _repository.GetByIdAsync(100);
        Assert.NotNull(result);
        Assert.Equal("V1Updated", result.Value);
    }
    
    [Fact]
    public async Task Delete_Removes_Entity()
    {
        TestEntity entity = new() { Id = 1000 };
        _repository.Add(entity);
        await _context.SaveChangesAsync();
        
        TestEntity? fetchedEntity = await _repository.GetByIdAsync(1000);
        Assert.NotNull(fetchedEntity);
        
        _repository.Delete(fetchedEntity);
        await _context.SaveChangesAsync();

        TestEntity? result = await _repository.GetByIdAsync(1000);
        
        Assert.Null(result);
    }
}

[ExcludeFromCodeCoverage]
public class TestEntity : INexusEntity
{
    public int Id { get; set; }
    public string? Value { get; set; }
}

[ExcludeFromCodeCoverage]
public class TestDbContext : AuditableDbContext
{
    public TestDbContext(
        DbContextOptions options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options, auditableEntitySaveChangesInterceptor)
    {
    }

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();
}

[ExcludeFromCodeCoverage]
public class TestRepository : EfNexusRepository<TestEntity>
{
    public TestRepository(DbContext context) : base(context)
    {
    }
}