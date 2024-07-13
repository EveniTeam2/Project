using System;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Unit.Boards.Blocks.Interfaces
{
    public interface IBlock
    {
        void Initialize(NewBlock info, Action<Vector3, Vector3> matchCheckHandler, Canvas canvas);
    }
}