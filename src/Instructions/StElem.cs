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
        internal class StElem : Instruction
        {
            int _value;
            public int Value { get { return _value; } }
            bool _noNullCheck;
            public bool NoNullCheck { get { return _noNullCheck; } }
            bool _noTypeCheck;
            public bool NoTypeCheck { get { return _noTypeCheck; } }
            bool _noBoundCheck;
            public bool NoBoundCheck { get { return _noBoundCheck; } }
            int _array;
            public int Array { get { return _array; } }
            int _index;
            public int Index { get { return _index; } }
            uint _typeToken;
            public uint TypeToken { get { return _typeToken; } }
            public StElem(int offset, bool noNullCheck, bool noTypeCheck, bool noBoundCheck, int array, int index, int value, uint typeToken) : base(offset)
            {
                _noNullCheck = noNullCheck;
                _noBoundCheck = noBoundCheck;
                _array = array;
                _index = index;
                _value = value;
                _typeToken = typeToken;
            }
            public override void ToC(Context context)
            {
                bool isValueType = context.IsValueType(_typeToken);
                byte[] signature;
                if (isValueType)
                {
                    List<byte> sig = new List<byte>();
                    (new Signature.Type.ValueType(_typeToken)).Emit(sig);
                    signature = sig.ToArray();
                }
                else
                {
                    List<byte> sig = new List<byte>();
                    (new Signature.Type.TypeToken(_typeToken)).Emit(sig);
                    signature = sig.ToArray();
                }
                context.EmitLine(context.GetStElemMethod(_noNullCheck, _noTypeCheck, _noBoundCheck, signature) + "(loc_" + _array.ToString("X4") + ", loc_" + _index.ToString("X4") + ", loc_" + _value.ToString("X4") + ");");
            }
        }
    }
}
