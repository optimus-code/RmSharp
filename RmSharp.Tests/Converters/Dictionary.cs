using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Dictionary
    {
        private readonly byte[] _rubyMarshalData = new byte[] {

            ( byte ) RubyMarshalToken.Hash, 0x01, 0x04,
            ( byte ) RubyMarshalToken.String, 0x01, 0x01, ( byte ) 'A',
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.String, 0x01, 0x01, ( byte ) 'B',
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.String, 0x01, 0x01, ( byte ) 'C',
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.String, 0x01, 0x01, ( byte ) 'D',
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
        };

        private readonly Dictionary<string, short> _expectedValue = new Dictionary<string, short>
        {
            { "A", 1234 },
            { "B", 1234 },
            { "C", 1234 },
            { "D", 1234 }
        };

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( Dictionary<string, short> ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = ( Dictionary<string, short> ) converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( Dictionary<string, short> ) );
                CollectionAssert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( Dictionary<string, short> ) );
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