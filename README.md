# Serialize.EfCore

Serialize.EfCore is a .NET library that provides functionality to serialize and deserialize EfCore LINQ expressions.
This library is useful in scenarios where you need to send LINQ expressions over the wire or persist them in a database.

## Features

- Serialize and deserialize EfCore expressions to XML, and JSON.
- Supports various types of expressions, including binary, unary, member access, lambda, and more.
- Extensible design that allows you to add support for custom expressions.

## Breaking Changes

## Getting Started

### Installation

You can install Serialize.EfCore via [NuGet][1]:

```
Install-Package Serialize.EfCore
```

### Usage

Here's a simple example of how to use Serialize.EfCore:

```csharp
// Create an expression
Expression<Func<int, bool>> expression = num => num < 5;

// Create a serializer
var serializer = new ExpressionSerializer(new JsonSerializer());

// Serialize the expression
string serializedExpression = serializer.SerializeText(expression);

//Set DbContext
var context = new ExpressionContext();
context.DbContext = new DbContext(new DbContextOptions<DbContext>());

// Deserialize the expression
var deserializedExpression = serializer.DeserializeText(serializedExpression, context);
```

## Contributing

We welcome contributions to Serialize.EfCore!
If you'd like to contribute, please fork the repository, make your changes, and submit a pull request.
If you're not sure where to start, take a look at our open issues.

For bugs: make sure you create a unit test, so it is easier for me to reproduce and fix it.

## Testing

Serialize.EfCore has a comprehensive test suite. You can run the tests using your preferred test runner.

## Supported Platforms (or known to work with)

- .NET 9.0
- .NET 8.0

## License

Serialize.EfCore is licensed under the MIT License. See the LICENSE file for more details.

