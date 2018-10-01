
using Equilaterus.Vortex.Saturn.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests
{
    public class PersistanceManagerTests
    {
        [Fact]
        public async Task ExecuteCommand()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);

            var vortexEvent = "randomEvent";
            var vortexData = new VortexData(new object());

            await persistanceManager.ExecuteCommandAsync(
                vortexEvent, vortexData);

            vortexExecutor.Verify(m => m.RaiseEventAsync(vortexEvent, vortexData), Times.Once);
        }

        [Fact]
        public async Task ExecuteCommandNullEvent()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);
            
            var vortexData = new VortexData(new object());

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => persistanceManager.ExecuteCommandAsync(null, vortexData));
        }

        [Fact]
        public async Task ExecuteQueryForEntities()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var vortexEvent = "randomEvent";
            var vortexData = new VortexData(new QueryParams<TestModel>());

            vortexExecutor.Setup(
                m => m.RaiseEventAsync(vortexEvent, vortexData))
                .ReturnsAsync(new VortexData(
                    new List<TestModel>()
                    {
                        new TestModel() { Id = 3 }
                    }));

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);
                        

            var result = await persistanceManager.ExecuteQueryForEntitiesAsync(
                vortexEvent, vortexData);

            vortexExecutor.Verify(m => m.RaiseEventAsync(vortexEvent, vortexData), Times.Once);

            Assert.NotEmpty(result);
            Assert.Equal(3, result[0].Id);
        }

        [Fact]
        public async Task ExecuteQueryForEntitiesNullEvent()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);

            var vortexData = new VortexData(new object());

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => persistanceManager.ExecuteQueryForEntitiesAsync(null, vortexData));
        }

        [Fact]
        public async Task ExecuteQueryForInt()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var vortexEvent = "randomEvent";
            var vortexData = new VortexData(new QueryParams<TestModel>());

            vortexExecutor.Setup(
                m => m.RaiseEventAsync(vortexEvent, vortexData))
                .ReturnsAsync(new VortexData(3));

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);


            var result = await persistanceManager.ExecuteQueryForIntAsync(
                vortexEvent, vortexData);

            vortexExecutor.Verify(m => m.RaiseEventAsync(vortexEvent, vortexData), Times.Once);
            
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task ExecuteQueryForIntNullEvent()
        {
            var vortexExecutor = new Mock<IVortexEngine<TestModel>>();

            var persistanceManager = new PersistanceManager<TestModel>(null, vortexExecutor.Object);

            var vortexData = new VortexData(new object());

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => persistanceManager.ExecuteQueryForIntAsync(null, vortexData));
        }
    }
}
