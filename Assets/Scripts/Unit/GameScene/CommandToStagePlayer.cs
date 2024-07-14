using System;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Stages.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using BlockType = Unit.GameScene.Boards.Blocks.Enums.BlockType;

namespace Unit.GameScene
{
    [Serializable]
    public class CommandToStagePlayer : ICommand<IStageCreature>
    {
        [SerializeField] private BlockType _blockType;
        [SerializeField] private int _count;
        [SerializeField] private float _targetNormalTime;

        public CommandToStagePlayer(BlockType blockType, int count, float targetNormalTime)
        {
            _blockType = blockType;
            _count = count;
            _targetNormalTime = targetNormalTime;
        }
        
        void ICommand<IStageCreature>.Execute(IStageCreature creature)
        {
            creature.Character.Input(_blockType, _count);
        }
        
        bool ICommand<IStageCreature>.IsExecutable(IStageCreature creature)
        {
            return (creature.Character.HFSM.GetCurrentAnimationNormalizedTime() > _targetNormalTime);
        }
    }
}