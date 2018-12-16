using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
  class Worker
  {
    static int CurrentID;
    static int TaskOverHead = 60;

    int ID { get; set; }

    public char? TaskChar { get; private set; }

    int BusyUntil { get; set; }

    public static void PrintStatus(Worker[] workers, int currentSec, List<char> queue, List<char> currentEdges)
    {
      Console.Write($"{currentSec}\t");
      foreach (var worker in workers)
        Console.Write($"{worker.TaskChar}\t");
      Console.WriteLine($"{new string(queue.ToArray())}\t${new string(currentEdges.ToArray())}");
    }

    public bool IsBusy(int currentSec)
    {
      return currentSec < BusyUntil;
    }

    public void AddTask(char task, int currentSec)
    {
      TaskChar = task;
      BusyUntil = currentSec + (task - 64) + TaskOverHead;
      //Console.WriteLine($"\tWorker {ID} : Added task {task} and busy {currentSec}-{BusyUntil}");
    }

    public char? FinishTask(int currentSec)
    {
      //Console.WriteLine($"Finished task ?? at sec {currentSec}");
      if (IsBusy(currentSec) || !TaskChar.HasValue)
        return null;

      var c = TaskChar;
      TaskChar = null;
      BusyUntil = 0;
      //Console.WriteLine($"\tWorker {ID} : Finished task {c} at sec {currentSec}");

      return c;
    }


    public Worker()
    {
      ID = CurrentID++;
    }
  }

}
