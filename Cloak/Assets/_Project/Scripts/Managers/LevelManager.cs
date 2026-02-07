using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Border")]
    [SerializeField] Vector2 levelWidth = new Vector2(-10, 10);
    [SerializeField] Vector2 levelHeight = new Vector2(10, -10);
    Vector2 LevelCenter { get { return new Vector2(levelWidth.y + (levelWidth.x - levelWidth.y) / 2, levelHeight.y + (levelHeight.x - levelHeight.y) / 2); } }
    Vector2 LevelSize { get { return new Vector2(levelWidth.x - levelWidth.y, levelHeight.x - levelHeight.y); } }

    public Vector2 LevelWidth { get { return levelWidth; } }
    public Vector2 LevelHeight { get { return levelHeight; } }


    [Header("Editor")]
    [SerializeField] GameObject editorLight;



    //static reference
    public static LevelManager instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (editorLight != null) Destroy(editorLight);
    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 roomSize = LevelSize;
        Gizmos.color = new Color(1, 1, 1, 1);
        Vector2 pos = LevelCenter;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0), new Vector3(roomSize.x, roomSize.y, 0));

    }

    /*void OnDrawGizmosSelected()//display selected chunk
    {
        Vector2Int roomSize = ChunkManager.roomSize;
        Gizmos.color = new Color(.25f, 0.25f, 1, 1);
        Vector2 halfRoomScale = new Vector3(roomSize.x / 2f, roomSize.y / 2f, 0);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0) + (Vector3)halfRoomScale, new Vector3(roomSize.x, roomSize.y, 0) + new Vector3(1, 1, 0));
    }*/
#endif
}
