namespace SixShaded.FourZeroOne.Roveggi;

public record VarovaWrapper<C, RKey, RVal>(RKey keyRoggi) : IRovu<C, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public RKey KeyRoggi { get; } = keyRoggi;
    public override int GetHashCode() => HashCode.Combine(KeyRoggi, typeof(RKey));
    public override string? ToString() => KeyRoggi.ToString();
    public virtual bool Equals(VarovaWrapper<C, RKey, RVal>? other) => other is not null && other.KeyRoggi.Equals(KeyRoggi);
}