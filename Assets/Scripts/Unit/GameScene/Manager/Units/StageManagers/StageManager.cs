using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class StageManager : MonoBehaviour, IStageCreature, ICommandReceiver<IStageCreature>
    {
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;
        
        private Queue<ICommand<IStageCreature>> _commands = new();
        
        private Character _character;
        private MonsterSpawnManager _monsterManager;
        private float _startTime;
        private Vector3 _zeroPosition;
        
        private void Update()
        {
            (this as ICommandReceiver<IStageCreature>).UpdateCommand();
            //TODO : _monsterManager null이라 잠깐 막아놨습니다.
            // _monsterManager.Update();
        }

        public void Received(ICommand<IStageCreature> command)
        {
            _commands.Enqueue(command);
        }

        void ICommandReceiver<IStageCreature>.UpdateCommand()
        {
            if (_commands.Count > 0)
                if (_commands.Peek().IsExecutable(this))
                    _commands.Dequeue().Execute(this);
        }

        public void AttachBoard(ISendCommand data)
        {
            Debug.Log("Attach Clear");
            data.OnSendCommand += Received;
        }

        // TODO 인호님! 이거 불러야 시작 가능함
        public void Initialize(SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting, Camera cam)
        {
            InitializeCharacter(extraSetting, defaultSetting);
            InitializeMonster(extraSetting, defaultSetting);
            InitializeCamera(cam);
            InitializeCommand();

            _monsterManager.Start();
        }

        private void InitializeCharacter(SceneExtraSetting extraSettings, SceneDefaultSetting defaultSetting)
        {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });
            
            var character = Instantiate(extraSettings.characterRef, defaultSetting.playerPosition, Quaternion.identity);
            // TODO 인호님 여기가 첫 시작에 대한 기준점입니다.
            _zeroPosition = defaultSetting.playerPosition;
            _startTime = Time.time;

            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(this, extraSettings.characterStat, defaultSetting.playerPosition.y, defaultSetting.actOnInputs.ToArray());   
            }
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting)
        {
            _monsterManager = new MonsterSpawnManager(this, extraSetting.monsterSpawnData, defaultSetting.playerPosition.y);
        }

        private void InitializeCamera(Camera cam)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform);
        }

        private void InitializeCommand()
        {
            if (_commands == null)
                _commands = new Queue<ICommand<IStageCreature>>();
            else
                _commands.Clear();
        }
    }
}