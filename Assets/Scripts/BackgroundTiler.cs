using UnityEngine;
using System.Collections.Generic;

public class BackgroundTiler : MonoBehaviour
{
    public List<GameObject> tilePrefabs; // Список префабов тайлов
    public Transform player;
    public float tileLength = 10f;
    public int tilesAhead = 3;
    public int tilesBehind = 2;

    private LinkedList<GameObject> tiles = new LinkedList<GameObject>();
    private float nextTileY = 0f;

    void Start()
    {
        for (int i = -tilesBehind; i < tilesAhead; i++)
        {
            SpawnTile(i * tileLength);
        }
        nextTileY = tiles.Last.Value.transform.position.y + tileLength;
    }

    void Update()
    {
        if (player.position.y + tilesAhead * tileLength > nextTileY - tileLength / 2)
        {
            SpawnTile(nextTileY);
            nextTileY += tileLength;
        }
        while (tiles.Count > 0 && player.position.y - tiles.First.Value.transform.position.y > tilesBehind * tileLength + tileLength / 2)
        {
            Destroy(tiles.First.Value);
            tiles.RemoveFirst();
        }
    }

    void SpawnTile(float yPos)
    {
        if (tilePrefabs == null || tilePrefabs.Count == 0)
        {
            Debug.LogWarning("Список префабов тайлов пуст!");
            return;
        }
        int randomIndex = Random.Range(0, tilePrefabs.Count);
        GameObject prefab = tilePrefabs[randomIndex];
        Vector3 pos = new Vector3(0, yPos, 0);
        GameObject tile = Instantiate(prefab, pos, Quaternion.identity);
        tiles.AddLast(tile);
    }
}
