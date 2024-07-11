using UnityEngine.EventSystems;

namespace Unit.Blocks.Interfaces
{
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}