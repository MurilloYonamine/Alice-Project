using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LEVEL {

    [CreateAssetMenu(fileName = "Create New Level", menuName = "Level Generation")]
    public class LevelGenerator : ScriptableObject {
        [field: SerializeField] public int _row = 10;
        [field: SerializeField] public int _col = 6;
    }
}