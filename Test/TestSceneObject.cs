using SFML.Window;
using SFMLGameFramework;
using SFMLGameFramework.Audio;

namespace Test;

public class TestSceneObject : SceneObject
{
    protected override void Initialize() {
        AudioManager.PlayMusic("mainmenu.wav");
    }

    public override void Update(float deltaTime) {
        if (Input.KeyPressed(Keyboard.Key.Space)) {
            AudioManager.PlaySound("powerup.wav");
        }
        if (Input.KeyPressed(Keyboard.Key.A)) {
            AudioManager.PauseMusic();
        }
        if (Input.KeyPressed(Keyboard.Key.K)) {
            AudioManager.PlayMusic("mainmenu.wav");
        }
        if (Input.KeyPressed(Keyboard.Key.B)) {
            AudioManager.PlayMusic("finale.wav");
        }
    }
}