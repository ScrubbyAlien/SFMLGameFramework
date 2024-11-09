using SFML.Window;

namespace SFMLGameFramework;

public static class Input
{
    private static readonly Dictionary<Keyboard.Key, KeyState> _keyboardState = [];
    
    static Input() {
        foreach (Keyboard.Key key in Enum.GetValues<Keyboard.Key>()) {
           _keyboardState.TryAdd(key, new KeyState());
        }
    }

    /// <summary>
    /// Returns true if the key was pressed this frame.
    /// </summary>
    public static bool KeyPressed(Keyboard.Key key) {
        return _keyboardState[key].Pressed;
    }
    /// <summary>
    /// Returns true if the key was released this frame.
    /// </summary>
    public static bool KeyReleased(Keyboard.Key key) {
        return _keyboardState[key].Released;
    }
    /// <summary>
    /// Returns true every frame from the key being pressed until it is released.
    /// </summary>
    public static bool KeyDown(Keyboard.Key key) {
        return _keyboardState[key].Down;
    }
    /// <summary>
    /// Returns true every frame that the key is not down.
    /// </summary>
    public static bool KeyUp(Keyboard.Key key) {
        return _keyboardState[key].Up;
    }

    internal static void UpdateKeyboardState() {
        foreach (KeyValuePair<Keyboard.Key,KeyState> pair in _keyboardState) {
            if (Keyboard.IsKeyPressed(pair.Key)) {
                if (!pair.Value.Down) pair.Value.Press();
                else pair.Value.NoChange();
            }
            else {
                if (pair.Value.Down) pair.Value.Release();
                else pair.Value.NoChange();
            }
        }
    }

    private class KeyState
    {
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }
        public bool Down { get; private set; }
        public bool Up => !Down;

        public void Press() {
            Pressed = true;
            Down = true;
        }

        public void Release() {
            Released = true;
            Down = false;
        }

        public void NoChange() {
            Pressed = false;
            Released = false;
        }
    }
}