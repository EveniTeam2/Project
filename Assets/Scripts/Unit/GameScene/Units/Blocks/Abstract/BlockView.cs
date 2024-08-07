using System;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Blocks.Abstract
{
    public class BlockView : MonoBehaviour
    {
        public BlockType Type { get; protected set; }
        
        [SerializeField] protected Image blockBackground;
        [SerializeField] protected Image blockIcon; // TODO : 스킬 아이콘 넣을 곳
        
        public void UpdateIcon(CharacterSkill skill)
        {
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
                
                blockIcon.sprite = skill.SkillIcon;
            }
        }
    }
}