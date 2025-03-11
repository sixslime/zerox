namespace SixShaded.FourZeroOne.FZOSpec;

public interface IStateFZO
{
    public IOption<IOrigin> Initialized { get; }
    public IEnumerable<IOperationNode> OperationStack { get; }
    public IEnumerable<EKorssaMutation> KorssaMutationStack { get; }

    // FIXME: needs updated specification
    /// <summary>
    ///     If <paramref name="step" /> is:<br></br>
    ///     <b><see cref="EProcessorStep.KorssaMutate" />:</b><br></br>
    ///     - Push 'Value' to <i>KorssaPrepStack</i><br></br>
    ///     <b><see cref="EProcessorStep.PushOperation" />:</b><br></br>
    ///     - Push the following to <i>OperationStack</i>:<br></br>
    ///     . ~ <i>Operation</i> = 'OperationKorssa'<br></br>
    ///     . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
    ///     . ~ <i>ArgRoggiStack</i> = { }<br></br>
    ///     <b><see cref="EProcessorStep.Resolve" />:</b><br></br>
    ///     - Pop from <i>OperationStack</i><br></br>
    ///     - If 'Roggi' is
    ///     <b>
    ///         <see cref="IOk{T, E}" />
    ///     </b>
    ///     :<br></br>
    ///     - - Push 'Value' to <i>OperationStack[0].ArgRoggiStack</i><br></br>
    ///     - - If 'Value' is
    ///     <b>
    ///         <see cref="IOk{T, E}" />
    ///     </b>
    ///     :<br></br>
    ///     - - - Push the equivalent of the following to <i>OperationStack[0].MemoryStack</i>:<br></br>
    ///     - - . # <c> Value.Instructions </c>\<br></br>
    ///     - - . # <c> .AccumulateInto(OperationStack[0].MemoryStack[0], </c>\<br></br>
    ///     - - . # <c> (memory, instruction) => instruction.TransformMemory(memory)); </c><br></br>
    ///     - If 'Roggi' is
    ///     <b>
    ///         <see cref="IErr{T, E}" />
    ///     </b>
    ///     :<br></br>
    ///     - - If 'Value' is
    ///     <b>
    ///         <see cref="EStateImplemented.MetaExecute" />
    ///     </b>
    ///     :<br></br>
    ///     - - - Push the following to <i>OperationStack</i>:<br></br>
    ///     - - . ~ <i>Operation</i> = 'FunctionKorssa'<br></br>
    ///     - - . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
    ///     - - . ~ <i>ArgRoggiStack</i> = { }<br></br>
    /// </summary>
    /// <param name="step"></param>
    /// <returns>
    ///     A new <see cref="IStateFZO" /> with the above changes.<br></br>
    ///     This <b>must</b> not mutate the original <see cref="IStateFZO" />.
    /// </returns>
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