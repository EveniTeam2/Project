using System;

namespace Unit.Character {
    public class PlayerCharacter : BaseCharacter, IDamageable {
        PlayerStat stat;

        public void Damage(int dmg) {
            stat.Health -= dmg;

        }

        public int GetHealth() {
            throw new NotImplementedException();
        }
    }
}