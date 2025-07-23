namespace SixShaded.FourZeroOne;

using GetMapping = Dictionary<Roveggi.Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>>;
using SetMapping = Dictionary<Roveggi.Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>>;
// TODO
// temporary name, this is the equavalent of an "Assembly" of 401
public class Master
{
    internal static readonly Master ASSEMBLY = new();

    internal RovenAssemblyData RovenData { get; } = new();


    /// <summary>
    /// type parameter must be the "Axoi" class of an axoi. <br></br>
    /// Quite frankly this is some of my most preposterous work.
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
        (Type genericType, int genericIndex)[] validFieldTypes = [(typeof(Roveggi.Defined.Rovu<,>), 0), (typeof(Roveggi.Defined.AbstractGetRovu<,>), 0), (typeof(Roveggi.Defined.AbstractSetRovu<,>), 0),];
        foreach (var rovetuType in rovetuTypes)
        {
            var fields = rovetuType.GetFields();
            bool isAbstract = !rovetuType.IsAssignableTo(typeof(IConcreteRovetu));
            foreach (var field in fields)
            {
                // validate field is static:
                if (!field.IsStatic) 
                    throw new MetaAssemblyException(axoi, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is not static.");

                // validate field is not null:
                object? fieldValue = field.GetValue(null);
                if (fieldValue is null) 
                    throw new MetaAssemblyException(axoi, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is null.");

                var fieldType = field.FieldType;

                if (field.Name == "__IMPLEMENTS")
                {
                    // validate __implements type:
                    if (!(fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Roveggi.ImplementationStatement<>) && fieldType.GetGenericArguments()[0] == rovetuType))
                        throw new MetaAssemblyException(axoi, $"'__IMPLEMENTS' field in '{rovetuType.Name}' must be of type ImplementationStatement<{rovetuType.Name}>");

                    // construct context:
                    var implContextType = typeof(Roveggi.ImplementationContext<>).MakeGenericType(rovetuType);
                    object? implContext = implContextType.GetConstructor([])?.Invoke([]);
                    if (implContext is null) 
                        throw new PleaseFixException("cannot create ImplementationContext (probably because it doesn't have an empty constructor)");

                    // call implementation statement:
                    ((Delegate)fieldValue).DynamicInvoke(implContext);

                    // get mappings from context object:
                    object? getMapValue = implContextType.GetProperty(ImplementationContext<IRovetu>.GETMAPPINGS_PROPERTY)?.GetValue(implContext);
                    object? setMapValue = implContextType.GetProperty(ImplementationContext<IRovetu>.SETMAPPINGS_PROPERTY)?.GetValue(implContext);
                    if (getMapValue is null) 
                        throw new PleaseFixException($"get mappings from ImplementationContext was null, probably because there is no property named '{ImplementationContext<IRovetu>.GETMAPPINGS_PROPERTY}'");
                    if (setMapValue is null)
                        throw new PleaseFixException($"set mappings from ImplementationContext was null, probably because there is no property named '{ImplementationContext<IRovetu>.SETMAPPINGS_PROPERTY}'");
                    if (getMapValue is not GetMapping getMap) 
                        throw new PleaseFixException($"get mappings in ImplementationContext is not of exact type '{typeof(GetMapping).Name}'");
                    if (setMapValue is not GetMapping setMap)
                        throw new PleaseFixException($"set mappings in ImplementationContext is not of exact type '{typeof(SetMapping).Name}'");

                    ASSEMBLY.RovenData.GetImplementations[rovetuType] = getMap;
                    ASSEMBLY.RovenData.SetImplementations[rovetuType] = setMap;
                    continue;
                }

                // validate rovu type:
                if (!(fieldType.IsGenericType && validFieldTypes.Any(x => fieldType.GetGenericTypeDefinition() == x.genericType && fieldType.GenericTypeArguments[x.genericIndex] == rovetuType)))
                    throw new MetaAssemblyException(axoi, $"'{field.Name}' field in rovetu '{rovetuType.Name}' is not a valid type ({fieldType.Name}). Valid types are:\n{validFieldTypes.Map(x => x.genericType.Name + "\n")}");

                // validate field is not abstract if rovetu is concrete:
                if (!isAbstract && fieldType.IsAssignableTo(typeof(Roveggi.Unsafe.IAbstractRovu)))
                    throw new MetaAssemblyException(axoi, $"'{field.Name}' field is abstract, but rovetu '{rovetuType.Name}' is concrete.");

            }
            // TODO: check that all abstract rovus are implemented
        }
    }

    public class MetaAssemblyException(Type axoi, string message) : Exception($"FourZeroOne Assembly Error\n Axoi: {axoi.Namespace!.Split(".")[^1]}\n{message}");

    internal class PleaseFixException(string message) : Exception($"[Master] PLZ FIX: {message}");
    internal class RovenAssemblyData
    {
        internal Dictionary<Type, GetMapping> GetImplementations { get; } = new();
        internal Dictionary<Type, SetMapping> SetImplementations { get; } = new();
    }
}