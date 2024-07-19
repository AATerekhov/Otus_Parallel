// See https://aka.ms/new-console-template for more information
using Otus_Parallel;
using System.Diagnostics;

Console.WriteLine("Ядер: "+Environment.ProcessorCount);
Console.WriteLine("ОС: " + Environment.OSVersion);
Console.WriteLine("Процессор: " + "11th Gen Intel(R) Core(TM) i5-1135G7 @ 2.40GHz   2.42 GHz");
Console.WriteLine("Оперативная память: " + "16,0 ГБ");

var sequenceGenerator = new SequenceGenerator();
var itemsOne = sequenceGenerator.Generate(100_000);
var itemsTwo = sequenceGenerator.Generate(10_000_000);
var itemsThree = sequenceGenerator.Generate(1_000_000_000);
var timer = new Stopwatch();

UsualСalculation(itemsOne, timer);
UsualСalculation(itemsTwo, timer);
UsualСalculation(itemsThree, timer);
Loq("__________________: ");
TaskСalculation(itemsOne, timer, 8);
TaskСalculation(itemsTwo, timer, 8);
TaskСalculation(itemsThree, timer, 8);
Loq("__________________: ");
PLinqСalculation(itemsOne, timer);
PLinqСalculation(itemsTwo, timer);
PLinqСalculation(itemsThree, timer);

void Loq(string massage)
{
    Console.WriteLine(massage);
}
void PLinqСalculation(long[] itemsOne, Stopwatch timer)
{
    timer.Start();

    long sum = itemsOne.AsParallel().Sum();
    timer.Stop();
    Console.WriteLine(timer.Elapsed + $": результат PLinq {itemsOne.Length} - : " + sum);
    timer.Restart();
}
void TaskСalculation(long[] itemsOne, Stopwatch timer, int countTask)
{
    timer.Start();

    List<Task<long>> tasks= new List<Task<long>>(countTask);
    int step = itemsOne.Length / countTask;
    for (int i = 0; i < countTask; i++)
    {
        tasks.Add(GetTotalTask(itemsOne, i * step, step));
    }
    long sum = 0;
    tasks.ForEach(f=> sum+=f.Result);
    timer.Stop();
    Console.WriteLine(timer.Elapsed + $": результат {countTask} потоков {itemsOne.Length} - : " + sum);
    timer.Restart();
}
void UsualСalculation(long[] itemsOne, Stopwatch timer)
{
    timer.Start();
    var sum = GetTotal(itemsOne,0, itemsOne.Length);
    timer.Stop();
    Console.WriteLine(timer.Elapsed + $": результат {itemsOne.Length} - : " + sum);
    timer.Restart();
}

Task<long> GetTotalTask(long[] items,int index, int count)
{
    long result = 0;
    for (int i = index; i < index + count; i++)
    {
        result += items[i];
    }
    return Task.FromResult(result);
}
long GetTotal(long[] items, int index, int count)
{
    long result = 0;
    for (int i = index; i < count; i++)
    {
        result += items[i];
    }
    return result;
}