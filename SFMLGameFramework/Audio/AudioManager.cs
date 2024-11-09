using SFML.Audio;
using SFML.System;

namespace SFMLGameFramework.Audio;

public static class AudioManager
{
    private static Music? _music;
    private static bool _paused;
    private static string _currentMusicFile = "";

    /// <summary>
    /// Play the music file located at music/fileName (relative to your assets directory). If it's paused then unpause it.
    /// If a different track is already playing, stop playback and start playing the new track.
    /// </summary>
    /// <param name="fileName"></param>
    public static void PlayMusic(string fileName) {
        if (_currentMusicFile != fileName) {
            // playing new track
            StopMusic();
            _currentMusicFile = fileName;
            _music = AssetManager.GetMusic(fileName);
            _music.Loop = true;
            _music.Play();
        }
        if (_paused) {  // unpause the music if its paused
            _paused = false;
            _music?.Play();
        }
    }

    public static void StopMusic() {
        _currentMusicFile = "";
        _paused = false;
        _music?.Stop();
        _music = null;
    }

    public static void PauseMusic() {
        _paused = true;
        _music?.Pause();
    }
    
    public static void PlaySpatialSound(string fileName,
                                        Vector3f position,
                                        float minDistance,
                                        bool relative = false,
                                        float attenuation = 1) 
    {
        Sound sound = AssetManager.GetSound(fileName);
        PlaySpatialSound(sound, position, minDistance, relative, attenuation);
    }
    public static void PlaySpatialSound(Sound sound,
                                        Vector3f position,
                                        float minDistance,
                                        bool relative = false,
                                        float attenuation = 1,
                                        bool interrupt = false) 
    {
        sound.Position = position;
        sound.RelativeToListener = relative;
        sound.MinDistance = minDistance;
        sound.Attenuation = attenuation;
        PlaySound(sound);
    }
    /// <summary>
    /// Create a new sound object from the file located at sounds/fileName (relative to your assets directory)
    /// and play it. 
    /// </summary>
    /// <param name="fileName"></param>
    public static void PlaySound(string fileName) {
        Sound sound = AssetManager.GetSound(fileName);
        PlaySound(sound);
    }
    /// <summary>
    /// Play the sound file provided
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound) {
        sound.Play();
    }

}