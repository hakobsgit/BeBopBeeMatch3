using Game.Data.Enums;
using UnityEngine;

namespace Game.Configs {
    [CreateAssetMenu(menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject {
        [field: SerializeField] public float TileSwipeSensitivity { get; private set; }
        [field: SerializeField] public bool UseAnimations { get; set; }
        [field: SerializeField] public MatchType MatchType { get; private set; }
    }
}