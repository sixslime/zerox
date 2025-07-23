namespace SixShaded.FourZeroOne.Roveggi;

// temp signature
public delegate void ImplementationStatement<C, A>(ImplementationContext<C, A> context)
    where A : IRovetu
    where C : IRovetu;