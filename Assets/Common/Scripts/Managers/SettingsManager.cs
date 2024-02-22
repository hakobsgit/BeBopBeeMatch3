using Zenject;

namespace Common.Managers {
    public class SettingsManager : IInitializable {
        public void Initialize() {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = 60;
#endif
        }
    }
}