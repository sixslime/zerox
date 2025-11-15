namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax.Structure;

using u.Anchors;
using u.Identifier;
using u.Data;
using u.Constructs.Move;
using Core = Core.Syntax.Core;
using u.Constructs;
using u.Constructs.Ability;
using Korvessas.Template;

public class Templates
{
    public TemplateNumericalMove NumericalMove => new();
    public TemplatePositionalMove PositionalMove => new();
}