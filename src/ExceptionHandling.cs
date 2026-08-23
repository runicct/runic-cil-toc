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

using System.Collections.Generic;
using System.Text;
using static Runic.CIL.ToC;
using static Runic.CIL.ToC.Context;
using static Runic.CIL.ToC.Signature;

namespace Runic.CIL
{
    public abstract partial class ToC
    {
#if NET6_0_OR_GREATER
        internal class TryCatchFinally : IComparable<TryCatchFinally>
#else
        internal class TryCatchFinally : System.IComparable
#endif
        {
            class Lock { }
            static Lock _lock = new Lock();
            static ulong _nextId = 0;
            ulong _id;
            public ulong Id { get { return _id; } }
            int _tryOffset;
            public int TryOffset { get { return _tryOffset; } }
            int _tryLength;
            public int TryLength { get { return _tryLength; } }
            List<ExceptionHandlingClause> _clauses = new List<ExceptionHandlingClause>();
            public List<ExceptionHandlingClause> Clauses { get { return _clauses; } }
#if NET6_0_OR_GREATER
            ExceptionHandlingClause.Finally? _finally = null;
            public ExceptionHandlingClause.Finally? Finally { get { return _finally; } }
#else
            ExceptionHandlingClause.Finally _finally = null;
            public ExceptionHandlingClause.Finally Finally { get { return _finally; } }
#endif
            public void AddClause(ExceptionHandlingClause clause)
            {
                clause.TryCatchFinally = this;
                switch (clause)
                {
                    case ExceptionHandlingClause.Finally @finally:
                        if (_finally != null) { throw new System.Exception("Cannot have more than one finally clause per try block."); }
                        _finally = @finally;
                        break;
                }
                _clauses.Add(clause);
            }
#if NET6_0_OR_GREATER
            public int CompareTo(TryCatchFinally? other)
#else
            public int CompareTo(object otherObj)
#endif
            {
#if NET6_0_OR_GREATER
                if (other == null) { return 1; }

#else
                    if (otherObj == null) { return 1; }
                    TryCatchFinally other = otherObj as TryCatchFinally;
                    if (other == null) { return 1; }
#endif


                if (_tryOffset < other._tryOffset) { return 1; }
                else if (_tryOffset > other._tryOffset) { return -1; }
                else
                {
                    if (_tryLength < other._tryLength) { return 1; }
                    else if (_tryLength > other._tryLength) { return -1; }
                    else { return -_id.CompareTo(other._id); }
                }
            }

            public TryCatchFinally(int tryOffset, int tryLength)
            {
                lock (_lock)
                {
                    _id = _nextId++;
                }
                _tryOffset = tryOffset;
                _tryLength = tryLength;
            }
        }
        internal abstract class EhcMapEntry
        {
#if NET6_0_OR_GREATER
            EhcMapEntry? _parent = null;
            public EhcMapEntry? Parent { get { return _parent; } internal set { _parent = value; } }
#else
            EhcMapEntry _parent = null;
            public EhcMapEntry Parent { get { return _parent; } internal set { _parent = value; } }
#endif
            public abstract int Length { get; }
            public abstract int Offset { get; }
            public bool Contains(EhcMapEntry entry)
            {
                if (entry == null) { return false; }
                return (entry.Offset >= Offset) && (entry.Offset < (Offset + Length));
            }
            internal class Try : EhcMapEntry
            {
                TryCatchFinally _tryCatchFinally;
                public TryCatchFinally TryCatchFinally { get { return _tryCatchFinally; } }
                public override int Length { get { return _tryCatchFinally.TryLength; } }
                public override int Offset { get { return _tryCatchFinally.TryOffset; } }

