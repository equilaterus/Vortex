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
    public class QueryCountTests : BaseActionTest<TestModel>
    {
        
        protected override GenericAction<TestModel> GetCommand(VortexContext<TestModel> context)
        {
            return new QueryCount<TestModel>(context);
        }

        protected override VortexData GetData()
        {
            return new VortexData(new QueryParams<TestModel>());
        }

        [Fact]
        public async Task Execute()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IDataStorage<TestModel>>();
            mock.Setup(m => m.Count(It.IsAny<Expression<Func<TestModel, bool>>>()))
                .ReturnsAsync(3);

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            mock.Verify(m => m.Count(null), Times.Once);

            Assert.Equal(3, command.Results.GetAsInt());            
        }

        [Fact]
        public async Task ExecuteWithFilter()
        {
            // Prepare data
            var data = new VortexData(
                new QueryParams<TestModel>() { Filter = t =>t.Id == 1 });

            // Prepare objects
            var mock = new Mock<IDataStorage<TestModel>>();
            mock.Setup(m => m.Count(It.IsAny<Expression<Func<TestModel, bool>>>()))
                .ReturnsAsync(3);

            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            Assert.True(command.IsReturnAction);

            // Execute
            await command.Execute();

            mock.Verify(m => m.Count(null), Times.Never);

            mock.Verify(m => m.Count(t => t.Id == 1), Times.Once);

            Assert.Equal(3, command.Results.GetAsInt());
        }
    }
}
