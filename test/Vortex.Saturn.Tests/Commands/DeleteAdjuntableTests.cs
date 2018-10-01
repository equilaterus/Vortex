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
    public class DeleteAdjuntableTests : BaseActionTest<AdjuntableTestModel>
    {
        public DeleteAdjuntableTests()
        {
            ThrowsOnNullDataStorage = false;
        }

        protected override GenericAction<AdjuntableTestModel> GetCommand(VortexContext<AdjuntableTestModel> context)
        {
            return new DeleteAttacheableFile<AdjuntableTestModel>(context);
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
