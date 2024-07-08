using System;
using System.Collections.Generic;
using Unit.Blocks;
using Unit.Character;
using UnityEngine;

public interface IRaceable {
    event Action OnFailed;
    event Action<List<Block>> OnSuccess;
}

namespace Unit.Character {
    public interface IShowable {
        void Move(float spd);
        void Stop();
    }

    public struct PlayerStat {
        public int Health;
        public int Speed;
    }

    public struct MonsterStat {
        public int Speed;
    }

    public class Race : MonoBehaviour {
        [SerializeField] PlayerCharacter player;
        [SerializeField] MonsterCharacter monster = null;
        [SerializeField] BackgroundDisplay background;
        public void AttachBoard(IRaceable data) {
            data.OnSuccess += OnSuccessMatch;
            data.OnFailed += OnFailedMatch;
        }
        private void OnFailedMatch() {
            background.Stop();
            
            // TODO
        }
        private void OnSuccessMatch(List<Block> list) {
            background.Move(player.Speed);
            foreach (var block in list) {
                // TODO
            }
        }
        public void Initialize(PlayerStat pStat, MonsterStat mStat) {
            player.Initialize(pStat);
            monster.Initialize(mStat);
        }
    }
}
