﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class GameText
    {
        // General
        public string ID { get; private set; }
        public Dictionary<GameLanguages, string> languageDict { get; private set; }

        // Constructor
        public GameText(
            string ID,
            Dictionary<GameLanguages, string> languageDict
            )
        {
            this.ID = ID;
            this.languageDict = languageDict;
        }

        // Get string
        public string GetText()
        {
            // by default return native language
            return GetText(GameSettings.language);
        }
        public string GetText(GameLanguages language)
        {
            if (languageDict.ContainsKey(language))
            {
                return languageDict[language];
            }
            return "";
        }
    }
}