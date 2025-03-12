namespace SixShaded.FourZeroOne.FZOSpec;

public interface IStateFZO
{
    public IOption<IOrigin> Initialized { get; }
    public IEnumerable<IOperationNode> OperationStack { get; }
    public IEnumerable<EKorssaMutation> KorssaMutationStack { get; }
    public IStateFZO WithStep(EProcessorStep step);

    public IStateFZO Initialize(IOrigin source);

    public interface IOperationNode
    {
        public Kor Operation { get; }
        public IEnumerable<RogOpt> ArgRoggiStack { get; }
        public IEnumerable<IMemoryFZO> MemoryStack { get; }
    }

    public interface IOrigin
    {
        public Kor Program { get; }
        public IMemoryFZO InitialMemory { get; }
    }
}