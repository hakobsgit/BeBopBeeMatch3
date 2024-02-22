using System.Collections.Generic;
using Game.Data;
using Game.Objects;
using UnityEngine;
using Zenject;

namespace Game.Processors {
    public abstract class BaseMatchProcessor {
        [Inject] protected IDestroyProcessor _destroyProcessor;

        protected GridData _grid;

        private List<Tile> _tileMatches = new();
        private List<Tile> _horizontalMatches = new();
        private List<Tile> _verticalMatches = new();
        private List<Tile> _lineMatches = new();

        protected List<Tile> FindMatchesAtTile(Tile tile) {
            var pos = tile.GridPoint;
            _tileMatches.Clear();
            _horizontalMatches.Clear();
            _verticalMatches.Clear();
            if (!tile) return _tileMatches;

            _horizontalMatches.AddRange(FindLineMatches(pos, Vector2Int.right));
            _horizontalMatches.AddRange(FindLineMatches(pos, Vector2Int.left));
            if (_horizontalMatches.Count >= 2) {
                _tileMatches.AddRange(_horizontalMatches);
                _tileMatches.Add(tile);
            }

            _verticalMatches.AddRange(FindLineMatches(pos, Vector2Int.up));
            _verticalMatches.AddRange(FindLineMatches(pos, Vector2Int.down));
            if (_verticalMatches.Count >= 2) {
                if (_horizontalMatches.Count < 2) _tileMatches.Add(tile);
                _tileMatches.AddRange(_verticalMatches);
            }

            return _tileMatches;
        }

        private List<Tile> FindLineMatches(Vector2Int startPos, Vector2Int direction) {
            _lineMatches.Clear();
            var nextX = startPos.x + direction.x;
            var nextY = startPos.y + direction.y;

            while (nextX >= 0 && nextX < _grid.Columns && nextY >= 0 && nextY < _grid.Rows) {
                var nextTile = _grid[nextX, nextY];
                var startTile = _grid[startPos.x, startPos.y];
                if (nextTile && startTile && nextTile.Data == startTile.Data) {
                    _lineMatches.Add(nextTile);
                    nextX += direction.x;
                    nextY += direction.y;
                }
                else {
                    break;
                }
            }

            return _lineMatches;
        }
    }
}