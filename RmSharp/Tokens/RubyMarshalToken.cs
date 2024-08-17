namespace RmSharp.Tokens
{
    /// <summary>
    /// Enum representing the tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyMarshalToken : byte
    {
        /// <summary>
        /// Represents an object ('o' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate the start of a serialized Ruby object.
        /// </remarks>
        Object = 0x6F,

        /// <summary>
        /// Represents a symbol (':' token).
        /// </summary>
        /// <remarks>
        /// This token is used for Ruby symbols, often as identifiers or property names in serialization.
        /// </remarks>
        Symbol = 0x3A,

        /// <summary>
        /// Represents a symbol link (';' token).
        /// </summary>
        /// <remarks>
        /// This token refers to a previously serialized symbol, avoiding duplication and saving space.
        /// </remarks>
        SymbolLink = 0x3B,

        /// <summary>
        /// Represents an object reference ('@' token).
        /// </summary>
        /// <remarks>
        /// This token is used to reference an object that has already been serialized, avoiding duplication.
        /// </remarks>
        ObjectReference = 0x40,

        /// <summary>
        /// Represents an instance variable ('I' token).
        /// </summary>
        /// <remarks>
        /// This token indicates that the object has instance variables that follow the token.
        /// </remarks>
        InstanceVariable = 0x49,

        /// <summary>
        /// Represents an extended module ('e' token).
        /// </summary>
        /// <remarks>
        /// This token is used when an object is extended by a module.
        /// </remarks>
        Extended = 0x65,

        /// <summary>
        /// Represents user-defined data serialization logic ('u' token).
        /// </summary>
        /// <remarks>
        /// This token is used for objects that have custom serialization logic.
        /// </remarks>
        UserDefined = 0x75,

        /// <summary>
        /// Represents a user-defined marshal ('U' token).
        /// </summary>
        /// <remarks>
        /// This token indicates that the object has a custom marshalling method.
        /// </remarks>
        UserMarshal = 0x55,

        /// <summary>
        /// Represents a module ('m' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby modules.
        /// </remarks>
        Module = 0x6D,

        /// <summary>
        /// Represents an array ('[' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate the start of a serialized Ruby array.
        /// </remarks>
        Array = 0x5B,

        /// <summary>
        /// Represents a regular expression ('/' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby regular expressions (Regexp).
        /// </remarks>
        RegularExpression = 0x2F,

        /// <summary>
        /// Represents a structure ('S' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby structs.
        /// </remarks>
        Struct = 0x53,

        /// <summary>
        /// Represents a hash ('{' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby hashes.
        /// </remarks>
        Hash = 0x7B,

        /// <summary>
        /// Represents a class ('c' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby classes.
        /// </remarks>
        Class = 0x63,

        /// <summary>
        /// Represents user-defined data ('d' token).
        /// </summary>
        /// <remarks>
        /// This token indicates user-defined data types that require special handling.
        /// </remarks>
        Data = 0x64,

        /// <summary>
        /// Represents a user-defined class ('C' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize an object that belongs to a user-defined class.
        /// </remarks>
        UserClass = 0x43,

        /// <summary>
        /// Represents a default hash ('}' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby hashes that have a default value.
        /// </remarks>
        DefaultHash = 0x7D,

        // Basic value tokens

        /// <summary>
        /// Represents a nil value ('0' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a nil value in Ruby, which is equivalent to a null value in C#.
        /// </remarks>
        Nil = 0x30,

        /// <summary>
        /// Represents a true value ('T' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a true boolean value in Ruby.
        /// </remarks>
        True = 0x54,

        /// <summary>
        /// Represents a false value ('F' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a false boolean value in Ruby.
        /// </remarks>
        False = 0x46,

        /// <summary>
        /// Represents a string ('"' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby strings.
        /// </remarks>
        String = 0x22,

        /// <summary>
        /// Represents a bignum ('l' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize large integers (Bignum) in Ruby.
        /// </remarks>
        Bignum = 0x6C,

        /// <summary>
        /// Represents a fixnum ('i' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize small integers (Fixnum) in Ruby.
        /// </remarks>
        Fixnum = 0x69,

        /// <summary>
        /// Represents a floating-point number ('f' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize floating-point numbers (Float) in Ruby.
        /// </remarks>
        Double = 0x66,
    }
}