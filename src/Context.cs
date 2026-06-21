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

using System.Collections.Generic;
using System.Text;
using static Runic.CIL.ToC;
using static Runic.CIL.ToC.Signature;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        internal class Context
        {
            ToC _toC;
            public ToC Parent { get { return _toC; } }
            HashSet<int> _neededLabels = new HashSet<int>();
            Disassembler _disassembler;
            Dictionary<int, Signature.Type> _locals = new Dictionary<int, Signature.Type>();
            HashSet<uint> _requiredTokens = new HashSet<uint>();
            public void Requires(uint metadataToken) { if (_requiredTokens.Contains(metadataToken)) { return; } _requiredTokens.Add(metadataToken); _toC.Requires(metadataToken); }
            public Context(ToC toC)
            {
                _toC = toC;
                _disassembler = new Disassembler(this);
            }
            internal byte[] GetMethodSignature(uint methodToken) { return _toC.GetMethodSignature(methodToken); }
            internal uint GetRuntimeTypeHandleToken() { return _toC.GetRuntimeTypeHandleToken(); }
            Dictionary<uint, byte[]> _fieldSignatures = new Dictionary<uint, byte[]>();
            internal byte[] GetFieldSignature(uint fieldToken) 
            {
                byte[] signature;
                if (_fieldSignatures.TryGetValue(fieldToken, out signature)) { return signature; }
                signature = _toC.GetFieldSignature(fieldToken);
                _fieldSignatures.Add(fieldToken, signature);
                return signature;
            }
            Dictionary<uint, Signature.Type> _fieldTypes = new Dictionary<uint, Signature.Type>();
            internal Signature.Type GetFieldType(uint fieldToken)
            {
                Signature.Type type;
                if (_fieldTypes.TryGetValue(fieldToken, out type)) { return type; }
                byte[] signature = GetFieldSignature(fieldToken);

                FieldSignature fieldSignature = new Signature.FieldSignature(signature);
                type = fieldSignature.FieldType;
                _fieldTypes.Add(fieldToken, type);
                return type;
            }
            internal byte[] GetLocalsSignature(uint methodToken) { return _toC.GetLocalsSignature(methodToken); }
            internal uint GetDeclaringType(uint methodToken) { return _toC.GetDeclaringType(methodToken); }
            internal uint GetTypeGenericParameterCount(uint typeToken) { return _toC.GetTypeGenericParameterCount(typeToken); }
            internal bool IsValueType(uint typeToken) { return _toC.IsValueType(typeToken); }
            internal void DeclareLocal(int local, byte[] signature)
            {
                if (signature == null || signature.Length == 0)
                {
                    _locals.Add(local, Signature.Type.Unknown.Instance);
                }
                else
                {
                    uint offset = 0;
                    Signature.Type localType = Signature.DecodeType(signature, ref offset);
                    _locals.Add(local, localType);
                }
            }
            internal string GetGCBoxMethod(byte[] valueType) { return _toC.GetGCBoxMethod(valueType); }
            internal string GetGCUnboxMethod(byte[] type, bool noTypeCheck) { return _toC.GetGCUnboxMethod(type, noTypeCheck); }
            internal string GetGCNewMethod(uint ctorToken) { return _toC.GetGCNewMethod(ctorToken); }
            internal string GetGCNewArrMethod(byte[] elementType) { return _toC.GetGCNewArrMethod(elementType); }
            internal string GetInitObjMethod(byte[] objType) { return _toC.GetInitObjMethod(objType); }
            internal string GetGCTrackMethod() { return _toC.GetGCTrackMethod(); }
            internal string GetGCUntrackMethod() { return _toC.GetGCUntrackMethod(); }
            internal string GetMethodName(uint methodToken) { return _toC.GetMethodName(methodToken); }
            internal string GetVirtMethodName(uint methodToken, uint typeToken) { return _toC.GetGCGetVirtMethodName(methodToken, typeToken); }
            internal string GetTypeName(uint typeToken) { return _toC.GetTypeName(typeToken); }
            internal string GetStaticFieldName(uint fieldToken) { return _toC.GetStaticFieldName(fieldToken); }
            internal string GetFieldName(uint fieldToken) { return _toC.GetFieldName(fieldToken); }
            internal string GetGCLdElemI1Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemI1Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemU1Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemU1Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemI2Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemI2Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemU2Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemU2Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemI4Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemI4Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemU4Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemU4Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemI8Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemI8Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemIMethod(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemIMethod(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemR4Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemR4Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemR8Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemR8Method(noNullCheck, noBoundCheck); }
            internal string GetGCLdElemRefMethod(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCLdElemRefMethod(noNullCheck, noBoundCheck); }
            internal string GetGCStElemMethod(bool noNullCheck, bool noTypeCheck, bool noBoundCheck, uint typeToken) { return _toC.GetGCStElemMethod(noNullCheck, noTypeCheck, noBoundCheck, typeToken); }
            internal string GetGCStElemIMethod(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemIMethod(noNullCheck, noBoundCheck); }
            internal string GetGCStElemI1Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemI1Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemI2Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemI2Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemI4Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemI4Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemI8Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemI8Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemR4Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemR4Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemR8Method(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemR8Method(noNullCheck, noBoundCheck); }
            internal string GetGCStElemRefMethod(bool noNullCheck, bool noBoundCheck) { return _toC.GetGCStElemRefMethod(noNullCheck, noBoundCheck); }
            internal string GetGCLdFldMethod(bool noNullCheck, bool volatilePrefix, int alignment, uint fieldToken) { return _toC.GetGCLdFldMethod(noNullCheck, volatilePrefix, alignment, fieldToken); }
            internal string GetGCLdLenMethod() { return _toC.GetGCLdLenMethod(); }
            internal string GetVolatileStoreMethod(byte[] type) { return _toC.GetVolatileStoreMethod(type); }
            internal string GetGCString(uint token) { return _toC.GetGCString(token); }
            internal string GetGCStringTypeName() { return _toC.GetGCStringTypeName(); }
#if NET6_0_OR_GREATER
            internal Signature.Type? GetType(int local)
#else
            internal Signature.Type GetType(int local)
#endif
            {
#if NET6_0_OR_GREATER
                Signature.Type? localType;
#else
                Signature.Type localType;
#endif
                if (_locals.TryGetValue(local, out localType)) { return localType; }
                return null;
            }
            internal void NeedLabel(int address) { _neededLabels.Add(address); }
            StringBuilder _currentLine = new StringBuilder();
            internal void EmitLine(string code) { _currentLine.Append(code); }
            HashSet<int> _isGCTracked = new HashSet<int>();
            internal bool IsGCTracked(int local) { return _isGCTracked.Contains(local); }
            internal IReadOnlyCollection<int> GetGCLocals() { return _isGCTracked; }

#if NET6_0_OR_GREATER
            public void Process(ExceptionHandlingClause[]? exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#else
            public void Process(ExceptionHandlingClause[] exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#endif
            {
                string name = GetMethodName(methodToken);
                HashSet<int> emittedOffsets = new HashSet<int>();
                uint typeGenericParameterCount = 0;
                uint signatureOffset = 0;
                Signature.Type[] parameters;
                Signature.Type[] locals;
                byte[] methodSignatureCode = GetMethodSignature(methodToken);
                Signature.MethodSignature methodSignature = new Signature.MethodSignature(methodSignatureCode);
                if (methodSignature.HasThis)
                {
                    Signature.Type[] newParameters = new Signature.Type[methodSignature.ParametersCount + 1];
                    uint declaringType = GetDeclaringType(methodToken);
                    if (IsValueType(declaringType)) { newParameters[0] = new Signature.Type.Pointer(new Signature.Type.ValueType(declaringType)); }
                    else { newParameters[0] = new Signature.Type.TypeToken(declaringType); }
                    for (int n = 0; n < methodSignature.ParametersCount; n++)
                    {
                        newParameters[n + 1] = methodSignature.GetParameterType(n);
                    }
                    parameters = newParameters;
                }
                else
                {
                    parameters = new Signature.Type[methodSignature.ParametersCount];
                    for (int n = 0; n < methodSignature.ParametersCount; n++)
                    {
                        parameters[n] = methodSignature.GetParameterType(n);
                    }
                }

                byte[] localSignatureCode = GetLocalsSignature(methodToken);
                Signature.LocalsSignature localsSignature = new Signature.LocalsSignature(localSignatureCode);
                for (int n = 0; n < localsSignature.LocalsCount; n++)
                {
                    _locals.Add(n, localsSignature.GetLocalType(n));
                }

                bool macro = false;
                if (methodSignature.GenericParametersCount > 0)
                {
                    macro = true;
                }
                else
                {
                    uint typeToken = GetDeclaringType(methodToken);
                    typeGenericParameterCount = GetTypeGenericParameterCount(typeToken);
                    if (typeGenericParameterCount > 0)
                    {
                        macro = true;
                    }
                }

#if NET6_0_OR_GREATER
                CIL.Destackifier.ExceptionHandlingClause[]? convertedEhc = null;
#else
                CIL.Destackifier.ExceptionHandlingClause[] convertedEhc = null;
#endif
                if (exceptionHandlingClauses != null)
                {
                    convertedEhc = new Destackifier.ExceptionHandlingClause[exceptionHandlingClauses.Length];
                    for (int n = 0; n < exceptionHandlingClauses.Length; n++)
                    {
                        switch (exceptionHandlingClauses[n])
                        {
                            case ExceptionHandlingClause.Filter filter: convertedEhc[n] = new Destackifier.ExceptionHandlingClause.Filter(filter.FilterOffset, filter.HandlerOffset); break;
                            case ExceptionHandlingClause.Clause clause: convertedEhc[n] = new Destackifier.ExceptionHandlingClause.Clause(clause.HandlerOffset); break;
                        }
                    }
                }

                if (convertedEhc != null) { _disassembler.Destackify(convertedEhc, methodToken, bytecode); }
                else { _disassembler.Destackify(methodToken, bytecode); }

                _isGCTracked = GC.GetGCLocals(this, _disassembler.Instructions, _locals);
                if (macro)
                {
                    string macroPrototype = "#define " + name + "(";
                    macroPrototype += ") \\";
                    _toC.Emit(0, macroPrototype);
                }
                Prototype.EmitPrototype(name, typeGenericParameterCount, methodSignature.GenericParametersCount, this, methodSignature.ReturnType, parameters);
                if (macro) { _toC.Emit(0, "{ \\"); }
                else { _toC.Emit(0, "{"); }
                VariableDeclaration.EmitLocals(this, _locals);
                for (int n = 0; n < _disassembler.Instructions.Count; n++)
                {
                    int offset = _disassembler.Instructions[n].Offset;
                    _currentLine = new StringBuilder();
                    _currentLine.Append("    ");
                    if (_neededLabels.Contains(offset) && !emittedOffsets.Contains(offset))
                    {
                        _currentLine.Append("lbl_" + offset.ToString("X4") + ": ");
                    }
                    emittedOffsets.Add(offset);
                    _disassembler.Instructions[n].ToC(this);
                    if (macro) { _currentLine.Append(" \\"); }
                    _toC.Emit(offset, _currentLine.ToString());
                }
                _toC.Emit(0, "}");
            }
        }
    }
}
