namespace SixShaded.FourZeroOne;

using GetMapping = Dictionary<Roveggi.Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>>;
// TODO
// temporary name, this is the equavalent of an "Assembly" of 401
public class Master
{
    internal static readonly Master ASSEMBLY = new();

    internal RovenAssemblyData RovenData { get; } = new();


    /// <summary>
    /// type parameter must be the "Axoi" class of an axoi. <br></br>
    /// It's so stupid it just might work.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    public static void RegisterAxoi<X>() where X : IsAxoi
    {
        // TODO
        // - make better exception messages.

        // PROCESS
        // - validate all rovetus
        // - map abstract rovus to their implementation



        Type axoi = typeof(X);

        // DEBUG
        Console.WriteLine($"REGISTERING AXOI: {axoi.Namespace!.Split(".")[^1]}");

        Type[] rovetuTypes = axoi.Assembly.ExportedTypes.Where(x => x.IsAssignableTo(typeof(IRovetu))).ToArray();
        foreach (var rovetuType in rovetuTypes)
        {
            var fields = rovetuType.GetFields();
            foreach (var field in fields)
            {
                if (!field.IsStatic) 
                    throw new MetaAssemblyException(axoi, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is not static.");

                object? fieldValue = field.GetValue(null);
                if (fieldValue is null) 
                    throw new MetaAssemblyException(axoi, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is null.");

                var fieldType = field.FieldType;
                if (field.Name == "__IMPLEMENTS")
                {
                    // validate field type:
                    if (!(fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Roveggi.ImplementationStatement<>) && fieldType.GetGenericArguments()[0] == rovetuType))
                        throw new MetaAssemblyException(axoi, $"'__IMPLEMENTS' field in '{rovetuType.Name}' must be of type ImplementationStatement<{rovetuType.Name}>");

                    // construct context:
                    Type implContextType = typeof(Roveggi.ImplementationContext<>).MakeGenericType(rovetuType);
                    object? implContext = implContextType.GetConstructor([])?.Invoke([]);
                    if (implContext is null) 
                        throw new PleaseFixException("cannot create ImplementationContext (probably because it doesn't have an empty constructor)");

                    // call implementation statement:
                    ((Delegate)fieldValue).DynamicInvoke(implContext);

                    // get mappings from context object:
                    var getMapValue = implContextType.GetProperty(ImplementationContext<IRovetu>.GETMAPPINGS_PROPERTY)?.GetValue(implContext);
                    if (getMapValue is null) 
                        throw new PleaseFixException($"get mappings from ImplementationContext was null, probably because there is no property named '{ImplementationContext<IRovetu>.GETMAPPINGS_PROPERTY}'");
                    if (getMapValue is not GetMapping getMap) 
                        throw new PleaseFixException($"get mappings in ImplementationContext is not of exact type '{typeof(GetMapping).Name}'");

                    ASSEMBLY.RovenData.GetImplementations[rovetuType] = getMap;
                    continue;
                }

            }
            // TODO: check that all abstract rovus are implemented
        }
    }

    public class MetaAssemblyException(Type axoi, string message) : Exception($"FourZeroOne Assembly Error\n Axoi: {axoi.Namespace!.Split(".")[^1]}\n{message}");

    internal class PleaseFixException(string message) : Exception($"(In Master): PLZ FIX (you goofed): {message}");
    internal class RovenAssemblyData
    {
        internal Dictionary<Type, GetMapping> GetImplementations { get; } = new();
    }
}