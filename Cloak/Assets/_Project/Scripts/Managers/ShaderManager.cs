using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    [Header("Colour Palette")]
    [SerializeField] PaletteCard currentPaletteCard;
    Palette currentPalette;

    public Color dimLightCol, sunlightCol, darkCol;
    [HideInInspector]
    public Color sunHighlightCol, accentCol, topAccentCol, noiseCol, fogCol, dimLightColBackground, sunlightColBackground;

    //_______________________________________________
    [SerializeField] AnimationCurve colourCurve;

    IEnumerator paletteSequence;

    public static ShaderManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        SetColours();

        //InvokeRepeating(nameof(RandomColourTest), 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
    }

    [ContextMenu("Set Colours")]
    public void SetColours()//instant change
    {
        currentPalette = currentPaletteCard.palette;

        dimLightCol = currentPalette.dimLightCol;
        sunlightCol = currentPalette.sunlightCol;
        sunHighlightCol = currentPalette.sunHighlightCol;
        darkCol = currentPalette.darkCol;
        accentCol = currentPalette.accentCol;
        topAccentCol = currentPalette.topAccentCol;
        noiseCol = currentPalette.noiseCol;

        fogCol = currentPalette.fogCol;

        CalcBackgroundCol();

        SetShaderColours();//set shader colours
    }

    public void UpdateCameraBackground()
    {
        //if (MainCam.instance) MainCam.instance.mainCam.backgroundColor = fogCol;
    }

    public void StartColourPaletteChange(Palette targetPalette)
    {
        currentPalette = targetPalette;
        if (paletteSequence != null) StopCoroutine(paletteSequence);
        paletteSequence = PaletteSequence();
        StartCoroutine(paletteSequence);
    }

    public void ShiftColours()
    {
        if (paletteSequence != null) StopCoroutine(paletteSequence);
        paletteSequence = PaletteSequence();
        StartCoroutine(paletteSequence);
    }

    IEnumerator PaletteSequence()
    {
        Color startLight = dimLightCol;
        Color startSun = sunlightCol;
        Color startHighlight = sunHighlightCol;
        Color startDark = darkCol;
        Color startAccent = accentCol;
        Color startTopAccent = topAccentCol;
        Color startNoise = noiseCol;
        Color startFog = fogCol;


        //Get Target Colours
        Color dimLightColTarget = currentPalette.dimLightCol;
        Color sunlightTarget = currentPalette.sunlightCol;
        Color highlightTarget = currentPalette.sunHighlightCol;
        Color darkColTarget = currentPalette.darkCol;
        Color accentTarget = currentPalette.accentCol;
        Color topAccentTarget = currentPalette.topAccentCol;
        Color noiseTarget = currentPalette.noiseCol;
        Color fogColTarget = currentPalette.fogCol;

        float t = 0;
        while (t < 1)
        {
            dimLightCol = Color.Lerp(startLight, dimLightColTarget, t);
            sunlightCol = Color.Lerp(startSun, sunlightTarget, t);
            sunHighlightCol = Color.Lerp(startHighlight, highlightTarget, t);
            darkCol = Color.Lerp(startDark, darkColTarget, t);
            accentCol = Color.Lerp(startAccent, accentTarget, t);
            topAccentCol = Color.Lerp(startTopAccent, topAccentTarget, t);
            noiseCol = Color.Lerp(startNoise, noiseTarget, t);
            fogCol = Color.Lerp(startFog, fogColTarget, t);
            CalcBackgroundCol();


            SetShaderColours();//set colours
            t += Time.deltaTime * .2f;
            yield return null;
        }
    }
    void CalcBackgroundCol()
    {
        Color backgroundCol = Color.Lerp(darkCol, fogCol, .75f);
        dimLightColBackground = Color.Lerp(backgroundCol, dimLightCol, .5f);
        sunlightColBackground = Color.Lerp(backgroundCol, sunlightCol, .5f);
    }
    void SetShaderColours()
    {
        Shader.SetGlobalColor("_DimLightCol", EvaluateColour(dimLightCol));
        Shader.SetGlobalColor("_SunlightCol", EvaluateColour(sunlightCol));
        Shader.SetGlobalColor("_Highlight", EvaluateColour(sunHighlightCol));
        Shader.SetGlobalColor("_DarkCol", EvaluateColour(darkCol));
        Shader.SetGlobalColor("_AccentCol", EvaluateColour(accentCol));
        Shader.SetGlobalColor("_TopAccentCol", EvaluateColour(topAccentCol));
        Shader.SetGlobalColor("_NoiseCol", EvaluateColour(noiseCol));

        //background colours
        Shader.SetGlobalColor("_FogCol", EvaluateColour(fogCol));
        Shader.SetGlobalColor("_BackgroundDimLight", EvaluateColour(dimLightColBackground));
        Shader.SetGlobalColor("_BackgroundSun", EvaluateColour(sunlightColBackground));

        //if (MainCam.instance) MainCam.instance.mainCam.backgroundColor = fogCol;
    }

    Color EvaluateColour(Color col)
    {
        return new Color(colourCurve.Evaluate(col.r), colourCurve.Evaluate(col.g), colourCurve.Evaluate(col.b), 1);
    }
}
