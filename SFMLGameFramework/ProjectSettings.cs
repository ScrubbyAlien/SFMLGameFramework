namespace SFMLGameFramework;

public sealed class ProjectSettings
{
    public uint ScreenWidth { get; init; } = 1000;
    public uint ScreenHeight { get; init; } = 1000;
    public uint FrameLimit { get; init; } = 144;
    public string MainScene { get; init; } = "main";
    public string ScenesNamespace { get; init; } = "Scenes";
    public string AssetPath { get; init; } = "assets";
    public Dictionary<string, string[]> PreLoadedAssets { get; init; } = new();
}