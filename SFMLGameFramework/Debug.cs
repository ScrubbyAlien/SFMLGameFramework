namespace SFMLGameFramework;

public static class Debug
{
    private static readonly List<string> _writtenStrings = new();
    private static readonly Dictionary<string, int> _loggedStrings = new();

    private static void PrintConsole() {
        Console.Clear();
        foreach (KeyValuePair<string, int> logged in _loggedStrings) {
            Console.WriteLine($"[{logged.Value}] {logged.Key}");
        }

        foreach (string s in _writtenStrings) {
            Console.WriteLine(s);
        }
    }
    
    public static void Write(object message) {
        string messageString = message.ToString() ?? "[object]";
        if (Game.Debugging) {
            _writtenStrings.Add(messageString);
            PrintConsole();
        }
    }
    
    public static void Log(object message) {
        string messageString = message.ToString() ?? "[object]";
        if (Game.Debugging) {
            if (!_loggedStrings.TryAdd(messageString, 1)) {
                _loggedStrings[messageString]++;
            }
            PrintConsole();
        }
    }
    
    public static void Warning(object message) {
        string messageString = message.ToString() ?? "[object]";
        if (Game.Debugging) {
            _writtenStrings.Add("WARNING: " + messageString);
            PrintConsole();
        }
    }
    
    public static void Error(object message) {
        string messageString = message.ToString() ?? "[object]";
        if (Game.Debugging) {
            _writtenStrings.Add("ERROR: " + messageString);
            PrintConsole();
        }
        else {
            throw new Exception("EXCEPTION: " + messageString);
        }
    }
}