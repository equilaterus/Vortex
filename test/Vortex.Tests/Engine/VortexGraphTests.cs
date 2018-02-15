using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Helpers;

namespace Vortex.Tests.Engine
{
    public class VortexGraphTests
    {
        interface IT1
        {
            string T1 { get; set; }
        }

        interface IT2
        {
            string T2 { get; set; }
        }

        class ClassTest : IT1, IT2
        {
            public string T1 { get; set; }
            public string T2 { get; set; }
        }

        class Action1 : VortexAction
        {
            public string Value { get; set; }

            public override void Initialize() 
            {
                Value = "TEST";
            }

            public override void Excecute()
            {
                var entity = _params.GetMainEntityAs<IT1>();
                if (entity != null)
                {
                    entity.T1 = Value;
                }
            }
        }

        class Action2 : VortexAction
        {
            public override void Excecute()
            {
                var entity = _params.GetMainEntityAs<IT2>();
                if (entity != null)
                {
                    entity.T2 = "DONE";
                }
            }
        }


        [Fact]
        public void SuccessExcecute()
        {
            VortexGraph g = new VortexGraph();

            g.CreateEvent("OnDelete");
            g.Bind("OnDelete", nameof(IT1), SubClassOf<VortexAction>.GetFrom<Action1>());
            g.Bind("OnDelete", nameof(IT2), SubClassOf<VortexAction>.GetFrom<Action2>());

            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Equal(2, actions.Count);

            var ActionParams = new ActionParams(objectTest);
            foreach (var actionType in actions)
            {
                var action = (VortexAction)Activator.CreateInstance(actionType.TypeOf);
                action.Initialize();
                action.SetParams(ActionParams);
                action.Excecute();
            }

            Assert.Equal("TEST", objectTest.T1);
            Assert.Equal("DONE", objectTest.T2);
        }

        [Fact]
        public void InterfaceWithoutAction()
        {
            VortexGraph g = new VortexGraph();

            g.CreateEvent("OnDelete");            
            g.Bind("OnDelete", nameof(IT2), SubClassOf<VortexAction>.GetFrom<Action2>());

            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Single(actions);

            var ActionParams = new ActionParams(objectTest);
            foreach (var actionType in actions)
            {
                var action = (VortexAction)Activator.CreateInstance(actionType.TypeOf);
                action.Initialize();
                action.SetParams(ActionParams);
                action.Excecute();
            }
            
            Assert.Equal("DONE", objectTest.T2);
        }

        [Fact]
        public void BindEventNotFound()
        {
            VortexGraph g = new VortexGraph();

            Assert.Throws<Exception>(
                () => g.Bind("OnDelete", nameof(IT1), SubClassOf<VortexAction>.GetFrom<Action1>())
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
    }
}
