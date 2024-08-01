using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Blocks.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Blocks.Units.MatchBlock
{
    /// <summary>
    ///     게임 내 블록을 나타내며, 드래그하여 다른 블록과 위치를 교환할 수 있습니다.
    /// </summary>
    public class MatchBlockView : BlockView, IDraggable
    {
        public event Action<Vector3, Vector3> OnMatchCheck;
        
        private Canvas _canvas;
        private Vector2 _startPosition;
        private RectTransform _rectTransform;
        
        public void Initialize(BlockType type, Action<Vector3, Vector3> matchCheckHandler, Canvas canvas, CharacterSkill skill, Sprite background)
        {
            Type = type;
            
            OnMatchCheck = matchCheckHandler;

            blockBackground.sprite = background;
            
            if (skill == null && blockIcon.gameObject.activeInHierarchy)
            {
                blockIcon.gameObject.SetActive(false);
            }
            else if (skill != null)
            {
                if (!blockIcon.gameObject.activeInHierarchy)
                {
                    blockIcon.gameObject.SetActive(true);     
                }
                
                blockIcon.sprite = skill.SkillIcon;
            }

            _canvas = canvas;
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