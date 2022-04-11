using CSP.Futoshiki;

var (cpsFuto4, fut4InitialAssigment) = FutoshikiLoader.LoadProblem("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\dane\\futoshiki_4x4");

var sollutions = cpsFuto4.BT(fut4InitialAssigment);

Console.WriteLine("END");