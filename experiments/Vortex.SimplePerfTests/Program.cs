using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Managers;
using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Services.EFCore;
using Equilaterus.Vortex.Services.EFCore.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static Equilaterus.Vortex.Engine.Configuration.CommandBindings;

namespace Vortex.SimplePerfTests
{
    class Program
    {
        const int ENTITIES = 100000;

        public class PersistanceTestModel : IActivable
        {
            public string Id { get; set; }
            public bool IsActive { get; set; }
            public int Counter { get; set; }

            public PersistanceTestModel()
            {
                Id = Guid.NewGuid().ToString();
                Counter = 1;
                IsActive = true;
            }
        }

        protected static TestContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TestContext>()
              .UseInMemoryDatabase(databaseName: databaseName)
              .Options;

            return new TestContext(options);
        }

        protected class TestContext : DbContext
        {
            public DbSet<PersistanceTestModel> Models { get; set; }

            public TestContext(DbContextOptions<TestContext> options)
                : base(options)
            { }
        }

        protected static List<PersistanceTestModel> GetSeed()
        {
            var list = new List<PersistanceTestModel>();
            for (int i = 0; i < ENTITIES; ++i)
            {
                list.Add(new PersistanceTestModel());
            }
            return list;
        }
        
        static async Task<TimeSpan> EFCoreTest(int i)
        {
            var efcoreEntities = GetSeed();            

            // EF CORE
            Stopwatch swEfCore = new Stopwatch();
            using (var context = GetContext("EFCORE"+i))
            {
                swEfCore.Start();
                foreach (var entity in efcoreEntities)
                {
                    // Just do the same operation as Vortex simple insert
                    context.Add(entity);
                    await context.SaveChangesAsync();
                    context.Entry(entity).State = EntityState.Detached;
                }                
            }
            swEfCore.Stop();

            using (var context = GetContext("EFCORE"+i))
            {
                if (await context.Models.CountAsync() != ENTITIES)
                {
                    throw new Exception();
                }
                context.Database.EnsureDeleted();
            }
            return swEfCore.Elapsed;
        }

        static async Task<TimeSpan> VortexTest(int i)
        {
            // Vortex full pipeline
            var vortexEntities = GetSeed();

            VortexGraph vortexGraph = new VortexGraph();
            vortexGraph.LoadDefaults();

            Stopwatch swVortex = new Stopwatch();
            using (var context = GetContext("Vortex"+i))
            {
                var dataStorage = new EFCoreDataStorage<PersistanceTestModel>(context);

                var executor = new VortexExecutor<PersistanceTestModel>(vortexGraph);

                IPersistanceManager<PersistanceTestModel> persistanceManager =
                    new PersistanceManager<PersistanceTestModel>(dataStorage, null, executor);



                swVortex.Start();
                foreach (var entity in vortexEntities)
                {
                    await persistanceManager.ExecuteCommand(
                    VortexEvents.InsertEntity.ToString(),
                    entity);
                }
            }
            swVortex.Stop();

            using (var context = GetContext("Vortex"+i))
            {
                if (await context.Models.CountAsync() != ENTITIES)
                {
                    throw new Exception();
                }
                context.Database.EnsureDeleted();
            }
            return swVortex.Elapsed;
        }


        static void Main(string[] args)
        { 
            Console.WriteLine("The objetive of this tests is to compare the performance of direct use of EFCore API vs Full Vortex pipeline");

            Console.WriteLine("We want to keep it simple and just compare the overhead of using Vortex pipeline, taking out almost every non required external component");

            Console.WriteLine("----");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Inserting: {0} entities for each test", ENTITIES);

                Console.WriteLine("EfCore Test {0} -> {1}", i, EFCoreTest(i).Result);

                Console.WriteLine("Vortex Test {0} -> {1}", i, VortexTest(i).Result);

                Console.WriteLine("----");
            }

            Console.ReadKey();
        }

       
    }
}
