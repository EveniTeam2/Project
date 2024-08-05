using TMPro;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Blocks.UI
{
    public class ComboBlockView : BlockView
    {
        [SerializeField] private TextMeshProUGUI comboCountText;
        
        public void Initialize(BlockType type, int comboCount, CharacterSkill skill, Sprite background)
        {
            Type = type;

            blockBackground.sprite = background;
            
            UpdateIcon(skill);
            
            comboCountText.text = $"x {comboCount}";
        }
    }
}