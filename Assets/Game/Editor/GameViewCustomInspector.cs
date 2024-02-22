using System;
using Game.Views;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
    [CustomEditor(typeof(GameView))]
    public class GameViewCustomInspector : UnityEditor.Editor {
        private IDisposable _simulationDisposable;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var gameView = (GameView)target;
            if (gameView.SimulationController == null) {
                return;
            }

            if (_simulationDisposable != null) {
                if (GUILayout.Button("Stop")) {
                    _simulationDisposable?.Dispose();
                    _simulationDisposable = null;
                }

                return;
            }

            if (GUILayout.Button("Simulate 1M Immediate")) {
                gameView.SimulationController.RandomMoves(1000000);
            }

            if (GUILayout.Button("Simulate 1M Animated")) {
                _simulationDisposable = Observable
                    .FromCoroutine(_ => gameView.SimulationController.RandomMovesWithAnimation(1000000))
                    .Subscribe();
            }
        }
    }
}