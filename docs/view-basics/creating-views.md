# Creating views

There are multipe ways of creating a view for a certain control. They all have in common that the resulting type is `IView` or `IView<'t>`.

## Creating views for common controls

FuncUI provides functions for creating standard avalonia controls. The create function always follows the same pattern.\


<pre class="language-fsharp" data-title="internal - signature"><code class="lang-fsharp"><strong>module Button =
</strong><strong>    val create: attrs: IAttr&#x3C;'control> list -> IView&#x3C;'control>
</strong></code></pre>

{% code title="user code" %}
```fsharp
Button.create [
    // view attributes
]
```
{% endcode %}

{% hint style="success" %}
The create function is always found on the module named the same as the Avalonia control.

So the create function for a `TextBlock` is `TextBlock.create`, for a `StackPanel` it's `StackPanel.create` and so on.&#x20;
{% endhint %}

## Creating views for custom controls

Even without creating bindings for a control you can create and embedd it in a view.&#x20;

```fsharp
View.createGeneric<MyCustomControl> [
    // view attributes
]
```

### Passing constructor arguments

Sometimes controls dont have a unit constructor and need constructor arguments. Here is how you  can pass them.

```fsharp
[
    // view attributes
]
|> View.createGeneric<MyCustomControl> 
|> View.withConstructorArgs [| "constructorArg1" :> obj; "constructorArg2" :> obj |]
    

```
