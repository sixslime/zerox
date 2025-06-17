namespace SixShaded.VeiledOhOne;

public class VeilBuilder
{
    public delegate T BindingExpression<out T>(IVeilBindingContext context);
    private Kor? _korssa = null;
    private IMemoryFZO? _memory = null;
    private IVeilBindingContext _bindContext = null;
    internal VeilBuilder()
    {

    }

    public VeilBuilder BindKorssa(BindingExpression<Kor> expression)
    {
        _korssa = expression(_bindContext);
        return this;
    }
    public VeilBuilder BindMemory(BindingExpression<IMemoryFZO> expression)
    {
        _memory = expression(_bindContext);
        return this;
    }

    public Veil Build()
    {
        throw new NotImplementedException();
    }
}