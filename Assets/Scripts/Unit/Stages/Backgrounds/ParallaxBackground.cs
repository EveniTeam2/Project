using System;
using UnityEngine;

namespace Unit.Stages.Backgrounds
{
    /// <summary>
    /// 패럴럭스 배경을 처리하는 클래스입니다.
    /// </summary>
    public class ParallaxBackground : MonoBehaviour
    {
        private float _spriteLength, _startPosition;
        public GameObject mainCamera;
        public float parallaxEffectMultiplier;

        private void Awake()
        {
            InitializeBackground();
        }

        /// <summary>
        /// 배경의 초기 위치와 길이를 초기화합니다.
        /// </summary>
        public void InitializeBackground()
        {
            _startPosition = transform.position.x;
            _spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;

            // z 위치를 기반으로 패럴럭스 효과를 조정합니다.
            parallaxEffectMultiplier = (transform.position.z + 2) * 0.1f;
        }

        private void FixedUpdate()
        {
            ApplyParallaxEffect();
            AdjustStartPosition();
        }

        /// <summary>
        /// 카메라의 이동에 따라 패럴럭스 효과를 적용합니다.
        /// </summary>
        private void ApplyParallaxEffect()
        {
            var cameraDisplacement = mainCamera.transform.position.x * parallaxEffectMultiplier;
            transform.position = new Vector3(_startPosition + cameraDisplacement, transform.position.y, transform.position.z);
        }

        /// <summary>
        /// 배경이 반복되도록 시작 위치를 조정합니다.
        /// </summary>
        private void AdjustStartPosition()
        {
            var cameraMovement = mainCamera.transform.position.x * (1 - parallaxEffectMultiplier);

            if (cameraMovement > _startPosition + _spriteLength)
            {
                _startPosition += _spriteLength;
            }
            else if (cameraMovement < _startPosition - _spriteLength)
            {
                _startPosition -= _spriteLength;
            }
        }
    }
}