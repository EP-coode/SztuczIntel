using CSP.Futoshiki;
using CSP.Helpers;
using CSP.CSPBase;
using CSP.Binary;


IVariableSelectionStrategy<(int, int), int> selectionStrategy = new FindFirst<(int, int), int>();

var (cpsFuto4, fut4InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_4x4", selectionStrategy);
var sollutions11 = cpsFuto4.BT(fut4InitialAssigment);
var sollutions12 = cpsFuto4.FC(fut4InitialAssigment);

Console.WriteLine("BT Futo 4x4: ");
foreach (var sollution in sollutions11)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Futo 4x4: ");
foreach (var sollution in sollutions12)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cpsFuto5, fut5InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_5x5", selectionStrategy);
var sollutions21 = cpsFuto5.BT(fut5InitialAssigment);
var sollutions22 = cpsFuto5.FC(fut5InitialAssigment);

Console.WriteLine("BC Futo 5x5: ");
foreach (var sollution in sollutions21)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Futo 5x5: ");
foreach (var sollution in sollutions22)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cpsFuto6, fut6InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_6x6", selectionStrategy);
var sollution31 = cpsFuto6.BT(fut6InitialAssigment);
var sollution32 = cpsFuto6.FC(fut6InitialAssigment);

Console.WriteLine("BC Futo 6x6: ");
foreach (var sollution in sollution31)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Futo 6x6: ");
foreach (var sollution in sollution32)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary6, bin6InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_6x6", selectionStrategy);
var binSollution61 = cspBinary6.BT(bin6InitialAssigment);
var binSollution62 = cspBinary6.FC(bin6InitialAssigment);

Console.WriteLine("BT Binary 6x6: ");
foreach (var sollution in binSollution61)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 6x6: ");
foreach (var sollution in binSollution62)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary8, bin8InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_8x8", selectionStrategy);
var binSollution81 = cspBinary8.BT(bin8InitialAssigment);
var binSollution82 = cspBinary8.FC(bin8InitialAssigment);

Console.WriteLine("BT Binary 8x8: ");
foreach (var sollution in binSollution81)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 8x8: ");
foreach (var sollution in binSollution82)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

var (cspBinary10, bin10InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_10x10", selectionStrategy);
var binSollution101 = cspBinary10.BT(bin10InitialAssigment);
var binSollution102 = cspBinary10.FC(bin10InitialAssigment);

Console.WriteLine("BT Binary 10x10: ");
foreach (var sollution in binSollution101)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("FC Binary 10x10: ");
foreach (var sollution in binSollution102)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("END");