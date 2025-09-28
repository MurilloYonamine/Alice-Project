using System.Collections.Generic;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE {
    [CreateAssetMenu(menuName = "LevelGenerator/LevelData")]
    public class LevelData : ScriptableObject {
        public List<LevelRow> Cells = new List<LevelRow>();
    }
}
