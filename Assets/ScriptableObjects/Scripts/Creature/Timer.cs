using System.Collections;
using Unit.GameScene.Interfaces;
using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(Timer), menuName = "State/" + nameof(Condition) + "/" + nameof(Timer))]
    public class Timer : Condition {
        [SerializeField] private float intervalTime = 1.0f;
        private bool _timer = true;
        private bool _timerRunning = false;
        public override bool CheckCondition(BaseCreature target) {
            return _timer;
        }

        public void StartTimer(BaseCreature target) {
            if (!_timerRunning)
                target.StartCoroutine(CheckTime(intervalTime));
        }

        private IEnumerator CheckTime(float time) {
            _timerRunning = true;
            _timer = false;
            var currentTime = 0f;
            while (currentTime < time) {
                currentTime += Time.deltaTime;
                yield return null;
            }
            _timer = true;
            _timerRunning = false;
        }
    }
}