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
    public class DeleteAdjuntableTests : BaseCommandTest<AdjuntableTestModel>
    {
        public DeleteAdjuntableTests()
        {
            ThrowsOnNullDataStorage = false;
        }

        protected override GenericAction<AdjuntableTestModel> GetCommand(VortexContext<AdjuntableTestModel> context)
        {
            return new DeleteAdjuntable<AdjuntableTestModel>(context);
        }

        protected override VortexData GetData()
        {
            return new VortexData(new AdjuntableTestModel("fileurl/dir/name.ext"));
        }

        [Fact]
        public async Task ExecuteCommand()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IFileStorage>();            
            var context = new VortexContext<AdjuntableTestModel>(null, mock.Object);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            // Execute
            await command.Execute();

            mock.Verify(m => m.DeleteFileAsync("fileurl/dir/name.ext"), Times.Once);

            Assert.Null(data.GetMainEntityAs<AdjuntableTestModel>().FileUrl);
        }

        
    }
}
