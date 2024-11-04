namespace SFMLGameFramework;

public static class Debug
{
    private static readonly List<string> _writtenStrings = new();
    private static readonly Dictionary<string, int> _loggedStrings = new();
    
    private static void PrintConsole() {
        Console.Clear();
        foreach (KeyValuePair<string,int> logged in _loggedStrings) {
            Console.WriteLine($"[{logged.Value}] {logged.Key}");
        }
        
        foreach (string s in _writtenStrings) {
            Console.WriteLine(s);
        }
    }

    public static void Write(string message) {
        if (Game.Debugging) {
            _writtenStrings.Add(message);
            PrintConsole();
        }
    }

    public static void Log(string message) {
        if (Game.Debugging) {
            if (!_loggedStrings.TryAdd(message, 1)) {
                _loggedStrings[message]++;
            }
            PrintConsole();
        }
    }

    public static void Warning(string message) {
        if (Game.Debugging) {
            _writtenStrings.Add("WARNING: " + message);
            PrintConsole();
        }
    }

    public static void Error(string message) {
        if (Game.Debugging) {
            _writtenStrings.Add("ERROR: " + message);
            PrintConsole();
        }
        else {
            throw new Exception("EXCEPTION: " + message);
        }
    }

}