Map m = new();
m.InitializeMap();
Console.Clear();
Console.WriteLine();

m.DisplayPaths("A");
m.DisplayPaths("B");
m.DisplayPaths("C");
m.DisplayPaths("D");
m.DisplayPaths("E");
m.DisplayPaths("F");
m.DisplayPaths("G");
m.DisplayPaths("H");
m.DisplayPaths("I");
m.DisplayPaths("J");
m.DisplayPaths("K");

Dictionary<string, int> d = m.DijkstraDistance("A");
foreach (var kvp in d)
{
    Console.WriteLine($"Distance from A to {kvp.Key} is {kvp.Value}.");
}

Dictionary<string, Danger> d2 = m.DijkstraSafest("A");
foreach (var kvp in d2)
{
    Console.WriteLine($"Danger level from A to {kvp.Key} is {kvp.Value}.");
}

Dictionary<string, int> d3 = m.DijkstraEnergy("A");
foreach (var kvp in d3)
{
    Console.WriteLine($"Energy from A to {kvp.Key} is {kvp.Value}.");
}

m.OptimalPath("C", "J");