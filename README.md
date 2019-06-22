# Vortex

Vortex is a .Net Standard framework focused on **Functional Programming**.

Write **elegant** and **testeable** solutions on C# using a Monadic Framework that comes in two flavours:

* From notation 

  ```csharp
  await 
    // Try to create an order
    from maybeOrder in
        from order in _orderRepository.GetByIdAsync(orderId)
        select OrderBehavior.TryCheckout(order)

    // Update database
    from result in maybeOrder.AwaitSideEffect(_orderRepository.UpdateAsync)

    // Return results
    select result.Match(Ok, InternalServerError("Error"));
  ```

* Fluent notation

  ```csharp
  return await 
      // Try to create an order
      _orderRepository.GetByIdAsync(orderId)    
      .Select(order => OrderBehavior.TryCheckout(order))

      // Update database
      .SelectMany(m => m.AwaitSideEffect(_orderRepository.UpdateAsync))
      
      // Return results
      .Select(m => m.Match(Ok, InternalServerError("Error")));
  ```

## Builds

* **Release**

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/release?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/release)  [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=release)](https://travis-ci.org/equilaterus/Vortex)
  [![nuget](https://img.shields.io/nuget/v/Equilaterus.Vortex.svg)](https://www.nuget.org/packages/Equilaterus.Vortex/)

* **Master**

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/master?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/master)  [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=master)](https://travis-ci.org/equilaterus/Vortex)

* **Dev** Unstable

  [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/dev?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/dev) [![Build Status](https://travis-ci.org/equilaterus/Vortex.svg?branch=dev)](https://travis-ci.org/equilaterus/Vortex)


## Releases

* Download [nuget package](https://www.nuget.org/packages/Equilaterus.Vortex/)  

* Download [GitHub releases](https://github.com/equilaterus/Vortex/releases)

* See [version change log](https://github.com/equilaterus/Vortex/wiki/Version-change-log)


## Links

* [Official website](https://equilaterus.github.io/Vortex/)

* [Sample Apps](https://github.com/equilaterus/Vortex.Samples)

* [Vortex Wiki](https://github.com/equilaterus/Vortex/wiki)
