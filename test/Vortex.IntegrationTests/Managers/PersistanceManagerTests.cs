using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Managers;
using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Services.EFCore;
using Equilaterus.Vortex.Services.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Equilaterus.Vortex.Engine.Configuration.CommandBindings;

namespace Equilaterus.Vortex.Tests.IntegrationTests.Managers
{
    public class PersistanceTestModel : MongoDbEntity, IActivable
    {
        public bool IsActive { get; set; }
        public int Counter { get; set; }

        public PersistanceTestModel() : base() { }
    }

    public class PersistanceManagerTests : IntegrationTest<PersistanceTestModel>
    {
        protected override List<PersistanceTestModel> GetSeed<PersistanceTestModel>()
        {
            return null;
        }

        [Fact]
        public async Task TestInsert()
        {
            var dbName = nameof(TestInsert);

            VortexGraph vortexGraph = new VortexGraph();
            vortexGraph.LoadDefaults();

            using (var context = GetContext(dbName))
            {
                var dataStorage = new EFCoreDataStorage<PersistanceTestModel>(context);

                var executor = new VortexExecutor<PersistanceTestModel>(vortexGraph);

                IPersistanceManager<PersistanceTestModel> persistanceManager =
                    new PersistanceManager<PersistanceTestModel>(dataStorage, null, executor);

                await persistanceManager.ExecuteCommand(
                    VortexEvents.InsertEntity, 
                    new VortexData(new PersistanceTestModel()));

                var result = await persistanceManager.ExecuteQueryForEntities(
                    VortexEvents.QueryForEntities, new VortexData(new QueryParams<PersistanceTestModel>()));

                Assert.NotEmpty(result);
            }
        }
    }
}
