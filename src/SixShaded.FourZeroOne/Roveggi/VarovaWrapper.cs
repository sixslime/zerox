﻿namespace SixShaded.FourZeroOne.Roveggi;

public class VarovaWrapper<C, RKey, RVal>(RKey keyRoggi) : IRovu<C, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public RKey KeyRoggi { get; } = keyRoggi;
    public override int GetHashCode() => KeyRoggi.GetHashCode();
    public override string? ToString() => KeyRoggi.ToString();
    public override bool Equals(object? other) => other is VarovaWrapper<C, RKey, RVal> v && v.KeyRoggi.Equals(KeyRoggi);
}