using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Filters;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Equilaterus.Vortex.Tests
{
    public class VortexEngineTests
    {
        // TODO:
        // Pending: 
        // non-generic actions, no return action, no actions, multiple bindings, no expecting return action.


        #region TestContext
        interface IInstigator { }

        public class MyEntity : IInstigator
        {
            public int Id { get; set; }
        }

        class GenericAction<T> : VortexAction<T>
        {
            public override Task Execute(T entity, params object[] parameters)
            {
                if (Context == null) throw new Exception("Context is null!!");

                if (entity is MyEntity myEntity)
                {
                    myEntity.Id++;
                }
                return Task.FromResult(true);
            }

            public GenericAction(IVortexContext<T> context) : base(context) { }
        }

        class GenericReturnAction<T> : VortexReturnAction<T, List<MyEntity>> where T : class
        {
            public override Task<List<MyEntity>> Execute(QueryParams<T> queryParams, params object[] parameters)
            {
                if (Context == null) throw new Exception("Context is null!!");
                if (FilterFactory == null) throw new Exception("FilterFactory is null!!");

                return Task.FromResult(new List<MyEntity>());
            }

            public GenericReturnAction(IVortexContext<T> context, IGenericFilterFactory filterFactory) : base(context, filterFactory) { }
        }
        #endregion

        [Fact]
        public async Task RaiseEventAsync_Success()
        {
            // Prepare
            var context = new Mock<IVortexContext<MyEntity>>();
            var filterFactory = new Mock<IGenericFilterFactory>();

            var vortexGraph = new Mock<IVortexGraph>();
            vortexGraph.Setup(
                    g => g.GetBindings("event", typeof(IInstigator).Name)
                ).Returns(
                    new List<VortexBinding>
                    {
                        new VortexBinding
                        {
                            Actions = new List<Type>
                            {
                                typeof(GenericAction<>),
                                typeof(GenericAction<>),
                                typeof(GenericAction<>)
                            },
                            ReturnAction = typeof(GenericReturnAction<>)
                        }
                    }
                );

            var vortexEngine = 
                new VortexEngine<MyEntity>(
                    vortexGraph.Object, 
                    context.Object, 
                    filterFactory.Object);

            var myEntity = new MyEntity();

            // Execute
            var result = await vortexEngine.RaiseEventAsync<List<MyEntity>>("event", myEntity);

            Assert.Empty(result);
            Assert.Equal(3, myEntity.Id);
        }
    }
}
