using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public class MonsterSkillDeciderTimer : IMonsterSkillDecider
    {
        private readonly IMonsterFsmController monsterController;
        private readonly IMonsterSkillAct targetSkill;
        float timer;
        Timer timerUpdater;

        public MonsterSkillDeciderTimer(IMonsterFsmController monsterController, IMonsterSkillAct targetSkill, float timer)
        {
            this.monsterController = monsterController;
            this.targetSkill = targetSkill;
            this.timer = timer;
            timerUpdater = new Timer(monsterController.UpdateEvent, timer);
            targetSkill.OnExcute += StartTimer;
        }

        private void StartTimer()
        {
            timerUpdater.Start();
        }

        bool IMonsterSkillDecider.CanExcute()
        {
            return timerUpdater.IsDone;
        }

        void IMonsterSkillDecider.ResetDecider()
        {
            timerUpdater.Reset();
        }
    }
}