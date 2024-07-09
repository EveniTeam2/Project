using ScriptableObjects.Scripts.Blocks;
using System;
using TMPro;
using Unit.Blocks.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unit.Blocks
{
    public class Block : MonoBehaviour, IDraggable
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
            Debug.Log($"{_startPosition}에서 드래그 시작");
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            var endPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            endPosition.z = 0;

            var direction = (endPosition - _startPosition).normalized;
            direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(Mathf.Sign(direction.x), 0, 0) : new Vector3(0, Mathf.Sign(direction.y), 0);

            Debug.Log($"{_startPosition}에서 {endPosition}으로 드래그, 드래그 방향 {direction}");
            OnMatchCheck?.Invoke(_startPosition, direction);
        }
    }
}