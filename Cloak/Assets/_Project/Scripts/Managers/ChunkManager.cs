using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static readonly Vector2Int roomSize = new Vector2Int(50, 50);
    Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    [SerializeField] Transform playerTrans;
    [SerializeField] Vector2Int currentChunkId;

    //static reference
    public static ChunkManager instance;

    //Awake
    void Awake()
    {
        instance = this;

        currentChunkId = new Vector2Int(100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentChunk();
    }

    void CheckCurrentChunk()
    {
        if (playerTrans == null) return;
        Vector2Int id = GetChunkId(playerTrans.position);
        if (currentChunkId == id) return;//no change
        Chunk thisChunk = chunks[id];
        if (thisChunk == null) return;//no chunk
        IsNewChunk(id, thisChunk);//update
    }

    void IsNewChunk(Vector2Int id, Chunk newChunk)//update
    {
        currentChunkId = id;
        if (newChunk.chunkData.colourPalette != null)//colours
            ShaderManager.instance.StartColourPaletteChange(newChunk.chunkData.colourPalette.palette);

        if (newChunk.chunkData.chunkAmbience != null)//ambience
            AudioManager.instance.PlayMusic(newChunk.chunkData.chunkAmbience, 1, false);
    }



    public void AddChunkToDictionary(Chunk newChunk)
    {
        chunks.Add(newChunk.chunkId, newChunk);
    }

    public Vector2Int GetChunkId(Vector2 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / roomSize.x), Mathf.FloorToInt(position.y / roomSize.y));
    }
}
