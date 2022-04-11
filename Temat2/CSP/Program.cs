using CSP.Futoshiki;

var (cpsFuto4, fut4InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_4x4");
var sollutions = cpsFuto4.BT(fut4InitialAssigment);

var (cpsFuto5, fut5InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_5x5");
sollutions = cpsFuto5.BT(fut5InitialAssigment);

var (cpsFuto6, fut6InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_6x6");
sollutions = cpsFuto6.BT(fut6InitialAssigment);

Console.WriteLine("END");