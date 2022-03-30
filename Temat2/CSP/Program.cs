// See https://aka.ms/new-console-template for more information
using Temat2_CSP.Binary;

char[][] binary6 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\Desktop\\SztuczIntel\\Temat2_CSP\\binary-futoshiki_dane_v1.0\\binary_6x6");
char[][] binary8 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\Desktop\\SztuczIntel\\Temat2_CSP\\binary-futoshiki_dane_v1.0\\binary_8x8");
char[][] binary10 = Utils.LoadFromFile("C:\\Users\\ErnestPrzybyl\\Desktop\\SztuczIntel\\Temat2_CSP\\binary-futoshiki_dane_v1.0\\binary_10x10");


void Binary(char[][] data, string datasetName)
{
    Console.WriteLine("Rozwiązania dla " + datasetName);
    BackTracking b = new(data);

    ICollection<BackTracking> sollutions = b.Solve();

    foreach (var s in sollutions)
    {
        Console.WriteLine(s);
        Console.WriteLine("=============================");
    }
}

void Futoshiki()
{

}

Binary(binary6, "binary 6x6");
Binary(binary8, "binary 8x8");
Binary(binary10, "binary 10x10");