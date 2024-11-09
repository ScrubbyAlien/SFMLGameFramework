using SFML.Window;
using SFMLGameFramework;

namespace Test;

public class TestSceneObject : SceneObject
{
    public override void Update(float deltaTime) {
        if (Input.KeyDown(Keyboard.Key.Space)) {
            Debug.Log("down");
        }
        if (Input.KeyPressed(Keyboard.Key.Space)) {
            Debug.Log("pressed");
        }
        if (Input.KeyUp(Keyboard.Key.Space)) {
            Debug.Log("up");
        }
        if (Input.KeyReleased(Keyboard.Key.Space)) {
            Debug.Log("released");
        }
    }
}