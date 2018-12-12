using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Helpers;

namespace Equilaterus.Vortex.Tests.Helpers
{
    public class ReflectionToolsTests
    {
        #region TestContext
        class Generic0
        {
            public int Id { get; set; }
        }

        class Generic0Ext : Generic0
        {
            public string ExtendedProp { get; set; }

            public Generic0Ext(string extendedProp) => ExtendedProp = extendedProp;
        }

        class Generic1<T>
        {
            public T Id { get; set; }
        }

        class Generic1Ext<T> : Generic1<T>
        {
            public string ExtendedProp { get; set; }

            public Generic1Ext(string extendedProp) => ExtendedProp = extendedProp;
        }

        class Constructor<T>
        {
            public T Id { get; set; }

            public Constructor(T id) => Id = id;
        }

        class Generic2<T, G>
        {
            public T Id; public G Id2;

            public Generic2(T id, G id2) => (Id, Id2) = (id, id2);
        }

        class Generic2Ext<T> : Generic2<T, int>
        {
            public Generic2Ext(T id, int id2) : base(id, id2) { }
        }
        #endregion

        [Fact]
        public void Instantiate_NoGenericParams_Success()
        {
            // Prepare
            Type type = typeof(Generic0);

            // Execute
            var result = ReflectionTools.Instantiate<Generic0>();

            // Check
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }

        [Fact]
        public void Instantiate_NoGenericParam_BadConstructorArg_ThrowsException()
        {
            // Prepare
            Type type = typeof(Generic0);

            // Execute and check
            Assert.Throws<MissingMethodException>(
                () => ReflectionTools.Instantiate<Generic0>("wrong-args"));
        }

        [Fact]
        public void Instantiate_1GenericParam_Success()
        {
            // Prepare
            Type type = typeof(Generic1<>);

            // Execute
            var result = ReflectionTools.Instantiate<Generic1<int>>();

            // Check
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }

        [Fact]
        public void Instantiate_1GenericParam_BadConstructorArg_ThrowsException()
        {
            // Prepare
            Type type = typeof(Generic1<>);

            // Execute and check
            Assert.Throws<MissingMethodException>(
                () => ReflectionTools.Instantiate<Generic1<int>>(1));
        }

        [Fact]
        public void Instantiate_Constructor_Success()
        {
            // Prepare
            Type type = typeof(Constructor<>);

            // Execute
            var result = ReflectionTools.Instantiate<Constructor<int>>(415);

            // Check
            Assert.NotNull(result);
            Assert.Equal(415, result.Id);
        }

        [Fact]
        public void Instantiate_Constructor_NoParamsBadConstructorArg_ThrowsException()
        {
            // Prepare
            Type type = typeof(Constructor<>);

            // Execute and check
            Assert.Throws<MissingMethodException>(
                () => ReflectionTools.Instantiate<Constructor<int>>());
        }

        [Fact]
        public void Instantiate_2GenericParams_Success()
        {
            // Prepare
            Type type = typeof(Generic2<,>);

            // Execute
            var result = ReflectionTools.Instantiate<Generic2<int, string>>(1, "ok");

            // Check
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("ok", result.Id2);
        }

        [Fact]
        public void Instantiate_2genericParams_BadConstructorArg_ThrowsException()
        {
            // Prepare
            Type type = typeof(Generic2<,>);

            // Execute and check
            Assert.Throws<MissingMethodException>(
                () => ReflectionTools.Instantiate<Generic2<int, string>>(1, 1));
        }

        [Fact]
        public void InstantiateAs_InheritType_Success()
        {
            // Prepare
            Type type = typeof(Generic0Ext);

            // Execute
            var result = ReflectionTools.InstantiateAs<Generic0>(type, "it works!");

            // Check
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("it works!", ((Generic0Ext)result).ExtendedProp);
        }

        [Fact]
        public void InstantiateAs_Type_Null_ThrowsException()
        {
            // Prepare
            Type type = typeof(Generic2<,>);

            // Execute and check
            Assert.Throws<ArgumentNullException>(
                () => ReflectionTools.InstantiateAs<Generic0>(null));
        }

        [Fact]
        public void InstantiateAs_InheritType_Generic_Success()
        {
            // Prepare
            Type type = typeof(Generic1Ext<>);

            // Execute
            var result = ReflectionTools.InstantiateAs<Generic1<string>>(type, "it works!");

            // Check
            Assert.NotNull(result);
            Assert.Null(result.Id);
            Assert.Equal("it works!", ((Generic1Ext<string>)result).ExtendedProp);
        }

        [Fact]
        public void InstantiateAs_Base2GenericParams_Extended1GenericParam_Success()
        {
            // Prepare
            Type type = typeof(Generic2Ext<>);

            // Execute
            var result = ReflectionTools.InstantiateAs<Generic2<string, int>>(type, "str", 415);

            // Check
            Assert.NotNull(result);
            Assert.Equal("str", result.Id);
            Assert.Equal(415, result.Id2);
        }

        [Fact]
        public void InstantiateAs_Base2GenericParams_Extended1GenericParam_WrongOrderGenericParams_ThrowsException()
        {
            // Prepare
            Type type = typeof(Generic2Ext<>);

            // Execute and check
            Assert.Throws<MissingMethodException>(
                () => ReflectionTools.InstantiateAs<Generic2<int, string>>(type, 415, "str"));
        }
    }
}
