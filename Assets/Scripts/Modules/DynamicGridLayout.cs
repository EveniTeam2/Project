using System;
using UnityEngine.Serialization;

namespace Modules
{
    using UnityEngine;
    using UnityEngine.UI;

    public enum LayoutControl
    {
        None,
        WidthControlsHeight,
        HeightControlsWidth
    }

    public class DynamicGridLayout : MonoBehaviour
    {
        public GridLayoutGroup gridLayoutGroup;
        public int columns; // 열의 개수
        public int rows;    // 행의 개수

        public LayoutControl layoutControl;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (gridLayoutGroup == null)
            {
                gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }

            UpdateCellSize();
        }

        private void UpdateCellSize()
        {
            if (gridLayoutGroup == null || _rectTransform == null)
            {
                return;
            }

            // 부모 객체의 너비와 높이
            float parentWidth = _rectTransform.rect.width;
            float parentHeight = _rectTransform.rect.height;

            float cellWidth;
            float cellHeight;

            switch (layoutControl)
            {
                case LayoutControl.None:
                    cellWidth = (parentWidth - gridLayoutGroup.spacing.x * (columns - 1) - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right) / columns;
                    cellHeight = (parentHeight - gridLayoutGroup.spacing.y * (rows - 1) - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom) / rows;
                    break;
                case LayoutControl.WidthControlsHeight:
                    cellWidth = (parentWidth - gridLayoutGroup.spacing.x * (columns - 1) - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right) / columns;
                    cellHeight = cellWidth;
                    break;
                case LayoutControl.HeightControlsWidth:
                    cellHeight = (parentHeight - gridLayoutGroup.spacing.y * (rows - 1) - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom) / rows;
                    cellWidth = cellHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // 셀 크기 설정
            gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
        }
    }
}