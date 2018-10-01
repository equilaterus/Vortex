using Equilaterus.Vortex.Saturn.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Saturn.Tests
{
    public abstract class BaseActionTest<T> where T : class
    {
        virtual protected VortexContext<T> GetContext()
        {
            var dataStorageMock = new Mock<IDataStorage<T>>();
            var fileStorageMock = new Mock<IFileStorage>();

            return new VortexContext<T>(dataStorageMock.Object, fileStorageMock.Object);
        }

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
            var context = GetContext();

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
            var context = new VortexContext<T>(null, GetContext().FileStorage);

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
            var context = new VortexContext<T>(GetContext().DataStorage, null);
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
