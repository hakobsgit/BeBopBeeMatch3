using System;
using DG.Tweening;
using Game.Configs;
using Game.Objects;
using UnityEngine;
using Zenject;

namespace Game.Processors {
    public class MoveProcessor : IMoveProcessor {
        [Inject] private GameConfig _gameConfig;

        public void Move(Tile tile, Vector3 pos, float duration, Action onComplete = null, float delay = 0) {
            if (!_gameConfig.UseAnimations) {
                tile.transform.position = pos;
                onComplete?.Invoke();
                return;
            }

            tile.transform.DOMove(pos, duration).SetDelay(delay).OnComplete(() => onComplete?.Invoke());
        }

        public void Jump(Tile tile, Vector3 pos, float duration, Action onComplete = null, float delay = 0) {
            if (!_gameConfig.UseAnimations) {
                tile.transform.position = pos;
                onComplete?.Invoke();
                return;
            }

            tile.transform.DOJump(pos, 2, 1, duration).SetDelay(delay).SetEase(Ease.OutBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void Rotate(Tile tile, Vector3 rotation, float duration, Action onComplete = null, float delay = 0) {
            if (!_gameConfig.UseAnimations) {
                tile.transform.localEulerAngles = rotation;
                onComplete?.Invoke();
                return;
            }

            tile.transform.DOLocalRotate(rotation, duration).SetEase(Ease.InOutSine).SetRelative().SetDelay(delay)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}