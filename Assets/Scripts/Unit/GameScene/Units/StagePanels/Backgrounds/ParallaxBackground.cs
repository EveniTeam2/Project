using Unit.GameScene.Units.Creatures.Units.Characters;
using UnityEngine;

namespace Unit.GameScene.Units.StagePanels.Backgrounds
{
    /// <summary>
    ///     패럴럭스 배경을 처리하는 클래스입니다.
    /// </summary>
    public class ParallaxBackground : MonoBehaviour
    {
        private Transform _character;

        private float _spriteLength, _startPosition;
        private float _parallaxEffectMultiplier;
        private void FixedUpdate()
        {
            ApplyParallaxEffect();
            AdjustStartPosition();
        }

        /// <summary>
        ///     배경의 초기 위치와 길이를 초기화합니다.
        /// </summary>
        public void InitializeBackground(Character character, float parallaxEffectMultiplier)
        {
            _parallaxEffectMultiplier = parallaxEffectMultiplier * -1;
            _startPosition = transform.position.x;
            _spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
            _character = character.transform;
        }

        /// <summary>
        ///     캐릭터의 이동에 따라 패럴럭스 효과를 적용합니다.
        /// </summary>
        private void ApplyParallaxEffect()
        {
            var cameraDisplacement = _character.position.x * _parallaxEffectMultiplier;
            transform.position = new Vector3(_startPosition + cameraDisplacement, transform.position.y, transform.position.z);
        }

        /// <summary>
        ///     배경이 반복되도록 시작 위치를 조정합니다.
        /// </summary>
        private void AdjustStartPosition()
        {
            var cameraMovement = _character.position.x * (1 - _parallaxEffectMultiplier);

            if (cameraMovement > _startPosition + _spriteLength) _startPosition += _spriteLength;
            else if (cameraMovement < _startPosition - _spriteLength) _startPosition -= _spriteLength;
        }
    }
}