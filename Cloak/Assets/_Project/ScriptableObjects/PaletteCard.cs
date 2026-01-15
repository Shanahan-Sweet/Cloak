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
    [Tooltip("UnlitShadow")]
    public Color darkCol;
    [Tooltip("Dark part of the sprite")]
    public Color ambientLightCol;
    [Tooltip("Light part of the sprite")]
    public Color highlightCol;

    public Color backWallCol;
    public Color fogCol;
    public Color noiseCol;
}
