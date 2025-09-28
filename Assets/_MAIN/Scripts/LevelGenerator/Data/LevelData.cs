using System.Collections.Generic;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.DATA {
    public class LevelData : ScriptableObject {
        [SerializeField] public List<LevelRow> Cells = new List<LevelRow>();
    }
}