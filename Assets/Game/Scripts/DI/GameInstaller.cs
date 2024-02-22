using Game.Configs;
using Game.Controllers;
using Game.Pools;
using Game.Views;
using UnityEngine;
using Zenject;

namespace Game.DI {
    public class GameInstaller : MonoInstaller {
        [SerializeField] private GameView _gameView;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private TilesConfig _tilesConfig;

        public override void InstallBindings() {
            InstallViews();
            InstallConfigs();
            InstallPools();
            InstallControllers();
        }

        private void InstallViews() {
            Container.Bind<GameView>().FromInstance(_gameView).AsSingle();
        }

        private void InstallConfigs() {
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            Container.Bind<TilesConfig>().FromInstance(_tilesConfig).AsSingle();
        }

        private void InstallPools() {
            Container.Bind<TilePool>().AsSingle();
        }

        private void InstallControllers() {
            Container.BindInterfacesTo<GameController>().AsSingle();
            Container.BindInterfacesTo<InputController>().AsSingle();
            Container.BindInterfacesTo<SimulationController>().AsSingle();
        }
    }
}