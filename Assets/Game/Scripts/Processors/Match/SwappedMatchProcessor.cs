using System.Collections.Generic;
using Game.Data;
using Game.Objects;

namespace Game.Processors {
    public class SwappedMatchProcessor : BaseMatchProcessor, IMatchProcessor {
        private List<Tile> _matches = new();

        public bool Process(GridData grid, Tile swapTile1, Tile swapTile2) {
            _grid = grid;

            _matches.Clear();
            _matches.AddRange(FindMatchesAtTile(swapTile1));
            _matches.AddRange(FindMatchesAtTile(swapTile2));

            if (_matches.Count <= 0) return false;

            for (int i = 0; i < _matches.Count; i++) {
                _grid.DestroyedPoints.Add(_matches[i].GridPoint);
            }

            _destroyProcessor.Destroy(_grid);
            return true;
        }
    }
}