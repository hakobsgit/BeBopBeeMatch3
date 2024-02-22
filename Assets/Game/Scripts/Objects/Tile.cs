using System;
using Game.Configs;
using Game.Data;
using Game.Extensions;
using Game.Processors;
using UniRx;
using UnityEngine;

namespace Game.Objects {
    public class Tile : MonoBehaviour {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private IDisposable _updateDisposable;
        private Vector2 _position;

        public ReactiveCommand<Tile> SetTileForInput { get; } = new();
        public ReactiveCommand OnAnimationComplete { get; } = new();
        public TileData Data { get; private set; }
        public Vector2Int GridPoint { get; set; }

        public void Init(TileData data, TilesConfig defaultTilesConfig) {
            Data = data;
            if (data.Prefab) {
                _spriteRenderer.enabled = false;
                Instantiate(data.Prefab, transform);
            }
            else if (data.Sprite) {
                _spriteRenderer.sprite = data.Sprite;
            }
            else {
                _spriteRenderer.sprite = defaultTilesConfig.DefaultSprite;
            }

            if (data.Color != default) {
                _spriteRenderer.color = data.Color;
            }
        }

        public void SetMaskInteraction(SpriteMaskInteraction interaction) {
            _spriteRenderer.maskInteraction = interaction;
        }

        public void SetStartPosition(float animationStartY, float animationDelay, Vector2 position,
            IMoveProcessor moveProcessor) {
            _position = position;
            transform.position = position.SetY(animationStartY + _spriteRenderer.size.y);
            AnimateToPosition(animationDelay, moveProcessor);
        }

        private void AnimateToPosition(float delay, IMoveProcessor moveProcessor) {
            moveProcessor.Jump(this, _position, 0.5f, AnimationComplete, delay);
            moveProcessor.Rotate(this, Vector3.forward * 360, 0.5f);
        }

        private void AnimationComplete() {
            SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
            OnAnimationComplete.Execute();
        }

        private void OnMouseDown() {
            SetTileForInput.Execute(this);
        }

        private void OnMouseUp() {
            SetTileForInput.Execute(null);
        }
    }
}