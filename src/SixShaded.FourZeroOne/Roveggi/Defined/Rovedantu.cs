namespace SixShaded.FourZeroOne.Roveggi.Defined;

using Roveggi;

/// <summary>
///     Allows 'Get' 'Insert' and 'Redact' Korssas.
/// </summary>
public abstract class Rovedantu<R> : Rovetu, IRovedantu<R>
    where R : class, Rog;