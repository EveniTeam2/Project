using System;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Unit.Boards.Blocks.Interfaces
{
    public interface IBlock
    {
        // BlockType Type { get; }
        void Initialize(NewBlock info, Action<Vector3, Vector3> matchCheckHandler);
    }
}