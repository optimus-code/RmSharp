# RmSharp

**RmSharp** is a C# .NET class library designed for deserializing and serializing Ruby Marshal files. It enables seamless translation between C# models and Ruby Marshal files, allowing you to work with Ruby data structures directly in your C# applications.

This library is built upon the work done in [HNIdesu's RubyMarshal project](https://github.com/HNIdesu/RubyMarshal). Please note that not all Ruby Marshal tokens are supported in the current version, making it unsuitable for production use at this time.

## Features

- **Deserialize** Ruby Marshal files directly into C# objects.
- **Serialize** C# objects into Ruby Marshal files.
- Support for Arrays, basic types, and objects.
- Attribute-based mapping to maintain Ruby naming conventions for classes and properties.

## Installation

You can install RmSharp via NuGet:

```shell
dotnet add package RmSharp
```

## Usage

### Deserializing Ruby Marshal Files

You can deserialize a Ruby Marshal file into your C# model as shown below:

```csharp
using (var stream = File.OpenRead(file))
{
    var instance = RmSerialiser.Deserialise<YourType>(stream);
}
```

### Serializing C# Objects to Ruby Marshal Files

To serialize your C# object into a Ruby Marshal file:

```csharp
using (var stream = File.OpenWrite(file))
{
    RmSerialiser.Serialise(stream, instance);
}
```

### Attribute-Based Mapping

To ensure that Ruby naming conventions are maintained in your C# classes, you need to use the `[RmName]` attribute:

```csharp
[RmName("ModuleName::ClassName")]
public class YourClass
{
    [RmName("ruby_name")]
    public string YourProperty { get; set; }
}
```

This will map your C# class and properties to the corresponding Ruby class and properties during serialization and deserialization.

## Limitations

- **Not Production-Ready**: This library is still in its early stages and is not ready for production use.
- **Limited Token Support**: Currently, only a subset of Ruby Marshal tokens is supported, including arrays, basic types, and objects.

## Credits

This library is based on the work done in the [RubyMarshal project by HNIdesu](https://github.com/HNIdesu/RubyMarshal). We are deeply grateful for their contributions to the Ruby Marshal serialization format.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests to improve the library.

## License

RmSharp is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.