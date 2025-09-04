using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData : ScriptableObject {
    public List<List<SquareStates>> squares;

    [SerializeField] private TileBase structureTile;
    [SerializeField] private TileBase enemyTile;
    [SerializeField] private TileBase emptyTile;
}