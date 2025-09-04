using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVEL_GENERATOR {
    public class LevelData : ScriptableObject {
        [SerializeField] public List<SquareRow> Squares = new List<SquareRow>();
        [field: SerializeField] public TileBase StructureTile { get; private set; }
        [field: SerializeField] public TileBase EnemyTile { get; private set; }
        [field: SerializeField] public TileBase EmptyTile { get; private set; }
    }
}