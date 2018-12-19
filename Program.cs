using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2018
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Exercise8a();
      Console.ReadLine();
    }

    #region Done
    private static void Exercise1a()
    {
      string output = File.ReadLines("1.txt").Select(x => int.Parse(x)).Sum().ToString();
      Console.WriteLine(output);
    }

    private static void Exercise1b()
    {
      var input = File.ReadLines("1.txt").Select(x => int.Parse(x)).ToArray();
      int sum = 0;
      HashSet<int> frequencies = new HashSet<int>() { sum };

      for (int i = 0; i < input.Count(); i++)
      {
        sum += input[i];
        if (frequencies.Contains(sum))
        {
          break;
        }

        frequencies.Add(sum);
        if (i + 1 == input.Count())
        {
          i = -1;
        }
      }
      Console.WriteLine(sum);
    }

    private static void Exercise2a()
    {
      int twoCount = 0, threeCount = 0;
      File.ReadLines("2.txt").ToList().ForEach(id =>
      {
        var counts = new HashSet<int>(id.ToCharArray().GroupBy(y => y).GroupBy(k => k.Count()).Select(y => y.Key));
        twoCount += (counts.Contains(2) ? 1 : 0);
        threeCount += (counts.Contains(3) ? 1 : 0);
        Console.WriteLine($"{(counts.Contains(2) ? 1 : 0)}-{(counts.Contains(3) ? 1 : 0)}");
      });
      Console.WriteLine(twoCount * threeCount);
    }

    private static void Exercise2b()
    {
      var ids = File.ReadLines("2.txt").ToArray();

      var len = ids.First().Length;
      //This does not work for 2+ diffs in split points....
      for (int i = 1; i < len; i++)
      {
        for (int k = 0; k < len; k++)
        {
          var commonIds = ids.Select(x => x.Remove(k, i)).GroupBy(x => x).Where(x => x.Count() > 1);
          if (commonIds.Count() > 0)
          {
            Console.WriteLine(commonIds.First().Key);
            return;
          }
        }
      }
    }

    private static void Exercise3a()
    {
      int?[,] fabric = new int?[1000, 1000];
      int result = 0;
      string[] inputs = File.ReadLines("3.txt").ToArray();

      foreach (var input in inputs)
      {
        var data = input.Split(new char[] { '#', '@', ',', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
        for (int x = data[2]; x < data[2] + data[4]; x++)
        {
          for (int y = data[1]; y < data[1] + data[3]; y++)
          {
            if (fabric[x, y].HasValue)
            {
              fabric[x, y] = 1;
            }
            else
            {
              fabric[x, y] = 0;
            }
          }
        }
      }

      for (int x = 0; x < 1000; x++)
      {
        for (int y = 0; y < 1000; y++)
        {
          if (fabric[x, y].HasValue && fabric[x, y].Value == 1)
          {
            result++;
          }
        }
      }

      Console.WriteLine(result);
    }

    private static void Exercise3b()
    {
      int?[,] fabric = new int?[1000, 1000];
      int result = 0;
      int[][] inputs = File.ReadLines("3.txt").ToArray().Select(x =>
          x.Split(new char[] { '#', '@', ',', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)
      ).ToArray()).ToArray();

      foreach (var input in inputs)
      {
        for (int x = input[2]; x < input[2] + input[4]; x++)
        {
          for (int y = input[1]; y < input[1] + input[3]; y++)
          {
            if (fabric[x, y].HasValue)
            {
              fabric[x, y] = 0;
            }
            else
            {
              fabric[x, y] = input[0];
            }
          }
        }
      }

      Dictionary<int, int> countsPerId = new Dictionary<int, int>();
      foreach (var id in fabric)
      {
        if (id.HasValue)
        {
          if (!countsPerId.ContainsKey(id.Value))
          {
            countsPerId.Add(id.Value, 1);
          }
          else
          {
            countsPerId[id.Value]++;
          }
        }
      }

      foreach (var input in inputs)
      {
        var area = input[3] * input[4];
        if (countsPerId.ContainsKey(input[0]) && area == countsPerId[input[0]])
        {
          result = input[0];
        }
      }
      Console.WriteLine(result);
    }

    private static void Exercise4a()
    {
      string[] inputs = File.ReadLines("4.txt").ToArray();
      Array.Sort(inputs);
      Dictionary<int, int[]> timesSleptPerMinute = new Dictionary<int, int[]>();
      int guard = -1;
      int startMinute = 0, endMinute = 0;

      foreach (var input in inputs)
      {
        if (input.EndsWith(" begins shift"))
          guard = int.Parse(input.Replace(" begins shift", "").Split('#').Last());
        else if (input.EndsWith(" falls asleep"))
          startMinute = int.Parse(input.Substring(15, 2));
        else if (input.EndsWith(" wakes up"))
        {
          endMinute = int.Parse(input.Substring(15, 2));
          for (int i = startMinute; i < endMinute; i++)
          {
            if (!timesSleptPerMinute.ContainsKey(guard))
              timesSleptPerMinute[guard] = new int[60];
            timesSleptPerMinute[guard][i]++;
          }
        }
      }
      int maxValue = timesSleptPerMinute.Select(x => x.Value.Sum()).Max();
      int resultGuard = timesSleptPerMinute.ToDictionary(x => x.Key, x => x.Value.Sum()).Where(x => x.Value == maxValue).Select(x => x.Key).First();
      int minute = timesSleptPerMinute[resultGuard].ToList().IndexOf(timesSleptPerMinute[resultGuard].Max());
      Console.WriteLine(resultGuard * minute);
    }

    private static void Exercise4b()
    {
      string[] inputs = File.ReadLines("4.txt").ToArray();
      Array.Sort(inputs);
      Dictionary<int, int[]> timesSleptPerMinute = new Dictionary<int, int[]>();
      int guard = -1;
      int startMinute = 0, endMinute = 0;

      foreach (var input in inputs)
      {
        if (input.EndsWith(" begins shift"))
          guard = int.Parse(input.Replace(" begins shift", "").Split('#').Last());
        else if (input.EndsWith(" falls asleep"))
          startMinute = int.Parse(input.Substring(15, 2));
        else if (input.EndsWith(" wakes up"))
        {
          endMinute = int.Parse(input.Substring(15, 2));
          for (int i = startMinute; i < endMinute; i++)
          {
            if (!timesSleptPerMinute.ContainsKey(guard))
              timesSleptPerMinute[guard] = new int[60];
            timesSleptPerMinute[guard][i]++;
          }
        }
      }
      int maxMinutesSlept = timesSleptPerMinute.Select(x => x.Value.Max()).Max();
      int resultGuard = timesSleptPerMinute.ToDictionary(x => x.Key, x => x.Value.Max()).Where(x => x.Value == maxMinutesSlept).Select(x => x.Key).First();
      int minute = timesSleptPerMinute[resultGuard].ToList().IndexOf(maxMinutesSlept);

      Console.WriteLine(resultGuard * minute);
    }

    private static void Exercise5a()
    {
      string input = File.ReadAllText("5.txt");
      var data = input.ToList();
      int i = 0;
      while (i < data.Count - 1)
      {
        if (Math.Abs((int)data[i] - (int)data[i + 1]) == 32)
        {
          data.RemoveRange(i, 2);
          if (i > 0) i--;
        }
        else i++;
      }
      Console.WriteLine(string.Concat(data));
      Console.WriteLine(data.Count);
    }

    private static void Exercise5b()
    {
      string input = File.ReadAllText("5.txt");
      var data = input.ToList();
      var chars = data.Distinct().Where(x => x <= 'Z').Select(x => x.ToString());
      int min = int.MaxValue;
      foreach (var ch in chars)
      {
        var newData = data.Where(x => x != ch.First() && x != ch.ToLower().First()).ToList();
        int i = 0;
        while (i < newData.Count - 1)
        {
          if (Math.Abs((int)newData[i] - (int)newData[i + 1]) == 32)
          {
            newData.RemoveRange(i, 2);
            if (i > 0) i--;
          }
          else i++;
        }
        if (newData.Count < min)
          min = newData.Count;
      }
      Console.WriteLine(min);
    }

    private static void Exercise6a()
    {
      var inputs = File.ReadLines("6.txt").Select(x =>
        x.Split(',').Select(y => int.Parse(y)).ToList()
      ).ToList();

      int maxX = inputs.Select(x => x[0]).Max() + 2;
      int maxY = inputs.Select(x => x[1]).Max() + 2;
      int[,][] map = new int[maxY, maxX][];
      int currentDist = 0;

      #region Fill map
      while (map.Cast<int[]>().Any(x => x == null))
      {
        for (int i = 0; i < inputs.Count; i++)
        {
          int x = inputs[i][0], y = inputs[i][1];

          // Mark the area arround the point at 1-distance increments
          for (int k = -currentDist; k <= currentDist; k++)
          {
            int y1 = y + (currentDist - Math.Abs(k));
            int y2 = y - (currentDist - Math.Abs(k));
            int x1 = x + k;

            if (y1 >= 0 && y1 < maxY && x1 >= 0 && x1 < maxX)
            {
              if (map[y1, x1] == null) {
                map[y1, x1] = new int[] { i, currentDist };
              }
              else if (map[y1, x1][1] == currentDist)
                map[y1, x1] = new int[] { -1, currentDist };
            }
            if (y2 >= 0 && y2 < maxY && x1 >= 0 && x1 < maxX && y1 != y2)
            {
              if (map[y2, x1] == null)
                map[y2, x1] = new int[] { i, currentDist };
              else if (map[y2, x1][1] == currentDist)
                map[y2, x1] = new int[] { -1, currentDist };
            }
          }
          //PrintMap(map, maxY, maxX);
        }
        currentDist++;
      }
      #endregion

      #region Get all infinite spaces 
      var infiniteSpaces = new HashSet<int>();
      for (int x = 0; x < maxX; x++)
      {
        infiniteSpaces.Add(map[0, x][0]);
        infiniteSpaces.Add(map[maxY - 1, x][0]);
      }
      for (int y = 0; y < maxY; y++)
      {
        infiniteSpaces.Add(map[y, 0][0]);
        infiniteSpaces.Add(map[y, maxX - 1][0]);
      }
      #endregion

      var maxArea = map.
        Cast<int[]>().
        Select(x => x.First()).
        Where(x => !infiniteSpaces.Contains(x)).
        GroupBy(x => x).
        Select(gr => new { ID = gr.Key, Count = gr.Count() }).
        OrderByDescending(gr => gr.Count).
        First();

      Console.WriteLine($"{maxArea.ID}:{maxArea.Count}");

    }

    private static void PrintMap(int[,][] map, int maxY, int maxX)
    {
      Console.WriteLine("-----------------------------");
      for (int y = 0; y < maxY; y++)
      {
        for (int x = 0; x < maxX; x++)
        {
          var v = map[y, x] == null ? "+": map[y, x][0] == -1 ? "." : map[y, x][0].ToString();
          Console.Write(v);
        }
        Console.WriteLine();
      }
      Console.WriteLine("-----------------------------");
    }

    private static void Exercise6b()
    {
      var inputs = File.ReadLines("6.txt").Select(x =>
        x.Split(',').Select(y => int.Parse(y)).ToList()
      ).ToList();

      int maxX = inputs.Select(x => x[0]).Max() + 2;
      int maxY = inputs.Select(x => x[1]).Max() + 2;
      int[,] map = new int[maxY, maxX];

      #region Fill map
        for (int i = 0; i < inputs.Count; i++)
        {
          int x = inputs[i][0], y = inputs[i][1];
          for (int currentDist = 0; currentDist <= maxX + maxY; currentDist++)
          {
            // Mark the area arround the point at 1-distance increments
            for (int k = -currentDist; k <= currentDist; k++)
            {
              int y1 = y + (currentDist - Math.Abs(k));
              int y2 = y - (currentDist - Math.Abs(k));
              int x1 = x + k;

              if (y1 >= 0 && y1 < maxY && x1 >= 0 && x1 < maxX)
                map[y1, x1] += currentDist;
              if (y2 >= 0 && y2 < maxY && x1 >= 0 && x1 < maxX && y1 != y2)
                map[y2, x1] += currentDist;
            }
          }
        }
      
      #endregion

      var areaSize = map.Cast<int>().Where(x => x < 10000).Count();
      Console.WriteLine($"{areaSize}");
    }

    private static void Exercise7a()
    {
      var inputs = File.ReadLines("7.txt").Select(x =>
        new char[] { x[5], x[36] }
      ).ToList();
      // Find start / end
      var sources = inputs.Select(x => x[0]).Distinct();
      var targets = inputs.Select(x => x[1]).Distinct();
      var all = sources.Concat(targets).Distinct().Count();
      var start = sources.Except(targets).ToList();
      var end = targets.Except(sources).First();
      List<char> sb = new List<char>();

      var edges = inputs.GroupBy(x=>x[0]).ToDictionary(k=>k.Key, v => v.Select(x=>x[1]).OrderBy(x=>x).ToArray());
      List<char> currentEdges = new List<char>(start);

      while (true)
      {
        (currentEdges = currentEdges.Distinct().ToList()).Sort();
        var currentTargets = edges.Values.SelectMany(x => x).GroupBy(x=>x).ToDictionary(k=>k.Key, v => v.Count());
        var c = currentEdges.First(x => !currentTargets.ContainsKey(x));

        currentEdges.Remove(c);
        if(!sb.Contains(c))
          sb.Add(c);
        if (c == end)
          break;
        currentEdges.AddRange(edges[c]);
        edges.Remove(c);
      }
      Console.WriteLine(new string(sb.ToArray()));
    }

    #endregion

    // TODO: Wrong answer... Pfff
    private static void Exercise7b()
    {
      var inputs = File.ReadLines("7.txt").Select(x => new char[] { x[5], x[36] }).ToList();

      var sources = inputs.Select(x => x[0]).Distinct();
      var targets = inputs.Select(x => x[1]).Distinct();
      var all = sources.Concat(targets).Distinct().Count();
      var start = sources.Except(targets).ToList();
      var end = targets.Except(sources).First();
      List<char> sb = new List<char>();

      var edges = inputs.GroupBy(x => x[0]).ToDictionary(k => k.Key, v => v.Select(x => x[1]).OrderBy(x => x).ToArray());
      List<char> currentEdges = new List<char>(start);

      var workers = new Worker[] { new Worker(), new Worker() , new Worker(), new Worker(), new Worker() 
      };

      for (int sec = 0; sec <= 3000; sec++)
      {
        foreach (var worker in workers)
        {
          // If the worker is free, add the task and remove from edges
          // If the worker just finished the task, add to sb, add currentEdges, remove from edges
          // If the worker is busy, continue
          char? cc = worker.FinishTask(sec);
          if (cc.HasValue)
          {
            char c = cc.Value;
            if (!sb.Contains(c))
              sb.Add(c);
            if (c == end)
              break;
            currentEdges.AddRange(edges[c]);
            (currentEdges = currentEdges.Distinct().ToList()).Sort();
            edges.Remove(c);
          }
          if (!worker.IsBusy(sec))
          {
            var currentTargets = edges.Values.SelectMany(x => x).GroupBy(x => x).ToDictionary(k => k.Key, v => v.Count());
            var c = currentEdges.FirstOrDefault(x => !currentTargets.ContainsKey(x));
            if (c == default(char)) continue;
            worker.AddTask(c, sec);
            currentEdges.Remove(c);
          }
        }
        Worker.PrintStatus(workers, sec, sb, currentEdges);
        if (sb.Count == all) break;
      }
      Console.WriteLine(new string(sb.ToArray()));
    }

    private static void Exercise8a()
    {
      var inputs = File.ReadAllText("8.txt").Split(' ').Select(x => int.Parse(x)).ToList();

      int idx = 2;
      var result = ParseLevel(inputs, inputs[0], inputs[1], ref idx).Sum();
      Console.WriteLine(result);

    }

    private static List<int> ParseLevel(List<int> tree, int nodes, int meta, ref int idx)
    {
      List<int> result = new List<int>();
      int[] ms = new int[nodes];

      for(int i = 0; i < nodes; i++)
      {
        int n = tree[idx];
        int m = tree[idx + 1];
        idx += 2;
        if (n > 0)
        {
          var data = ParseLevel(tree, n, m, ref idx);
          result.AddRange(data);
        }

        if(n == 0)
        {
          var data = tree.GetRange(idx, m);
          idx += m;
          result.AddRange(data);
          Console.WriteLine(string.Join(" ", data));
        }
      }
      var d = tree.GetRange(idx, meta);
      idx += meta;
      result.AddRange(d);

      return result;
    }

    private static void Exercise8b() { }

    //private static void Exercise9a() {}
    //private static void Exercise9b() {}

    //private static void Exercise10a() {}
    //private static void Exercise10b() {}

    //private static void Exercise11a() {}
    //private static void Exercise11b() {}

    //private static void Exercise12a() {}
    //private static void Exercise12b() {}

    //private static void Exercise13a() {}
    //private static void Exercise13b() {}

    //private static void Exercise14a() {}
    //private static void Exercise14b() {}

    //private static void Exercise15a() {}
    //private static void Exercise15b() {}

    //private static void Exercise15a() {}
    //private static void Exercise15b() {}

    //private static void Exercise16a() {}
    //private static void Exercise16b() {}

    //private static void Exercise17a() {}
    //private static void Exercise17b() {}

    //private static void Exercise18a() {}
    //private static void Exercise18b() {}

    //private static void Exercise19a() {}
    //private static void Exercise19b() {}

    //private static void Exercise20a() {}
    //private static void Exercise20b() {}

    //private static void Exercise21a() {}
    //private static void Exercise21b() {}

    //private static void Exercise22a() {}
    //private static void Exercise22b() {}

    //private static void Exercise23a() {}
    //private static void Exercise23b() {}

    //private static void Exercise24a() {}
    //private static void Exercise24b() {}

    //private static void Exercise25a() {}
    //private static void Exercise25b() {}
  }

}
