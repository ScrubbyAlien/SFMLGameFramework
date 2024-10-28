using Framework;

namespace Test;

static class Program
{
    static void Main(string[] args) {
        bool debug = args[0] == "debug";
        Game.StartGame("config", debug);
    }
}