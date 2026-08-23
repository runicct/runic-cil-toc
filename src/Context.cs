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
using static Runic.CIL.ToC.Context;
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
            internal string GetBoxMethod(byte[] valueType) { return _toC.GetBoxMethod(valueType); }
            internal string GetUnboxMethod(byte[] type, bool noTypEhceck) { return _toC.GetUnboxMethod(type, noTypEhceck); }
            internal string GetNewMethod(uint ctorToken) { return _toC.GetNewMethod(ctorToken); }
            internal string GetNewArrMethod(byte[] elementType) { return _toC.GetNewArrMethod(elementType); }
            internal string GetInitObjMethod(byte[] objType) { return _toC.GetInitObjMethod(objType); }
            internal string GetGCTrackMethod() { return _toC.GetGCTrackMethod(); }
            internal string GetGCUntrackMethod() { return _toC.GetGCUntrackMethod(); }
            internal string GetMethodName(uint methodToken) { return _toC.GetMethodName(methodToken); }
            internal string GetLdVirtMethodName(uint methodToken, uint typeToken) { return _toC.GetLdVirtMethodName(methodToken, typeToken); }
            internal string GetTypeName(uint typeToken) { return _toC.GetTypeName(typeToken); }
            internal string GetLdFldMethodName(uint fieldToken) { return _toC.GetLdFldMethodName(fieldToken); }
            internal string GetLdFldAMethodName(uint fieldToken) { return _toC.GetLdFldAMethodName(fieldToken); }
            internal string GetStFldMethodName(uint fieldToken) { return _toC.GetStFldMethodName(fieldToken); }
            internal string GetLdSFldMethodName(uint fieldToken) { return _toC.GetLdSFldMethodName(fieldToken); }
            internal string GetLdSFldAMethodName(uint fieldToken) { return _toC.GetLdSFldAMethodName(fieldToken); }
            internal string GetStSFldMethodName(uint fieldToken) { return _toC.GetStSFldMethodName(fieldToken); }
            internal string GetLdElemMethod(bool noNullCheck, bool noBoundCheck, byte[] elementType) { return _toC.GetLdElemMethod(noNullCheck, noBoundCheck, elementType); }
            internal string GetStElemMethod(bool noNullCheck, bool noTypEhceck, bool noBoundCheck, byte[] elementType) { return _toC.GetStElemMethod(noNullCheck, noTypEhceck, noBoundCheck, elementType); }
            internal string GetLdLenMethod() { return _toC.GetLdLenMethod(); }
            internal string GetString(uint token) { return _toC.GetString(token); }
            internal string GetStringTypeName() { return _toC.GetStringTypeName(); }
            internal string GetTryPrologue(int tryBlockId) { return _toC.GetTryPrologue(tryBlockId); }
            internal string GetTryEpilogue(int tryBlockId, string exceptionHandlerLabel) { return _toC.GetTryEpilogue(tryBlockId, exceptionHandlerLabel); }
            internal string GetGetExceptionMethodName() { return _toC.GetGetExceptionMethodName(); }
            internal string GetClearExceptionMethodName() { return _toC.GetClearExceptionMethodName(); }
            internal string GetSetTryBlockEnvMethodName() { return _toC.GetSetTryBlockEnvMethodName(); }
            internal string GetGetTryBlockEnvMethodName() { return _toC.GetGetTryBlockEnvMethodName(); }
            internal string GetThrowMethodName() { return _toC.GetThrowMethodName(); }
            internal string GetIsInstMethodName(uint typeToken) { return _toC.GetGetIsInstMethodName(typeToken); }

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
            Stack<TryCatchFinally> _tryCatchStack;
#if NET6_0_OR_GREATER
            EhcMapEntry[]? _ehcMap;
#else
            EhcMapEntry[] _ehcMap;
