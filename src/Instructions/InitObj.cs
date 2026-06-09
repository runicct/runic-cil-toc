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
        internal class InitObj : Instruction
        {
            int _obj;
            public int Object { get { return _obj; } }
            uint _typeToken;
            public InitObj(int offset, uint typeToken, int obj) : base(offset)
            {
                _obj = obj;
                _typeToken = typeToken;
            }
            public override void ToC(Context context)
            {
#if NET6_0_OR_GREATER
                Signature.Type? type = context.GetType(_obj);
#else
                Signature.Type type = context.GetType(_obj);
#endif
                List<byte> typeSignature = new List<byte>();
                switch (type)
                {
                    case Signature.Type.ByRef byref: byref.Target.Emit(typeSignature); break;
                    case Signature.Type.Pointer pointer: pointer.Target.Emit(typeSignature); break;
                    default: new Signature.Type.ValueType(_typeToken).Emit(typeSignature); break;
                }

                context.EmitLine(context.GetInitObjMethod(typeSignature.ToArray()) + "(loc_" + _obj.ToString("X4") + ");");
            }
        }
    }
}
