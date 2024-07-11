using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    public class MonsterCharacter : BaseCharacter {
        private InstanceStat<MonsterStat> _stats;
        [SerializeField] CharacterStateData characterStateData;
        public override int Health { get => _stats.Current.Health; protected set => _stats.Current.Health = value; }

        public void Initialize(MonsterStat stat) {
            _stats = new InstanceStat<MonsterStat>(stat);
            StateBuilder.BuildState(HFSM, characterStateData);
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        public override void SetHealth(int health) {
            _stats.Origin.Health = health;
            _stats.Current.Health = health;
        }
        public override Animator Animator => animator;
        [SerializeField] Animator animator;
    }
}