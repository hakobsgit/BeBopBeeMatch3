using Game.Data;
using Game.Data.Enums;
using Game.Objects;
using UniRx;
using UnityEngine;

namespace Game.Controllers {
    public interface IGameController {
        ReactiveProperty<bool> IsProcessing { get; }

        GridData Grid { get; }

        Tile AddTile(int x, int y, float animationDelay, Vector2 position,
            bool forceCreateNew = false, bool noMatch3 = true);

        void SwipeTile(Tile tile, Direction direction);
    }
}