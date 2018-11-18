using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Equilaterus.Vortex
{
    public class VortexGraph : IVortexGraph
    {
        public const string DEFAULT_INSTIGATOR = "_default";

        /// <summary>
        /// VortexGraph representation.
        /// Dictionary {Event, {Instigator, Actions }}
        /// </summary>
        protected Dictionary<string, Dictionary<string, List<VortexBinding>>> _graph;
                
        public VortexGraph()
        {
            _graph = new Dictionary<string, Dictionary<string, List<VortexBinding>>>();
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

        public void Bind(string eventName, VortexBinding binding, string instigator = null)
        {
            if (eventName == null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }
            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            if (!_graph.ContainsKey(eventName))
            {
                throw new Exception("Event not found.");
            }

            var instigatorName = instigator ?? DEFAULT_INSTIGATOR;
            if (!_graph[eventName].ContainsKey(instigatorName))
            {
                _graph[eventName].Add(instigatorName, new List<VortexBinding>());
            }
            _graph[eventName][instigatorName].Add(binding);
        }

        public List<VortexBinding> GetBindings(string eventName, params string[] instigators)
        {
            if (eventName == null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }            

            if (!_graph.ContainsKey(eventName))
            {
                throw new Exception("Event not found.");
            }

            List<VortexBinding> bindings = new List<VortexBinding>();

            // Set defaults bindings
            var defaultBindings = GetDefaultBindings(eventName);
            if (defaultBindings != null)
            {
                bindings.AddRange(defaultBindings);
            }

            // Set instigator bindings
            foreach (var instigator in instigators)
            {
                var graphEvent = _graph[eventName];
                if (graphEvent.ContainsKey(instigator))
                {
                    var action = _graph[eventName][instigator];
                    bindings.AddRange(action);
                }
            }
            
            // Prepare bindings according to metadata
            return PrepareBindings(bindings);
        }
        
        protected List<VortexBinding> GetDefaultBindings(string eventName)
        {
            var graphEvent = _graph[eventName];
            if (!graphEvent.ContainsKey(DEFAULT_INSTIGATOR))            
                return null;            
            else 
                return _graph[eventName][DEFAULT_INSTIGATOR];
        }

        protected List<VortexBinding> PrepareBindings(List<VortexBinding> bindings)
        {
            bindings.Sort((VortexBinding x, VortexBinding y) => {
                return y.Priority.CompareTo(x.Priority);
            });

            var result = new List<VortexBinding>();
            foreach (var binding in bindings)
            {
                result.Add(binding);
                if (!binding.ApplyLowerPriorityActions)
                    break;
            }
            return result;
        }
    }
}