# Vortex

**Check our [Official Site](https://equilaterus.github.io/Vortex/).**

Vortex is a .Net Standard framework focused on **Functional Programming**.

Write **elegant** and **testeable** solutions on C# using a Monadic Framework that comes in two flavours:

* From notation 

  ```csharp
  await 
    from maybeOrder in
        from order in _orderRepository.GetById(orderId)
        select OrderBehavior.TryCheckout(order)
    from result in maybeOrder.AwaitSideEffect(_orderRepository.Update)
    select result.Match(Ok, InternalServerError("Error"));
  ```

* Fluent notation

  ```csharp
  return await _orderRepository.GetById(orderId)    
        .Select(order => OrderBehavior.TryCheckout(order))
        .SelectMany(m => m.AwaitSideEffect(_orderRepository.Update))
        .Select(m => m.Match(Ok, InternalServerError("Error")));
  ```

## Builds

* **Master** v0.3.1-alpha

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/master?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/master)  [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=master)](https://travis-ci.org/equilaterus/Vortex)
  [![nuget](https://img.shields.io/nuget/v/Equilaterus.Vortex.svg)](https://www.nuget.org/packages/Equilaterus.Vortex/)

* **Dev** Unstable

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/dev?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/dev) [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=dev)](https://travis-ci.org/equilaterus/Vortex)


## Releases

* Github releases: [Download center](https://github.com/equilaterus/Vortex/releases)

* History: [Version change log](https://github.com/equilaterus/Vortex/wiki/Version-change-log)


## More info


* Check our [wiki](https://github.com/equilaterus/Vortex/wiki)!

