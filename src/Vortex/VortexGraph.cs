using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Equilaterus.Vortex
{
    public class VortexGraph : IVortexGraph
    {
        /// <summary>
        /// VortexGraph representation.
        /// Dictionary {Event, {Instigator, Actions }}
        /// </summary>
        protected Dictionary<string, Dictionary<string, List<VortexBinding>>> _graph;
                
        public VortexGraph()
        {
            _graph = new Dictionary<string, Dictionary<string, List<VortexBinding>>>();
        }

        public void Bind(string eventName, Type instigator, VortexBinding binding)
        {
            if (!_graph.ContainsKey(eventName))
            {
                throw new Exception("Event not found.");
            }

            var instigatorName = nameof(instigator);
            if (!_graph[eventName].ContainsKey(instigatorName))
            {
                _graph[eventName].Add(instigatorName, new List<VortexBinding>());
            }
            _graph[eventName][instigatorName].Add(binding);
        }

        public void CreateEvent(string eventName)
        {
            if (eventName == null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (_graph.ContainsKey(eventName))
            {
                throw new Exception("Event already exist.");
            }

            _graph.Add(eventName, new Dictionary<string, List<VortexBinding>>());
        }

        public List<VortexBinding> GetBindings(string eventName, Type instigator)
        {
            if (!_graph.ContainsKey(eventName))
            {
                throw new Exception("Event not found.");
            }

            List<VortexBinding> bindings = new List<VortexBinding>();
            var defaultBindings = GetDefaultBindings(eventName);
            if (defaultBindings != null)
            {
                bindings.AddRange(defaultBindings);
            }

            var implementedInterfaces = instigator.GetInterfaces();
            foreach (var interfaceType in implementedInterfaces)
            {
                var interfaceName = interfaceType.Name;
                var graphEvent = _graph[eventName];
                if (graphEvent.ContainsKey(interfaceName))
                {
                    var action = _graph[eventName][interfaceName];
                    bindings.AddRange(action);
                }
            }
            bindings.Sort((VortexBinding x, VortexBinding y) => {
                return y.Priority.CompareTo(x.Priority);
            });


            return bindings.TakeWhile(e => e.ApplyLowerPriorityActions).ToList();
        }

        private List<VortexBinding> GetDefaultBindings(string eventName)
        {
            var interfaceName = "_default";
            var graphEvent = _graph[eventName];
            if (!graphEvent.ContainsKey(interfaceName))            
                return null;            
            else 
                return _graph[eventName][interfaceName];
        }
    }
}