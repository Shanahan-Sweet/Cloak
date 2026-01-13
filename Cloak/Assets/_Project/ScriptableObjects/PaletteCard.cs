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

    public Color sunlightCol;
    public Color sunHighlightCol;
    public Color darkCol;
    public Color backWallCol;
    public Color fogCol;
    public Color noiseCol;
}
