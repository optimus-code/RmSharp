using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Regex
    {
        private readonly byte[] _rubyMarshalData = new byte[] {(byte)RubyMarshalToken.RegularExpression, 0x01, 0x0b, 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0x00 };
        private readonly System.Text.RegularExpressions.Regex _expectedValue = new System.Text.RegularExpressions.Regex( "Hello World" );

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( System.Text.RegularExpressions.Regex ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( System.Text.RegularExpressions.Regex ) );
                Assert.AreEqual( _expectedValue.ToString( ), result.ToString( ) );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( System.Text.RegularExpressions.Regex ) );
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
