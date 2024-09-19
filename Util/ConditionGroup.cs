using System.Collections;
using System.Collections.Generic;

public interface ICondition<T>
{
    public abstract bool Evaluate(T input);
}
public class ConditionGroup<T>
{
    public enum ECombiner
    {
        And,
        Or
    }
    private List<ConditionGroup<T>> _conditions;
    private List<ECombiner> _combiners;
    public List<ConditionGroup<T>> Conditions => new(_conditions);
    public List<ECombiner> Combiners => new(_combiners);

    public ConditionGroup<T> Combine(ECombiner combiner, ConditionGroup<T> condition)
    {
        _conditions.Add(condition);
        _combiners.Add(combiner);
        return this;
    }
    public ConditionGroup<T> Combine(ECombiner combiner, ICondition<T> condition)
    {
        _conditions.Add(new IdentityCondition(condition));
        _combiners.Add(combiner);
        return this;
    }
    public static ConditionGroup<T> Identity(ICondition<T> condition)
    {
        return new()
        {
            _conditions = new() { new IdentityCondition(condition) }
        };
    }
    private ConditionGroup()
    {
        _conditions = new();
        _combiners = new();
    }
    public virtual bool Evaluate(T input)
    {
        bool o = _conditions[0].Evaluate(input);
        for (int i = 1; i < _conditions.Count; i++)
        {
            switch (_combiners[i - 1])
            {
                case ECombiner.And:
                    o = (o && _conditions[i].Evaluate(input));
                    break;
                case ECombiner.Or:
                    o = (o || _conditions[i].Evaluate(input));
                    break;
            }
        }
        return o;
    }
    private class IdentityCondition : ConditionGroup<T>
    {
        public ICondition<T> Value { get; private set; }
        public IdentityCondition(ICondition<T> identity) { Value = identity; }
        public override bool Evaluate(T input) => Value.Evaluate(input);
    }
}