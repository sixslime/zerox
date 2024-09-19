/// <summary>
/// [Specialized Delegate] <br></br>
/// <i>A template to create new objects with the same construction.</i>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Must construct and <see langword="return"/> -> a <see langword="new"/> <typeparamref name="T"/> instance.
/// <br></br>
/// > Constructed type may inherit[ : ] from <typeparamref name="T"/>.
/// </remarks>
public delegate T ConstructionTemplate<T>() where T : class;