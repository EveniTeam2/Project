using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters;
using UnityEngine;

namespace Unit.GameScene.Units.StagePanels.Backgrounds
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f), Tooltip("배율이 높을수록 캐릭터보다 빠르게 움직입니다."), Space(5)]
        private float parallaxEffectSpeed;
        [SerializeField, Range(1f, 5f), Tooltip("각 레이어 별로 가중치를 곱합니다."), Space(5)]
        private float parallaxEffectMultiplier;
        [SerializeField] private List<ParallaxBackground> parallaxBackgrounds;

        public void Initialize(Character character)
        {
            for (var i = 0; i < parallaxBackgrounds.Count; i++)
            {
                var parallaxBackground = parallaxBackgrounds[i];
                parallaxBackground.InitializeBackground(character, parallaxEffectSpeed * (i + (float) Math.Pow(parallaxEffectMultiplier, i)));
            }
        }
    }
}