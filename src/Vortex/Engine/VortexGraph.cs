using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexGraph
    {
        /* {OnAction, {Interface, Action} }*/
        private Dictionary<string, Dictionary<string, Action>> _graph;

        public VortexGraph()
        {
            _graph = new Dictionary<string, Dictionary<string, Action>>();
        }

        public void Bind(string instigatorEvent, string objectInterface, Action action)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");                
            }

            _graph[instigatorEvent].Add(objectInterface, action);
        }

        public void CreateEvent(string instigatorEvent)
        {
            if (_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("The event already exist.");
            }

            _graph.Add(instigatorEvent, new Dictionary<string, Action>());
        }

        public List<Action> GetActions(string instigatorEvent, Type typeEntity)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            List<Action> actions = new List<Action>();

            var implementedInterfaces = typeEntity.GetInterfaces();            
            foreach (var tinterface in implementedInterfaces)
            {
                var interfaceName = tinterface.Name;
                var graphEvent = _graph[instigatorEvent];
                if (graphEvent.ContainsKey(interfaceName))
                {
                    var action = _graph[instigatorEvent][interfaceName];
                    actions.Add(action);
                }                
            }
            return actions;
        }
    }
}