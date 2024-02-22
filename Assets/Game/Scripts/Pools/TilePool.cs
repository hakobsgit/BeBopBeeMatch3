using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Controllers;
using Game.Data;
using Game.Data.Enums;
using Game.Objects;
using Game.Views;
using UniRx;
using Zenject;

namespace Game.Pools {
    public class TilePool {
        [Inject] private DiContainer _diContainer;
        [Inject] private GameView _gameView;
        [Inject] private TilesConfig _defaultTilesConfig;
        [Inject] private IInputController _inputController;

        private readonly List<Tile> _tiles = new();

        public Tile GetTile(TileData data, bool forceCreateNew = false) {
            Tile tile = null;
            if (!forceCreateNew) {
                for (int i = 0; i < _tiles.Count; i++) {
                    var t = _tiles[i];
                    if (!t.gameObject.activeSelf && t.Data == data) {
                        tile = t;
                        break;
                    }
                }
            }

            if (!tile) {
                tile = CreateNewTile(data);
                tile.SetTileForInput.Subscribe(_inputController.SetTile);
                _tiles.Add(tile);
            }
            else {
                tile.gameObject.SetActive(true);
            }

            return tile;
        }

        private Tile CreateNewTile(TileData data) {
            var tile = _diContainer.InstantiatePrefab(_gameView.TilePrefab, _gameView.TilesParent).GetComponent<Tile>();
            tile.Init(data, _defaultTilesConfig);
            return tile;
        }
    }
}