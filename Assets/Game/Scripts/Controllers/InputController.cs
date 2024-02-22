using System;
using Game.Configs;
using Game.Data.Enums;
using Game.Objects;
using UnityEngine;
using Zenject;

namespace Game.Controllers {
    public class InputController : IInputController, ITickable {
        [Inject] private IGameController _gameController;
        [Inject] private GameConfig _gameConfig;

        public event Action<Tile, Direction> OnSwiped;

        private Vector2 _initialPosition;
        private Tile _tile;

        public void SetTile(Tile tile) {
            _initialPosition = Input.mousePosition;
            _tile = tile;
        }

        public void Tick() {
            if (!_tile) {
                return;
            }

            if (Input.mousePosition.x - _initialPosition.x > _gameConfig.TileSwipeSensitivity) {
                Swipe(Direction.Right);
            }

            if (Input.mousePosition.x - _initialPosition.x < -_gameConfig.TileSwipeSensitivity) {
                Swipe(Direction.Left);
            }

            if (Input.mousePosition.y - _initialPosition.y > _gameConfig.TileSwipeSensitivity) {
                Swipe(Direction.Up);
            }

            if (Input.mousePosition.y - _initialPosition.y < -_gameConfig.TileSwipeSensitivity) {
                Swipe(Direction.Down);
            }
        }

        private void Swipe(Direction direction) {
            _gameController.SwipeTile(_tile, direction);
            _tile = null;
        }
    }
}