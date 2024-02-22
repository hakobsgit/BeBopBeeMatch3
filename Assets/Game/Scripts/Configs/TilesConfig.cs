using Game.Data;
using UnityEngine;

namespace Game.Configs {
    [CreateAssetMenu(menuName = "Configs/TilesConfig")]
    public class TilesConfig : ScriptableObject {
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        [field: SerializeField] public TileData[] Tiles { get; private set; }
    }
}