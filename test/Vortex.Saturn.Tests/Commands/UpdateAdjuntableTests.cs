using Equilaterus.Vortex.Saturn.Commands;
using Equilaterus.Vortex.Saturn.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Services.Commands
{
    public class UpdateAdjuntableTests : BaseActionTest<AdjuntableTestModel>
    {
        public UpdateAdjuntableTests()
        {
            ThrowsOnNullDataStorage = false;
        }

        protected override GenericAction<AdjuntableTestModel> GetCommand(VortexContext<AdjuntableTestModel> context)
        {
            return new UpdateAttacheableFile<AdjuntableTestModel>(context);
        }

        protected override VortexData GetData()
        {
            var entity = new AdjuntableTestModel("fileurl/dir/name.ext");
            var file = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));
            var data = new VortexDataAttacheable(entity, file, "ext");

            return data;
        }

        [Fact]
        public async Task ExecuteCommand()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IFileStorage>();
            mock.Setup(m => m.StoreFileAsync(It.IsAny<MemoryStream>(), "ext"))
                .ReturnsAsync("url.ext");
            var context = new VortexContext<AdjuntableTestModel>(null, mock.Object);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            // Execute
            await command.Execute();

            mock.Verify(m => m.DeleteFileAsync("fileurl/dir/name.ext"), Times.Once);

            mock.Verify(m => m.StoreFileAsync(It.IsAny<MemoryStream>(), "ext"), Times.Once);

            Assert.NotNull(data.GetMainEntityAs< AdjuntableTestModel>().FileUrl);
        }        
    }
}
