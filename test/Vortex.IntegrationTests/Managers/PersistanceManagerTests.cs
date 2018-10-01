using Equilaterus.Vortex.Saturn;
using Equilaterus.Vortex.Saturn.Configuration;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Queries;
using Equilaterus.Vortex.Saturn.Services.EFCore;
using Equilaterus.Vortex.Saturn.Services.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Equilaterus.Vortex.IntegrationTests.Managers
{
    public class PersistanceTestModel : MongoDbEntity, IActivable
    {
        public bool IsActive { get; set; }
        public int Counter { get; set; }

        public PersistanceTestModel() : base() { }
    }

    public class PersistanceManagerTests : IntegrationTest<PersistanceTestModel>
    {
        protected override List<PersistanceTestModel> GetSeed()
        {
            return null;
        }

        [Fact]
        public async Task TestInsert()
        {
            var dbName = nameof(TestInsert);

            VortexGraph vortexGraph = new VortexGraph();
            vortexGraph.LoadDefaults();

            using (var context = GetEfCoreContext(dbName))
            {
                var dataStorage = new EFCoreDataStorage<PersistanceTestModel>(context);

                var executor = new VortexEngine<PersistanceTestModel>(vortexGraph, new VortexRelationalContext<PersistanceTestModel>(dataStorage, null));

                IPersistanceManager<PersistanceTestModel> persistanceManager =
                    new PersistanceManager<PersistanceTestModel>(dataStorage, null, executor);

                await persistanceManager.ExecuteCommandAsync(
                    VortexEvents.InsertEntity,
                    new VortexData(new PersistanceTestModel() { IsActive = true }));

                var result = await persistanceManager.ExecuteQueryForEntitiesAsync(
                    VortexEvents.QueryForEntities, new VortexData(new QueryParams<PersistanceTestModel>()));

                Assert.NotEmpty(result);
            }
        }
    }
}
