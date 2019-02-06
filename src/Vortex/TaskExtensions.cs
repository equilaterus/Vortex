using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public static class TaskExtensions
    {
        // ma -> (a -> b) -> mb
        public static async Task<TResult> Select<TSource, TResult> (
            this Task<TSource> source, 
            Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(await source);
        }

        // ma -> (a -> mb) -> mb
        public static async Task<TResult> SelectMany<TSource, TResult> (
            this Task<TSource> source, 
            Func<TSource, Task<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return await selector(await source);
        }

        public static async Task<TResult> SelectMany<TSource, TSelector, TResult>(
            this Task<TSource> source, 
            Func<TSource, Task<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            TSource value = await source;
            return resultSelector(value, await selector(value));            
        }
    }
}