#endif

            static ExceptionHandlingClause.Finally[] GetFinally(EhcMapEntry from, EhcMapEntry.Try to)
            {
#if NET6_0_OR_GREATER
                EhcMapEntry? current = from;
#else
                EhcMapEntry current = from;
#endif
                List<ExceptionHandlingClause.Finally> finallyClauses = new List<ExceptionHandlingClause.Finally>();
                while (current != null && current != to)
                {
                    switch (current)
                    {
                        case EhcMapEntry.Clause clauseEntry:
                            if (clauseEntry.EhcClause.TryCatchFinally != null && clauseEntry.EhcClause.TryCatchFinally.Finally != null)
                            {
                                finallyClauses.Add(clauseEntry.EhcClause.TryCatchFinally.Finally);
                            }
                            break;
                        case EhcMapEntry.Try tryEntry:
                            if (tryEntry.TryCatchFinally.Finally != null)
                            {
                                finallyClauses.Add(tryEntry.TryCatchFinally.Finally);
                            }
                            break;
                    }
                    current = current.Parent;
                }
                return finallyClauses.ToArray();
            }
            internal ExceptionHandlingClause.Finally[] GetFinally(int from, int to)
            {
                return GetFinally(_ehcMap[from], _ehcMap[to] as EhcMapEntry.Try);
            }
#if NET6_0_OR_GREATER
            internal TryCatchFinally? GetTryCatchFinally(int offset)
#else
            internal TryCatchFinally GetTryCatchFinally(int offset)
#endif
            {
                EhcMapEntry entry = _ehcMap[offset];
                if (entry != null)
                {
                    switch (entry)
                    {
                        case EhcMapEntry.Try tryEntry: return tryEntry.TryCatchFinally;
                        case EhcMapEntry.Clause clauseEntry: return clauseEntry.EhcClause.TryCatchFinally;
                    }
                }
                return null;
            }

            void EmitTryCatchFinally(int offset, int nextInstructionOffset)
            {
                while (_tryCatchStack.Count > 0)
                {
                    TryCatchFinally tryCatchFinally = _tryCatchStack.Peek();
                    if (nextInstructionOffset >= tryCatchFinally.TryOffset + tryCatchFinally.TryLength)
                    {
                        _toC.Emit(offset, "    " + GetTryEpilogue((int)tryCatchFinally.Id, "exceptionHandler_" + tryCatchFinally.Id.ToString("X4")));
                        _tryCatchStack.Pop();
                    }
                    else
                    {
                        break;
                    }
                }
                if (_neededLabels.Contains(nextInstructionOffset) && !_emittedOffsets.Contains(nextInstructionOffset))
                {
                    _emittedOffsets.Add(nextInstructionOffset);
                    _toC.Emit("    lbl_" + nextInstructionOffset.ToString("X4") + ": ;");
                }

                EhcMapEntry entry = _ehcMap[nextInstructionOffset];
                if (entry != null)
                {
                    EhcMapEntry nestedTry = entry;
                    Stack<TryCatchFinally> nestedTryStack = new Stack<TryCatchFinally>();
                    while (nestedTry != null)
                    {
                        if (nestedTry is EhcMapEntry.Try tryEntry)
                        {
                            nestedTryStack.Push(tryEntry.TryCatchFinally);
                        }
                        nestedTry = nestedTry.Parent;
                    }

                    while (nestedTryStack.Count > 0)
                    {
                        TryCatchFinally tryCatchFinally = nestedTryStack.Pop();
                        _toC.Emit(nextInstructionOffset, "    " + GetTryPrologue((int)tryCatchFinally.Id));
                        _tryCatchStack.Push(tryCatchFinally);
                    }

                    if (entry is EhcMapEntry.Clause)
                    {



                        EhcMapEntry parent = entry.Parent;
                        switch (parent)
                        {
                            case EhcMapEntry.Try tryEntry:
                                _toC.Emit(nextInstructionOffset, "    " + GetSetTryBlockEnvMethodName() + "(&tryblockenv_" + tryEntry.TryCatchFinally.Id.ToString("X4") + ");");
                                break;
                            case EhcMapEntry.Clause clauseEntry:
                                if (clauseEntry.EhcClause.TryCatchFinally != null)
                                {
                                    _toC.Emit(nextInstructionOffset, "    " + GetSetTryBlockEnvMethodName() + "(&tryblockenv_" + clauseEntry.EhcClause.TryCatchFinally.Id.ToString("X4") + ");");
                                }
                                break;
                            default:
                                _toC.Emit(nextInstructionOffset, "    " + GetSetTryBlockEnvMethodName() + "(parent_tryblockenv);");
                                break;
                        }
                    }
                }
            }
            HashSet<int> _emittedOffsets = new HashSet<int>();

