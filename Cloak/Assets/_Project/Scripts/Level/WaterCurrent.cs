using UnityEngine;

public class WaterCurrent : MonoBehaviour
{

    [SerializeField] float currentForce = 10;
    public Vector2 CurrentVelocity { get { return transform.right * currentForce; } }

}
