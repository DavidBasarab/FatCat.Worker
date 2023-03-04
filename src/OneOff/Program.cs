using FatCat.Toolkit.Console;
using FatCat.Worker;

namespace OneOff;

public static class Program
{
    public static void Main(params string[] args)
    {
        ConsoleLog.WriteMagenta("This is working");
        
        var timerWrapper = new TimerWrapper();
        
        timerWrapper.Testing();
    }
}