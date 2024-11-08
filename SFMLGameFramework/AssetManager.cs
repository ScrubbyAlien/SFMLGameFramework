using SFML.Audio;
using SFML.Graphics;

namespace SFMLGameFramework;

public static class AssetManager
{
    static AssetManager() {
        AssetPath = Game.ProjectSettings.AssetPath;
    }
    internal static readonly string AssetPath;
    private const string TexturesPath = "textures";
    private const string FontsPath = "fonts";
    private const string SoundsPath = "sounds";
    private const string MusicsPath = "music";

    private static Dictionary<string, Texture> _preLoadedTextures = new();
    private static Dictionary<string, Texture> _loadedTextures = new();
    private static Texture LoadTexture(string fileName) {
        return new Texture($"{AssetPath}/{TexturesPath}/{fileName}");
    }

    private static Dictionary<string, Font> _preLoadedFonts = new();
    private static Dictionary<string, Font> _loadedFonts = new();
    private static Font LoadFont(string fileName) {
        return new Font($"{AssetPath}/{FontsPath}/{fileName}");
    }

    private static Dictionary<string, SoundBuffer> _preLoadedSounds = new();
    private static Dictionary<string, SoundBuffer> _loadedSounds = new();
    private static SoundBuffer CreateSoundBuffer(string fileName) {
        return new SoundBuffer($"{AssetPath}/{SoundsPath}/{fileName}");
    }

    private static Dictionary<string, Music> _preLoadedMusics = new();
    private static Dictionary<string, Music> _loadedMusics = new();
    private static Music CreateMusic(string fileName) {
        return new Music($"{AssetPath}/{MusicsPath}/{fileName}");
    }

    private static void PreLoadFiles<T>(
        string[] fileNames,
        ref Dictionary<string, T> preLoadedDict,
        Func<string, T> constructor
    ) {
        foreach (string fileName in fileNames) {
            preLoadedDict.TryAdd(fileName, constructor(fileName));
        }
    }

    internal static void PreLoadAssets(Dictionary<string, string[]> paths) {
        foreach (string directory in paths.Keys) {
            switch (directory) {
                case TexturesPath:
                    PreLoadFiles(paths[directory], ref _preLoadedTextures, LoadTexture);
                    break;
                case FontsPath:
                    PreLoadFiles(paths[directory], ref _preLoadedFonts, LoadFont);
                    break;
                case SoundsPath:
                    PreLoadFiles(paths[directory], ref _preLoadedSounds, CreateSoundBuffer);
                    break;
                case MusicsPath:
                    PreLoadFiles(paths[directory], ref _preLoadedMusics, CreateMusic);
                    break;
                default: continue;
            }
        }
    }

    internal static void UnloadAssets() {
        _loadedFonts.Clear();
        _loadedTextures.Clear();
        _loadedSounds.Clear();
        _loadedMusics.Clear();
    }

    public static Texture GetTexture(string fileName) {
        return GetGenericAsset(fileName, ref _preLoadedTextures, ref _loadedTextures, LoadTexture);
    }
    public static Font GetFont(string fileName) {
        return GetGenericAsset(fileName, ref _preLoadedFonts, ref _loadedFonts, LoadFont);
    }
    public static Sound GetSound(string fileName) {
        return new Sound(GetGenericAsset(fileName, ref _preLoadedSounds, ref _loadedSounds, CreateSoundBuffer));
    }
    public static Music GetMusic(string fileName) {
        return GetGenericAsset(fileName, ref _preLoadedMusics, ref _loadedMusics, CreateMusic);
    }

    private static T GetGenericAsset<T>(
        string fileName,
        ref Dictionary<string, T> preLoaded,
        ref Dictionary<string, T> loaded,
        Func<string, T> constructor
    ) {
        if (preLoaded.TryGetValue(fileName, out T? preT)) return preT;
        if (loaded.TryGetValue(fileName, out T? loadT)) return loadT;
        loaded.Add(fileName, constructor(fileName));
        return loaded[fileName];
    }
}