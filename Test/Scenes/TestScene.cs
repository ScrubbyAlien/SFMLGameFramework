using Framework;

namespace Test.Scenes;

public class TestScene() : Scene("main")
{
    protected override void LoadObjects() {
        // Debug.Write("Test scene");
        AddObject(new TestSceneObject());
    }
}