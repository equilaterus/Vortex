using System;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class Maybe<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        public Maybe()
        {
            _hasValue = false;
        }

        public Maybe(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _value = value;
            _hasValue = true;
        }

        // ma -> (a -> b) -> mb
        public Maybe<TResult> Select<TResult>(Func<T, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (!_hasValue)
                return new Maybe<TResult>();

            return new Maybe<TResult>(function(_value));
        }

        // ma -> (a -> mb) -> mb
        public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (!_hasValue)
                return new Maybe<TResult>();

            return function(_value);
        }

        // ma -> b -> (a -> b) -> b
        public TResult Match<TResult>(Func<T, TResult> just, TResult nothing)
        {
            if (just == null)
                throw new ArgumentNullException(nameof(just));

            return _hasValue ? just(_value) : nothing;
        }
                
        // ma -> (a -> tb) -> tmb
        public async Task<Maybe<TResult>> AwaitSideEffect<TResult>(Func<T, Task<TResult>> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (!_hasValue)
                return new Maybe<TResult>();

            var result = await function(_value);
            return result == null ?
                new Maybe<TResult>() :
                new Maybe<TResult>(result);
        }
    }
}
