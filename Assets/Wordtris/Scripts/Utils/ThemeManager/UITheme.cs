﻿using UnityEngine;

namespace Assets.Wordis.Frameworks.ThemeManager.Scripts
{
    /// <summary>
    /// UI Theme scriptable instance store all the tags and associated colors or sprite to form a  theme. 
    /// This settings can be configured from Hyperbyte -> Theme Settings menu item.
    /// </summary>
    public class UITheme : ScriptableObject
    {
        public ColorThemeTag[] colorTags;
        public SpriteThemeTag[] spriteTags;
    }

    /// <summary>
    /// Color tags attached with theme.
    /// </summary>
    [System.Serializable]
    public class ColorThemeTag
    {
        // Tag name.
        public string tagName;

        // Color attached with tag.
        public Color tagColor;
    }

    /// <summary>
    /// Sprite tags attached with theme.
    /// </summary>
    [System.Serializable]
    public class SpriteThemeTag
    {
        // Tag name.
        public string tagName;

        // Sprite attached with tag.
        public Sprite tagSprite;
    }
}