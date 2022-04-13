using CSP.Futoshiki;
using CSP.Helpers;
using CSP.CSPBase;
using CSP.Binary;


IVariableSelectionStrategy<(int, int), int> selectionStrategy = new FindFirst<(int, int), int>();

//var (cpsFuto4, fut4InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_4x4", selectionStrategy);
//var sollutions11 = cpsFuto4.BT(fut4InitialAssigment);
//var sollutions12 = cpsFuto4.FC(fut4InitialAssigment);

//Console.WriteLine("BT Futo 4x4: ");
//foreach (var sollution in sollutions11)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

//Console.WriteLine("FC Futo 4x4: ");
//foreach (var sollution in sollutions12)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

//var (cpsFuto5, fut5InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_5x5", selectionStrategy);
//var sollutions21 = cpsFuto5.BT(fut5InitialAssigment);
//var sollutions22 = cpsFuto5.FC(fut5InitialAssigment);

//Console.WriteLine("BC Futo 5x5: ");
//foreach (var sollution in sollutions21)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

//Console.WriteLine("FC Futo 5x5: ");
//foreach (var sollution in sollutions22)
//    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

//var (cpsFuto6, fut6InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_6x6", selectionStrategy);
//var sollution31 = cpsFuto6.BT(fut6InitialAssigment);
//var sollution32 = cpsFuto6.FC(fut6InitialAssigment);

var (cspBinary6, bin6InitialAssigment) = BinaryLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\binary_6x6", selectionStrategy);
var binSollution61 = cspBinary6.BT(bin6InitialAssigment);
//var binSollution62 = cspBinary6.FC(bin6InitialAssigment);

foreach (var sollution in binSollution61)
    Console.WriteLine(PreetyPrinter.variablesToTable(sollution));

Console.WriteLine("END");