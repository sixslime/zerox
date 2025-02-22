
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    public interface IStateFZO
    {
        public IOption<FZOSource> Initialized { get; }
        public IEnumerable<IOperationNode> OperationStack { get; }
        public IEnumerable<ETokenMutation> TokenMutationStack { get; }

        // FIXME: needs updated specification
        /// <summary>
        /// If <paramref name="step"/> is:<br></br>
        /// <b><see cref="EProcessorStep.TokenMutate"/>:</b><br></br>
        /// - Push 'Value' to <i>TokenPrepStack</i><br></br>
        /// <b><see cref="EProcessorStep.PushOperation"/>:</b><br></br>
        /// - Push the following to <i>OperationStack</i>:<br></br>
        /// . ~ <i>Operation</i> = 'OperationToken'<br></br>
        /// . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
        /// . ~ <i>ArgResolutionStack</i> = { }<br></br>
        /// <b><see cref="EProcessorStep.Resolve"/>:</b><br></br>
        /// - Pop from <i>OperationStack</i><br></br>
        /// - If 'Resolution' is <b><see cref="IOk{int,}"/></b>:<br></br>
        /// - - Push 'Value' to <i>OperationStack[0].ArgResolutionStack</i><br></br>
        /// - - If 'Value' is <b><see cref="IOk{T, E}"/></b>:<br></br>
        /// - - - Push the equivalent of the following to <i>OperationStack[0].MemoryStack</i>:<br></br>
        /// - - . # <c> Value.Instructions </c>\<br></br>
        /// - - . # <c> .AccumulateInto(OperationStack[0].MemoryStack[0], </c>\<br></br>
        /// - - . # <c> (memory, instruction) => instruction.TransformMemory(memory)); </c><br></br>
        /// - If 'Resolution' is <b><see cref="IErr{T, E}"/></b>:<br></br>
        /// - - If 'Value' is <b><see cref="EStateImplemented.MetaExecute"/></b>:<br></br>
        /// - - - Push the following to <i>OperationStack</i>:<br></br>
        /// - - . ~ <i>Operation</i> = 'FunctionToken'<br></br>
        /// - - . ~ <i>MemoryStack</i> = { <i>OperationStack[0].MemoryStack[0]</i> }<br></br>
        /// - - . ~ <i>ArgResolutionStack</i> = { }<br></br>
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// A new <see cref="IStateFZO"/> with the above changes.<br></br>
        /// This <b>must</b> not mutate the original <see cref="IStateFZO"/>.
        /// </returns>
        public IStateFZO WithStep(EProcessorStep step);
        public IStateFZO Initialize(FZOSource source);
        public interface IOperationNode
        {
            public Tok Operation { get; }
            public IEnumerable<IOption<Res>> ArgResolutionStack { get; }
            public IEnumerable<IMemoryFZO> MemoryStack { get; }
        }
    }
}