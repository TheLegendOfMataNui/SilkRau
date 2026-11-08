using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c03be959-5524-4d2f-ba5d-186ab28a27de")]


// The following lines make internal classes visible to tests and also
// the mocking framework. We don't want the signed assembly to expose
// any internal classes so that's guarded behind a macro.
#if !SIGN
[assembly: InternalsVisibleTo("SilkRau.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif
