using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Equilaterus.Vortex.Engine;

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

        class Action1 : Equilaterus.Vortex.Engine.Action
        {
            public override void Excecute()
            {
                var entity = _params.GetMainEntityAs<IT1>();
                if (entity != null)
                {
                    entity.T1 = "DONE";
                }
            }
        }

        class Action2 : Equilaterus.Vortex.Engine.Action
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
        public void FirstTest()
        {
            VortexGraph g = new VortexGraph();
            g.CreateEvent("OnDelete");
            g.Bind("OnDelete", "IT1", new Action1());
            g.Bind("OnDelete", "IT2", new Action2());
            ClassTest objectTest = new ClassTest();
            var actions = g.GetActions("OnDelete", objectTest.GetType());

            Assert.Equal(2, actions.Count);

            var ActionParams = new ActionParams(objectTest);

            foreach (var action in actions)
            {
                action.SetParams(ActionParams);
                action.Excecute();
            }

            Assert.Equal("DONE", objectTest.T1);
            Assert.Equal("DONE", objectTest.T2);
        }
    }
}
