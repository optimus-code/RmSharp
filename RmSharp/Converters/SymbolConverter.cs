using RmSharp.Exceptions;
using RmSharp.Extensions;
using RmSharp.Tokens;
using System.Collections.Generic;
using System.IO;

namespace RmSharp.Converters
{
    public class SymbolConverter : RmConverter
    {
        private readonly List<string> _readSymbols = [];
        private readonly List<string> _writeSymbols = [];

        public override object Read( BinaryReader reader )
        {
            var token = reader.ReadToken( RubyMarshalToken.Symbol, RubyMarshalToken.SymbolLink );
            var symbol = "";

            if ( token == RubyMarshalToken.Symbol )
            {
                symbol = reader.ReadRubyString( false );
                _readSymbols.Add( symbol );
            }
            else
            {
                var symbolID = reader.ReadFixNum<int>( );

                if ( symbolID >= _readSymbols.Count )
                    throw new RmException( $"No valid Symbol Link '{symbolID}''." );

                symbol = _readSymbols[symbolID];
            }

            return symbol;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var symbol = ( string ) instance;

            if ( HasWriteSymbol( symbol, out var id ) )
            {
                writer.Write( RubyMarshalToken.SymbolLink );
                writer.WriteFixNum( id );
            }
            else
            {
                writer.Write( RubyMarshalToken.Symbol );
                writer.WriteRubyString( symbol, false );
                _writeSymbols.Add( symbol );
            }
        }

        private bool HasWriteSymbol( string symbol, out int id )
        {
            id = _writeSymbols.IndexOf( symbol );

            return id != -1;
        }

        public void Reset( )
        {
            _writeSymbols.Clear( );
            _readSymbols.Clear( );
        }
    }
}