﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class VortexEngine<T> : IVortexEngine<T> where T : class
    {
        protected IVortexContext<T> _context;

        protected IVortexGraph _graph;

        public VortexEngine() { }

        public VortexEngine(IVortexGraph graph, IVortexContext<T> vortexContext)
        {            
            _graph = graph;
            _context = vortexContext;
        }

        public async Task<VortexData> RaiseEventAsync(string vortexEvent, VortexData actionParams)
        {
            List<VortexAction> actionsToExecute = new List<VortexAction>();
            var actions = _graph.GetActions(vortexEvent, typeof(T));
            foreach (var actionType in actions)
            {
                VortexAction vortexAction;
                if (actionType.TypeOf.ContainsGenericParameters)
                {
                    Type[] typeArgs = { typeof(T) };
                    var genericType = actionType.TypeOf.MakeGenericType(typeArgs);
                    vortexAction = (VortexAction)Activator.CreateInstance(genericType, _context);
                }
                else
                {
                    vortexAction = (VortexAction)Activator.CreateInstance(actionType.TypeOf, _context);
                }
                vortexAction.Initialize();
                vortexAction.Params = actionParams;
                actionsToExecute.Add(vortexAction);
            }

            actionsToExecute.Sort((VortexAction x, VortexAction y) => {
                return y.Priority.CompareTo(x.Priority);
            });

            foreach (var vortexAction in actionsToExecute)
            {
                await vortexAction.Execute();

                if (vortexAction.IsReturnAction)
                {
                    return vortexAction.Results;
                }

                if (vortexAction.PreventDefault)
                {
                    break;
                }
            }
            return null;
        }
    }
}