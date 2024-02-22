using System;
using System.Collections;
using Game.Configs;
using Game.Data.Enums;
using Game.Extensions;
using Game.Utils;
using UnityEngine;
using Zenject;

namespace Game.Controllers {
    public class SimulationController : ISimulationController {
        [Inject] private IGameController _gameController;
        [Inject] private GameConfig _gameConfig;

        public void RandomMoves(int count = 1) {
            var grid = _gameController.Grid;
            var animationValue = _gameConfig.UseAnimations;
            _gameConfig.UseAnimations = false;

            var allDirections = (Direction[])Enum.GetValues(typeof(Direction));

            for (int i = 0; i < count; i++) {
                var tile = grid.RandomTile();

                var direction = allDirections.RandomElement();

                while (!GameUtils.IsPossibleSwipeDirection(grid, tile, direction)) {
                    direction = GetOppositeDirection(direction);
                }

                _gameController.SwipeTile(tile, direction);
            }

            _gameConfig.UseAnimations = animationValue;
        }

        public IEnumerator RandomMovesWithAnimation(int count = 1) {
            var grid = _gameController.Grid;
            var animationValue = _gameConfig.UseAnimations;
            _gameConfig.UseAnimations = true;

            var allDirections = (Direction[])Enum.GetValues(typeof(Direction));
            var interval = new WaitForSeconds(0.1f);

            for (int i = 0; i < count; i++) {
                var tile = grid.RandomTile();

                var direction = allDirections.RandomElement();

                while (!GameUtils.IsPossibleSwipeDirection(grid, tile, direction)) {
                    direction = GetOppositeDirection(direction);
                }

                _gameController.SwipeTile(tile, direction);

                while (_gameController.IsProcessing.Value) {
                    yield return null;
                }

                yield return interval;
            }

            _gameConfig.UseAnimations = animationValue;
        }

        private Direction GetOppositeDirection(Direction direction) {
            switch (direction) {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
            }

            return direction;
        }
    }
}