using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Commands;
using Equilaterus.Vortex.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vortex.Tests.Engine.Commands
{
    public class InsertEntityTests : BaseCommandTest<TestModel>
    {
        public InsertEntityTests()
        {
            ThrowsOnNullFileStorage = false;

            // Service is responsible for throwing
            ThrowsOnNullData = false;
        }

        protected override GenericAction<TestModel> GetCommand(VortexContext<TestModel> context)
        {
            return new InsertEntity<TestModel>(context);
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

            mock.Verify(m => m.InsertAsync(data.GetMainEntityAs<TestModel>()), Times.Once);
        }

        
    }
}