                public Try(TryCatchFinally tryCatchFinally)
                {
                    _tryCatchFinally = tryCatchFinally;
                }
            }
            internal class Clause : EhcMapEntry
            {
                ExceptionHandlingClause _clause;
                public ExceptionHandlingClause EhcClause { get { return _clause; } }
                public override int Length { get { return _clause.HandlerLength; } }
                public override int Offset { get { return _clause.HandlerOffset; } }
                public Clause(ExceptionHandlingClause clause)
                {
                    _clause = clause;
                }
            }
        }
        static internal TryCatchFinally[] MergeEhc(ExceptionHandlingClause[] exceptionHandlingClauses)
        {
            List<TryCatchFinally> tryCatchFinallyList = new List<TryCatchFinally>();
            Dictionary<int, Dictionary<int, TryCatchFinally>> tryEntries = new Dictionary<int, Dictionary<int, TryCatchFinally>>();
            for (int n = 0; n < exceptionHandlingClauses.Length; n++)
            {
                TryCatchFinally tryCatchFinally;
                Dictionary<int, TryCatchFinally> tryCatchFinallyDict;
                if (!tryEntries.TryGetValue(exceptionHandlingClauses[n].TryOffset, out tryCatchFinallyDict))
                {
                    tryCatchFinallyDict = new Dictionary<int, TryCatchFinally>();
                    tryEntries.Add(exceptionHandlingClauses[n].TryOffset, tryCatchFinallyDict);
                }
                if (!tryCatchFinallyDict.TryGetValue(exceptionHandlingClauses[n].TryLength, out tryCatchFinally))
                {
                    tryCatchFinally = new TryCatchFinally(exceptionHandlingClauses[n].TryOffset, exceptionHandlingClauses[n].TryLength);
                    tryCatchFinallyDict.Add(exceptionHandlingClauses[n].TryLength, tryCatchFinally);
                    tryCatchFinallyList.Add(tryCatchFinally);

                }
                tryCatchFinally.AddClause(exceptionHandlingClauses[n]);
            }

            return tryCatchFinallyList.ToArray();
        }
        static internal EhcMapEntry[] CreateEhcMap(uint totalSize, TryCatchFinally[] ehc)
        {
            // To simplify the logic we allocate one extra entry so we can always compare the next instruction offset without having to check for out of bounds.
            List<EhcMapEntry> ehcMapEntries = new List<EhcMapEntry>();
            for (int n = 0; n < ehc.Length; n++)
            {
                ehcMapEntries.Add(new EhcMapEntry.Try(ehc[n]));
                foreach (ExceptionHandlingClause clause in ehc[n].Clauses)
                {
                    ehcMapEntries.Add(new EhcMapEntry.Clause(clause));
                }
            }

            ehcMapEntries.Sort((EhcMapEntry a, EhcMapEntry b) => { return -a.Length.CompareTo(b.Offset); });

            EhcMapEntry[] map = new EhcMapEntry[totalSize + 1];
            for (int n = 0; n < ehcMapEntries.Count; n++)
            {
                for (int x = 0; x < ehcMapEntries[n].Length; x++)
                {
                    int offset = ehcMapEntries[n].Offset + x;
                    if (map[offset] == null) { map[offset] = ehcMapEntries[n]; }
                    else { ehcMapEntries[n].Parent = map[offset]; map[offset] = ehcMapEntries[n]; }
                }
            }

            return map;
        }


        static Destackifier.ExceptionHandlingClause[] ConvertToDestakifierEhc(ExceptionHandlingClause[] exceptionHandlingClauses)
        {
            Destackifier.ExceptionHandlingClause[] convertedEhc = new Destackifier.ExceptionHandlingClause[exceptionHandlingClauses.Length];

            for (int n = 0; n < exceptionHandlingClauses.Length; n++)
            {
                switch (exceptionHandlingClauses[n])
                {
                    case ExceptionHandlingClause.Filter filter: convertedEhc[n] = new Destackifier.ExceptionHandlingClause.Filter(filter.FilterOffset, filter.HandlerOffset); break;
                    case ExceptionHandlingClause.Clause clause: convertedEhc[n] = new Destackifier.ExceptionHandlingClause.Clause(clause.HandlerOffset); break;
                    case ExceptionHandlingClause.Finally @finally: convertedEhc[n] = new Destackifier.ExceptionHandlingClause.Clause(@finally.HandlerOffset); break;
                }
            }

            return convertedEhc;
        }
    }

}
