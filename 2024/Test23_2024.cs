using System.Net.Sockets;
using static Test19_2022;

public class Test23_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 23;
    }

    Dictionary<string, Connection> ConnectionMap = new Dictionary<string, Connection>();
    public override void Execute()
    {
        long total = 0;
        foreach (string line in m_dataFileContents)
        {
            Connection connectionFrom = null;
            Connection connectionTo;

            string[] tokens = line.Split('-');

            if (string.Compare(tokens[0], tokens[1]) > 0)
            {
                string t = tokens[0];
                tokens[0] = tokens[1];
                tokens[1] = t;
            }

            if (!ConnectionMap.TryGetValue(tokens[0], out connectionFrom))
            {
                connectionFrom = new Connection();
                connectionFrom.Id = tokens[0];
                ConnectionMap[connectionFrom.Id] = connectionFrom;
            }
            if (!ConnectionMap.TryGetValue(tokens[1], out connectionTo))
            {
                connectionTo = new Connection();
                connectionTo.Id = tokens[1];
                ConnectionMap[connectionTo.Id] = connectionTo;
            }

            connectionFrom.AddConnection(connectionTo);

        }
        if (IsPart2)
        {
            BuildFullConnectionMap();
            int ibreak = 0;
        }
        else
        {
            List<Connection[]> result = BuildThreesConnectionMap("t");

            foreach (Connection[] group in result)
            {
                string key = "";

                for (int i = 0; i < group.Length; ++i)
                {
                    key += group[i].Id;
                    if (i < group.Length - 1)
                    {
                        key += ",";
                    }
                }
                DebugOutput("" + key);
            }

            DebugOutput($"There are a total of  {result.Count} groups of 3");

        }
    }
    // find 3's

    public List<Connection> tempList = new List<Connection>();

    List<Connection[]> BuildThreesConnectionMap(string startsWith)
    {

        List<Connection[]> result = new List<Connection[]>();
        List<string> keyList = new List<string>();
        foreach (Connection c in ConnectionMap.Values)
        {
            foreach (var e in c.Connections.SelectMany((fst, i) => c.Connections.Skip(i + 1).Select(snd => (fst, snd))))
            {

                if (e.fst.Connections.Contains(e.snd) && e.snd.Connections.Contains(e.fst))
                {
                    bool isValid = startsWith == "" || c.Id.StartsWith(startsWith) || e.fst.Id.StartsWith(startsWith) || e.snd.Id.StartsWith(startsWith);
                    if (isValid)
                    {
                        tempList.Clear();
                        tempList.Add(c);
                        tempList.Add(e.fst);
                        tempList.Add(e.snd);
                        tempList.Sort((x, y) => string.Compare(x.Id, y.Id));

                        Connection[] ca = tempList.ToArray();
                        string key = "";
                        foreach (Connection tc in ca)
                        {
                            key += tc.Id;
                        }
                        if (!keyList.Contains(key))
                        {
                            keyList.Add(key);
                            result.Add(ca);
                        }
                    }
                }
            }
        }

        return result;
    }


    public void BuildFullConnectionMap()
    {
        List<List<Connection>> connectionGroupList = new List<List<Connection>>();

        foreach (Connection c in ConnectionMap.Values)
        {
            List<Connection> processedMap = new List<Connection>();
            List<Connection> toProcessMap = new List<Connection>();

            connectionGroupList.Add(processedMap);

            toProcessMap.Add(c);

            while (toProcessMap.Count != 0)
            {
                Connection toProcess = toProcessMap.First();
                toProcessMap.RemoveAt(0);

                processedMap.Add(toProcess);


                foreach (Connection c2 in c.Connections)
                {
                    if (!processedMap.Contains(c2) && !toProcessMap.Contains(c2))
                    {
                        toProcessMap.Add(c2);
                    }
                }

            }
            connectionGroupList.Last().Sort((x, y) => string.Compare(x.Id, y.Id));
        }

        float highestMatch = 0.0f;
        // prune the list
        List<Connection> finalResult = new List<Connection>();
        int ibreak = 0;
        foreach (List<Connection> connectionGroup in connectionGroupList)
        {

            Dictionary<Connection, int> scores = new Dictionary<Connection, int>();
            foreach (Connection c in connectionGroup)
            {
                var intersect = connectionGroup.Intersect(c.Connections);

                //float matchRatio = (float)intersect.Count() / (float)connectionGroup.Count();

                if (intersect != null)
                {
                    foreach (Connection c2 in intersect)
                    {
                        if (!scores.ContainsKey(c2))
                        {
                            scores[c2] = 0;
                        }

                        scores[c2]++;
                    }

                }
            }

            float sum = 0;
            foreach (int score in scores.Values)
            {
                sum += score;
            }
            float averageScore = sum / (float)scores.Count;

            if (averageScore > highestMatch)
            {
                highestMatch = averageScore;
                finalResult.Clear();

                int maxScore = 0;


                foreach (Connection c in scores.Keys)
                {
                    maxScore = Math.Max(maxScore, scores[c]);
                }
                foreach (Connection c in scores.Keys)
                {
                    if (scores[c] >= maxScore-1)
                    {
                        finalResult.Add(c);
                    }
                }


                finalResult.Sort((x, y) => string.Compare(x.Id, y.Id));
            }
        }
        DebugOutput(string.Join(",", finalResult));



    }

    public void BuildLargestConnectionMap()
    {
        HashSet<string> allComputers = new HashSet<string>();
        HashSet<string> currentList = new HashSet<string>();
        HashSet<string> currentListCopy = new HashSet<string>();

        HashSet<string> addedList = new HashSet<string>();

        foreach (string id in ConnectionMap.Keys)
        {
            allComputers.Add(id);
        }

        foreach (Connection connection in ConnectionMap.Values.OrderByDescending(x => x.Connections.Count))
        {
            currentListCopy.Clear();
            foreach (string s in currentList)
            {
                currentListCopy.Add(s);
            }
            foreach (Connection c2 in connection.Connections)
            {
                currentListCopy.Add(c2.Id);
            }

            if (currentListCopy.Count > currentList.Count)
            {
                addedList.Add(connection.Id);
                foreach (string v in currentListCopy)
                {
                    currentList.Add(v);
                }
            }

        }

        int ibreak = 0;


    }

}


public class Connection
{
    public string Id;
    public HashSet<Connection> Connections = new HashSet<Connection>();

    public void AddConnection(Connection c)
    {
        Connections.Add(c);
        c.Connections.Add(this);
    }

    public void RemoveConnection(Connection c)
    {
        Connections.Remove(c);
        c.Connections.Remove(this);

    }

    public override bool Equals(object? obj)
    {
        return obj is Connection connection &&
               Id == connection.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string? ToString()
    {
        return Id;
    }
}

