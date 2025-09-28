using System.Collections.Generic;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.DATA {
    public class LevelData : ScriptableObject {
        
        [SerializeField] public List<SquareRow> Squares = new List<SquareRow>();
    }
}