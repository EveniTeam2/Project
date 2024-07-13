using UnityEngine.EventSystems;

namespace Unit.Boards.Blocks.Interfaces
{
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}