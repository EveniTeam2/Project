using ScriptableObjects.Scripts.Blocks;
using System;
using TMPro;
using Unit.Blocks.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unit.Blocks
{
    /// <summary>
    /// 게임 내 블록을 나타내며, 드래그하여 다른 블록과 위치를 교환할 수 있습니다.
    /// </summary>
    public class Block : MonoBehaviour, IDraggable, IBlock
    {
        public event Action<Vector3, Vector3> OnMatchCheck;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro textMeshPro;

        public BlockType Type { get; private set; }

        private Vector3 _startPosition;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// 블록을 지정된 정보로 초기화합니다.
        /// </summary>
        /// <param name="info">초기화할 블록 정보입니다.</param>
        /// <param name="matchCheckHandler">매치 검사를 처리할 핸들러입니다.</param>
        public void Initialize(NewBlock info, Action<Vector3, Vector3> matchCheckHandler)
        {
            textMeshPro.text = info.text;
            spriteRenderer.color = info.color;
            Type = info.type;
            OnMatchCheck = matchCheckHandler;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            var endPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            endPosition.z = 0;

            var direction = (endPosition - _startPosition).normalized;
            direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? new Vector3(Mathf.Sign(direction.x), 0, 0)
                : new Vector3(0, Mathf.Sign(direction.y), 0);

            OnMatchCheck?.Invoke(transform.localPosition, direction);
        }
    }
}