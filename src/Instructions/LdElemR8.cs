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
        internal class LdElemR8 : Instruction
        {
            int _destination;
            public int Destination { get { return _destination; } }
            bool _noNullCheck;
            public bool NoNullCheck { get { return _noNullCheck; } }
            bool _noBoundCheck;
            public bool NoBoundCheck { get { return _noBoundCheck; } }
            int _array;
            public int Array { get { return _array; } }
            int _index;
            public int Index { get { return _index; } }
            public LdElemR8(int offset, bool noNullCheck, bool noBoundCheck, int destination, int array, int index) : base(offset)
            {
                _destination = destination;
                _array = array;
                _index = index;
                _noNullCheck = noNullCheck;
                _noBoundCheck = noBoundCheck;
            }
            public override void ToC(Context context)
            {
                context.EmitLine("loc_" + _destination.ToString("X4") + " = " + context.GetGCLdElemR8Method(_noNullCheck, _noBoundCheck) + "(loc_" + _array.ToString("X4") + ", loc_" + _index.ToString("X4") + ");");
            }
        }
    }
}
