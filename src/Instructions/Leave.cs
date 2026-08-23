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
        internal class Leave : Instruction
        {
            int _address;
            public int Address { get { return _address; } }
            public Leave(int offset, int address) : base(offset)
            {
                _address = address;
            }
            public override void ToC(Context context)
            {
                ExceptionHandlingClause.Finally[] finallyClauses = context.GetFinally(Offset, _address);

                context.EmitLine(context.GetClearExceptionMethodName() + "();");
                if (finallyClauses.Length == 0)
                {
                    context.EmitLine("goto lbl_" + _address.ToString("X4") + ";");
                    return;
                }
                if (finallyClauses.Length == 1)
                {
                    context.EmitLine("finallyTarget = 0x" + _address.ToString("X4") + "; ");
                    finallyClauses[0].AddTarget(_address);
                    context.EmitLine("goto lbl_" + finallyClauses[0].HandlerOffset.ToString("X4") + ";");
                    return;
                }

                throw new Exception("Multiple finally clauses are not yet supported.");
                context.EmitLine("finallyTarget = 0x" + _address.ToString("X4") + "; ");
                finallyClauses[0].AddTarget(_address);
                context.EmitLine("goto lbl_" + finallyClauses[0].HandlerOffset.ToString("X4") + ";");
            }
        }
    }
}
