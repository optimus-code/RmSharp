using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class UInt64
    {
        private readonly byte[] _rubyMarshalData = new byte[]
        {
            (byte)RubyMarshalToken.Bignum, // Bignum token (0x6C)
            (byte)'+',                     // Sign byte ('+' for positive)
            0x08,                          // Length of the Bignum in shorts (3 shorts, 6 bytes)
            0x00, 0xF2,                   
            0x05, 0x2A,             
            0x01            
        };

        private readonly ulong _expectedValue = 5000000000;

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( ulong ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( ulong ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( ulong ) );
            using ( var memoryStream = new MemoryStream( ) )
            using ( var writer = new BinaryWriter( memoryStream ) )
            {
                converter.Write( writer, _expectedValue );

                byte[] writtenData = memoryStream.ToArray( );
                CollectionAssert.AreEqual( _rubyMarshalData, writtenData );
            }
        }
    }
}