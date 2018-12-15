using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Filters;
using Equilaterus.Vortex.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class VortexEngine<TEntity> : IVortexEngine<TEntity> where TEntity : class
    {
        protected IVortexContext<TEntity> _context;

        protected IVortexGraph _graph;

        protected IGenericFilterFactory _filterFactory;

        public VortexEngine(IVortexGraph graph, IVortexContext<TEntity> vortexContext, IGenericFilterFactory filterFactory)
        {            
            _graph = graph;
            _context = vortexContext;
            _filterFactory = filterFactory;
        }

        public async Task<TReturn> RaiseEventAsync<TReturn>(
            string eventName, 
            TEntity entity = null,
            QueryParams<TEntity> queryParams = null, 
            params object[] parameters)
        {   
            // TODO: REFACTOR!
            // Prepare execution
            List<VortexAction<TEntity>> actionsToExecute = new List<VortexAction<TEntity>>();
            VortexReturnAction<TEntity, TReturn> returnAction = null;            

            // Get bindings
            var bindings = _graph.GetBindings(eventName, typeof(TEntity));
            foreach (var binding in bindings)
            {
                // Instantiate Vortex Actions from bindings
                foreach (var bindedActionType in binding.Actions)
                {
                    actionsToExecute.Add(
                        ReflectionTools.InstantiateAs<VortexAction<TEntity>>(bindedActionType, _context));
                }

                // Prepare execution until a return action is found
                if (returnAction == null)
                {
                    if (binding.ReturnAction != null)
                    {
                        returnAction =
                            ReflectionTools.InstantiateAs<VortexReturnAction<TEntity, TReturn>>(
                                binding.ReturnAction, _context, _filterFactory);
                        break;
                    }
                }
            }

            if (returnAction == null && queryParams != null)
            {
                throw new Exception("Expected return action not found.");
            }

            // Execute!
            foreach (var vortexAction in actionsToExecute)
            {
                await vortexAction.Execute(entity, parameters);
            }

            if (returnAction == null)
            {
                return default;
            }
            else
            {
                return await returnAction.Execute(queryParams, parameters);
            }
        }
    }
}
