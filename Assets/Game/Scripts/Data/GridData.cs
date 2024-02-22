using System.Collections.Generic;
using Game.Objects;
using UnityEngine;

namespace Game.Data {
    public class GridData {
        private readonly Tile[,] _matrix;
        private readonly Grid _sceneGrid;

        public int Columns { get; }
        public int Rows { get; }
        public float CellSize { get; }

        public readonly List<Vector2Int> DestroyedPoints = new();
        public readonly List<Tile> LastUpdatedTiles = new();

        public GridData(int columns, int rows, float cellSize, Vector2 startPoint, Grid sceneGrid) {
            Columns = columns;
            Rows = rows;
            CellSize = cellSize;
            _sceneGrid = sceneGrid;
            _matrix = new Tile[columns, rows];
            _sceneGrid.transform.position = startPoint - new Vector2(cellSize / 2, cellSize / 2);
            _sceneGrid.cellSize = Vector3.one * cellSize;
        }

        public Tile this[int x, int y] {
            get => _matrix[x, y];
            set => _matrix[x, y] = value;
        }

        public Vector2 this[Vector3Int point] => _sceneGrid.GetCellCenterWorld(point);

        public Tile this[Vector2Int point] {
            get => _matrix[point.x, point.y];
            set => _matrix[point.x, point.y] = value;
        }

        public Tile RandomTile() {
            return _matrix[Random.Range(0, Columns), Random.Range(0, Rows)];
        }
    }
}