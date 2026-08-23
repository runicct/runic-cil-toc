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
using System.Text;
using static Runic.CIL.ToC;
using static Runic.CIL.ToC.Signature.Type;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        public class ExceptionHandlingClause
        {
            string _label;
            internal string Label { get { return _label; } set { _label = value; } }
#if NET6_0_OR_GREATER
            TryCatchFinally? _tryCatchFinally;
            internal TryCatchFinally? TryCatchFinally { get { return _tryCatchFinally; } set { _tryCatchFinally = value; } }
#else
            TryCatchFinally _tryCatchFinally;
            internal TryCatchFinally TryCatchFinally { get { return _tryCatchFinally; } set { _tryCatchFinally = value; } }
#endif
            public class Filter : ExceptionHandlingClause
            {
                int _filterOffset;
                internal int FilterOffset { get { return _filterOffset; } }
                public Filter(int tryOffset, int tryLength, int filterOffset, int handlerOffset, int handlerLength) : base(tryOffset, tryLength, handlerOffset, handlerLength) { _filterOffset = filterOffset; }
            }
            public class Clause : ExceptionHandlingClause
            {
                public Clause(int tryOffset, int tryLength, int handlerOffset, int handlerLength) : base(tryOffset, tryLength, handlerOffset, handlerLength) { }
            }
            public class Finally : ExceptionHandlingClause
            {
                HashSet<int> _targets = new HashSet<int>();
                internal void AddTarget(int target) { _targets.Add(target); }
                internal HashSet<int> Targets { get { return _targets; } }
                public Finally(int tryOffset, int tryLength, int handlerOffset, int handlerLength) : base(tryOffset, tryLength, handlerOffset, handlerLength) { }
            }
            int _tryOffset;
            internal int TryOffset { get { return _tryOffset; } }
            int _tryLength;
            internal int TryLength { get { return _tryLength; } }
            int _handlerOffset;
            internal int HandlerOffset { get { return _handlerOffset; } }
            int _handlerLength;
            internal int HandlerLength { get { return _handlerLength; } }

            internal ExceptionHandlingClause(int tryOffset, int tryLength, int handlerOffset, int handlerLength)
            {
                _tryOffset = tryOffset;
                _tryLength = tryLength;
                _handlerOffset = handlerOffset;
                _handlerLength = handlerLength;
            }
        }
        public virtual void Requires(uint metadataToken) { }
        public abstract byte[] GetMethodSignature(uint methodToken);
        public abstract uint GetRuntimeTypeHandleToken();
        public abstract byte[] GetFieldSignature(uint fieldToken);
        public abstract byte[] GetLocalsSignature(uint methodToken);
        public abstract uint GetDeclaringType(uint methodToken);
        public abstract bool IsValueType(uint typeToken);
        public virtual string GetInitObjMethod(byte[] objType) { return "initobj"; }
        public virtual string GetNewMethod(uint ctorToken) { return "new_" + ctorToken.ToString("x8"); }
        public virtual string GetNewArrMethod(byte[] elementType) { return "newarr"; }
        public virtual string GetGCTrackMethod() { return "gc_track"; }
        public virtual string GetGCUntrackMethod() { return "gc_untrack"; }
        public virtual string GetString(uint token) { return "const_string_" + token.ToString("x8"); }
        public virtual string GetStringTypeName() { return "string"; }
        public virtual string GetLdElemMethod(bool noNullCheck, bool noBoundCheck, byte[] elementType) { return "ldelem"; }
        public virtual string GetStElemMethod(bool noNullCheck, bool noTypeCheck, bool noBoundCheck, byte[] elementType) { return "stelem"; }
        public virtual string GetBoxMethod(byte[] type) { return "box"; }
        public virtual string GetUnboxMethod(byte[] type, bool noTypeCheck) { return "unbox"; }
        public virtual string GetLdLenMethod() { return "ldlen"; }
        public virtual uint GetTypeGenericParameterCount(uint typeToken) { return 0; }
        public virtual string GetMethodName(uint methodToken) { return "m_" + methodToken.ToString("X8"); }
        public virtual string GetLdVirtMethodName(uint methodToken, uint typeToken) { return "ldvirtmethod_" + methodToken.ToString("X8") + "_" + typeToken.ToString("X8"); }
        public virtual string GetTypeName(uint typeToken) { return "t_" + typeToken.ToString("X8"); }
        public virtual string GetLdFldMethodName(uint fieldToken) { return "ldfld_" + fieldToken.ToString("X8"); }
        public virtual string GetLdFldAMethodName(uint fieldToken) { return "ldflda_" + fieldToken.ToString("X8"); }
        public virtual string GetStFldMethodName(uint fieldToken) { return "stfld_" + fieldToken.ToString("X8"); }
        public virtual string GetStSFldMethodName(uint fieldToken) { return "stsfld_" + fieldToken.ToString("X8"); }
        public virtual string GetLdSFldMethodName(uint fieldToken) { return "ldsfld_" + fieldToken.ToString("X8"); }
        public virtual string GetLdSFldAMethodName(uint fieldToken) { return "ldsflda_" + fieldToken.ToString("X8"); }
        public virtual string GetGetIsInstMethodName(uint typeToken) { return "isinst_" + typeToken.ToString("X8"); }
        public virtual string GetGetExceptionMethodName() { return "get_exception"; }
        public virtual string GetClearExceptionMethodName() { return "clear_exception"; }
        public virtual string GetSetTryBlockEnvMethodName() { return "set_tryblockenv"; }
        public virtual string GetGetTryBlockEnvMethodName() { return "get_tryblockenv"; }
        public virtual string GetThrowMethodName() { return "throw"; }
        public virtual string GetTryPrologue(int tryBlockId)
        {
            return ("jmp_buf tryblockenv_" + tryBlockId.ToString("X4") + "; int tryblockret_" + tryBlockId.ToString("X4") + " = setjmp(tryblockenv_" + tryBlockId.ToString("X4") + "); if (tryblockret_" + tryBlockId.ToString("X4") + " == 0) { " + GetSetTryBlockEnvMethodName() + "(&tryblockenv_" + tryBlockId.ToString("X4") + ");");
        }
        public virtual string GetTryEpilogue(int tryBlockId, string exceptionHandlerLabel)
        {
            return ("} else { goto " + exceptionHandlerLabel + "; }");
        }
        public virtual string BuildExceptionHandler(ExceptionHandlingClause[] exceptionHandlingClauses)
        {
            return "goto " + exceptionHandlingClauses[0].Label + "; /* Default exception handler. Doesn't work for filtering override BuildExceptionHandler if needed */";
        }
        public virtual void Emit(int offset, string code) { Emit(code); }
        public abstract void Emit(string code);
        public void Process(uint methodToken, byte[] bytecode) { Process(null, methodToken, bytecode); }
#if NET6_0_OR_GREATER
        public void Process(ExceptionHandlingClause[]? exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#else
        public void Process(ExceptionHandlingClause[] exceptionHandlingClauses, uint methodToken, byte[] bytecode)
#endif
        {
            Context context = new Context(this);
            context.Process(exceptionHandlingClauses, methodToken, bytecode);
        }
    }
}
