using UnityEngine;

public class DetailPlant : DetailObject
{
    [SerializeField] Transform[] rotationTransform;

    [SerializeField] float swayMagnitude, swaySpd;

    float swayT;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        swayT = Random.value * 6.3f;
    }

    // Update is called once per frame
    void Update()
    {
        swayT += Time.deltaTime * swaySpd;
        for (int i = 0; i < rotationTransform.Length; i++)
        {
            rotationTransform[i].localRotation = Quaternion.Euler(0, 0, Mathf.Sin(swayT + i * 1.6f) * swayMagnitude);
        }
    }

    public override void Collide(float speed)
    {

    }
}
