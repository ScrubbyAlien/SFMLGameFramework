namespace Framework;

public static class Debug
{
    public static void Write(string message) {
        if (Game.Debugging) {
            Console.WriteLine(message);
        }
    }
}