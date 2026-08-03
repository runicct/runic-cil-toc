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
        internal static class VariableDeclaration
        {
            public static void EmitLocals(Context context, Dictionary<int, Signature.Type> locals, bool initializeLocals)
            {
                foreach (var kvp in locals)
                {
                    if (context.IsGCTracked(kvp.Key))
                    {
                        context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;");
                    }
                    else
                    {
                        if (initializeLocals)
                        {
                            switch (kvp.Value)
                            {
                                case Signature.Type.Int8 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.UInt8 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0U;"); break;
                                case Signature.Type.Int16 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.UInt16 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0U;"); break;
                                case Signature.Type.Int32 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0;"); break;
                                case Signature.Type.UInt32 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0U;"); break;
                                case Signature.Type.Int64 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0LL;"); break;
                                case Signature.Type.UInt64 _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0ULL;"); break;
                                case Signature.Type.Float _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0.0f;"); break;
                                case Signature.Type.Double _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0.0;"); break;
                                case Signature.Type.ValueType _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = {0};"); break;
                                case Signature.Type.Pointer _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.TypeToken _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.Bool _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = 0;"); break;
                                case Signature.Type.NInt _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.NUInt _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.Object _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.ArrayType _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                case Signature.Type.Char _: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + " = (" + kvp.Value.ToC(context) + ")0;"); break;
                                default: context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + ";"); break;
                            }
                        }
                        else
                        {
                            context.Parent.Emit(0, "    " + kvp.Value.ToC(context) + " loc_" + kvp.Key.ToString("X4") + ";");
                        }
                    }
                }
            }
        }
    }
}