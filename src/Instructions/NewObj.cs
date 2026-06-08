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
        internal class NewObj : Instruction
        {
            int _destination;
            public int Destination { get { return _destination; } }
            int[] _parameters;
            public int[] Parameters { get { return _parameters; } }
            uint _ctorToken;
            public uint CtorToken { get { return _ctorToken; } }
            public NewObj(int offset, uint ctorToken, int destination, int[] parameters) : base(offset)
            {
                _destination = destination;
                _parameters = parameters;
                _ctorToken = ctorToken;
            }
            public override void ToC(Context context)
            {
                int slot = context.GetGCSlot(_destination);

                string call = "loc_" + _destination.ToString("X4") + " = " + context.GetGCNewMethod(_ctorToken) + "(";
                call += "gcslot_" + slot.ToString("X8");
                for (int n = 0; n < _parameters.Length; n++)
                {
                    call += ", ";
                    call += "loc_" + _parameters[n].ToString("X4");
                }
                call += ");";
                context.EmitLine(call);
            }
        }
    }
}
