using System.Text.Json;
using SFML.Graphics;
using SFML.Window;

namespace Framework;

public static class Game
{
    private static RenderWindow? _window;

    public static void StartGame(string configPath = "") {

        GameSettings gs;
        if (configPath != "") {
            string json = File.ReadAllText(configPath + "/gamesettings.json");
            gs = JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
        }
        else {
            gs = new GameSettings();
        }
        
        
        _window = new(new VideoMode(gs.ScreenWidth, gs.ScreenHeight), "Invaders");
        // ReSharper disable once AccessToDisposedClosure
        _window.Closed += (_, _) => _window.Close();
        _window.SetFramerateLimit(gs.FrameLimit);

        while (_window.IsOpen) {
            _window.DispatchEvents();
            // float deltaTime = clock.Restart().AsSeconds();
            // Scene.UpdateAll(deltaTime);
            //
            // window.Clear();
            // Scene.RenderAll(window);
            //
            // window.Display();
        }
    
    }
}