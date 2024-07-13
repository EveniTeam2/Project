using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects.Scripts.Blocks;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class UserInput {
        private readonly Character _character;
        Dictionary<BlockType, ActOnInput> actDic;
        /// <summary>
        /// 입력을 처리하는 클래스입니다.
        /// </summary>
        /// <param name="target">입력이 처리되기를 바라는 타겟입니다.</param>
        /// <param name="acts">타겟이 입력에 따라 행동하기를 원하는 행동입니다.</param>
        public UserInput(Character target, params ActOnInput[] acts) {
            _character = target;
            actDic = new Dictionary<BlockType, ActOnInput>();
            foreach (var act in acts.ToArray()) {
                actDic.Add(act.InputType, act);
            }
        }

        public bool AddInput(ActOnInput act) {
            return actDic.TryAdd(act.InputType, act);
        }

        public void AddInput(ActOnInput[] acts) {
            foreach (var act in acts) {
#if UNITY_EDITOR
                if (!AddInput(act)) {
                    Debug.LogWarning(_character.name + " already has same input.");
                }
#else
                AddInput(act);
#endif
            }
        }

        public void Clear() {
            actDic.Clear();
        }

        /// <summary>
        /// 입력을 처리합니다.
        /// </summary>
        public void Input(NewBlock block, int count) {
            actDic[block.type].Act(_character, count);
        }
    }
}