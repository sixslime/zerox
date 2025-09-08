namespace SixShaded.FourZeroOne;

using System.Reflection;
using GetMapping = Dictionary<Roveggi.Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>>;
using SetMapping = Dictionary<Roveggi.Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>>;

// TODO
// temporary name, this is the equavalent of an "Assembly" of 401
// refactor this bullshit to use IsAxoi objects.
public class Master
{
    internal static readonly Master ASSEMBLY = new();
    internal RovenAssemblyData RovenData { get; } = new();
    internal Dictionary<Type, IsAxoi> RegisteredAxois { get; } = new();

    public static event EventHandler<RegisterAxoiEventArgs> OnRegisterAxoiEvent;
    static Master()
    {
        RegisterAxoi<Core.Axoi>();
    }
    /// <summary>
    /// type parameter must be the "Axoi" class of an axoi. <br></br>
    /// Quite frankly this is some of my most preposterous work. <br></br>
    /// MUST BE RAN FOR EVERY INCLUDED AXOI BEFORE PROGRAM STARTS.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    public static void RegisterAxoi<X>()
        where X : IsAxoi
    {
        // TODO
        // - make better exception messages.
        // - split into functions

        var axoiType = typeof(X);
        var axoi = (IsAxoi)axoiType.GetConstructor([typeof(AxoiCreationKey)])!.Invoke([AxoiCreationKey.KEY]);
        if (!ASSEMBLY.RegisteredAxois.TryAdd(axoiType, axoi)) return;

        // DEBUG
        Console.WriteLine($"REGISTERING AXOI: {axoiType.Namespace!.Split(".")[^1]}");

        Type[] rovetuTypes = axoiType.Assembly.ExportedTypes.Where(x => x.IsAssignableTo(typeof(IRovetu))).ToArray();
        (Type genericType, int genericIndex)[] validFieldTypes = [(typeof(Roveggi.Defined.Rovu<,>), 0), (typeof(Roveggi.Defined.AbstractGetRovu<,>), 0), (typeof(Roveggi.Defined.AbstractSetRovu<,>), 0), (typeof(Roveggi.Defined.Varovu<,,>), 0)];
        var implementedAbstractRovus = new Dictionary<Type, HashSet<Roveggi.Unsafe.IAbstractRovu>>();

        // for all rovetus:
        foreach (var rovetuType in rovetuTypes)
        {
            var fields = rovetuType.GetFields();
            bool isAbstract = !rovetuType.IsAssignableTo(typeof(IConcreteRovetu));
            implementedAbstractRovus[rovetuType] = new();

            // for all fields:
            foreach (var field in fields)
            {
                // validate field is static:
                if (!field.IsStatic)
                    throw new MetaAssemblyException(axoiType, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is not static.");

                // validate field is not null:
                object? fieldValue = field.GetValue(null);
                if (fieldValue is null)
                    throw new MetaAssemblyException(axoiType, $"field '{field.Name}' in rovetu '{rovetuType.Name}' is null.");
                var fieldType = field.FieldType;
                if (field.Name == "__IMPLEMENTS")
                {
                    // validate rovetu is not abstract:
                    if (isAbstract)
                        throw new MetaAssemblyException(axoiType, $"'{rovetuType.Name}' is abstract but has an '__IMPLEMENTS' field (not allowed).");

                    // validate __implements type:
                    if (!(fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(ImplementationStatement<>) && fieldType.GetGenericArguments()[0] == rovetuType))
                        throw new MetaAssemblyException(axoiType, $"'__IMPLEMENTS' field in '{rovetuType.Name}' must be of type ImplementationStatement<{rovetuType.Name}>");

                    // construct context:
                    var implContextType = typeof(ImplementationContext<>).MakeGenericType(rovetuType);
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
                    implementedAbstractRovus[rovetuType] = [..getMap.Keys, ..setMap.Keys];
                    continue;
                }
                // validate rovu type:
                if (!(fieldType.IsGenericType && validFieldTypes.Any(x => fieldType.GetGenericTypeDefinition() == x.genericType && fieldType.GenericTypeArguments[x.genericIndex] == rovetuType)))
                {
                    throw new MetaAssemblyException(
                    axoiType, $"'{field.Name}' field in rovetu '{rovetuType.Name}' is not a valid type ({fieldType.Name})." +
                          $"Valid types are:\n{string.Join("\n", validFieldTypes.Map(x => x.genericType.Name))}");
                }

                // validate field is not abstract if rovetu is concrete:
                if (!isAbstract && fieldType.IsAssignableTo(typeof(Roveggi.Unsafe.IAbstractRovu)))
                    throw new MetaAssemblyException(axoiType, $"'{field.Name}' field is abstract, but rovetu '{rovetuType.Name}' is concrete.");
            }
        }

        // for all concrete rovetus:
        foreach (var rovetuType in rovetuTypes.Where(x => x.IsAssignableTo(typeof(IConcreteRovetu))))
        {
            // validate all abstract rovus are implemented:
            foreach (var abstractBaseRovetuType in rovetuType.GetInterfaces()
                .Where(x => x.IsAssignableTo(typeof(IRovetu)) && !x.IsAssignableTo(typeof(IConcreteRovetu)) && x != typeof(IRovetu) && x != typeof(IConcreteRovetu)))
            {
                foreach (var field in abstractBaseRovetuType.GetFields())
                {
                    if (field.GetValue(null) is Roveggi.Unsafe.IAbstractRovu abstractRovu && !implementedAbstractRovus[rovetuType].Remove(abstractRovu))
                        throw new MetaAssemblyException(axoiType, $"rovetu '{rovetuType.Name}' does not implement '{field.Name}' from '{abstractBaseRovetuType.Name}'.");
                }
            }
            if (implementedAbstractRovus[rovetuType].Count > 0)
                throw new MetaAssemblyException(axoiType, $"rovetu '{rovetuType.Name}' implements rovus from rovetus it does not inheret from:\n {string.Join("\n", implementedAbstractRovus[rovetuType])})");
        }
        // maybe not pass ASSEMBLY?
        // idk this shit is a mess.
        OnRegisterAxoiEvent(
        ASSEMBLY, new RegisterAxoiEventArgs()
        {
            Axoi = axoi
        });
    }

    public record RegisterAxoiEventArgs
    {
        public required IsAxoi Axoi { get; init; }
    }
    public class AxoiCreationKey
    {
        internal static readonly AxoiCreationKey KEY = new();
        private AxoiCreationKey() { }
    }
    public class MetaAssemblyException(Type axoi, string message) : Exception($"FourZeroOne Assembly Error\n Axoi: {axoi.Namespace!.Split(".")[^1]}\n{message}");

    internal class PleaseFixException(string message) : Exception($"[Master] PLZ FIX: {message}");

    internal class RovenAssemblyData
    {
        /// <summary>
        /// every entry should satisfy <b>{ for A, R | AbstractGetRovu&lt;A, R&gt; -&gt; MetaFunction&lt;IRoveggi&lt;C&gt;, R&gt; }</b>
        /// </summary>
        internal Dictionary<Type, GetMapping> GetImplementations { get; } = new();
        /// <summary>
        /// every entry should satisfy <b>{ for A, R | AbstractSetRovu&lt;A, R&gt; -&gt; MetaFunction&lt;IRoveggi&lt;C&gt;&gt;, R, IRoveggi&lt;C&gt;&gt; }</b>
        /// </summary>
        internal Dictionary<Type, SetMapping> SetImplementations { get; } = new();
    }
}