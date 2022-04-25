using CSP.Futoshiki;
using CSP.Helpers;
using CSP.CSPBase;
using CSP.Binary;
using CSP.CSPBase.VariableSelection;


//IVariableSelectionStrategy<(int, int), int> selectionStrategy = new FindFirst<(int, int), int>();
IVariableSelectionStrategy<(int, int), int> selectionStrategy = new BiggestAmmoutOfConstraints<(int, int), int>();

var (cpsFuto4, fut4InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_4x4", selectionStrategy);
var sollutions11 = cpsFuto4._BT(fut4InitialAssigment);
Console.WriteLine(cpsFuto4.GetMeasurements());
var sollutions12 = cpsFuto4.FC(fut4InitialAssigment);
Console.WriteLine(cpsFuto4.GetMeasurements());

Console.WriteLine("BT Futo 4x4: ");
foreach (var sollution in sollutions11)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Futo 4x4: ");
foreach (var sollution in sollutions12)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cpsFuto5, fut5InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_5x5", selectionStrategy);
var sollutions21 = cpsFuto5._BT(fut5InitialAssigment);
Console.WriteLine(cpsFuto5.GetMeasurements());
var sollutions22 = cpsFuto5.FC(fut5InitialAssigment);
Console.WriteLine(cpsFuto5.GetMeasurements());

Console.WriteLine("BC Futo 5x5: ");
foreach (var sollution in sollutions21)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Futo 5x5: ");
foreach (var sollution in sollutions22)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cpsFuto6, fut6InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_6x6", selectionStrategy);
var sollution31 = cpsFuto6._BT(fut6InitialAssigment);
Console.WriteLine(cpsFuto6.GetMeasurements());
var sollution32 = cpsFuto6.FC(fut6InitialAssigment);
Console.WriteLine(cpsFuto6.GetMeasurements());

//Console.WriteLine("BC Futo 6x6: ");
//foreach (var sollution in sollution31)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

//Console.WriteLine("FC Futo 6x6: ");
//foreach (var sollution in sollution32)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary6, bin6InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_6x6", selectionStrategy);
var binSollution61 = cspBinary6._BT(bin6InitialAssigment);
Console.WriteLine(cspBinary6.GetMeasurements());
var binSollution62 = cspBinary6.FC(bin6InitialAssigment);
Console.WriteLine(cspBinary6.GetMeasurements());

Console.WriteLine("BT Binary 6x6: ");
foreach (var sollution in binSollution61)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 6x6: ");
foreach (var sollution in binSollution62)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary8, bin8InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_8x8", selectionStrategy);
var binSollution81 = cspBinary8._BT(bin8InitialAssigment);
Console.WriteLine(cspBinary8.GetMeasurements());
var binSollution82 = cspBinary8.FC(bin8InitialAssigment);
Console.WriteLine(cspBinary8.GetMeasurements());

Console.WriteLine("BT Binary 8x8: ");
foreach (var sollution in binSollution81)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 8x8: ");
foreach (var sollution in binSollution82)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary10, bin10InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_10x10", selectionStrategy);
var binSollution101 = cspBinary10._BT(bin10InitialAssigment);
Console.WriteLine(cspBinary10.GetMeasurements());
var binSollution102 = cspBinary10.FC(bin10InitialAssigment);
Console.WriteLine(cspBinary10.GetMeasurements());

Console.WriteLine("BT Binary 10x10: ");
foreach (var sollution in binSollution101)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 10x10: ");
foreach (var sollution in binSollution102)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("END");