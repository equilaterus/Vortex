> Write **elegant** and **testeable** solutions on C# using a Monadic Framework

## Choose your hero

### From Notation

   ```csharp
  await 
    from maybeOrder in
        from order in _orderRepository.GetById(orderId)
        select OrderBehavior.TryCheckout(order)
    from result in maybeOrder.AwaitSideEffect(_orderRepository.Update)
    select result.Match(Ok, InternalServerError("Error"));
  ```

### Fluent Notation

  ```csharp
  return await _orderRepository.GetById(orderId)    
        .Select(order => OrderBehavior.TryCheckout(order))
        .SelectMany(m => m.AwaitSideEffect(_orderRepository.Update))
        .Select(m => m.Match(Ok, InternalServerError("Error")));
  ```

## Inject Behaviors not Dependencies

  With a monadic architecture your applications are going to have a pure business logic that allows you to write easy tests and do not depend of any other layer. Also, your side effects are going to be centralized.

  Instead of injecting the dependencies in all your project, you just have to inject the behavior with the monads giving live the pure business logic interacting with the exterior world.

## Builds

* **Master** v0.3.1-alpha

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/master?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/master)  [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=master)](https://travis-ci.org/equilaterus/Vortex)

* **Dev** Unstable

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/dev?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/dev) [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=dev)](https://travis-ci.org/equilaterus/Vortex)


## Releases

* Nuget releases: [Nuget.org](https://www.nuget.org/packages/Equilaterus.Vortex)

* Github releases: [Download center](https://github.com/equilaterus/Vortex/releases)

* History: [Version change log](https://github.com/equilaterus/Vortex/wiki/Version-change-log)


## More info


* Check our [wiki](https://github.com/equilaterus/Vortex/wiki)!
