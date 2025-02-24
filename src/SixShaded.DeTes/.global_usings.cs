global using SixShaded.NotRust;
global using SixShaded.SixLib.GFunc;
global using SixShaded.FourZeroOne.FZOSpec;
global using SixShaded.FourZeroOne.Token;
global using SixShaded.DeTes.Analysis;
global using SixShaded.DeTes.Realization;
global using SixShaded.DeTes.Declaration;

global using ResOpt = SixShaded.NotRust.IOption<SixShaded.FourZeroOne.Resolution.IResolution>;
global using Res = SixShaded.FourZeroOne.Resolution.IResolution;
global using Tok = SixShaded.FourZeroOne.Token.IToken<SixShaded.FourZeroOne.Resolution.IResolution>;
global using CriticalPointType = SixShaded.NotRust.IResult<SixShaded.NotRust.IResult<SixShaded.FourZeroOne.FZOSpec.EProcessorHalt, System.Exception>, SixShaded.DeTes.Analysis.IDeTesSelectionPath[]>;