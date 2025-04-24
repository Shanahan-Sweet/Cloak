using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] Vector2Int roomSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 1);
        Vector2 halfRoomScale = new Vector3(roomSize.x / 2f, roomSize.y / 2f, 0);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0) + (Vector3)halfRoomScale, new Vector3(roomSize.x, roomSize.y, 0));

    }

    void OnDrawGizmosSelected()//display selected chunk
    {
        Gizmos.color = new Color(.25f, 0.25f, 1, 1);
        Vector2 halfRoomScale = new Vector3(roomSize.x / 2f, roomSize.y / 2f, 0);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0) + (Vector3)halfRoomScale, new Vector3(roomSize.x, roomSize.y, 0) + new Vector3(1, 1, 0));
    }
#endif
}
