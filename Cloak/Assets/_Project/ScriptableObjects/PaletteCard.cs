using UnityEngine;
[CreateAssetMenu(fileName = "PaletteCard", menuName = "CustomScriptObj/PaletteCard", order = 1)]
public class PaletteCard : ScriptableObject
{
    [Header("Colour Palette")]
    public Palette palette;
}

[System.Serializable]
public class Palette
{
    [Header("Level")]
    [Tooltip("UnlitShadow")]
    public Color darkCol;
    [Tooltip("Dark part of the sprite")]
    public Color outlineCol;
    [Tooltip("Light part of the sprite")]
    public Color wallCol;
    [Header("Background")]
    public Color backWallCol;
    public Color fogCol;
    [Header("Effects")]
    public Color noiseCol;
}
