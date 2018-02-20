using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexGraph
    {
        /// <summary>
        /// VortexGraph representation.
        /// Dictionary {Event, {Inteface, Action }}
        /// </summary>
        private Dictionary<string, Dictionary<string, SubClassOf<VortexAction>>> _graph;

        public VortexGraph()
        {
            _graph = new Dictionary<string, Dictionary<string, SubClassOf<VortexAction>>>();
        }

        public void Bind(string instigatorEvent, string objectInterface, SubClassOf<VortexAction> action)
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

            _graph.Add(instigatorEvent, new Dictionary<string, SubClassOf<VortexAction>>());
        }

        public List<SubClassOf<VortexAction>> GetActions(string instigatorEvent, Type typeEntity)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            List<SubClassOf<VortexAction>> actions = new List<SubClassOf<VortexAction>>();
            var defaultAction = GetDefaultAction(instigatorEvent);
            if (defaultAction != null)
            {
                actions.Add(defaultAction);
            }

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

        public SubClassOf<VortexAction> GetDefaultAction(string instigatorEvent)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            SubClassOf<VortexAction> defaultAction = null;

            var interfaceName = "_default";
            var graphEvent = _graph[instigatorEvent];
            if (graphEvent.ContainsKey(interfaceName))
            {
                var action = _graph[instigatorEvent][interfaceName];
                defaultAction = action;
            }

            return defaultAction;
        }
    }
}