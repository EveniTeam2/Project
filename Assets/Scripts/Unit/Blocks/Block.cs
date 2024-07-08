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

        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TextMeshPro text;

        public BlockType Type { get; private set; }

        private Vector3 _startPosition;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Initialize(NewBlock info, Action<Vector3, Vector3> matchCheckHandler)
        {
            text.text = info.text;
            sprite.color = info.color;
            Type = info.type;

            OnMatchCheck = matchCheckHandler;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = transform.position;
            Debug.Log($"블록 클릭 {_startPosition}");
        }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            var endPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            endPosition.z = 0;

            var direction = (endPosition - _startPosition).normalized;
            direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(Mathf.Sign(direction.x), 0, 0) : new Vector3(0, Mathf.Sign(direction.y), 0);

            OnMatchCheck?.Invoke(_startPosition, direction);

            Debug.Log($"블록 {_startPosition} 클릭 이후 {direction} 방향으로 드래그");
        }
    }
}