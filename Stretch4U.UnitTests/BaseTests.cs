using Microsoft.EntityFrameworkCore;
using Moq;
using Stretch4U.Data;

namespace Stretch4U.UnitTests;

public class BaseTests
{
    protected Mock<Stretch4UContext> _context;

    protected Mock<DbSet<T>> CreateMockDbSet<T>(List<T> dataList) where T : class
    {
        var data = dataList.AsQueryable();

        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>((entity) => 
        { 
            dataList.Add(entity);
            data = dataList.AsQueryable();
        });

        mockSet.Setup(m => m.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .Callback<T, CancellationToken>((entity, token) =>
            {
                dataList.Add(entity);
            })!
            .ReturnsAsync((T entity, CancellationToken token) => null!);

        mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
            .Callback<IEnumerable<T>>((entities) =>
            {
                foreach (var entity in entities)
                {
                    dataList.Remove(entity);
                }
            });

        return mockSet;
    }
}
