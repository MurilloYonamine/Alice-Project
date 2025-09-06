using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVELGENERATOR.DATA {
    public class LevelData : ScriptableObject {
        
        [SerializeField] public List<SquareRow> Squares = new List<SquareRow>();
    }
}