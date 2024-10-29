namespace Framework;

public static class Debug
{
    public static void Write(string message) {
        if (Game.Debugging) {
            Console.WriteLine(message);
        }
    }

    public static void Warning(string message) {
        if (Game.Debugging) {
            Console.WriteLine("WARNING: " + message);
        }
    }

    public static void Exception(string message) {
        throw new Exception("EXCEPTION: " + message);
    }
}