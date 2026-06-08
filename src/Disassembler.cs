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
        class Disassembler : Runic.CIL.Destackifier
        {
            Context _context;
            public override uint GetRuntimeTypeHandleToken() { return _context.GetRuntimeTypeHandleToken(); }
            public override byte[] GetFieldSignature(uint fieldToken) { return _context.GetFieldSignature(fieldToken); }
            public override byte[] GetLocalsSignature(uint methodToken) { return _context.GetLocalsSignature(methodToken); }
            public override byte[] GetMethodSignature(uint methodToken) { return _context.GetMethodSignature(methodToken); }
            public override void DeclareLocal(int local, byte[] signature) { _context.DeclareLocal(local, signature); }
            public override bool IsValueType(uint typeToken) { return _context.IsValueType(typeToken); }
            public Disassembler(Context context)
            {
                _context = context;
            }
            List<Instruction> _instructions = new List<Instruction>();
            public IReadOnlyList<Instruction> Instructions { get { return _instructions; } }

            public override void Add(int offset, int destination, int a, int b) { var inst = new Add(offset, destination, a, b); _instructions.Add(inst); }
            public override void AddOvf(int offset, int destination, int a, int b) { var inst = new AddOvf(offset, destination, a, b); _instructions.Add(inst); }
            public override void AddOvfUn(int offset, int destination, int a, int b) { _instructions.Add(new AddOvfUn(offset, destination, a, b)); }
            public override void And(int offset, int destination, int a, int b) { _instructions.Add(new And(offset, destination, a, b)); }
            public override void ArgList(int offset, int destination) { _instructions.Add(new ArgList(offset, destination)); }
            public override void Box(int offset, uint typeToken, int destination, int source) { _instructions.Add(new Box(offset, typeToken, destination, source)); }
            public override void Br(int offset, int address) { _instructions.Add(new Br(offset, address)); _context.NeedLabel(address); }
            public override void Break(int offset) { _instructions.Add(new Break(offset)); }
            public override void BrEq(int offset, int address, int a, int b) { _instructions.Add(new BrEq(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrFalse(int offset, int address, int condition) { _instructions.Add(new BrFalse(offset, address, condition)); _context.NeedLabel(address); }
            public override void BrTrue(int offset, int address, int condition) { _instructions.Add(new BrTrue(offset, address, condition)); _context.NeedLabel(address); }
            public override void BrGe(int offset, int address, int a, int b) { _instructions.Add(new BrGe(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrGt(int offset, int address, int a, int b) { _instructions.Add(new BrGt(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrLe(int offset, int address, int a, int b) { _instructions.Add(new BrLe(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrLt(int offset, int address, int a, int b) { _instructions.Add(new BrLt(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrGeUn(int offset, int address, int a, int b) { _instructions.Add(new BrGeUn(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrGtUn(int offset, int address, int a, int b) { _instructions.Add(new BrGtUn(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrLeUn(int offset, int address, int a, int b) { _instructions.Add(new BrLeUn(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrLtUn(int offset, int address, int a, int b) { _instructions.Add(new BrLtUn(offset, address, a, b)); _context.NeedLabel(address); }
            public override void BrNeqUn(int offset, int address, int a, int b) { _instructions.Add(new BrNeqUn(offset, address, a, b)); _context.NeedLabel(address); }
            public override void Call(int offset, bool tail, uint methodToken, int destination, int[] parameters) { _instructions.Add(new Call(offset, tail, methodToken, destination, parameters)); }
            public override void Call(int offset, bool tail, uint methodToken, int[] parameters) { _instructions.Add(new CallProc(offset, tail, methodToken, parameters)); }
            public override void CallI(int offset, bool tail, uint signatureToken, int destination, int[] parameters)
            {
                throw new NotImplementedException();
            }
            public override void CallI(int offset, bool tail, uint signatureToken, int[] parameters)
            {
                throw new NotImplementedException();
            }
            public override void CallVirt(int offset, bool noNullCheck, bool tail, uint methodToken, int destination, int[] parameters)
            {
                _instructions.Add(new CallVirt(offset, noNullCheck, tail, methodToken, destination, parameters));
            }
            public override void CallVirt(int offset, bool noNullCheck, bool tail, uint methodToken, int[] parameters)
            {
                _instructions.Add(new CallVirtProc(offset, noNullCheck, tail, methodToken, parameters));
            }

            public override void CastClass(int offset, bool noTypeCheck, uint typeToken, int destination, int value) { _instructions.Add(new CastClass(offset, noTypeCheck, typeToken, destination, value)); }
            public override void Ceq(int offset, int destination, int a, int b) { _instructions.Add(new Ceq(offset, destination, a, b)); }
            public override void Cgt(int offset, int destination, int a, int b) { _instructions.Add(new Cgt(offset, destination, a, b)); }
            public override void CgtUn(int offset, int destination, int a, int b) { _instructions.Add(new CgtUn(offset, destination, a, b)); }
            public override void Clt(int offset, int destination, int a, int b) { _instructions.Add(new Clt(offset, destination, a, b)); }
            public override void CltUn(int offset, int destination, int a, int b) { _instructions.Add(new CltUn(offset, destination, a, b)); }
            public override void CkFinite(int offset, int destination, int source) { _instructions.Add(new CkFinite(offset, destination, source)); }
            public override void ConvI(int offset, int destination, int source) { _instructions.Add(new ConvI(offset, destination, source)); }
            public override void ConvI1(int offset, int destination, int source) { _instructions.Add(new ConvI1(offset, destination, source)); }
            public override void ConvI2(int offset, int destination, int source) { _instructions.Add(new ConvI2(offset, destination, source)); }
            public override void ConvI4(int offset, int destination, int source) { _instructions.Add(new ConvI4(offset, destination, source)); }
            public override void ConvI8(int offset, int destination, int source) { _instructions.Add(new ConvI8(offset, destination, source)); }
            public override void ConvOvfI(int offset, int destination, int source) { _instructions.Add(new ConvOvfI(offset, destination, source)); }
            public override void ConvOvfI1(int offset, int destination, int source) { _instructions.Add(new ConvOvfI1(offset, destination, source)); }
            public override void ConvOvfI2(int offset, int destination, int source) { _instructions.Add(new ConvOvfI2(offset, destination, source)); }
            public override void ConvOvfI4(int offset, int destination, int source) { _instructions.Add(new ConvOvfI4(offset, destination, source)); }
            public override void ConvOvfI8(int offset, int destination, int source) { _instructions.Add(new ConvOvfI8(offset, destination, source)); }
            public override void ConvU1(int offset, int destination, int source) { _instructions.Add(new ConvU1(offset, destination, source)); }
            public override void ConvU2(int offset, int destination, int source) { _instructions.Add(new ConvU2(offset, destination, source)); }
            public override void ConvU4(int offset, int destination, int source) { _instructions.Add(new ConvU4(offset, destination, source)); }
            public override void ConvU8(int offset, int destination, int source) { _instructions.Add(new ConvU8(offset, destination, source)); }
            public override void ConvU(int offset, int destination, int source) { _instructions.Add(new ConvU(offset, destination, source)); }
            public override void ConvR4(int offset, int destination, int source) { _instructions.Add(new ConvR4(offset, destination, source)); }
            public override void ConvR8(int offset, int destination, int source) { _instructions.Add(new ConvR8(offset, destination, source)); }
            public override void Div(int offset, int destination, int a, int b) { _instructions.Add(new Div(offset, destination, a, b)); }
            public override void DivUn(int offset, int destination, int a, int b) { _instructions.Add(new DivUn(offset, destination, a, b)); }
            public override void InitObj(int offset, uint typeToken, int destination) { _instructions.Add(new InitObj(offset, typeToken, destination)); }
            public override void LdArg(int offset, int destination, int index) { _instructions.Add(new LdArg(offset, destination, index)); }
            public override void Ret(int offset) { _instructions.Add(new Ret(offset)); }
            public override void Ret(int offset, int value) { _instructions.Add(new RetVal(offset, value)); }
            public override void Nop(int offset) { _instructions.Add(new Nop(offset)); }
            public override void StLoc(int offset, int destination, int source) { _instructions.Add(new StLoc(offset, destination, source)); }
            public override void LdArgA(int offset, int destination, int index) { _instructions.Add(new LdArgA(offset, destination, index)); }
            public override void LdcI4(int offset, int destination, int value) { _instructions.Add(new LdcI4(offset, destination, value)); }
            public override void LdcI8(int offset, int destination, long value) { _instructions.Add(new LdcI8(offset, destination, value)); }
            public override void LdcR4(int offset, int destination, float value) { _instructions.Add(new LdcR4(offset, destination, value)); }
            public override void LdcR8(int offset, int destination, double value) { _instructions.Add(new LdcR8(offset, destination, value)); }
            public override void LdLocA(int offset, int destination, int index) { _instructions.Add(new LdLocA(offset, destination, index)); }
            public override void LdNull(int offset, int destination) { _instructions.Add(new LdNull(offset, destination)); }
            public override void LdStr(int offset, uint stringToken, int destination) { _instructions.Add(new LdStr(offset, destination, stringToken)); }
            public override void Mul(int offset, int destination, int a, int b) { _instructions.Add(new Mul(offset, destination, a, b)); }
            public override void NewObj(int offset, uint ctorToken, int destination, int[] parameters) { _instructions.Add(new NewObj(offset, ctorToken, destination, parameters)); }
            public override void NewArr(int offset, uint typeToken, int destination, int size) { _instructions.Add(new NewArr(offset, typeToken, destination, size)); }
            public override void LdElemI1(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemI1(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemU1(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemU1(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemI2(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemI2(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemU2(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemU2(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemI4(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemI4(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemU4(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemU4(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemI8(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemI8(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemR4(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemR4(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemR8(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemR8(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdElemI(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) { _instructions.Add(new LdElemI(offset, noNullCheck, noBoundCheck, destination, array, index)); }
            public override void LdFld(int offset, bool noNullCheck, bool volatilePrefix, int alignment, uint fieldToken, int destination, int obj) { _instructions.Add(new LdFld(offset, noNullCheck, volatilePrefix, alignment, fieldToken, destination, obj)); }
            public override void LdIndI1(int offset, bool volatilePrefix, int destination, int address) { _instructions.Add(new LdIndI1(offset, volatilePrefix, destination, address)); }
            public override void LdIndU1(int offset, bool volatilePrefix, int destination, int address) { _instructions.Add(new LdIndU1(offset, volatilePrefix, destination, address)); }
            public override void LdIndI2(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndI2(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndU2(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndU2(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndI4(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndI4(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndU4(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndU4(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndI8(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndI8(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndR4(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndR4(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndR8(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndR8(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdIndI(int offset, bool volatilePrefix, int alignment, int destination, int address) { _instructions.Add(new LdIndI(offset, volatilePrefix, alignment, destination, address)); }
            public override void LdLen(int offset, int destination, int array) { _instructions.Add(new LdLen(offset, destination, array)); }
            public override void Neg(int offset, int destination, int source) { _instructions.Add(new Neg(offset, destination, source)); }
            public override void Not(int offset, int destination, int source) { _instructions.Add(new Not(offset, destination, source)); }
            public override void Rem(int offset, int destination, int a, int b) { var inst = new Rem(offset, destination, a, b); _instructions.Add(inst); }
            public override void RemUn(int offset, int destination, int a, int b) { var inst = new RemUn(offset, destination, a, b); _instructions.Add(inst); }
            public override void StElem(int offset, bool noNullCheck, bool noTypeCheck, bool noBoundCheck, uint typeToken, int array, int index, int value) { _instructions.Add(new StElem(offset, noNullCheck, noTypeCheck, noBoundCheck, array, index, value, typeToken)); }
            public override void StElemI(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemI(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemI1(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemI1(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemI2(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemI2(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemI4(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemI4(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemI8(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemI8(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemR4(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemR4(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StElemR8(int offset, bool noNullCheck, bool noBoundCheck, int array, int index, int value) { _instructions.Add(new StElemR8(offset, noNullCheck, noBoundCheck, array, index, value)); }
            public override void StIndI(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndI(offset, volatilePrefix, alignment, address, value)); }
            public override void StIndI1(int offset, bool volatilePrefix, int address, int value) { _instructions.Add(new StIndI1(offset, volatilePrefix, address, value)); }
            public override void StIndI2(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndI2(offset, volatilePrefix, alignment, address, value)); }
            public override void StIndI4(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndI4(offset, volatilePrefix, alignment, address, value)); }
            public override void StIndI8(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndI8(offset, volatilePrefix, alignment, address, value)); }
            public override void StIndR4(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndR4(offset, volatilePrefix, alignment, address, value)); }
            public override void StIndR8(int offset, bool volatilePrefix, int alignment, int address, int value) { _instructions.Add(new StIndR8(offset, volatilePrefix, alignment, address, value)); }
            public override void StArg(int offset, int index, int source) { _instructions.Add(new StArg(offset, index, source)); }
            public override void Shl(int offset, int destination, int a, int b) { var inst = new Shl(offset, destination, a, b); _instructions.Add(inst); }
            public override void Shr(int offset, int destination, int a, int b) { var inst = new Shr(offset, destination, a, b); _instructions.Add(inst); }
            public override void ShrUn(int offset, int destination, int a, int b) { var inst = new ShrUn(offset, destination, a, b); _instructions.Add(inst); }
            public override void Sub(int offset, int destination, int a, int b) { var inst = new Sub(offset, destination, a, b); _instructions.Add(inst); }
            public override void Switch(int offset, int[] addresses, int value) { _instructions.Add(new Switch(offset, addresses, value)); for (int n = 0; n < addresses.Length; n++) { _context.NeedLabel(addresses[n]); } }
            public override void Unbox(int offset, bool noTypeCheck, uint typeToken, int destination, int source) { _instructions.Add(new Unbox(offset, noTypeCheck, typeToken, destination, source)); }
            public override void Or(int offset, int destination, int a, int b) { var inst = new Or(offset, destination, a, b); _instructions.Add(inst); }
            public override void Xor(int offset, int destination, int a, int b) { var inst = new Xor(offset, destination, a, b); _instructions.Add(inst); }
        }
    }
}