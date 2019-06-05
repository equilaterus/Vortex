using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex
{
    public static class MaybeExtensions
    {
        public static TResult MatchBool<TResult>(this Maybe<bool> maybe, Func<bool, TResult> just, TResult nothing)
        {
            if (just == null)
                throw new ArgumentNullException(nameof(just));
            bool value = maybe.Match(t => t, false);
            return value ? just(value) : nothing;
        }
    }
}
