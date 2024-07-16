using System;
using Unit.GameScene.Stages.Creatures.Characters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput
{
    /// <summary>
    ///     여러 act를 묶어서 순차적으로 실행하는 클래스입니다.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = nameof(CompositeActCharacter), menuName = "Input/" + nameof(CompositeActCharacter))]
    public class CompositeActCharacter : ActCharacter
    {
        [SerializeField] private ActCharacter[] compositeActs;

        public override void Act(ActOnInput inputData, Character character, int count)
        {
            foreach (var act in compositeActs) act.Act(inputData, character, count);
        }
    }
}