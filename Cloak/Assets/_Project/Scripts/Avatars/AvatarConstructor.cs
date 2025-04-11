using UnityEngine;

public class AvatarConstructor : MonoBehaviour
{
    [SerializeField] Avatar avatarScript;
    [Header("Avatar skeleton")]
    [SerializeField] Transform headHolder;
    [SerializeField] Transform bodyHolder, legsHolder;

    [Header("Avatar prefabs")]
    [SerializeField] GameObject[] headPrefabs;
    [SerializeField] GameObject[] bodyPrefabs, legPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateAvatar();
    }

    void CreateAvatar()
    {

    }

    void RemoveAvatar()//reset
    {

    }
}
