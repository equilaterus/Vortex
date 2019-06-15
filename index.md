---
#
# By default, content added below the "---" mark will appear in the home page
# between the top bar and the list of recent posts.
# To change the home page layout, edit the _layouts/home.html file.
# See: https://jekyllrb.com/docs/themes/#overriding-theme-defaults
#
layout: page
is_home: true
---

![](https://camo.githubusercontent.com/f9710988c1416e0e8da4a9e8f838980792b763b7/68747470733a2f2f73636f6e74656e742e66656f68332d312e666e612e666263646e2e6e65742f762f74312e302d392f36323139313033315f323232373033313435343034383233365f383039373436383134343238323530313132305f6f2e706e673f5f6e635f6361743d313039265f6e635f657569323d416548646379795a34743845414c633969486b316f67726f4a4858575447557049616c466258554a324d544b6a4849482d354633666f7571516d494b54495a6c4547467a48565175593035737932777344344c305646683459614352456a5a6943543375736c674b364836504277265f6e635f68743d73636f6e74656e742e66656f68332d312e666e61266f683d6531376134316361313132613638396134303537303537303066623637353430266f653d3544393430453733)

Write **elegant** and **testeable** solutions on C# using a Monadic Framework.

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
