﻿using System;
using System.Collections.Generic;
using Xunit;
using System.Reflection;
using Equilaterus.Vortex.Saturn.Queries.Filters;
using Equilaterus.Vortex.Saturn.Models;
using Equilaterus.Vortex.Saturn.Configuration;

namespace Equilaterus.Vortex.Saturn.Tests.Queries.Filters
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
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(typeof(IActivable), null));
            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(null, typeof(ActivableFilter<>)));
            Assert.Throws<ArgumentNullException>(() => filterFactory.Bind(null, null));
        }
        
        [Fact]
        public void TestMultipleBindings()
        {
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            // DO EXTRA BINDING
            filterFactory.Bind(typeof(ISoftDeleteable), typeof(ActivableFilter<>));

            Dictionary<Type, List<Type>> bindings = null;

            bindings = filterFactory.GetType()
                .GetField("_bindings", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(filterFactory) as Dictionary<Type, List<Type>>;

            Assert.NotNull(bindings);            

            Assert.True(2 == bindings.Count);
            Assert.True(1 == bindings[typeof(IActivable)].Count);
            Assert.True(typeof(ActivableFilter<>) == bindings[typeof(IActivable)][0]);
            
            Assert.True(2 == bindings[typeof(ISoftDeleteable)].Count);
            Assert.True(typeof(SoftDeleteableFilter<>) == bindings[typeof(ISoftDeleteable)][0]);
            Assert.True(typeof(ActivableFilter<>) == bindings[typeof(ISoftDeleteable)][1]);
        }


        [Fact]
        public void GetFilterForVanilla()
        {
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            var result = filterFactory.GetFilters<MyModel>();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetFilterForActivable()
        {
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            var result = filterFactory.GetFilters<ActiveModel>();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveModel>);
        }

        [Fact]
        public void GetFilterForActivableAndSoftDeleteable()
        {
            GenericFilterFactory filterFactory = new GenericFilterFactory();
            filterFactory.LoadDefaults();

            var result = filterFactory.GetFilters<ActiveSoftDeleteableModel>();


            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result[0] is ActivableFilter<ActiveSoftDeleteableModel>);
            Assert.True(result[1] is SoftDeleteableFilter<ActiveSoftDeleteableModel>);
        }
    }
}
