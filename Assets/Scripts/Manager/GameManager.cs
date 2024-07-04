using System;
using System.Collections.Generic;
using Core.Utils;
using UnityEngine;

namespace Manager
{
    public class GameManager : UnitySingleton<GameManager>
    {
        [Header("보드 가로 세로 길이")]
        public int boardWidth;
        public int boardHeight;
        
        [Header("타일 정보")]
        public List<ScriptableObject> tileInfos;

        protected override GameManager Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
