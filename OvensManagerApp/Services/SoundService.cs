using Microsoft.Extensions.Configuration;
using OvensManagerApp.Enums;
using System.IO;
using System.Media;

namespace OvensManagerApp.Services;

public class SoundService
{
    private static SoundService _instance;
    private bool _isPlayingSound;
    private static readonly object _lock = new object();
    private readonly Dictionary<SoundsList, string> _soundPaths;
    private readonly SoundPlayer _soundPlayer;
    private readonly Queue<string> _soundsQueue = new Queue<string>();

    // Private constructor for singleton pattern
    private SoundService()
    {
        // Initialize SoundPlayer
        _soundPlayer = new SoundPlayer();

        // Load sound paths from appsettings.json
        _soundPaths = new Dictionary<SoundsList, string>();

        // Load configuration
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Load sound file paths into dictionary
        var soundsConfig = config.GetSection("SoundFiles");

        _soundPaths[SoundsList.ReadytoUnload] = soundsConfig["ReadytoUnload"];
        _soundPaths[SoundsList.OvenCanBeOpened] = soundsConfig["OvenCanBeOpened"];
    }

    // Singleton instance with thread-safe initialization
    public static SoundService Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new SoundService();
                }
                return _instance;
            }
        }
    }

    // Method to play sound by enum
    public void PlaySound(SoundsList soundType)
    {
        if (_soundPaths.ContainsKey(soundType))
        {
            string soundFilePath = AppDomain.CurrentDomain.BaseDirectory + _soundPaths[soundType];
            if (File.Exists(soundFilePath))
            {
                _soundsQueue.Enqueue(soundFilePath);
                PlaySoundsInQueue();
            }
            else
            {
                throw new FileNotFoundException($"Sound file not found: {soundFilePath}");
            }
        }
        else
        {
            throw new ArgumentException($"Sound type {soundType} not configured.");
        }
    }

    // Method to play sound by sound path
    public void PlaySound(string path)
    {
        string soundFilePath = AppDomain.CurrentDomain.BaseDirectory + path;
        if (File.Exists(soundFilePath))
        {
            _soundsQueue.Enqueue(soundFilePath);
            PlaySoundsInQueue();
        }
        else
        {
            throw new FileNotFoundException($"Sound file not found: {soundFilePath}");
        }
    }

    private async void PlaySoundsInQueue()
    {
        if (_isPlayingSound)
        {
            return;
        }
        _isPlayingSound = true;
        while (_soundsQueue.Count > 0)
        {
            string soundLocation = _soundsQueue.Dequeue();
            _soundPlayer.SoundLocation = soundLocation;
            await Task.Run(() => _soundPlayer.PlaySync());
        }
        _isPlayingSound = false;
    }
}
