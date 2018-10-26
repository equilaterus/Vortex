using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;

namespace Equilaterus.Vortex
{
    public class VortexGraph : IVortexGraph
    {
        /// <summary>
        /// VortexGraph representation.
        /// Dictionary {Event, {Instigator, Actions }}
        /// </summary>
        protected Dictionary<string, Dictionary<string, List<SubTypeOf<IVortexAction>>>> _graph;
                
        public VortexGraph()
        {
            _graph = new Dictionary<string, Dictionary<string, List<SubTypeOf<VortexAction>>>>();
        }

        public void Bind(string instigatorEvent, string objectInterface, SubTypeOf<VortexAction> action)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            if (!_graph[instigatorEvent].ContainsKey(objectInterface))
            {
                _graph[instigatorEvent].Add(objectInterface, new List<SubTypeOf<VortexAction>>());
            }
            _graph[instigatorEvent][objectInterface].Add(action);
        }

        public void CreateEvent(string instigatorEvent)
        {
            if (_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("The event already exist.");
            }

            _graph.Add(instigatorEvent, new Dictionary<string, List<SubTypeOf<VortexAction>>>());
        }

        public List<SubTypeOf<VortexAction>> GetActions(string instigatorEvent, Type typeEntity)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            List<SubTypeOf<VortexAction>> actions = new List<SubTypeOf<VortexAction>>();
            var defaultAction = GetDefaultActions(instigatorEvent);
            if (defaultAction != null)
            {
                actions.AddRange(defaultAction);
            }

            var implementedInterfaces = typeEntity.GetInterfaces();
            foreach (var tinterface in implementedInterfaces)
            {
                var interfaceName = tinterface.Name;
                var graphEvent = _graph[instigatorEvent];
                if (graphEvent.ContainsKey(interfaceName))
                {
                    var action = _graph[instigatorEvent][interfaceName];
                    actions.AddRange(action);
                }
            }
            return actions;
        }

        public List<SubTypeOf<VortexAction>> GetDefaultActions(string instigatorEvent)
        {
            if (!_graph.ContainsKey(instigatorEvent))
            {
                throw new Exception("Instigator Event not found.");
            }

            List<SubTypeOf<VortexAction>> defaultAction = null;

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