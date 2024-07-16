using UnityEngine;

namespace Unit.GameScene.Stages.Backgrounds
{
    /// <summary>
    ///     패럴럭스 배경을 처리하는 클래스입니다.
    /// </summary>
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] [Tooltip("배율이 높을수록 카메라보다 빠르게 움직입니다. 배율이 낮을수록 카메라보다 느리게 움직입니다.")]
        private float parallaxEffectMultiplier;

        private Camera _mainCamera;

        private float _spriteLength, _startPosition;

        private void FixedUpdate()
        {
            ApplyParallaxEffect();
            AdjustStartPosition();
        }

        /// <summary>
        ///     배경의 초기 위치와 길이를 초기화합니다.
        /// </summary>
        public void InitializeBackground(Camera mainCamera)
        {
            parallaxEffectMultiplier *= -1;
            _startPosition = transform.position.x;
            _spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
            _mainCamera = mainCamera;
        }

        /// <summary>
        ///     카메라의 이동에 따라 패럴럭스 효과를 적용합니다.
        /// </summary>
        private void ApplyParallaxEffect()
        {
            var cameraDisplacement = _mainCamera.transform.position.x * (1 + parallaxEffectMultiplier);
            transform.position = new Vector3(_startPosition + cameraDisplacement, transform.position.y,
                transform.position.z);
        }

        /// <summary>
        ///     배경이 반복되도록 시작 위치를 조정합니다.
        /// </summary>
        private void AdjustStartPosition()
        {
            var cameraMovement = _mainCamera.transform.position.x * (1 - parallaxEffectMultiplier);

            if (cameraMovement > _startPosition + _spriteLength)
                _startPosition += _spriteLength;
            else if (cameraMovement < _startPosition - _spriteLength) _startPosition -= _spriteLength;
        }
    }
}