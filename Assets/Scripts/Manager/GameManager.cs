using System.Collections.Generic;
using Core.Utils;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Manager
{
    public class GameManager : UnitySingleton<GameManager>
    {
        protected override GameManager Initialize()
        {
            return this;
        }
    }
}
