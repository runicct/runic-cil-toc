# Introduction
Runic.CIL.ToC provides a way to convert a method into pure C code. Note that this is only limitted to methods and you will need to use
Runic.Dotnet.AssemblyToC if you want a full solution that can convert an entire assembly to pure C code.

# Sample

The following toy sample illustrate how to use Runic.CIL.ToC. Note that a real implementation would have proper error checking and
a much better harness but this showcases the basics:

```csharp
        static int Add(int a, int b) { return a + b; }
        static System.Reflection.MethodInfo GetMethodInfo<T>(T de) where T : Delegate
        {
            return de.Method;
        }
        static unsafe void Main(string[] args)
        {
            int[] result = new int[0];
            System.Reflection.MethodInfo mi = GetMethodInfo(Add);
            ToC toC = new ToC(mi.Module);
            toC.Process((uint)mi.MetadataToken, mi.GetMethodBody().GetILAsByteArray());
        }
        class ToC : Runic.CIL.ToC
        {
            System.Reflection.Module _module;
            public ToC(System.Reflection.Module module) { _module = module; }
            public override byte[] GetMethodSignature(uint methodToken)
            {
                if (methodToken == 0) return new byte[0];
                return _module.ResolveSignature((int)methodToken);
            }
            public override byte[] GetLocalsSignature(uint methodToken) 
            {
                if (methodToken == 0) return new byte[0];
                int localSigToken = _module.ResolveMethod((int)methodToken).GetMethodBody().LocalSignatureMetadataToken;
                if (localSigToken == 0) return new byte[0];
                return _module.ResolveSignature(localSigToken);
            }
            public override byte[] GetFieldSignature(uint fieldToken) 
            {
                if (fieldToken == 0) return new byte[0];
                return _module.ResolveSignature((int)fieldToken);
            }
            public override uint GetDeclaringType(uint methodToken) { return (uint)(_module.ResolveMethod((int)methodToken)).MetadataToken; }
            public override bool IsValueType(uint typeToken) { return _module.ResolveType((int)typeToken, null, null).IsValueType; }
            public override void Emit(int offset, string code) { Console.WriteLine(code); }
            public override void Emit(string code) { Console.WriteLine(code); }

            public override uint GetRuntimeTypeHandleToken()
            {
                throw new NotImplementedException();
            }
        }
```

And it should produce in Release mode:
```
int32_t m_06000001(int32_t arg_0000, int32_t arg_0001)
{
    int32_t loc_0000;
    int32_t loc_0001;
    int32_t loc_0002;
    loc_0000 = (int32_t)(arg_0000);
    loc_0001 = (int32_t)(arg_0001);
    loc_0002 = (int32_t)(loc_0000 + loc_0001);
    return loc_0002;
}
```
