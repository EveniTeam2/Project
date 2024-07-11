using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public interface IStageable {
    event Action<ICommand<IBattleStageTarget>> OnSendCommand;
}

public interface ICommand<T> {
    public bool IsExecutable(T target);
    public void Execute(T target);
}

public interface ICommandReceiver<T> {
    public void Received(ICommand<T> command);
    void UpdateCommand();
}

public interface IBattleStageTarget {
    Unit.Character.PlayerCharacter Player { get; }
    List<Unit.Character.MonsterCharacter> Monsters { get; }
}

namespace Unit.Character {
    public interface IShowable {
        void Move(IRunnable target);
        void Stop();
    }

    [Serializable]
    public struct PlayerStat {
        public int Attack;
        public int Health;
        public int Speed;
    }
    [Serializable]
    public struct MonsterStat {
        public int Attack;
        public int Health;
    }

    // TODO factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    [Serializable]
    public struct BattleStageSetting {
        public AssetReference PlayerRef;
        public PlayerStat PlayerStat;
        public AssetReference[] MonstersRef;
        public MonsterStat[] MonsterStats;
        public BackgroundDisplay Background;
        //public int[] BackgroundCoef;
        public Vector3 PlayerPosition;
        public Vector3 MonsterSpawnOffset;
    }

    public class BattleStageManager : MonoBehaviour, IBattleStageTarget, ICommandReceiver<IBattleStageTarget> {
        PlayerCharacter player;
        List<MonsterCharacter> monsters;
        BackgroundDisplay backgroundDisplay;
        public PlayerCharacter Player => player;
        public List<MonsterCharacter> Monsters => monsters;
        Queue<ICommand<IBattleStageTarget>> commands = new Queue<ICommand<IBattleStageTarget>>();
        public void AttachBoard(IStageable data) {
            data.OnSendCommand += Received;
        }

        // TODO 인호님! 이거 불러야 시작 가능함
        public void Initialize(BattleStageSetting settings) {
            backgroundDisplay = settings.Background;

            Core.Utils.AddressableLoader.DeployAsset(settings.PlayerRef, settings.PlayerPosition, Quaternion.identity, null, (obj) => {
                if (obj.TryGetComponent(out player))
                    player.Initialize(settings.PlayerStat, backgroundDisplay);
            });

            monsters = new List<MonsterCharacter>();
            for (int i = 0; i < settings.MonsterStats.Length; ++i) {
                int index = i;
                Core.Utils.AddressableLoader.DeployAsset(settings.MonstersRef[i], settings.MonsterSpawnOffset, Quaternion.identity, null, (obj) => {
                    if (obj.TryGetComponent(out MonsterCharacter mon)) {
                        monsters.Add(mon);
                        mon.Initialize(settings.MonsterStats[index]);
                    }
                });
            }

            if (commands == null)
                commands = new Queue<ICommand<IBattleStageTarget>>();
            else
                commands.Clear();
        }

        private void Update() {
            (this as ICommandReceiver<IBattleStageTarget>).UpdateCommand();
        }

        public void Received(ICommand<IBattleStageTarget> command) {
            commands.Enqueue(command);
        }

        void ICommandReceiver<IBattleStageTarget>.UpdateCommand() {
            if (commands.Count > 0) {
                if (commands.Peek().IsExecutable(this))
                    commands.Dequeue().Execute(this);
            }
        }
    }
}
