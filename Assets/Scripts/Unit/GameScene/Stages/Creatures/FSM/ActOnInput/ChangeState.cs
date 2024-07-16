using System;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput
{
    /// <summary>
    ///     입력에 따라 상태를 바꾸는 행동입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ChangeState), menuName = "Input/" + nameof(ChangeState))]
    public class ChangeState : ActCharacter
    {
        public override void Act(ActOnInput inputData, Character character, int count)
        {
            character.HFSM.TryChangeState(Enum.Parse<StateEnums>(inputData.StateName));
        }
    }
}