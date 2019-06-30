---
#
# By default, content added below the "---" mark will appear in the home page
# between the top bar and the list of recent posts.
# To change the home page layout, edit the _layouts/home.html file.
# See: https://jekyllrb.com/docs/themes/#overriding-theme-defaults
#
layout: page
hide_menu: true
hide_title: true
---

<div class="jumbotron bg-shadow-low" markdown="1">

# Welcome to Vortex

Write **elegant** and **testeable** solutions on C# using a Monadic Framework.

[![GitHub license](https://img.shields.io/github/license/equilaterus/Vortex.svg)](https://github.com/equilaterus/Vortex/blob/master/LICENSE) [![Build status](https://ci.appveyor.com/api/projects/status/04uwh93rktkowhvk/branch/release?svg=true)](https://ci.appveyor.com/project/dacanizares/vortex/branch/release) [![nuget](https://img.shields.io/nuget/v/Equilaterus.Vortex.svg)](https://www.nuget.org/packages/Equilaterus.Vortex/)

</div>

## Choose your hero

### From Notation

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

### Fluent Notation

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

## Inject Behaviors not Dependencies

  With a monadic architecture your applications are going to have a pure business logic that allows you to write easy tests and do not depend of any other layer. Also, your side effects are going to be centralized.

  Instead of injecting the dependencies in all your project, you just have to inject the behavior with the monads giving live the pure business logic interacting with the exterior world.
