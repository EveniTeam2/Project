using UnityEngine;
using UnityEngine.EventSystems;

namespace Unit.Blocks.Interface
{
    public interface IDraggable : IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
    }
}