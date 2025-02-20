using UnityEngine;
[CreateAssetMenu(fileName = "PaletteCard", menuName = "CustomScriptObj/PaletteCard", order = 1)]
public class PaletteCard : ScriptableObject
{
    [Header("Colour Palette")]
    public Palette[] palette;
}

[System.Serializable]
public class Palette
{
    [Header("Linked Colours")]
    public Color dimLightCol;
    public Color sunlightColCol;
    public Color darkCol;
    [Header("Unlinked Colours")]
    public Color fogCol;
    public Color accentCol;
    public Color noiseCol;
    public Color bloodCol;
    /*
    [Header("Linked Colours")]
    public Gradient lightCol;
    public Gradient midCol;
    public Gradient darkCol;
    [Header("Unlinked Colours")]
    public Gradient fogCol;

    public Gradient accentCol;
    public Gradient bloodCol;
    */
}