#if NET6_0_OR_GREATER
            public void Process(ExceptionHandlingClause[]? exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#else
            public void Process(ExceptionHandlingClause[] exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#endif
            {
                string name = GetMethodName(methodToken);
                uint typeGenericParameterCount = 0;
                Signature.Type[] parameters;
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
                TryCatchFinally[]? mergedEhc = null;
#else
                CIL.Destackifier.ExceptionHandlingClause[] convertedEhc = null;
                TryCatchFinally[] mergedEhc = null;
#endif
                if (exceptionHandlingClauses != null && exceptionHandlingClauses.Length > 0)
                {
                    for (int n = 0; n < exceptionHandlingClauses.Length; n++)
                    {
                        exceptionHandlingClauses[n].Label = "lbl_" + exceptionHandlingClauses[n].HandlerOffset.ToString("X4");
                        _neededLabels.Add(exceptionHandlingClauses[n].HandlerOffset);
                    }
                    mergedEhc = MergeEhc(exceptionHandlingClauses);
                    _ehcMap = CreateEhcMap((uint)bytecode.Length, mergedEhc);
                    convertedEhc = ConvertToDestakifierEhc(exceptionHandlingClauses);
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
                VariableDeclaration.EmitLocals(this, _locals, true, (exceptionHandlingClauses != null) && (exceptionHandlingClauses.Length > 0));
                _tryCatchStack = new Stack<TryCatchFinally>();

                if (_ehcMap != null && _ehcMap[0] != null) 
                {
                    _toC.Emit(0, "    uint32_t finallyTarget = 0;");
                    _toC.Emit(0, "    void* parent_tryblockenv = (void*) " + GetGetTryBlockEnvMethodName() + "();");
                    EmitTryCatchFinally(-1, 0); 
                }
                for (int n = 0; n < _disassembler.Instructions.Count; n++)
                {
                    int offset = _disassembler.Instructions[n].Offset;
                    int nextInstructionOffset = (n + 1 >= _disassembler.Instructions.Count) ? bytecode.Length : _disassembler.Instructions[n + 1].Offset;
                    ExceptionHandlingClause clause;
                    _currentLine = new StringBuilder();
                    _currentLine.Append("    ");
                    if (_neededLabels.Contains(offset) && !_emittedOffsets.Contains(offset))
                    {
                        _currentLine.Append("lbl_" + offset.ToString("X4") + ": ");
                    }
                    _emittedOffsets.Add(offset);
                    _disassembler.Instructions[n].ToC(this);
                    if (macro) { _currentLine.Append(" \\"); }
                    _toC.Emit(offset, _currentLine.ToString());

                    // Check if we have an EHC transition
                    bool hasEhcTransition = _ehcMap != null && (_ehcMap[offset] != _ehcMap[nextInstructionOffset]);
                    if (hasEhcTransition) { EmitTryCatchFinally(offset, nextInstructionOffset); }
                }
                while (_tryCatchStack.Count > 0)
                {
                    TryCatchFinally tryCatchFinally = _tryCatchStack.Pop();
                    _toC.Emit(0, "    " + GetTryEpilogue(0, "exceptionHandler_" + tryCatchFinally.Id.ToString("X4")));
                }
                foreach (TryCatchFinally tryCatchFinally in mergedEhc)
                {
                    _toC.Emit(0, "    exceptionHandler_" + tryCatchFinally.Id.ToString("X4") + ":");
                    _toC.Emit(0, _toC.BuildExceptionHandler(tryCatchFinally.Clauses.ToArray()));
                }
                _toC.Emit(0, "}");
            }
        }
    }
}
