using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Managers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vortex.Tests.Managers
{
    public class PersistanceManagerTests
    {
        [Fact]
        public async Task ExecuteCommand()
        {
            var vortexExecutor = new Mock<IVortexExecutor<TestModel>>();

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);

            var vortexEvent = "randomEvent";
            var vortexData = new VortexData(new object());

            await persistanceManager.ExecuteCommandAsync(
                vortexEvent, vortexData);

            vortexExecutor.Verify(m => m.ExecuteAsync(vortexEvent, vortexData), Times.Once);
        }
    }
}
