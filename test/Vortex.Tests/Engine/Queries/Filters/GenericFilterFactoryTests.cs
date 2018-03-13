using Equilaterus.Vortex.Models;
using Equilaterus.Vortex.Engine.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Engine.Configuration;

namespace Vortex.Tests.Engine.Queries.Filters
{
    public class GenericFilterFactoryTests
    {
        class MyModel
        {
            public int Id { get; set; }
        }


        class ActiveModel : MyModel, IActivable
        {
            public bool IsActive { get; set; }
        }

        class ActiveSoftDeleteableModel : MyModel, IActivable, ISoftDeleteable
        {
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }
             

        [Fact]
        public void NullBindings()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(typeof(IActivable), null));
            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(null, typeof(ActivableFilter<>)));
            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(null, null));
        }

        [Fact]
        public void TestBindings()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
                        
            Assert.True(2 == filterFactory.Bindings.Count);
            Assert.True(1 == filterFactory.Bindings[typeof(IActivable)].Count);
            Assert.True(typeof(ActivableFilter<>) == filterFactory.Bindings[typeof(IActivable)][0]);

            Assert.True(1 == filterFactory.Bindings[typeof(ISoftDeleteable)].Count);
            Assert.True(typeof(SoftDeleteableFilter<>) == filterFactory.Bindings[typeof(ISoftDeleteable)][0]);
        }

        [Fact]
        public void TestMultipleBindings()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
            
            filterFactory.Bind(typeof(ISoftDeleteable), typeof(ActivableFilter<>));

            Assert.True(2 == filterFactory.Bindings.Count);
            Assert.True(1 == filterFactory.Bindings[typeof(IActivable)].Count);
            Assert.True(typeof(ActivableFilter<>) == filterFactory.Bindings[typeof(IActivable)][0]);
            
            Assert.True(2 == filterFactory.Bindings[typeof(ISoftDeleteable)].Count);
            Assert.True(typeof(SoftDeleteableFilter<>) == filterFactory.Bindings[typeof(ISoftDeleteable)][0]);
            Assert.True(typeof(ActivableFilter<>) == filterFactory.Bindings[typeof(ISoftDeleteable)][1]);
        }


        [Fact]
        public void GetFilterForVanilla()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
           
            var result = filterFactory.GetFilters<MyModel>();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetFilterForActivable()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
            
            var result = filterFactory.GetFilters<ActiveModel>();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveModel>);
        }

        [Fact]
        public void GetFilterForActivableAndSoftDeleteable()
        {
            GenericFilterFactory filterFactory = GenericFilterFactory.GetInstance();
            
            var result = filterFactory.GetFilters<ActiveSoftDeleteableModel>();


            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveSoftDeleteableModel>);
            Assert.True(result[1] is SoftDeleteableFilter<ActiveSoftDeleteableModel>);
        }
    }
}
