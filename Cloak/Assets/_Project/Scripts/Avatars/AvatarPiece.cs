using UnityEngine;

public class AvatarPiece : MonoBehaviour
{


    [SerializeField] public Vector2 moveStrength = new Vector2(1, 1);
    [SerializeField] public float rotationStrength = 0, torqueStrength = 0;
    public Vector2 offset = new Vector2(0, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.localPosition;
    }
    public virtual void SetPos(Vector2 lookDir, float torque)
    {
        Vector2 pos = lookDir * moveStrength + offset;
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
        SetRotation(lookDir, torque);
    }

    public virtual void SetRotation(Vector2 lookDir, float torque)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (lookDir.x * rotationStrength) + (torque * torqueStrength)));
    }
}
