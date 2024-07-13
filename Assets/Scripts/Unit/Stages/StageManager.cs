using System.Collections.Generic;
using Unit.Stages.Creatures;
using Unit.Stages.Creatures.Characters;
using Unit.Stages.Creatures.Interfaces;
using Unit.Stages.Creatures.Monsters;
using Unit.Stages.Interfaces;
using UnityEngine;

namespace Unit.Stages
{
    public class StageManager : MonoBehaviour, IStageCreature, ICommandReceiver<IStageCreature>
    {
        public PlayerCreature Character => _character;
        private PlayerCreature _character;
        public List<MonsterCreature> Monsters => _monsters;
        private List<MonsterCreature> _monsters;
        private BackgroundDisplay _backgroundDisplay;
        
        private Queue<ICommand<IStageCreature>> _commands = new();

        [SerializeField] float groundYPosition;
        public void AttachBoard(IStageable data)
        {
            data.OnSendCommand += Received;
        }

        // TODO 인호님! 이거 불러야 시작 가능함
        public void Initialize(StageSetting settings)
        {
            InitializeBackground(settings);

            InitializeCharacter(settings);

            InitializeMonster(settings);

            InitializeCommand();
        }

        private void InitializeBackground(StageSetting settings)
        {
            _backgroundDisplay = settings.background;
        }

        private void InitializeCharacter(StageSetting settings)
        {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });

            var character = Instantiate(settings.characterRef, settings.playerPosition, Quaternion.identity);
            
            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(settings.characterStat, groundYPosition);
            }
        }
        
        private void InitializeMonster(StageSetting settings)
        {
            
            _monsters = new List<MonsterCreature>();
            for (int i = 0; i < settings.monsterStats.Length; ++i)
            {
                int index = i;
                Core.Utils.AddressableLoader.DeployAsset(settings.monstersRef[i], settings.monsterSpawnOffset, Quaternion.identity, null, (obj) => {
                    if (obj.TryGetComponent(out MonsterCreature mon))
                    {
                        _monsters.Add(mon);
                        mon.Initialize(settings.monsterStats[index], groundYPosition);
                    }
                });
            }
        }
        
        private void InitializeCommand()
        {
            if (_commands == null)
                _commands = new Queue<ICommand<IStageCreature>>();
            else
                _commands.Clear();
        }
        
        private void Update()
        {
            (this as ICommandReceiver<IStageCreature>).UpdateCommand();
        }

        public void Received(ICommand<IStageCreature> command)
        {
            _commands.Enqueue(command);
        }

        void ICommandReceiver<IStageCreature>.UpdateCommand()
        {
            if (_commands.Count > 0)
            {
                if (_commands.Peek().IsExecutable(this))
                    _commands.Dequeue().Execute(this);
            }
        }
    }
}