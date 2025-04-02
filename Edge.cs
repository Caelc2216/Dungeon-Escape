public class Edge
{
    public string To;
    public int Distance;
    public int EnergyCost;
    public Danger DangerLevel;

    public Edge(string to, int distance, int energyCost, Danger dangerLevel)
    {
        To = to;
        Distance = distance;
        EnergyCost = energyCost;
        DangerLevel = dangerLevel;
    }
}