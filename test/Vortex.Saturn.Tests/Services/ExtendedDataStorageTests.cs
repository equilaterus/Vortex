using Equilaterus.Vortex.Saturn.Queries;
using Equilaterus.Vortex.Saturn.Services;
using Equilaterus.Vortex.Saturn.Tests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Services
{
    public class ExtendedDataStorageTests
    {
        [Fact]
        public async Task FindNullParams()
        {
            var ds = new Mock<IDataStorage<TestModel>>();

            await ds.Object.FindAsync(queryParams: null);

            ds.Verify(d => d.FindAllAsync(), Times.Once);
        }

        [Fact]
        public async Task FindWithParams()
        {
            var ds = new Mock<IDataStorage<TestModel>>();

            await ds.Object.FindAsync(
                queryParams: new QueryParams<TestModel>()
                {
                    Filter = f => f.Id > 0,
                    OrderBy = e => e.OrderByDescending(f => f.Id),
                    Skip = 3,
                    Take = 4
                });

            // Checking that it sends correctly all values 
            // (Order By was not fully supported by verify method)

            ds.Verify(d => d.FindAsync(f => f.Id > 0,
                    It.IsNotNull<Func<IQueryable<TestModel>, IOrderedQueryable<TestModel>>>(),
                    3, 4), Times.Once);
        }

        [Fact]
        public async Task FindRelationalNullParams()
        {
            var ds = new Mock<IRelationalDataStorage<TestModel>>();

            await ds.Object.FindAsync(queryParams: null);

            ds.Verify(d => d.FindAllAsync(), Times.Once);
        }

        [Fact]
        public async Task FindRelationalWithParams()
        {
            var ds = new Mock<IRelationalDataStorage<TestModel>>();

            await ds.Object.FindAsync(
                queryParams: new RelationalQueryParams<TestModel>()
                {
                    Filter = f => f.Id > 0,
                    OrderBy = e => e.OrderByDescending(f => f.Id),
                    Skip = 3,
                    Take = 4,
                    IncludeProperties = new [] { "prop1", "prop2" }
                });

            // Checking that it sends correctly all values 
            // (Order By was not fully supported by verify method)

            ds.Verify(d => d.FindAsync(f => f.Id > 0,
                    It.IsNotNull<Func<IQueryable<TestModel>, IOrderedQueryable<TestModel>>>(),
                    3, 4, new[] { "prop1", "prop2" }), Times.Once);
        }
    }
}
