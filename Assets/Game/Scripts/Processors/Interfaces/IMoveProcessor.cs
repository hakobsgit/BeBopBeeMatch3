using System;
using Game.Objects;
using UnityEngine;

namespace Game.Processors {
    public interface IMoveProcessor {
        void Move(Tile tile, Vector3 pos, float duration, Action onComplete = null, float delay = 0);
        void Jump(Tile tile, Vector3 pos, float duration, Action onComplete = null, float delay = 0);
        void Rotate(Tile tile, Vector3 rotation, float duration, Action onComplete = null, float delay = 0);
    }
}