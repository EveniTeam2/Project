using System.Collections;
using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(Timer), menuName = "State/" + nameof(Condition) + "/" + nameof(Timer))]
    public class Timer : Condition
    {
        [SerializeField] private float intervalTime = 1.0f;

        public override IStateCondition GetStateCondition()
        {
            var ret = new StateConditionTimer(intervalTime);
            return ret;
        }
    }

    public class StateConditionTimer : IStateCondition {
        private float intervalTime;
        private bool _timer;

        public StateConditionTimer(float intervalTime) {
            this.intervalTime = intervalTime;
            _timer = true;
        }

        public bool CheckCondition(BaseCreature target) {
            return _timer;
        }

        public void StartTimer(BaseCreature target) {
            if (_timer)
                target.StartCoroutine(CheckTime(intervalTime));
        }

        private IEnumerator CheckTime(float time) {
            _timer = false;
            var currentTime = 0f;
            while (currentTime < time) {
                currentTime += Time.deltaTime;
                yield return null;
            }
            _timer = true;
        }
    }
}