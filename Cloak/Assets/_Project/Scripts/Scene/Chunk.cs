using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int chunkId;
    public ChunkData chunkData;

    //manager
    ChunkManager chunkManager;
    ChunkManager CManager
    {
        get
        {
            if (chunkManager == null)
            {
                chunkManager = ChunkManager.instance;
            }
            return chunkManager;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chunkId = CManager.GetChunkId(transform.position);
        CManager.AddChunkToDictionary(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2Int roomSize = ChunkManager.roomSize;
        Gizmos.color = new Color(1, 1, 1, 1);
        Vector2 halfRoomScale = new Vector3(roomSize.x / 2f, roomSize.y / 2f, 0);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0) + (Vector3)halfRoomScale, new Vector3(roomSize.x, roomSize.y, 0));

    }

    void OnDrawGizmosSelected()//display selected chunk
    {
        Vector2Int roomSize = ChunkManager.roomSize;
        Gizmos.color = new Color(.25f, 0.25f, 1, 1);
        Vector2 halfRoomScale = new Vector3(roomSize.x / 2f, roomSize.y / 2f, 0);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawWireCube(new Vector3(pos.x, pos.y, 0) + (Vector3)halfRoomScale, new Vector3(roomSize.x, roomSize.y, 0) + new Vector3(1, 1, 0));
    }
#endif
}
[Serializable]
public class ChunkData
{
    public PaletteCard colourPalette;
    public AudioClip chunkAmbience;
}