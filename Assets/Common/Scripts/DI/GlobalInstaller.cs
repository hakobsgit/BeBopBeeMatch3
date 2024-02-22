using Common.Managers;
using Zenject;

namespace Common.Scripts.DI {
    public class GlobalInstaller : MonoInstaller {
        public override void InstallBindings() {
            InstallManagers();
        }

        private void InstallManagers() {
            Container.BindInterfacesTo<SettingsManager>().AsSingle();
        }
    }
}