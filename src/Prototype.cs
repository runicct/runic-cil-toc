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
using System.Threading.Tasks.Dataflow;
using static Runic.CIL.ToC;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        internal static class Prototype
        {
            public static void EmitPrototype(string methodName, uint genericTypeParameterCount, uint genericMethodParameterCount, ToC.Context context, Signature.Type returnType, Signature.Type[] parameters)
            {
                bool macro = false;
                string prototype = returnType.ToC(context) + " " + methodName;
                if (genericTypeParameterCount > 0)
                {
                    for (int n = 0; n < genericTypeParameterCount; n++)
                    {
                        prototype += "_##TT" + n.ToString() + "##";
                    }
                    macro = true;
                }
                if (genericMethodParameterCount > 0)
                {
                    for (int n = 0; n < genericMethodParameterCount; n++)
                    {
                        prototype += "_##TM" + n.ToString() + "##";
                    }
                    macro = true;
                }
                prototype += "(";
                for (int n = 0; n < parameters.Length; n++)
                {
                    if (n > 0) prototype += ", ";
                    switch (parameters[n])
                    {
                        case Signature.Type.TypeToken _:
                        case Signature.Type.String _:
                        case Signature.Type.Object _:
                            prototype += "void* arg_" + n.ToString("X4");
                            break;
                        default:
                            prototype += parameters[n].ToC(context) + " arg_" + n.ToString("X4");
                            break;
                    }
                    
                }
                prototype += ")";
                if (macro) { prototype += " \\"; }
                context.Parent.Emit(0,prototype);
            }
        }
    }
}