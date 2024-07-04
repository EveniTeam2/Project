using UnityEngine;

namespace Unit.Character {
    public class NormalSkinData : ScriptableObject, IStatConvertable {
        public Sprite PreViewSprite => preViewSprite;
        public PlayerStat Stat => stat;

        [SerializeField] protected PlayerStat stat;
        [SerializeField] protected Sprite preViewSprite;

        public PlayerStat GetPlayerStat() {
            return Stat;
        }
    }
}