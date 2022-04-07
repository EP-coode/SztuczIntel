// See https://aka.ms/new-console-template for more information
using CSP.CSP;
using CSP.CSP.Constraints;
using CSP.Futoshiki;
using System.Text;
using Temat2_CSP.Binary;
using Temat2_CSP.Futoshiki;


char[][] binary6 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\binary_6x6");
char[][] binary8 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\binary_8x8");
char[][] binary10 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\binary_10x10");

void Binary(char[][] data, string datasetName)
{
    Console.WriteLine("Rozwiązania dla " + datasetName);
    BinaryBackTrackingSolver b = new(data);

    ICollection<BinaryBackTrackingSolver> sollutions = b.Solve();

    foreach (var s in sollutions)
    {
        Console.WriteLine(s);
        Console.WriteLine("=============================");
    }
}



Binary(binary6, "binary 6x6");
Binary(binary8, "binary 8x8");
Binary(binary10, "binary 10x10");


void Futoshiki(char[][] variables, char[][] constraints, string name)
{

    FutoshikiArray f1 = new(variables, constraints);

    var res = f1.Solve();
    StringBuilder sb = new StringBuilder();
    sb.Append("----------------------------------------");
    sb.AppendLine(name);
    sb.AppendLine("Total sollutions: " + res.Count().ToString());

    foreach (var v in res)
    {
       sb.AppendLine(ToString(v));
    }
    Console.WriteLine(sb.ToString());

}


void FutoV2(char[][] variables2, char[][] constraints2, string name)
{
    Variable[][] variables = new Variable[variables2.Length][];
    for (int y = 0; y < variables2.Length; y++)
    {
        variables[y] = new Variable[variables2[y].Length];
        for (int x = 0; x < variables2[y].Length; x++)
        {
            int value = -1;

            if (variables2[y][x] != 'x')
            {
                value = int.Parse(variables2[y][x].ToString());
            }

            variables[y][x] = new Variable(Enumerable.Range(1, variables2.Length).ToList(), value);
        }
    }

    List<IConstraint> constraints = new();

    constraints.Add(new NoRepeatsInRowsAndCols());
    constraints.Add(new FutoConstraint(constraints2));

    CSPProblem csp = new(variables, constraints, new FutoFC(constraints2));

    var outcomes = csp.Solve();

    Console.WriteLine("-------------------------------------------------------");
    Console.WriteLine("Sollutions count: " + outcomes.Count());
    foreach(var outcome in outcomes)
    {
        Console.WriteLine(ToString2(outcome));
    }
}

string ToString(char[][] arr)
{

    StringBuilder sb = new();
    sb.AppendLine();

    foreach (var b in arr)
    {
        foreach (var c in b)
        {
            if (c == -1)
                sb.Append('-');
            else
                sb.Append(c.ToString());
        }
        sb.AppendLine();
    }
    sb.AppendLine();


    return sb.ToString();
}


string ToString2(Variable[][] arr)
{

    StringBuilder sb = new();
    sb.AppendLine();

    foreach (var b in arr)
    {
        foreach (var c in b)
        {
            if (c.Value == -1)
                sb.Append('-');
            else
                sb.Append(c.Value.ToString());
        }
        sb.AppendLine();
    }
    sb.AppendLine();


    return sb.ToString();
}

var (variables0, constraints0) = FutoshikiUtils.LoadFromFileV2("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\futoshiki_4x4");
var (variables1, constraints1) = FutoshikiUtils.LoadFromFileV2("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\futoshiki_5x5");
var (variables2, constraints2) = FutoshikiUtils.LoadFromFileV2("C:\\Users\\ErnestPrzybyl\\googledrive\\Studia\\Semestr6\\lab\\SztuczIntel\\Temat2\\CSP\\binary-futoshiki_dane_v1.0\\futoshiki_6x6");


Console.WriteLine("aa");

FutoV2(variables0, constraints0, "Futo 4x4");
FutoV2(variables1, constraints1, "Futo 4x4");
FutoV2(variables2, constraints2, "Futo 4x4");

//Futoshiki(variables1, constraints1, "Futo 5x5");

//Futoshiki(variables2, constraints2, "Futo 6x6");