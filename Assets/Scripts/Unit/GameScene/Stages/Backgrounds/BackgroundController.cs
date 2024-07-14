using System.Collections.Generic;
using UnityEngine;

namespace Unit.GameScene.Stages.Backgrounds
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private List<ParallaxBackground> parallaxBackgrounds;

        public void Initialize(Camera mainCamera)
        {
            foreach (var parallaxBackground in parallaxBackgrounds)
            {
                parallaxBackground.InitializeBackground(mainCamera);
            }
        }
    }
}