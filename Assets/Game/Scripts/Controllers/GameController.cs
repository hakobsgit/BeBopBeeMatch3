using Game.Configs;
using Game.Data;
using Game.Data.Enums;
using Game.Extensions;
using Game.Objects;
using Game.Pools;
using Game.Processors;
using Game.Utils;
using Game.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Controllers {
    public class GameController : IGameController, IInitializable {
        [Inject] private DiContainer _diContainer;
        [Inject] private GameView _gameView;
        [Inject] private GameConfig _gameConfig;
        [Inject] private LevelConfig _levelConfig;
        [Inject] private TilesConfig _defaultTilesConfig;
        [Inject] private TilePool _tilePool;

        private IMatchProcessor _matchProcessor;
        private IMoveProcessor _moveProcessor;
        private TilesConfig _tilesConfig;
        private GridData _grid;
        private float _tileScale;
        private float _topY;

        public ReactiveProperty<bool> IsProcessing { get; } = new();
        public GridData Grid => _grid;

        public void Initialize() {
            var processorsContainer = _diContainer.CreateSubContainer();
            switch (_gameConfig.MatchType) {
                case MatchType.SwappedTiles:
                    processorsContainer.Bind<IMatchProcessor>().To<SwappedMatchProcessor>().AsSingle().NonLazy();
                    break;
                case MatchType.FullGrid:
                    processorsContainer.Bind<IMatchProcessor>().To<FullGridMatchProcessor>().AsSingle().NonLazy();
                    break;
            }

            processorsContainer.Bind<IRefillProcessor>().To<RefillProcessor>().AsSingle().NonLazy();
            processorsContainer.Bind<IDestroyProcessor>().To<DestroyProcessor>().AsSingle().NonLazy();
            processorsContainer.Bind<IMoveProcessor>().To<MoveProcessor>().AsSingle().NonLazy();
            _matchProcessor = processorsContainer.Resolve<IMatchProcessor>();
            _moveProcessor = processorsContainer.Resolve<IMoveProcessor>();
            CreatLevel();
        }

        public Tile AddTile(int x, int y, float animationDelay, Vector2 position, bool forceCreateNew = false,
            bool noMatch3 = true) {
            var tileData = _tilesConfig.Tiles.RandomElement();
            if (noMatch3) {
                while ((x > 1 && tileData == _grid[x - 1, y]?.Data && tileData == _grid[x - 2, y]?.Data) ||
                       (y > 1 && tileData == _grid[x, y - 1]?.Data && tileData == _grid[x, y - 2]?.Data)) {
                    tileData = _tilesConfig.Tiles.RandomElement();
                }
            }

            var tile = _tilePool.GetTile(tileData, forceCreateNew);
            tile.GridPoint = new Vector2Int(x, y);
            tile.SetStartPosition(_topY, animationDelay, position, _moveProcessor);
            tile.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
            tile.transform.localScale = Vector3.one * _tileScale;
            _grid[x, y] = tile;
            return tile;
        }

        public void SwipeTile(Tile tile, Direction direction) {
            if (IsProcessing.Value || !GameUtils.IsPossibleSwipeDirection(_grid, tile, direction)) {
                return;
            }

            Tile swapTile = null;

            switch (direction) {
                case Direction.Up:
                    swapTile = _grid[tile.GridPoint.x, tile.GridPoint.y + 1];
                    break;
                case Direction.Down:
                    swapTile = _grid[tile.GridPoint.x, tile.GridPoint.y - 1];
                    break;
                case Direction.Left:
                    swapTile = _grid[tile.GridPoint.x - 1, tile.GridPoint.y];
                    break;
                case Direction.Right:
                    swapTile = _grid[tile.GridPoint.x + 1, tile.GridPoint.y];
                    break;
            }

            SwapTiles(tile, swapTile);
        }

        private void CreatLevel() {
            _tilesConfig = _levelConfig.OverrideTilesConfig ? _levelConfig.OverrideTilesConfig : _defaultTilesConfig;

            var startX = _levelConfig.Columns / 2f * -_levelConfig.CellSize + _levelConfig.CellSize / 2;
            var startY = _levelConfig.Rows / 2f * -_levelConfig.CellSize + _levelConfig.CellSize / 2;
            _topY = startY + _levelConfig.Rows * _levelConfig.CellSize;
            _tileScale = _levelConfig.CellSize - 0.1f;
            _grid = new GridData(_levelConfig.Columns, _levelConfig.Rows, _levelConfig.CellSize,
                new Vector2(startX, startY), _gameView.SceneGrid);

            var intervalBetweenRowAnimations = 0.2f;
            var animationDelay = _levelConfig.Rows * intervalBetweenRowAnimations;
            for (int x = 0; x < _levelConfig.Columns; x++) {
                for (int y = 0; y < _levelConfig.Rows; y++) {
                    var position = new Vector2(startX + x * _levelConfig.CellSize, startY + y * _levelConfig.CellSize);
                    var tileContainer = _diContainer.InstantiatePrefab(_gameView.TileContainerPrefab, position,
                        Quaternion.identity, _gameView.TileContainersParent);

                    tileContainer.transform.localScale = Vector3.one * _tileScale;

                    var tile = AddTile(x, y, animationDelay, position, true);
                    tile.SetMaskInteraction(SpriteMaskInteraction.None);
                }

                animationDelay -= intervalBetweenRowAnimations;
            }

            _gameView.Panel.size = new Vector2(_levelConfig.Columns * _levelConfig.CellSize + 0.4f,
                _levelConfig.Rows * _levelConfig.CellSize + 0.5f);
            _gameView.Mask.transform.localScale = new Vector2(_levelConfig.Columns * _levelConfig.CellSize,
                _levelConfig.Rows * _levelConfig.CellSize);
        }

        private void SwapTiles(Tile tile1, Tile tile2) {
            IsProcessing.Value = true;
            var tile1NewGridPoint = tile2.GridPoint;
            var tile2NewGridPoint = tile1.GridPoint;
            var tile1Destination = tile2.transform.position;
            var tile2Destination = tile1.transform.position;
            _grid[tile1NewGridPoint] = tile1;
            _grid[tile2NewGridPoint] = tile2;
            tile1.GridPoint = tile1NewGridPoint;
            tile2.GridPoint = tile2NewGridPoint;
            _moveProcessor.Move(tile1, tile1Destination, 0.25f);
            _moveProcessor.Move(tile2, tile2Destination, 0.25f, () => {
                if (_matchProcessor.Process(_grid, tile1, tile2)) {
                    return;
                }

                ReturnBack();
            });

            void ReturnBack() {
                _moveProcessor.Move(tile1, tile2Destination, 0.25f, () => IsProcessing.Value = false);
                _moveProcessor.Move(tile2, tile1Destination, 0.25f);
                _grid[tile1NewGridPoint] = tile2;
                _grid[tile2NewGridPoint] = tile1;
                tile1.GridPoint = tile2NewGridPoint;
                tile2.GridPoint = tile1NewGridPoint;
            }
        }
    }
}