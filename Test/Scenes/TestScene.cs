using Framework;

namespace Test.Scenes;

public class TestScene() : Scene("main")
{
    protected override void LoadObjects() {
        Console.WriteLine("testing scene");
    }
}