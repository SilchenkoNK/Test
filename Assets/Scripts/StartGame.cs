using System;
using UnityEngine;
using Assets.SimpleLocalization;

static class StartGame
{
    const int CurrentVersion = 1;

    [RuntimeInitializeOnLoadMethod]
    static void Start()
    {
        int oldVersion = Save.GetInt("Version");

        for (int i = 1; i <= CurrentVersion; i++)
            if (oldVersion < i && CurrentVersion >= i)
                UpdateGame(i);

        Save.SetInt("Version", CurrentVersion);

        LocalizationManager.Init();

        LoadValues();
    }

    static void LoadValues()
    {
        LocalizationManager.Language = Save.GetString("Language");
    }

    static void UpdateGame(int version)
    {
        if (version == 1)
        {
            FirstInitOfValues();
        }
    }

    static void FirstInitOfValues()
    {
        SystemLanguage language = Application.systemLanguage;

        bool supported = false;

        foreach (SystemLanguage l in LocalizationManager.SupportedLanguages)
        {
            if (l == language)
            {
                supported = true;
                break;
            }
        }

        if (!supported)
            language = SystemLanguage.English;

        Save.SetString("Language", language.ToString());
    }
}
