using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.Blocks.Units.ComboBlock
{
    public class ComboBlockView : BlockView
    {
        [SerializeField] private TextMeshProUGUI comboCountText;
        
        public void Initialize(BlockType type, int comboCount, CharacterSkill skill, Sprite background)
        {
            Type = type;

            blockBackground.sprite = background;
            
            if (skill == null && blockIcon.gameObject.activeInHierarchy)
            {
                blockIcon.gameObject.SetActive(false);
            }
            else if (skill != null)
            {
                if (!blockIcon.gameObject.activeInHierarchy)
                {
                    blockIcon.gameObject.SetActive(true);     
                }
                
                blockIcon.sprite = skill.Icon;
            }
            
            comboCountText.text = $"x {comboCount}";
        }
    }
}