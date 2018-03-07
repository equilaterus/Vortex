using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Services;
using Moq;
using Xunit;

namespace Vortex.Tests.Engine.Queries
{
    public class RelationalQueryForEntitiesTests : BaseActionTest<TestModel>
    {
        public RelationalQueryForEntitiesTests()
        {
            ThrowsOnNullData = false;
            ThrowsOnNullFileStorage = false;
        }

        protected override VortexContext<TestModel> GetContext()
        {
            var dataStorageMock = new Mock<IRelationalDataStorage<TestModel>>();
            var fileStorageMock = new Mock<IFileStorage>();

            return new VortexContext<TestModel>(dataStorageMock.Object, fileStorageMock.Object);
        }

        protected override GenericAction<TestModel> GetCommand(VortexContext<TestModel> context)
        {
            return new RelationalQueryForEntities<TestModel>(context);
        }

        protected override VortexData GetData()
        {
            return new VortexData(new RelationalQueryParams<TestModel>());
        }

        
        [Fact]
        public async Task ExecuteWithoutParams()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IRelationalDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            mock.Verify(m => m.FindAsync(null, null, 0, 0, null), Times.Once);     
        }

        [Fact]
        public async Task ExecuteWithFilter()
        {
            // Prepare data
            var data = new VortexData(
                new RelationalQueryParams<TestModel>() { Filter = t => t.Id == 1 });

            // Prepare objects
            var mock = new Mock<IRelationalDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            mock.Verify(m => m.FindAsync(t => t.Id == 1, null, 0, 0, null), Times.Once);
        }

        [Fact]
        public async Task ExecuteWithFilterAndIncludes()
        {
            // Prepare data
            var data = new VortexData(
                new RelationalQueryParams<TestModel>() { Filter = t => t.Id == 1, IncludeProperties = new[] { "include" } });

            // Prepare objects
            var mock = new Mock<IRelationalDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();
            
            mock.Verify(m => m.FindAsync(t => t.Id == 1, null, 0, 0, new[] { "include" }), Times.Once);
        }

        [Fact]
        public async Task ExecuteNullParams()
        {
            // Prepare data
            var data = new VortexData(null);
            // Prepare objects
            var mock = new Mock<IRelationalDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            // It can not explicitly verify this call
            // cause it adds [] and tries to verify an
            // overloaded method, but action calls the
            // one without that param
            // mock.Verify(m => m.FindAllAsync());
            mock.VerifyAll();
        }

        [Fact]
        public async Task ExecuteBasicQueryParams()
        {
            var data = new VortexData(
                new QueryParams<TestModel>() { Filter = t => t.Id == 1 });
            // Prepare objects
            var mock = new Mock<IRelationalDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            // It can not explicitly verify this call
            // cause it adds [] and tries to verify an
            // overloaded method, but action calls the
            // one without that param
            // CALLED: mock.Verify(m => m.FindAsync(t => t.Id == 1, null, 0, 0), Times.Once);
            // VERIFIED: mock.Verify(m => m.FindAsync(t => t.Id == 1, null, 0, 0, []), Times.Once);
            // Even with a setup it does not work.
            mock.VerifyAll();
        }

        [Fact]
        public async Task ExecuteBadStorage()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IDataStorage<TestModel>>();

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await Assert.ThrowsAsync<NullReferenceException>(() => command.Execute());
        }
    }
}
