using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase[] _tiles;

   // [SerializeField] private float _tilemapWidth = 10f;


    private void Start()
    {
        _tilemap.CompressBounds();

        _tilemap.SetTile(new Vector3Int(0, 0, 0), _tiles[2]);
        _tilemap.SetTile(new Vector3Int(10, 0, 0), _tiles[1]);
    }
}
