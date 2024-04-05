﻿using RubyMarshal.Reader;
using RubyMarshal.Types;

namespace RubyMarshal
{
    public class Decoder
    {
        internal static Dictionary<byte, BaseReader> ReaderMap { get; private set; } = new()
        {
            {(byte)'[',new ArrayReader() },
            {(byte)'"',new Reader.StringReader() },
            {(byte)'l',new BignumReader() },
            {(byte)'i',new FixnumReader() },
            {(byte)'0',new NilReader() },
            {(byte)'o',new ObjectReader() },
            {(byte)':',new SymbolReader() },
            {(byte)';',new SymbolLinkReader() },
            {(byte)'I',new InstanceVariableReader() },
            {(byte)'T',new TrueReader() },
            {(byte)'F',new FalseReader() },
            {(byte)'e',new ExtendedReader()},
            {(byte)'f',new FloatReader() },
            {(byte)'d',new DataReader()},
            {(byte)'{',new HashReader() },
            {(byte)'@',new ObjectReferencesReader() },
            {(byte)'u',new UserDefinedReader() },
            {(byte)'/',new RegexReader()},
            {(byte)'S',new StructReader() },
            {(byte)'C',new UserClassReader() },
            {(byte)'U',new UserMarshalReader() },
            {(byte)'}',new DefaultHashReader() },
            {(byte)'m',new ModuleReader() },
            {(byte)'c',new ClassReader() },
        };
        public List<Symbol> SymbolList { get; private set; } = [];
        public List<Base> ObjectList { get; private set; } = [];

        private Decoder(){}

        public static Base Decode(Stream utf8Stream)
        {
            using (var br=new BinaryReader(utf8Stream))
            {
                var signature = br.ReadInt16();
                if (signature != 0x0804)
                    throw new Exception("signature mismatch");
                return ReaderMap[br.ReadByte()].Read(new Decoder(),br);
            }
                
        }
    }
}
