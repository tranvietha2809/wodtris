using System;
using System.Linq;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Assets.Wordis.Frameworks.ThemeManager.Scripts
{
    /// <summary>
    /// Theme manager contrll game themes, returns requires images, colors attached to tags and controlls the event callbacks.
    /// </summary>
    public class ThemeManager : Singleton<ThemeManager>
    {
        string currentThemeName = "";
        [SerializeField] UIThemeSettings uiThemeSettings;

        [SerializeField] UITheme currentUITheme;
        [NonSerialized] public bool hasInitialised = false;

        public static event Action<string> OnThemeInitializedEvent;
        public static event Action<string> OnThemeChangedEvent;

        // List<ThemeConfig> allActiveThemes = new List<ThemeConfig>();

        [HideInInspector] public bool UIThemeEnabled = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (uiThemeSettings == null)
            {
                uiThemeSettings = (UIThemeSettings)Resources.Load("UIThemeSettings");
            }

            if (uiThemeSettings.useUIThemes)
            {
                UIThemeEnabled = true;
                Initialise();
            }
            else
            {
                currentUITheme = (UITheme)Resources.Load("UIThemes/DefaultTheme");
            }
        }


        /// <summary>
        /// Initializes theme manager.
        /// </summary>
        void Initialise()
        {
            if (!hasInitialised)
            {
                int defaultTheme = uiThemeSettings.defaultTheme;

                if (!PlayerPrefs.HasKey("currentThemeName"))
                {
                    PlayerPrefs.SetString("currentThemeName", "Theme-6");
                    currentThemeName = "Theme-6";
                }
                else
                {
                    currentThemeName = PlayerPrefs.GetString("currentThemeName");
                }

                currentUITheme = uiThemeSettings.allThemeConfigs.ToList().Find(o => o.themeName == currentThemeName)
                    .uiTheme;
                uiThemeSettings = null;
                hasInitialised = true;

                OnThemeInitializedEvent?.Invoke(currentThemeName);
            }
        }

        /// <summary>
        /// Applies given theme to app.
        /// </summary>
        public void SetTheme(ThemeConfig themeSetting)
        {
            currentUITheme = themeSetting.uiTheme;
            currentThemeName = themeSetting.themeName;
            PlayerPrefs.SetString("currentThemeName", currentThemeName);

            OnThemeChangedEvent?.Invoke(currentThemeName);
        }

        /// <summary>
        /// Returns instance of current active theme.
        /// </summary>
        public UITheme GetCurrentTheme()
        {
            return currentUITheme;
        }

        /// <summary>
        /// Returns current active theme id.
        /// </summary>
        public string GetCurrentThemeName()
        {
            return currentThemeName;
        }

        /// <summary>
        /// Returns color for the given tag from selected theme scriptable.
        /// </summary>
        public Color GetThemeColorWithTag(string colorTag)
        {
            return currentUITheme.colorTags.FirstOrDefault(o => o.tagName == colorTag).tagColor;
        }

        /// <summary>
        /// Returns Sprite for the given tag from selected theme scriptable.
        /// </summary>
        public Sprite GetThemeSpriteWithTag(string spriteTag)
        {
            return currentUITheme.spriteTags.FirstOrDefault(o => o.tagName == spriteTag).tagSprite;
        }

        public Sprite GetBlockSpriteWithTag(string spriteTag)
        {
            return currentUITheme.spriteTags.FirstOrDefault(o => o.tagName == spriteTag).tagSprite;
        }
    }
}