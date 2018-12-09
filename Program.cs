using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Exercise6a();
      Console.ReadLine();
    }

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
            if (y2 >= 0 && y2 < maxY && x1 >= 0 && x1 < maxX)
            {
              if (map[y2, x1]?.First() == i) continue;
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
    private static int ManhDistance(int x1, int y1, int x2, int y2)
    {
      return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private static void Exercise6b()
    {

    }

    //private static void Exercise5a()
    //{

    //}

    //private static void Exercise5b()
    //{

    //}
    //using (WebClient client = new WebClient())
    //{
    //    string input = client.DownloadString("https://adventofcode.com/2018/day/1/input");
    //    string output = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(x => int.Parse(x)).Sum().ToString();
    //    Console.WriteLine(output);
    //}
  }
}
