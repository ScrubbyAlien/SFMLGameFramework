using SFMLGameFramework;

namespace Test.Scenes;

public class TestScene() : Scene("main")
{
    protected override void LoadObjects() {
        AddObject(new TestSceneObject());
    }
}