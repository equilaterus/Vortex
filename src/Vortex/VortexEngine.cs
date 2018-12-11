using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Filters;
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
                VortexAction<TEntity> vortexAction;

                // Instantiate Vortex Actions from bindings
                foreach (var bindedActionType in binding.Actions)
                {
                    if (!bindedActionType.ContainsGenericParameters)
                    {
                        vortexAction = (VortexAction<TEntity>)Activator.CreateInstance(bindedActionType, _context);
                    }
                    else
                    {
                        Type[] typeArgs = { typeof(TEntity) };
                        var genericType = bindedActionType.MakeGenericType(typeArgs);
                        vortexAction = (VortexAction<TEntity>)Activator.CreateInstance(genericType, _context);
                    }
                    actionsToExecute.Add(vortexAction);
                }

                // Keep first return action (if exists)
                if (returnAction == null)
                {
                    if (binding.ReturnAction != null)
                    {
                        if (!binding.ReturnAction.ContainsGenericParameters)
                        {
                            returnAction = (VortexReturnAction<TEntity, TReturn>)Activator.CreateInstance(binding.ReturnAction, _context, _filterFactory);
                        }
                        else
                        {
                            Type[] typeArgs;
                            if (binding.ReturnAction.GetGenericArguments().Count() == 1)
                            {
                                typeArgs = new Type[] { typeof(TEntity) };
                            }
                            // TODO: Discuss if we really need this:
                            else
                            {
                                typeArgs = new Type[] { typeof(TEntity), typeof(TReturn) };
                            }

                            var genericType = binding.ReturnAction.MakeGenericType(typeArgs);
                            returnAction = (VortexReturnAction<TEntity, TReturn>)Activator.CreateInstance(genericType, _context, _filterFactory);
                        }
                    }
                }
            }

            // Execute!
            foreach (var vortexAction in actionsToExecute)
            {
                await vortexAction.Execute(entity, parameters);
            }

            if (returnAction == null)
            {
                // TODO: Discuss this
                if (queryParams != null)
                {
                    throw new Exception("Expected return action not found.");
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return await returnAction.Execute(queryParams, parameters);
            }
        }
    }
}
