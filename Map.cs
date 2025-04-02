using System.Runtime.InteropServices;

public class Map
{
    public Dictionary<string, List<Edge>> Graph { get; set; }

    public Map()
    {
        Graph = new Dictionary<string, List<Edge>>();
    }

    public class DangerComparer : IComparer<Danger>
    {
        public int Compare(Danger x, Danger y)
        {
            return x.CompareTo(y);
        }
    }

    public void AddRoom(string name)
    {
        if (HasRoom(name))
        {
            Console.WriteLine($"{name} already exists.");
            return;
        }

        if (!Graph.ContainsKey(name))
        {
            Graph[name] = new List<Edge>();
            Console.WriteLine($"{name} has been added.");
        }
    }

    public Dictionary<string, List<Edge>> GetNetwork()
    {
        return Graph;
    }

    public void AddPath(string room, Edge path)
    {
        if (!HasRoom(room))
        {
            Console.WriteLine($"Room does not exist.");
            return;
        }
        if (HasRoom(room) && HasPath(room, path))

            if (HasPath(room, path))
            {
                Console.WriteLine($"{room} has a path to {path.To} already.");
                return;
            }

        if (Graph.ContainsKey(room)) /* if directional delete && AdjacencyList.ContainsKey(vertex2) */
        {
            if (!Graph[room].Contains(path))
            {
                Graph[room].Add(path);
            }

            Console.WriteLine($"{room} now has a path to {path.To}.");
        }
    }

    public void RemoveRoom(string room)
    {
        if (!HasRoom(room))
        {
            Console.WriteLine($"{room} does not exist.");
            return;
        }
        // Remove all edges that contain this vertex
        foreach (var r in Graph)
        {
            for (int i = 0; i < r.Value.Count; i++)
            {
                if (r.Value[i].To == room)
                {
                    r.Value.RemoveAt(i);
                }
            }
        }
        // Remove vertex
        Graph.Remove(room);
        Console.WriteLine($"{room} has been removed from the map.");
    }

    public bool HasPath(string startVertex, Edge path)
    {
        return Graph[startVertex].Contains(path);
    }

    public bool HasRoom(string room)
    {
        return Graph.ContainsKey(room);
    }

    public void RemovePath(string room, Edge path
    )
    {
        if (!HasRoom(room))
        {
            Console.WriteLine($"{room} does not exist.");
            return;
        }

        if (!HasPath(room, path))
        {
            Console.WriteLine($"{room} does not have a path to {path.To}.");
            return;
        }

        Graph[room].Remove(path);
        Graph[room].Remove(path);
        Console.WriteLine($"{room} no longer has a path to {path.To}.");
    }

