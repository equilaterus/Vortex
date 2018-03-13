using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Helpers;
using System.Threading.Tasks;

namespace Vortex.Tests.Engine
{
    public class VortexGraphTests
    {
        public interface IT1
        {
            string T1 { get; set; }
        }

        public interface IT2
        {
            string T2 { get; set; }
        }

        public class ClassTest : IT1, IT2
        {
            public string T1 { get; set; }
            public string T2 { get; set; }
        }

        class BasicAction1 : VortexAction
        {
            public override async Task Execute()
            {
                var entity = Params.GetMainEntityAs<IT1>();
                if (entity != null)
                {
                    entity.T1 = "DONE";
                }
            }
        }

        class BasicAction2 : VortexAction
        {
            public override async Task Execute()
            {
                var entity = Params.GetMainEntityAs<IT2>();
                if (entity != null)
                {
                    entity.T2 = "DONE";
                }
            }
        }

        class Action1<T> : GenericAction<T> where T : class
        {
            public Action1(VortexContext<T> context) : base(context)
            {
            }

            public string Value { get; set; }

            public override void Initialize() 
            {
                Value = "TEST";
            }

            public override async Task Execute()
            {
                var entity = Params.GetMainEntityAs<IT1>();
                if (entity != null)
                {
                    entity.T1 = Value;
                }
            }
        }

        class Action2<T> : GenericAction<T> where T : class
        {
            public Action2(VortexContext<T> context) : base(context)
            {
            }

            public override async Task Execute()
            {
                var entity = Params.GetMainEntityAs<IT2>();
                if (entity != null)
                {
                    entity.T2 = "DONE";
                }
            }
        }


        [Fact]
        public void SuccessExcecuteGenericAction()
        {
            VortexGraph g = new VortexGraph();

            g.CreateEvent("OnDelete");
            g.Bind("OnDelete", nameof(IT1), SubClassOf<VortexAction>.GetFrom<Action1<ClassTest>>());
            g.Bind("OnDelete", nameof(IT2), SubClassOf<VortexAction>.GetFrom<Action2<ClassTest>>());

            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Equal(2, actions.Count);

            var ActionParams = new VortexData(objectTest);
            VortexContext<ClassTest> context = new VortexContext<ClassTest>();

            foreach (var actionType in actions)
            {                
                var action = (VortexAction)Activator.CreateInstance(actionType.TypeOf, context);
                action.Initialize();
                action.Params = ActionParams;
                action.Execute();
            }

            Assert.Equal("TEST", objectTest.T1);
            Assert.Equal("DONE", objectTest.T2);
        }

        [Fact]
        public void SuccessExcecuteVortexAction()
        {
            VortexGraph g = new VortexGraph();

            g.CreateEvent("OnDelete");
            g.Bind("OnDelete", nameof(IT1), SubClassOf<VortexAction>.GetFrom<BasicAction1>());
            g.Bind("OnDelete", nameof(IT2), SubClassOf<VortexAction>.GetFrom<BasicAction2>());

            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Equal(2, actions.Count);

            var ActionParams = new VortexData(objectTest);

            foreach (var actionType in actions)
            {
                var action = (VortexAction)Activator.CreateInstance(actionType.TypeOf);
                action.Initialize();
                action.Params = ActionParams;
                action.Execute();
            }

            Assert.Equal("DONE", objectTest.T1);
            Assert.Equal("DONE", objectTest.T2);
        }

        [Fact]
        public void InterfaceWithoutAction()
        {
            VortexGraph g = new VortexGraph();

            g.CreateEvent("OnDelete");            
            g.Bind("OnDelete", nameof(IT2), SubClassOf<VortexAction>.GetFrom<Action2<ClassTest>>());

            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Single(actions);

            var ActionParams = new VortexData(objectTest);
            foreach (var actionType in actions)
            {
                var action = (VortexAction)Activator.CreateInstance(actionType.TypeOf, new VortexContext<ClassTest>());
                action.Initialize();
                action.Params = ActionParams;
                action.Execute();
            }
            
            Assert.Equal("DONE", objectTest.T2);
        }

        [Fact]
        public void BindEventNotFound()
        {
            VortexGraph g = new VortexGraph();

            Assert.Throws<Exception>(
                () => g.Bind("OnDelete", nameof(IT1), SubClassOf<VortexAction>.GetFrom<Action1<ClassTest>>())
            );
        }

        [Fact]
        public void CreateEventAlreadyExist()
        {
            VortexGraph g = new VortexGraph();
            g.CreateEvent("OnDelete");

            Assert.Throws<Exception>(
                () => g.CreateEvent("OnDelete")
            );
        }

        [Fact]
        public void GetActionsFromNonExistentEvent()
        {
            VortexGraph g = new VortexGraph();
            ClassTest objectTest = new ClassTest();

            Assert.Throws<Exception>(
                () => g.GetActions("OnDelete", objectTest.GetType())
            );
        }

        [Fact]
        public void GetDefaultActionsWithNonExistentEvent()
        {
            VortexGraph g = new VortexGraph();

            Assert.Throws<Exception>(
                () => g.GetDefaultActions("OnDelete")
            );
        }
    }
}
