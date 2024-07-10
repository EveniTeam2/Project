using UnityEngine.EventSystems;

namespace Unit.Blocks.Interface
{
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}