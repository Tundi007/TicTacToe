namespace Connect4;

class Program
{

    public static bool initialStart_Bool = true;

    static void Main(string[] args)
    {

        do
        {

            Game.Load_Function();

            initialStart_Bool = false;

        }while(Game.Rematch_Function());

        Console.Clear();

        System.Console.WriteLine("Have A Nice Day!");

        Thread.Sleep(300);

    }

}