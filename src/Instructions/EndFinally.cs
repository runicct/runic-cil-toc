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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        internal class EndFinally : Instruction
        {
            public EndFinally(int offset) : base(offset)
            {
            }
            public override void ToC(Context context)
            {
                TryCatchFinally tryCatchFinally = context.GetTryCatchFinally(Offset);
                string exceptionPrefix = "if (" + context.GetGetExceptionMethodName() + "()) { " + context.GetThrowMethodName() + "(" + context.GetGetExceptionMethodName() + "()); } ";
                if (tryCatchFinally != null && tryCatchFinally.Finally != null)
                {
                    ExceptionHandlingClause.Finally @finally = tryCatchFinally.Finally;
                    switch (@finally.Targets.Count)
                    {
                        case 0: break;
                        case 1: context.EmitLine(exceptionPrefix + "goto lbl_" + @finally.Targets.First().ToString("X4") + ";"); break;
                        default:
                            StringBuilder switchCaseBuilder = new StringBuilder();
                            switchCaseBuilder.Append(exceptionPrefix);
                            switchCaseBuilder.Append("switch (finallyTarget) {");
                            foreach (int target in @finally.Targets)
                            {
                                switchCaseBuilder.Append("case 0x" + target.ToString("X4") + ": goto lbl_" + target.ToString("X4") + ";");
                            }
                            switchCaseBuilder.Append("}");
                            context.EmitLine(switchCaseBuilder.ToString());
                            break;
                    }
                }
                else
                {
                    throw new Exception("Invalid EndFinally instruction at offset " + Offset.ToString("X4") + ": no matching try-catch-finally block found.");
                }
            }
        }
    }
}