    public void DisplayPaths(string room)
    {
        List<Edge> paths = Graph[room];
        if (!HasRoom(room))
        {
            Console.WriteLine($"{room} does not exist.");
            return;
        }
        Console.Write($"{room} contains paths to: ");
        foreach (var path in paths)
        {
            Console.Write($"{path.To},");
        }
        if (paths.Count == 0)
        {
            Console.WriteLine($"{room} has no paths");
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    public void FindBidirectionalPaths(string room1, string room2)
    {
        List<string> BiPaths = new();

        if (!HasRoom(room1) || !HasRoom(room2))
        {
            Console.WriteLine($"One or both rooms do not exist");
            return;
        }

        foreach (var p in Graph[room1])
        {
            if (Graph[room1].Contains(p)
                && Graph[room2].Contains(p))
            {
                BiPaths.Add(p.To);
            }
        }

        if (BiPaths.Count == 0)
        {
            Console.WriteLine($"{room1} and {room2} have no mutual friends.");
            return;
        }

        Console.WriteLine($"Mutual friends of {room1} and {room2}: ");
        foreach (var friend in BiPaths)
        {
            Console.Write($"{friend}, ");
        }
    }

    public void InitializeMap()
    {
        AddRoom("A");
        AddRoom("B");
        AddRoom("C");
        AddRoom("D");
        AddRoom("E");
        AddRoom("F");
        AddRoom("G");
        AddRoom("H");
        AddRoom("I");
        AddRoom("J");
        AddRoom("K");

        AddPath("A", new Edge("B", 2, 0, Danger.Low));
        AddPath("A", new Edge("C", 4, 0, Danger.Medium));
        AddPath("B", new Edge("D", 1, 0, Danger.Low));
        AddPath("B", new Edge("E", 3, 2, Danger.Medium));
        AddPath("C", new Edge("D", 2, 3, Danger.High));
        AddPath("C", new Edge("F", 3, 0, Danger.Medium));
        AddPath("D", new Edge("G", 4, 0, Danger.High));
        AddPath("D", new Edge("H", 5, 0, Danger.Low));
        AddPath("E", new Edge("H", 2, 0, Danger.Medium));
        AddPath("E", new Edge("B", 3, 0, Danger.Medium));
        AddPath("F", new Edge("I", 2, 0, Danger.Medium));
        AddPath("G", new Edge("J", 6, 0, Danger.Medium));
        AddPath("H", new Edge("J", 3, 0, Danger.Low));
        AddPath("I", new Edge("J", 1, 0, Danger.Medium));
        AddPath("F", new Edge("K", 3, 4, Danger.High));
        Console.Clear();
    }

    public Dictionary<string, int> DijkstraDistance(string start)
    {
        Dictionary<string, int> distances = new Dictionary<string, int>();

        PriorityQueue<string, int> queue = new();

        foreach (var room in Graph.Keys)
        {
            distances[room] = int.MaxValue;
        }
        distances[start] = 0;
        queue.Enqueue(start, 0);
        while (queue.Count > 0)
        {
            if (queue.Count == 0)
            {
                Console.WriteLine("Error: Attempted to dequeue from an empty queue.");
                break;
            }
            string currentRoom = queue.Dequeue();
            foreach (var path in Graph[currentRoom])
            {
                int newDistance = distances[currentRoom] + path.Distance;
                if (!distances.ContainsKey(path.To))
                {
                    distances[path.To] = int.MaxValue;
                }

                if (newDistance < distances[path.To])
                {
                    distances[path.To] = newDistance;
                    queue.Enqueue(path.To, newDistance);
                }
            }
        }
        return distances;
    }

    public Dictionary<string, Danger> DijkstraSafest(string start)
    {
        Dictionary<string, Danger> safe = new Dictionary<string, Danger>();
        PriorityQueue<string, Danger> queue = new(new DangerComparer());

        foreach (var room in Graph.Keys)
        {
            safe[room] = Danger.High;
        }

        safe[start] = Danger.Low;  // Start should be safest
        queue.Enqueue(start, Danger.Low);

        while (queue.Count > 0)
        {
            string currentRoom = queue.Dequeue();

            foreach (var path in Graph[currentRoom])
            {
                // Take the maximum danger level along the path
                Danger newDanger;
                //  = Math.Max(safe[currentRoom], path.DangerLevel)
                if (safe[currentRoom] > path.DangerLevel)
                {
                    newDanger = safe[currentRoom];
                }
                else
                {
                    newDanger = path.DangerLevel;
                }

                if (newDanger < safe[path.To])
                {
                    safe[path.To] = newDanger;
                    queue.Enqueue(path.To, newDanger);
                }
            }
        }

        return safe;
    }

    public Dictionary<string, int> DijkstraEnergy(string start)
    {
        Dictionary<string, int> energy = new Dictionary<string, int>();

        PriorityQueue<string, int> queue = new();

        foreach (var room in Graph.Keys)
        {
            energy[room] = int.MaxValue;
        }
        energy[start] = 0;
        queue.Enqueue(start, 0);
        while (queue.Count > 0)
        {
            if (queue.Count == 0)
            {
                Console.WriteLine("Error: Attempted to dequeue from an empty queue.");
                break;
            }
            string currentRoom = queue.Dequeue();
            foreach (var path in Graph[currentRoom])
            {
                int newEnergy = energy[currentRoom] + path.EnergyCost;
                if (!energy.ContainsKey(path.To))
                {
                    energy[path.To] = int.MaxValue;
                }

                if (newEnergy < energy[path.To])
                {
                    energy[path.To] = newEnergy;
                    queue.Enqueue(path.To, newEnergy);
                }
            }
            foreach (var key in energy.Keys)
            {
                if(energy[key] == int.MaxValue)
                {
                    energy.Remove(key);
                }
            }
        }
        return energy;
    }

    public void OptimalPath(string start, string end)
    {
        Dictionary<string, int> distances = DijkstraDistance(start);
        Dictionary<string, Danger> dangers = DijkstraSafest(start);
        Dictionary<string, int> energy = DijkstraEnergy(start);

        Console.WriteLine($"Optimal path from {start} to {end}:");
        Console.WriteLine($"Distance: {distances[end]}");
        Console.WriteLine($"Danger level: {dangers[end]}");
        Console.WriteLine($"Energy cost: {energy[end]}");
    }
}