namespace Connect4;

class Program
{

    public static bool initialStart_Bool = true;

    static void Main(string[] args)
    {

        Console.Clear();        

        System.Console.WriteLine("Made By 'Tundi007'");

        System.Console.WriteLine("Shout Out To 'SSS-class Righteous GIRL' For Testing The Game And Also Helping With QOL Improvements!");

        System.Console.WriteLine("Press Any Key To Start The Game");

        Console.ReadKey();        

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