using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    public class MonsterCharacter : BaseCharacter {
        public override int Speed => _speed.Current;
        private InstanceStat<int> _speed;
        private float _position;
        private bool _run;
        public override float GetCurrentPosition() {
            return _position;
        }

        public override void SetRun(bool isRun) {
            _run = isRun;
        }

        private void Update() {
            if (_run) {

            }
            else {

            }
        }

        protected override void RecalculateSpeed() {
            if (_run) {
                _speed.Current = _speed.Origin;
                foreach (var spd in spdModifier) {
                    _speed.Current += spd;
                }
            }
        }

        public void Initialize(MonsterStat stat) {
            _speed.Origin = stat.Speed;
        }
    }
}