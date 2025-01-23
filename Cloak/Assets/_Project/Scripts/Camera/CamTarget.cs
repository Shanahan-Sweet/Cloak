using System.Collections;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    [SerializeField] float pullStrength = 1;
    public float PullStrength { get { return pullStrength; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    protected void OnEnable()
    {
        MainCam.instance.AddToTargetList(this);
    }

    protected void OnDisable()
    {
        MainCam.instance.RemoveFromTargetList(this);
    }
}
