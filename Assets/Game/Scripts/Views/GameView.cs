using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game.Views {
    public class GameView : MonoBehaviour {
        [field: SerializeField] public GameObject TileContainerPrefab { get; private set; }
        [field: SerializeField] public GameObject TilePrefab { get; private set; }
        [field: SerializeField] public Transform TileContainersParent { get; private set; }
        [field: SerializeField] public Transform TilesParent { get; private set; }
        [field: SerializeField] public SpriteRenderer Panel { get; private set; }
        [field: SerializeField] public SpriteMask Mask { get; private set; }
        [field: SerializeField] public Grid SceneGrid { get; private set; }

        [Inject] public ISimulationController SimulationController { get; private set; }
    }
}