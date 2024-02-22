using System.Collections.Generic;
using Game.Configs;
using Game.Controllers;
using Game.Data;
using Game.Data.Enums;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Processors {
    public class RefillProcessor : IRefillProcessor {
        [Inject] private IGameController _gameController;
        [Inject] private IMoveProcessor _moveProcessor;
        [Inject] private GameConfig _gameConfig;

        private HashSet<int> _refillingColumns = new();
        private GridData _grid;

        public ReactiveCommand OnRefilled { get; } = new();

        public void Refill(GridData grid) {
            _grid = grid;
            _grid.LastUpdatedTiles.Clear();
            _refillingColumns.Clear();
            var animationDelay = 0f;
            var maxDestroyedColumn = 0;

            for (int i = 0; i < _grid.DestroyedPoints.Count; i++) {
                _refillingColumns.Add(_grid.DestroyedPoints[i].x);
            }

            foreach (var column in _refillingColumns) {
                var delay = FillColumn(column);
                if (delay > animationDelay) {
                    animationDelay = delay;
                    maxDestroyedColumn = column;
                }
            }

            _grid.DestroyedPoints.Clear();

            if (_gameConfig.UseAnimations) {
                _grid[maxDestroyedColumn, _grid.Rows - 1].OnAnimationComplete.First().Subscribe(_ => Refilled());
            }
            else {
                Refilled();
            }
        }

        private void Refilled() {
            if (_gameConfig.MatchType != MatchType.FullGrid) _gameController.IsProcessing.Value = false;
            OnRefilled.Execute();
        }

        private float FillColumn(int x) {
            var emptyY = FindEmptyYInColumn(x);
            var emptyCount = 0;
            for (int y = emptyY; y < _grid.Rows; y++) {
                var tile = _grid[x, y];
                if (!tile) {
                    emptyCount++;
                    continue;
                }

                for (int newY = emptyY; newY < _grid.Rows; newY++) {
                    if (_grid[x, newY]) continue;
                    _grid[tile.GridPoint] = null;
                    _grid[x, newY] = tile;
                    tile.GridPoint = new Vector2Int(x, newY);
                    var position = _grid[new Vector3Int(x, newY)];
                    _moveProcessor.Move(tile, position, 0.25f);
                    _grid.LastUpdatedTiles.Add(tile);
                    break;
                }

                emptyY = FindEmptyYInColumn(x, emptyY);
            }

            var animationDelay = 0f;
            var animationInterval = 0.1f;
            for (int i = emptyCount; i > 0; i--) {
                var tile = _gameController.AddTile(x, _grid.Rows - i, animationDelay,
                    _grid[new Vector3Int(x, _grid.Rows - i)],
                    false, false);
                _grid.LastUpdatedTiles.Add(tile);
                animationDelay += animationInterval;
            }

            return animationDelay;
        }

        private int FindEmptyYInColumn(int x, int startY = 0) {
            for (int y = startY; y < _grid.Rows; y++) {
                if (!_grid[x, y]) {
                    return y;
                }
            }

            return -1;
        }
    }
}