using Microsoft.Extensions.Configuration;
using OvensManagerApp.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OvensManagerApp.Services;
public class SoundService
{
    private static SoundService _instance;
    private static readonly object _lock = new object();
    private readonly Dictionary<SoundsList, string> _soundPaths;
    private readonly SoundPlayer _soundPlayer;

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
            string soundFilePath = AppDomain.CurrentDomain.BaseDirectory+ _soundPaths[soundType];
            if (File.Exists(soundFilePath))
            {
                _soundPlayer.SoundLocation = soundFilePath;
                Task.Run(()=>_soundPlayer.PlaySync());
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
}
