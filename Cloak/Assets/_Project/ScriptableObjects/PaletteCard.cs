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
    [Header("Linked Colours")]
    public Color dimLightCol;
    public Color sunlightCol;
    public Color darkCol;
    [Header("Unlinked Colours")]
    public Color fogCol;
    public Color topAccentCol, accentCol;
    public Color noiseCol;
    //public Color bloodCol;
}
