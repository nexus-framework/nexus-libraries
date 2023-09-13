namespace Nexus.Common.UnitTests;

public class PaginatedListTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        List<int> items = new() { 1, 2, 3, 4, 5 };
        int count = 5;
        int pageIndex = 1;
        int pageSize = 3;

        // Act
        PaginatedList<int> paginatedList = new(items, count, pageIndex, pageSize);

        // Assert
        Assert.Equal(pageIndex, paginatedList.PageIndex);
        Assert.Equal(pageSize, paginatedList.PageSize);
        Assert.Equal(count, paginatedList.TotalCount);
        Assert.Equal(2, paginatedList.TotalPages); // TotalPages should be 2 for pageSize 3 and count 5
        Assert.Equal(items, paginatedList.Items);
    }

    [Theory]
    [InlineData(1, false)] // Page 1 doesn't have a previous page
    [InlineData(2, true)] // Page 2 has a previous page
    public void HasPreviousPage_ReturnsCorrectValue(int pageIndex, bool expected)
    {
        // Arrange
        List<int> items = new() { 1, 2, 3, 4, 5 };
        int count = 5;
        int pageSize = 3;
        PaginatedList<int> paginatedList = new(items, count, pageIndex, pageSize);

        // Act
        bool hasPreviousPage = paginatedList.HasPreviousPage;

        // Assert
        Assert.Equal(expected, hasPreviousPage);
    }

    [Theory]
    [InlineData(1, true)] // Page 1 has a next page
    [InlineData(2, false)] // Page 2 doesn't have a next page
    public void HasNextPage_ReturnsCorrectValue(int pageIndex, bool expected)
    {
        // Arrange
        List<int> items = new() { 1, 2, 3, 4, 5 };
        int count = 5;
        int pageSize = 3;
        PaginatedList<int> paginatedList = new(items, count, pageIndex, pageSize);

        // Act
        bool hasNextPage = paginatedList.HasNextPage;

        // Assert
        Assert.Equal(expected, hasNextPage);
    }

    [Fact]
    public void Create_ReturnsPaginatedList()
    {
        // Arrange
        IQueryable<int> items = Enumerable.Range(1, 10).AsQueryable();
        int pageIndex = 2;
        int pageSize = 3;

        // Act
        PaginatedList<int> paginatedList = PaginatedList<int>.Create(items, pageIndex, pageSize);

        // Assert
        Assert.NotNull(paginatedList);
        Assert.Equal(pageIndex, paginatedList.PageIndex);
        Assert.Equal(pageSize, paginatedList.PageSize);
        Assert.Equal(10, paginatedList.TotalCount);
        Assert.Equal(4, paginatedList.TotalPages); // TotalPages should be 4 for pageSize 3 and count 10
        Assert.Equal(new List<int> { 4, 5, 6 }, paginatedList.Items); // Verify the items on the second page
    }
}