using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    public interface IDamageable {
        void Damage(int dmg);
        int GetHealth();
    }

    public interface IStatConvertable {
        PlayerStat GetPlayerStat();
    }

    [Serializable]
    public struct PlayerStat {
        public int Health;
        public float Speed;
    }

    public abstract class  BaseCharacter : MonoBehaviour {
        
    }
}