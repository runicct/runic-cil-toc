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
        internal class Add : Instruction
        {

            int _destination;
            public int Destination { get { return _destination; } }
            int _a;
            public int A { get { return _a; } }
            int _b;
            public int B { get { return _b; } }
            public Add(int offset, int destination, int a, int b) : base(offset)
            {
                _destination = destination;
                _a = a;
                _b = b;
            }
            public override void ToC(Context context)
            {
                Signature.Type? type = context.GetType(_destination);
                if (type != null) { context.EmitLine("loc_" + _destination.ToString("X4") + " = (" + type.ToC(context) + ")(loc_" + _a.ToString("X4") + " + loc_" + _b.ToString("X4") + ");"); }
                else { context.EmitLine("loc_" + _destination.ToString("X4") + " = loc_" + _a.ToString("X4") + " + loc_" + _b.ToString("X4") + ";"); }
            }
        }
    }
}
