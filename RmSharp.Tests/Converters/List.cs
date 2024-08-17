using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class List
    {
        private readonly byte[] _rubyMarshalData = new byte[] {

            ( byte ) RubyMarshalToken.Array, 0x09,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
            ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04,
        };
        private readonly List<short> _expectedValue = [1234, 1234, 1234, 1234];

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( List<short> ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = ( List<short> ) converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( List<short> ) );
                CollectionAssert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( List<short> ) );
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