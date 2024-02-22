using System.Collections.Generic;
using Game.Controllers;
using Game.Data;
using Game.Objects;
using UniRx;
using Zenject;

namespace Game.Processors {
    public class FullGridMatchProcessor : BaseMatchProcessor, IMatchProcessor {
        [Inject] private IGameController _gameController;

        private List<Tile> _matches = new();

        public FullGridMatchProcessor(IRefillProcessor refillProcessor) {
            refillProcessor?.OnRefilled.Subscribe(_ => OnRefilled());
        }

        public bool Process(GridData grid, Tile swapTile1, Tile swapTile2) {
            _grid = grid;

            _matches.Clear();
            _matches.AddRange(FindMatchesAtTile(swapTile1));
            _matches.AddRange(FindMatchesAtTile(swapTile2));

            if (_matches.Count == 0) return false;

            Destroy();

            return true;
        }

        public bool IsAnyMatch(GridData grid) {
            _grid = grid;

            _matches.Clear();

            for (int x = 0; x < grid.Columns; x++) {
                for (int y = 0; y < grid.Rows; y++) {
                    _matches.AddRange(FindMatchesAtTile(grid[x, y]));
                }
            }

            if (_matches.Count == 0) return false;

            return true;
        }

        private void ProcessRefilledTiles() {
            if (_grid.LastUpdatedTiles.Count == 0) {
                _gameController.IsProcessing.Value = false;
                return;
            }

            _matches.Clear();

            foreach (var tile in _grid.LastUpdatedTiles) {
                _matches.AddRange(FindMatchesAtTile(tile));
            }

            if (_matches.Count == 0) {
                _gameController.IsProcessing.Value = false;
                return;
            }

            Destroy();
        }

        private void Destroy() {
            foreach (var t in _matches) {
                _grid.DestroyedPoints.Add(t.GridPoint);
            }

            _destroyProcessor.Destroy(_grid);
        }

        private void OnRefilled() {
            ProcessRefilledTiles();
        }
    }
}