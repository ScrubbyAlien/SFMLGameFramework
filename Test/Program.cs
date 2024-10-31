using Framework;

namespace Test;

static class Program
{
    static void Main(string[] args) {
        bool debug = false;
        if (args.Length > 0) debug = args[0] == "debug"; 
        GameProcess.SetDebugMode(debug);
        GameProcess.StartGame();
    }
}