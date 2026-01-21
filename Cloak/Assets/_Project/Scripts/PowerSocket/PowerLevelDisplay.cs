using UnityEngine;

public class PowerLevelDisplay : MonoBehaviour
{
    [SerializeField] Transform powerSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerSprite.transform.localScale = new Vector3(powerSprite.transform.localScale.x, 0, 1);//set start state
    }

    public void UpdatePowerLevel(float level)
    {
        powerSprite.transform.localScale = new Vector3(powerSprite.transform.localScale.x, level, 1);
    }
}
