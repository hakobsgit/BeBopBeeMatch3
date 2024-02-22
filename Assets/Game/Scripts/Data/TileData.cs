using System;
using UnityEngine;

namespace Game.Data {
    [Serializable]
    public class TileData {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}