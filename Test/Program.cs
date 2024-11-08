using SFMLGameFramework;

namespace Test;

internal static class Program
{
    private static void Main(string[] args) {
        bool debug = false;
        if (args.Length > 0) debug = args[0] == "debug";
        Game.SetDebugMode(debug);
        Game.Start();
    }
}