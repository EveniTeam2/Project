using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.Boards.Blocks.Enums;
using Unit.Boards.Blocks.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unit.Boards.Blocks
{
    /// <summary>
    /// 게임 내 블록을 나타내며, 드래그하여 다른 블록과 위치를 교환할 수 있습니다.
    /// </summary>
    public class Block : MonoBehaviour, IDraggable, IBlock
    {
        public event Action<Vector3, Vector3> OnMatchCheck;

        [FormerlySerializedAs("blockInfo")] [SerializeField] private BlockSo blockSoInfo;
        [SerializeField] private Image blockImage;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _startPosition;

        public BlockType Type { get; private set; }

        public void Initialize(BlockSo info, Action<Vector3, Vector3> matchCheckHandler, Canvas canvas)
        {
            blockSoInfo = info;
            textMeshPro.text = blockSoInfo.text;
            blockImage.color = blockSoInfo.color;
            Type = blockSoInfo.type;
            OnMatchCheck = matchCheckHandler;
            _canvas = canvas;

            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, _canvas.worldCamera, out _startPosition);
            Debug.Log("드래그 시작");
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("드래그 종료");
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, _canvas.worldCamera, out var localPoint);
            var direction = (localPoint - _startPosition).normalized;
            direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(Mathf.Sign(direction.x), 0, 0) : new Vector3(0, Mathf.Sign(direction.y), 0);

            Debug.Log($"타겟 블록 좌표 {_rectTransform.anchoredPosition} / 드래그 방향 : {direction}");
            OnMatchCheck?.Invoke(_rectTransform.anchoredPosition, direction);
        }
    }
}