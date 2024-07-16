using System;
using Unit.GameScene.Manager.Interfaces;
using UnityEngine;
using BlockType = Unit.GameScene.Boards.Blocks.Enums.BlockType;

namespace Unit.GameScene.Manager.Modules
{
    [Serializable]
    public class CommandPacket : ICommand<IStageCreature>
    {
        [SerializeField] private BlockType _blockType;
        [SerializeField] private int _count;
        [SerializeField] private float _targetNormalTime;

        public CommandPacket(BlockType blockType, int count, float targetNormalTime)
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
            return creature.Character.HFSM.GetCurrentAnimationNormalizedTime() > _targetNormalTime;
        }
    }
}