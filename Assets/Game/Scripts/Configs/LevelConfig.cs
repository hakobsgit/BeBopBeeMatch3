using UnityEngine;

namespace Game.Configs {
    [CreateAssetMenu(menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject {
        [field: SerializeField] public int Columns { get; private set; }
        [field: SerializeField] public int Rows { get; private set; }
        [field: SerializeField] public float CellSize { get; private set; } = 0.5f;
        [field: SerializeField] public TilesConfig OverrideTilesConfig { get; private set; }
    }
}