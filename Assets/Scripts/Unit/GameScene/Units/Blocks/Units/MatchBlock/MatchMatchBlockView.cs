using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Boards.Blocks.Interfaces;
using Unit.GameScene.Units.Blocks.Abstract;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Blocks.Units.MatchBlock
{
    /// <summary>
    ///     게임 내 블록을 나타내며, 드래그하여 다른 블록과 위치를 교환할 수 있습니다.
    /// </summary>
    public class MatchMatchBlockView : BlockView, IDraggable
    {
        public event Action<Vector3, Vector3> OnMatchCheck;
        
        [SerializeField] private TextMeshProUGUI temp;

        private BlockModel _blockModel;
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private Vector2 _startPosition;

        public void Initialize(BlockModel info, Action<Vector3, Vector3> matchCheckHandler, Canvas canvas)
        {
            OnMatchCheck = matchCheckHandler;
            _canvas = canvas;
            _blockModel = info;
            temp.text = info.text;
            blockBackground.sprite = _blockModel.background;
            Type = _blockModel.type;

            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, _canvas.worldCamera, out _startPosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                eventData.position, _canvas.worldCamera, out var localPoint);
            var direction = (localPoint - _startPosition).normalized;
            direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? new Vector3(Mathf.Sign(direction.x), 0, 0)
                : new Vector3(0, Mathf.Sign(direction.y), 0);
            
            OnMatchCheck?.Invoke(_rectTransform.anchoredPosition, direction);
        }
    }
}