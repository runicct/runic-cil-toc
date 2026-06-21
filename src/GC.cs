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

namespace Runic.CIL
{
    public abstract partial class ToC
    {
        internal static class GC
        {
            public static HashSet<int> GetGCLocals(ToC.Context context, IReadOnlyList<Instruction> instructions, Dictionary<int, Signature.Type> locals)
            {
                HashSet<int> gcvariables = new HashSet<int>();
                foreach (var kvp in locals)
                {
                    switch (kvp.Value)
                    {
                        case Signature.Type.TypeToken _:
                        case Signature.Type.String _:
                        case Signature.Type.ArrayType _:
                        case Signature.Type.Object _: gcvariables.Add(kvp.Key); break;
                    }
                }

                HashSet<int> gcwithslots = new HashSet<int>();
                foreach (Instruction instruction in instructions)
                {
                    switch (instruction)
                    {
                        case StLoc stloc: if (gcvariables.Contains(stloc.Destination)) { gcwithslots.Add(stloc.Destination); } break;
                        case NewArr newArr: if (gcvariables.Contains(newArr.Destination)) { gcwithslots.Add(newArr.Destination); } break;
                        case NewObj newObj: if (gcvariables.Contains(newObj.Destination)) { gcwithslots.Add(newObj.Destination); } break;
                        case Call call: if (gcvariables.Contains(call.Destination)) { gcwithslots.Add(call.Destination); } break;
                        case CallVirt callVirt: if (gcvariables.Contains(callVirt.Destination)) { gcwithslots.Add(callVirt.Destination); } break;
                    }
                }

                return gcvariables;
            }
        }
    }
}