global using SixShaded.NotRust;
global using SixShaded.SixLib.GFunc;
global using SixShaded.FourZeroOne.FZOSpec;
global using SixShaded.FourZeroOne.Korssa;
global using SixShaded.DeTes.Analysis;
global using SixShaded.DeTes.Realization;
global using SixShaded.DeTes.Declaration;

global using RogOpt = SixShaded.NotRust.IOption<SixShaded.FourZeroOne.Roggi.IRoggi>;
global using Rog = SixShaded.FourZeroOne.Roggi.IRoggi;
global using Kor = SixShaded.FourZeroOne.Korssa.IKorssa<SixShaded.FourZeroOne.Roggi.IRoggi>;
global using CriticalPointType = SixShaded.NotRust.IResult<SixShaded.NotRust.IResult<SixShaded.FourZeroOne.FZOSpec.EProcessorHalt, System.Exception>, SixShaded.DeTes.Analysis.IDeTesSelectionPath[]>;