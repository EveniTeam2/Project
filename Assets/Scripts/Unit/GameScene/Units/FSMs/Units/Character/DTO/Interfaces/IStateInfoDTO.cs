using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.FSMs.Units.Character.DTO.Interfaces
{
    public interface IStateInfoDTO
    {
        public CharacterBaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums);
    }
}