using UnityEngine;
[CreateAssetMenu(fileName = "EffectCard", menuName = "Effects/EffectCard", order = 1)]
public class EffectCard : ScriptableObject
{
    [Header("Effect Info")]
    public string effectName;
    public Sprite effectIcon;

}
