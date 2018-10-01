using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Equilaterus.Vortex.Saturn.Commands;
using Equilaterus.Vortex.Saturn.Services;
using Moq;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests.Services.Commands
{
    public class SoftDeleteTests : BaseActionTest<SoftDeleteableTestModel>
    {
        public SoftDeleteTests()
        {
            ThrowsOnNullData = true;
            ThrowsOnNullDataStorage = true;
            ThrowsOnNullFileStorage = false;
        }

        protected override GenericAction<SoftDeleteableTestModel> GetCommand(VortexContext<SoftDeleteableTestModel> context)
        {
            return new SoftDelete<SoftDeleteableTestModel>(context);
        }

        protected override VortexData GetData()
        {
            return new VortexData(new SoftDeleteableTestModel() { Id = 1, IsDeleted = false });
        }

        [Fact]
        public async Task ExecuteCommand()
        {
            // Prepare data
            var data = GetData();

            // Prepare objects
            var mock = new Mock<IDataStorage<SoftDeleteableTestModel>>();
            var context = new VortexContext<SoftDeleteableTestModel>(mock.Object, null);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            // Execute
            await command.Execute();

            mock.Verify(m => m.UpdateAsync(data.GetMainEntityAs<SoftDeleteableTestModel>()), Times.Once);

            Assert.True(data.GetMainEntityAs<SoftDeleteableTestModel>().IsDeleted);
        }
    }
}
