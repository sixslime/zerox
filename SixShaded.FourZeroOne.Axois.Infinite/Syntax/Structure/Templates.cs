namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax.Structure;

using u.Anchors;
using u.Identifier;
using u.Data;
using u.Constructs.Move;
using Core = Core.Syntax.Core;
using u.Constructs;
using u.Constructs.Ability;

public class Templates
{
    public IKorssa<IRoveggi<uNumericalMove>> NumericalMove => Korvessas.Template.TemplateNumericalMove.Construct();
    public IKorssa<IRoveggi<uPositionalMove>> PositionalMove => Korvessas.Template.TemplatePositionalMove.Construct();
}