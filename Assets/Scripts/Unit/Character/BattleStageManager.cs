using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
// TODO 이름 IStageable로 바꿔야됨.
public interface IRaceable {
    event Action<ICommand<IRace>> OnSendCommand;
}

public interface ICommand<T> {
    public bool IsExecutable(T target);
    public void Execute(T target);
}

public interface ICommandReceiver<T> {
    public void Received(ICommand<T> command);
    void UpdateCommand(T target);
}

// TODO 이름 IStage로 바꿔야됨.
public interface IRace {
    Unit.Character.PlayerCharacter Player { get; }
    List<Unit.Character.MonsterCharacter> Monsters { get; }
}

namespace Unit.Character {
    public interface IShowable {
        void Move(IRunnable target);
        void Stop();
    }

    public struct PlayerStat {
        public int Attack;
        public int Health;
        public int Speed;
    }

    public struct MonsterStat {
        public int Attack;
        public int Health;
    }

    // TODO factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    public interface IBattleStageSetting {
        public AssetReference PlayerRef { get; }
        public PlayerStat PlayerStat { get; }
        public AssetReference[] MonstersRef { get; }
        public MonsterStat[] MonsterStats { get; }
        public AssetReference[] Background { get; }
        public int[] BackgroundCoef { get; }
        public Vector3 PlayerPosition { get; }
        public Vector3 MonsterSpawnOffset { get; }
    }

    public class BattleStageManager : MonoBehaviour, IRace, ICommandReceiver<IRace> {
        PlayerCharacter player;
        List<MonsterCharacter> monsters;
        BackgroundDisplay backgroundDisplay;
        public PlayerCharacter Player => player;
        public List<MonsterCharacter> Monsters => monsters;
        Queue<ICommand<IRace>> commands = new Queue<ICommand<IRace>>();
        public void AttachBoard(IRaceable data) {
            data.OnSendCommand += Received;
        }

        public void Initialize(IBattleStageSetting setting) {
            // TODO 모든 deploy가 끝났는지 확인하고 전부 끝났으면 게임 준비가 되었음을 알려야 한다.
            Core.Utils.AddressableLoader.DeployAsset(setting.PlayerRef, setting.PlayerPosition, Quaternion.identity, null, (obj) => {
                if (obj.TryGetComponent(out player))
                    player.Initialize(setting.PlayerStat, backgroundDisplay);
            });

            monsters = new List<MonsterCharacter>();
            for (int i = 0; i < setting.MonsterStats.Length; ++i) {
                int index = i;
                Core.Utils.AddressableLoader.DeployAsset(setting.MonstersRef[i], setting.MonsterSpawnOffset, Quaternion.identity, null, (obj) => {
                    if (obj.TryGetComponent(out MonsterCharacter mon)) {
                        monsters.Add(mon);
                        mon.Initialize(setting.MonsterStats[index]);
                    }
                });
            }

            if (commands == null)
                commands = new Queue<ICommand<IRace>>();
            else
                commands.Clear();
        }

        private void Update() {
            (this as ICommandReceiver<IRace>).UpdateCommand(this);
        }

        public void Received(ICommand<IRace> command) {
            commands.Enqueue(command);
        }

        void ICommandReceiver<IRace>.UpdateCommand(IRace target) {
            if (commands.Count > 0) {
                if (commands.Peek().IsExecutable(target))
                    commands.Dequeue().Execute(target);
            }
        }
    }
}
