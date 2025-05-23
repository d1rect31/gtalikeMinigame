using UnityEngine;
using System.Collections.Generic;

public class BackgroundTiler : MonoBehaviour
{
    public GameObject tilePrefab;
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

    // Update is called once per frame
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
        Vector3 pos = new Vector3(0, yPos, 0);
        GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
        tiles.AddLast(tile);
    }
}
