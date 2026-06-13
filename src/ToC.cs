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
using static Runic.CIL.ToC;
using static Runic.CIL.ToC.Signature.Type;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        public class ExceptionHandlingClause
        {
            public class Filter : ExceptionHandlingClause
            {
                int _filterOffset;
                internal int FilterOffset { get { return _filterOffset; } }
                public Filter(int filterOffset, int handlerOffset) : base(handlerOffset) { _filterOffset = filterOffset; }
            }
            public class Clause : ExceptionHandlingClause
            {
                public Clause(int handlerOffset) : base(handlerOffset) { }
            }
            int _handlerOffset;
            internal int HandlerOffset { get { return _handlerOffset; } }

            internal ExceptionHandlingClause(int handlerOffset)
            {
                _handlerOffset = handlerOffset;
            }
        }
        public abstract byte[] GetMethodSignature(uint methodToken);
        public abstract uint GetRuntimeTypeHandleToken();
        public abstract byte[] GetFieldSignature(uint fieldToken);
        public abstract byte[] GetLocalsSignature(uint methodToken);
        public abstract uint GetDeclaringType(uint methodToken);
        public abstract bool IsValueType(uint typeToken);
        public virtual string GetInitObjMethod(byte[] objType) { return "initobj"; }
        public virtual string GetGCNewMethod(uint ctorToken) { return "gc_new_" + ctorToken.ToString("x8"); }
        public virtual string GetGCNewArrMethod(byte[] elementType) { return "gc_newarr"; }
        public virtual string GetGCStSlotMethod() { return "gc_stslot"; }
        public virtual string GetGCLdSlotMethod() { return "gc_ldslot"; }
        public virtual string GetGCClearSlotMethod() { return "gc_clrslot"; }
        public virtual string GetGCSetRetSlotMethod() { return "gc_setretslot"; }
        public virtual string GetGCMoveRetSlotMethod() { return "gc_moveretslot"; }
        public virtual string GetGCString(uint token) { return "gc_str_" + token.ToString("x8"); }
        public virtual string GetGCLdElemI1Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemi1"; }
        public virtual string GetGCLdElemU1Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemu1"; }
        public virtual string GetGCLdElemI2Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemi2"; }
        public virtual string GetGCLdElemU2Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemu2"; }
        public virtual string GetGCLdElemI4Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemi4"; }
        public virtual string GetGCLdElemU4Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemu4"; }
        public virtual string GetGCLdElemI8Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemi8"; }
        public virtual string GetGCLdElemIMethod(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemi"; }
        public virtual string GetGCLdElemR4Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemr4"; }
        public virtual string GetGCLdElemR8Method(bool noNullCheck, bool noBoundCheck) { return "gc_ldelemr8"; }
        public virtual string GetGCStElemMethod(bool noNullCheck, bool noTypeCheck, bool noBoundCheck, uint typeToken) { return "gc_stelem_" + typeToken.ToString("x8"); }
        public virtual string GetGCStElemIMethod(bool noNullCheck, bool noBoundCheck) { return "gc_stelemi"; }
        public virtual string GetGCStElemI1Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemi1"; }
        public virtual string GetGCStElemI2Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemi2"; }
        public virtual string GetGCStElemI4Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemi4"; }
        public virtual string GetGCStElemI8Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemi8"; }
        public virtual string GetGCStElemR4Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemr4"; }
        public virtual string GetGCStElemR8Method(bool noNullCheck, bool noBoundCheck) { return "gc_stelemr8"; }
        public virtual string GetGCBoxMethod(byte[] type) { return "gc_box"; }
        public virtual string GetGCUnboxMethod(byte[] type, bool noTypeCheck) { return "gc_unbox"; }
        public virtual string GetGCLdLenMethod() { return "gc_ldlen"; }
        public virtual string GetGCLdFldMethod(bool noNullCheck, bool volatilePrefix, int alignment, uint fieldToken) { return "gc_ldfld_" + fieldToken.ToString("x8"); }
        public virtual uint GetTypeGenericParameterCount(uint typeToken) { return 0; }
        public virtual string GetMethodName(uint methodToken) { return "m_" + methodToken.ToString("X8"); }
        public virtual string GetVirtMethodName(uint methodToken) { return "gc_getvirtmethod_" + methodToken.ToString("X8"); }
        public virtual string GetValueTypeName(uint typeToken) { return "t_" + typeToken.ToString("X8"); }
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
