using UnityEngine.EventSystems;

namespace Unit.GameScene.Boards.Blocks.Interfaces
{
    /// <summary>
    /// 드래그 가능한 블록 인터페이스
    /// </summary>
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}