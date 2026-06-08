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
        internal class Unbox : Instruction
        {
            int _destination;
            public int Destination { get { return _destination; } }
            int _source;
            public int Source { get { return _source; } }
            uint _typeToken;
            public uint TypeToken { get { return _typeToken; } }
            bool _noTypeCheck;
            public bool NoTypeCheck { get { return _noTypeCheck; } }

            public Unbox(int offset, bool noTypeCheck, uint typeToken, int destination, int source) : base(offset)
            {
                _destination = destination;
                _source = source;
                _typeToken = typeToken;
                _noTypeCheck = noTypeCheck;
            }
            public override void ToC(Context context)
            {
                Signature.Type? type = context.GetType(_destination);

                if (type != null)
                {
                    switch (type)
                    {
                        case Signature.Type.Unknown unk: type = new Signature.Type.ValueType(_typeToken); break;
                    }
                }
                else
                {
                    type = new Signature.Type.ValueType(_typeToken);
                }

                List<byte> typeEncoding = new List<byte>();
                type.Emit(typeEncoding);

                context.EmitLine("loc_" + _destination.ToString("X4") + " = " + context.GetGCUnboxMethod(typeEncoding.ToArray(), _noTypeCheck) + "(loc_" + _source.ToString("X4") + ");");
            }
        }
    }
}
