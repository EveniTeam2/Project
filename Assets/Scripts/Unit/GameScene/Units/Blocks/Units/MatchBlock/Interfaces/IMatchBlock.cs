using System;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Unit.GameScene.Units.Blocks.Units.MatchBlock.Interfaces
{
    /// <summary>
    ///     블록 인터페이스
    /// </summary>
    public interface IMatchBlock
    {
        void Initialize(BlockModel info, Action<Vector3, Vector3> matchCheckHandler, Canvas canvas);
    }
}