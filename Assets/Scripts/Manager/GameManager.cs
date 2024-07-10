using System.Collections.Generic;
using Core.Utils;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Manager
{
    public class GameManager : UnitySingleton<GameManager>
    {
        [Header("보드 가로 세로 길이")]
        public int boardWidth;
        public int boardHeight;

        [Header("타일 정보")]
        public List<NewBlock> blockInfos;

        protected override GameManager Initialize()
        {
            return this;
        }
    }
}
