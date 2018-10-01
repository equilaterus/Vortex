using Equilaterus.Vortex.Saturn.Commands;
using Equilaterus.Vortex.Saturn.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Commands
{
    public class DeleteEntityTests : BaseActionTest<TestModel>
    {
        public DeleteEntityTests()
        {
            ThrowsOnNullFileStorage = false;

            // Service is responsible for throwing
            ThrowsOnNullData = false;
        }

        protected override GenericAction<TestModel> GetCommand(VortexContext<TestModel> context)
        {
            return new DeleteEntity<TestModel>(context);
        }

        protected override VortexData GetData()
        {
            return new VortexData(new TestModel());
        }

        [Fact]
        public async Task ExecuteCommand()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IDataStorage<TestModel>>();            
            var context = new VortexContext<TestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            // Execute
            await command.Execute();

            mock.Verify(m => m.DeleteAsync(data.GetMainEntityAs<TestModel>()), Times.Once);
        }

        
    }
}
