using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class DyamicArray
    {
        private readonly byte[] _rubyMarshalData = new byte[] {

            ( byte ) RubyMarshalToken.Array, 0x0A,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.String, 0x10, 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.Double, 0x08, 0x31, 0x2E, 0x32
        };

        private readonly dynamic[] _expectedValue = [1234, "Hello World", 1234, 1234, 1.2];

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( _expectedValue.GetType() );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = ( dynamic[] ) converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, _expectedValue.GetType( ) );

                for ( var i = 0; i < result.Length; i++ )
                {
                    if ( result[i] != _expectedValue[i] )
                        Assert.Fail( $"Item at index '{i}' is not equal to '{_expectedValue[i]}' got '{result[i]}'." );
                }
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( _expectedValue.GetType( ) );
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