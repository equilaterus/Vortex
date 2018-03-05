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
    public abstract class BaseCommandTest<T> where T : class
    {
        protected abstract GenericAction<T> GetCommand(VortexContext<T> context);
        protected abstract VortexData GetData();

        protected bool ThrowsOnNullData = true;
        protected bool ThrowsOnNullDataStorage = true;
        protected bool ThrowsOnNullFileStorage = true;


        [Fact]
        public async Task NullData()
        {
            // Prepare data
            var data = new VortexData(null);

            // Prepare objects
            var dataStorageMock = new Mock<IDataStorage<T>>();
            var fileStorageMock = new Mock<IFileStorage>();

            var context = new VortexContext<T>(dataStorageMock.Object, fileStorageMock.Object);
            var command = GetCommand(context);
            command.Params = data;
            command.Initialize();

            // Execute
            if (ThrowsOnNullData)
                await Assert.ThrowsAnyAsync<Exception>(() => command.Execute());
            else
                await command.Execute();

        }

        [Fact]
        public async Task NullDataStorage()
        {
            // Prepare objects
            var fileStorageMock = new Mock<IFileStorage>();

            // Prepare objects
            var context = new VortexContext<T>(null, fileStorageMock.Object);
            var command = GetCommand(context);
            command.Params = GetData();
            command.Initialize();

            // Execute
            if (ThrowsOnNullDataStorage)
                await Assert.ThrowsAnyAsync<Exception>(() => command.Execute());
            else
                await command.Execute();
        }

        [Fact]
        public async Task NullFileStorage()
        {
            // Prepare objects
            var dataStorageMock = new Mock<IDataStorage<T>>();

            // Prepare objects
            var context = new VortexContext<T>(dataStorageMock.Object, null);
            var command = GetCommand(context);
            command.Params = GetData();
            command.Initialize();

            // Execute
            if (ThrowsOnNullFileStorage)
                await Assert.ThrowsAnyAsync<Exception>(() => command.Execute());
            else
                await command.Execute();
        }
    }
}
