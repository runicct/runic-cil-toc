/*
 * MIT License
 * 
 * Copyright (c) 2026 Runic Compiler Toolkit Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        internal class Signature
        {
            internal static uint ReadCompressedInteger(byte[] data, ref uint offset)
            {
                byte firstByte = data[offset]; offset++;
                if ((firstByte & 0x80) == 0) { return (uint)firstByte; }
                byte secondByte = data[offset]; offset++;
                if ((firstByte & 0x40) == 0) { return (uint)(((uint)firstByte << 8) | (uint)secondByte) & 0x3FFF; }
                byte thirdByte = data[offset]; offset++;
                byte forthByte = data[offset]; offset++;

                return (uint)(((uint)firstByte << 24) | ((uint)secondByte << 16) | ((uint)thirdByte << 8) | ((uint)forthByte));
            }
            internal static void EncodeCompressedInteger(uint value, List<byte> signature)
            {
                if (value <= 0x7F)
                {
                    signature.Add((byte)(value & 0x7F));
                }
                else if (value <= 0x3FFF)
                {
                    signature.Add((byte)(((value >> 8) & 0x3F) | 0x80));
                    signature.Add((byte)(value & 0xFF));
                }
                else
                {
                    signature.Add((byte)(((value >> 24) & 0x3F) | 0xC0));
                    signature.Add((byte)((value >> 16) & 0xFF));
                    signature.Add((byte)((value >> 8) & 0xFF));
                    signature.Add((byte)(value & 0xFF));
                }
            }

            internal class Type
            {
                public virtual string ToC(Context context) { return "void*"; }
                public virtual void Emit(List<byte> output)
                {
                }
                public virtual Signature.Type ToUnsigned() { return this; }
                internal class Unknown : Type
                {
                    static Unknown _instance = new Unknown();
                    public static Unknown Instance { get { return _instance; } }
                    public override void Emit(List<byte> output) { output.Add(0x1C); }
                }
                internal class Void : Type
                {
                    static Void _instance = new Void();
                    public static Void Instance { get { return _instance; } }
                    public override void Emit(List<byte> output) { output.Add(0x01); }
                    public override string ToC(Context context) { return "void"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class Object : Type
                {
                    static Object _instance = new Object();
                    public static Object Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x1C };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x1C); }
                    public override string ToC(Context context) { return "void*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class Bool : Type
                {
                    static Bool _instance = new Bool();
                    public static Bool Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x02 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x02); }
                    public override string ToC(Context context) { return "uint32_t"; }
                }
                internal class Char : Type
                {
                    static Char _instance = new Char();
                    public static Char Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x03 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x03); }
                    public override string ToC(Context context) { return "uint16_t"; }
                }
                internal class UInt64 : Type
                {
                    static UInt64 _instance = new UInt64();
                    public static UInt64 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x0B };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x0B); }
                    public override string ToC(Context context) { return "uint64_t"; }
                }
                internal class Int64 : Type
                {
                    static Int64 _instance = new Int64();
                    public static Int64 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x0A };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x0A); }
                    public override string ToC(Context context) { return "int64_t"; }
                    public override Signature.Type ToUnsigned() { return Type.UInt64.Instance; }
                }
                internal class UInt32 : Type
                {
                    static UInt32 _instance = new UInt32();
                    public static UInt32 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x09 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x09); }
                    public override string ToC(Context context) { return "uint32_t"; }

                }
                internal class Int32 : Type
                {
                    static Int32 _instance = new Int32();
                    public static Int32 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x08 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x08); }
                    public override string ToC(Context context) { return "int32_t"; }
                    public override Signature.Type ToUnsigned() { return Type.UInt32.Instance; }
                }
                internal class UInt16 : Type
                {
                    static UInt16 _instance = new UInt16();
                    public static UInt16 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x07 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x07); }
                    public override string ToC(Context context) { return "uint16_t"; }
                    public override Signature.Type ToUnsigned() { return Type.UInt16.Instance; }
                }
                internal class Int16 : Type
                {
                    static Int16 _instance = new Int16();
                    public static Int16 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x06 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x06); }
                    public override string ToC(Context context) { return "int16_t"; }
                }
                internal class UInt8 : Type
                {
                    static UInt8 _instance = new UInt8();
                    public static UInt8 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x05 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x05); }
                    public override string ToC(Context context) { return "uint8_t"; }
                }
                internal class Int8 : Type
                {
                    static Int8 _instance = new Int8();
                    public static Int8 Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x04 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x04); }
                    public override string ToC(Context context) { return "int8_t"; }
                    public override Signature.Type ToUnsigned() { return Type.UInt8.Instance; }
                }
                internal class Float : Type
                {
                    static Float _instance = new Float();
                    public static Float Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x0C };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x0C); }
                    public override string ToC(Context context) { return "float"; }
                }
                internal class Double : Type
                {
                    static Double _instance = new Double();
                    public static Double Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x0D };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x0D); }
                    public override string ToC(Context context) { return "double"; }
                }
                internal class NInt : Type
                {
                    static NInt _instance = new NInt();
                    public static NInt Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x18 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x18); }
                    public override string ToC(Context context) { return "intptr_t"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class NUInt : Type
                {
                    static NUInt _instance = new NUInt();
                    public static NUInt Instance { get { return _instance; } }
                    static byte[] _standaloneSignature = new byte[] { 0x19 };
                    public static byte[] StandaloneSignature { get { return _standaloneSignature; } }
                    public override void Emit(List<byte> output) { output.Add(0x19); }
                    public override string ToC(Context context) { return "uintptr_t"; }
                }
                internal class String : Type
                {
                    static String _instance = new String();
                    public static String Instance { get { return _instance; } }
                    public override void Emit(List<byte> output) { output.Add(0x0E); }
                    public override string ToC(Context context) { return context.GetStringTypeName() + "*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class Pointer : Type
                {
                    Type _target;
                    public Type Target { get { return _target; } }
                    public Pointer(Type target)
                    {
                        _target = target;
                    }
                    public override void Emit(List<byte> output) { output.Add(0x0F); _target.Emit(output); }
                    public override string ToC(Context context) { return _target.ToC(context) + "*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }

                }
                internal class ByRef : Type
                {
                    Type _target;
                    public Type Target { get { return _target; } }
                    public ByRef(Type target)
                    {
                        _target = target;
                    }
                    public override void Emit(List<byte> output) { output.Add(0x10); _target.Emit(output); }
                    public override string ToC(Context context) { return _target.ToC(context) + "*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class TypeToken : Type
                {
                    uint _token;
                    public uint Token { get { return _token; } }
                    public TypeToken(uint token)
                    {
                        _token = token;
                    }
                    public override void Emit(List<byte> output)
                    {
                        output.Add(0x12);
                        EncodeCompressedInteger(TokenToTypeDefOrRefOrSpec(_token), output);
                    }
                    public override string ToC(Context context) { return context.GetTypeName(_token) + "*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class ArrayType : Type
                {
                    Type _elementType;
                    public Type ElementType { get { return _elementType; } }
                    public ArrayType(Type elementType)
                    {
                        _elementType = elementType;
                    }
                    public override void Emit(List<byte> output) { output.Add(0x1D); _elementType.Emit(output); }
                    public override string ToC(Context context) { return "void*"; }
                    public override Signature.Type ToUnsigned() { return Type.NUInt.Instance; }
                }
                internal class GenericTypeInType : Type
                {
                    uint _index;
                    public GenericTypeInType(uint index)
                    {
                        _index = index;
                    }
                    public override void Emit(List<byte> output)
                    {
                        output.Add(0x13);
                        EncodeCompressedInteger(_index, output);
                    }
                    public override string ToC(Context context) { return "TT" + _index.ToString(); }
                }

                internal class GenericTypeInMethod : Type
                {
                    uint _index;
                    public GenericTypeInMethod(uint index)
                    {
                        _index = index;
                    }
                    public override void Emit(List<byte> output)
                    {
                        output.Add(0x1E);
                        EncodeCompressedInteger(_index, output);
                    }
                    public override string ToC(Context context) { return "TM" + _index.ToString(); }
                }
                internal class GenericTypeInstantiation : Type
                {
                    Type _type;
                    Type[] _args;
                    public GenericTypeInstantiation(Type type, Type[] args)
                    {
                        _type = type;
                        _args = args;
                    }
                    public override void Emit(List<byte> output)
                    {
                        output.Add(0x15);
                        _type.Emit(output);
                        EncodeCompressedInteger((uint)_args.Length, output);
                        for (int n = 0; n < _args.Length; n++)
                        {
                            _args[n].Emit(output);
                        }
                    }
                    public override string ToC(Context context)
                    {
                        return _type.ToC(context) + "(" + string.Join(", ", _args.Select(a => a.ToC(context))) + ")";
                    }
                }
                internal class ValueType : Type
                {
                    uint _token;
                    public ValueType(uint token)
                    {
                        _token = token;
                    }
                    public override void Emit(List<byte> output)
                    {
                        output.Add(0x11);
                        EncodeCompressedInteger(TokenToTypeDefOrRefOrSpec(_token), output);
                    }
                    public override string ToC(Context context) { return context.GetTypeName(_token); }
                }
                internal class Sentinel : Type
                {
                    public Sentinel() { }
                    public override void Emit(List<byte> output) { output.Add(0x41); }
                }
            }

            static uint TokenToTypeDefOrRefOrSpec(uint token)
            {
                switch (token & 0xFF000000)
                {
                    case 0x02000000: return ((token & 0x00FFFFFF) << 2) | (0x00);
                    case 0x01000000: return ((token & 0x00FFFFFF) << 2) | (0x01);
                    case 0x1B000000: return ((token & 0x00FFFFFF) << 2) | (0x02);
                    default: throw new ArgumentException("Invalid token");
                }
            }
            static uint TypeDefOrRefOrSpecToToken(uint input)
            {
                switch (input & 0x03)
                {
                    case 0x00: return (input >> 2) | 0x02000000;
                    case 0x01: return (input >> 2) | 0x01000000;
                    case 0x02: return (input >> 2) | 0x1B000000;
                    default: throw new ArgumentException("Invalid token");
                }
            }
            bool IsVoidType(byte[] signature, ref uint index)
            {
                switch (signature[index])
                {
                    case 0x01: return true;
                    case 0x1F:
                        {
                            index++;
                            ReadCompressedInteger(signature, ref index);
                            return IsVoidType(signature, ref index);
                        }
                    default:
                        return false;
                }
            }

            static internal Signature.Type DecodeType(byte[] signature, ref uint offset)
            {
                byte firstByte = signature[offset];
                offset++;
                switch (firstByte)
                {
                    case 0x01: return Signature.Type.Void.Instance;
                    case 0x02: return Signature.Type.Bool.Instance;
                    case 0x03: return Signature.Type.Char.Instance;
                    case 0x04: return Signature.Type.Int8.Instance;
                    case 0x05: return Signature.Type.UInt8.Instance;
                    case 0x06: return Signature.Type.Int16.Instance;
                    case 0x07: return Signature.Type.UInt16.Instance;
                    case 0x08: return Signature.Type.Int32.Instance;
                    case 0x09: return Signature.Type.UInt32.Instance;
                    case 0x0A: return Signature.Type.Int64.Instance;
                    case 0x0B: return Signature.Type.UInt64.Instance;
                    case 0x0C: return Signature.Type.Float.Instance;
                    case 0x0D: return Signature.Type.Double.Instance;
                    case 0x0E: return Signature.Type.String.Instance;
                    case 0x0F: return new Signature.Type.Pointer(DecodeType(signature, ref offset));
                    case 0x10: return new Signature.Type.ByRef(DecodeType(signature, ref offset));
                    case 0x13: return new Signature.Type.GenericTypeInType(ReadCompressedInteger(signature, ref offset));
                    case 0x1E: return new Signature.Type.GenericTypeInMethod(ReadCompressedInteger(signature, ref offset));
                    case 0x15:
                        {
                            Signature.Type type = DecodeType(signature, ref offset);
                            uint argCount = ReadCompressedInteger(signature, ref offset);
                            Signature.Type[] args = new Signature.Type[argCount];
                            for (uint n = 0; n < argCount; n++)
                            {
                                args[n] = DecodeType(signature, ref offset);
                            }
                            return new Signature.Type.GenericTypeInstantiation(type, args);
                        }
                    case 0x18: return Signature.Type.NInt.Instance;
                    case 0x19: return Signature.Type.NUInt.Instance;
                    case 0x1C: return Signature.Type.Object.Instance;
                    case 0x1D: return new Signature.Type.ArrayType(DecodeType(signature, ref offset));
                    case 0x11: return new Signature.Type.ValueType(TypeDefOrRefOrSpecToToken(ReadCompressedInteger(signature, ref offset)));
                    case 0x12: return new Signature.Type.TypeToken(TypeDefOrRefOrSpecToToken(ReadCompressedInteger(signature, ref offset)));
                    case 0x41: return new Signature.Type.Sentinel();
                    default:
                        throw new ArgumentException("Invalid type signature");
                }
            }

            internal class MethodSignature : Signature
            {
                uint _genericParametersCount;
                public uint GenericParametersCount { get { return _genericParametersCount; } }
                uint _parametersCount;
                public uint ParametersCount { get { return _parametersCount; } }
                public bool ReturnVoid { get { return _returnType is Signature.Type.Void; } }
                bool _hasThis = false;
                public bool HasThis { get { return _hasThis; } }
                bool _explicitThis = false;
                Signature.Type _returnType;
                public Signature.Type ReturnType { get { return _returnType; } }
                Signature.Type[] _parameters;
                public Signature.Type[] GetParameters() { return _parameters; }
                public Signature.Type GetParameterType(int index)
                {
                    if (index < 0 || index >= _parametersCount) { return Signature.Type.Unknown.Instance; }
                    return _parameters[index];
                }
                public MethodSignature(byte[] signature)
                {
                    if (signature == null || signature.Length == 0)
                    {
                        _returnType = Signature.Type.Void.Instance;
                        _parametersCount = 0;
                        _parameters = new Signature.Type[0];
                        return;
                    }

                    uint paramSignatureByteIndex = 0;

                    byte flag = signature[paramSignatureByteIndex];
                    paramSignatureByteIndex++;
                    if ((flag & 0x20) != 0) { _hasThis = true; }
                    if ((flag & 0x40) != 0) { _explicitThis = true; }
                    if ((flag & 0x10) != 0) { _genericParametersCount = ReadCompressedInteger(signature, ref paramSignatureByteIndex); }

                    _parametersCount = ReadCompressedInteger(signature, ref paramSignatureByteIndex);
                    _returnType = DecodeType(signature, ref paramSignatureByteIndex);
                    _parameters = new Signature.Type[_parametersCount];
                    for (int n = 0; n < _parametersCount; n++)
                    {
                        _parameters[n] = DecodeType(signature, ref paramSignatureByteIndex);
                    }
                }
            }

            internal class LocalsSignature : Signature
            {
                Signature.Type[] _locals;
                public int LocalsCount { get { return _locals.Length; } }
                public Signature.Type GetLocalType(int index)
                {
                    if (index < 0 || index >= _locals.Length) { return Signature.Type.Unknown.Instance; }
                    return _locals[index];
                }
                internal LocalsSignature(byte[] signature)
                {
                    uint count = 0;
                    if (signature.Length == 0 || signature[0] != 0x07)
                    {
                        _locals = new Signature.Type[0];
                        return;
                    }
                    uint localSignatureByteIndex = 1;
                    count = ReadCompressedInteger(signature, ref localSignatureByteIndex);
                    _locals = new Type[count];
                    for (int n = 0; n < count; n++)
                    {
                        _locals[n] = DecodeType(signature, ref localSignatureByteIndex);
                    }
                }
                internal LocalsSignature(int count)
                {
                    _locals = new Type[count];
                    for (int n = 0; n < count; n++)
                    {
                        _locals[n] = Type.Unknown.Instance;
                    }
                }
            }

            internal class FieldSignature : Signature
            {
                Signature.Type _fieldType;
                public Signature.Type FieldType { get { return _fieldType; } }
                internal FieldSignature(byte[] signature)
                {
                    if (signature == null || signature.Length == 0)
                    {
                        _fieldType = Signature.Type.Unknown.Instance;
                        return;
                    }
                    uint fieldSignatureByteIndex = 1;
                    if (signature[0] != 0x06) { throw new Exception("Invalid field signature"); }
                    _fieldType = DecodeType(signature, ref fieldSignatureByteIndex);
                }
            }
        }
    }
}
