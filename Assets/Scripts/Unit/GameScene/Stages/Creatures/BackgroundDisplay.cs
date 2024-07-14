using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Interfaces.Unit.Character;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures
{
    /// <summary>
    /// 배경을 표시하는 클래스입니다.
    /// </summary>
    public class BackgroundDisplay : MonoBehaviour, IShowable
    {
        [SerializeField] private List<SpriteRenderer> _backgrounds;
        private List<Material> _backMat;
        [SerializeField] private List<float> _spdCoef;
        private float _targetSpd;
        private bool _move;
        private float[] _dist;

        public void Move(IRunnable target)
        {
            _move = target.IsRun;
            _targetSpd = target.Speed;
        }

        public void Stop()
        {
            _move = false;
        }

        private void Awake()
        {
            _backMat = new List<Material>();
            _dist = new float[_backgrounds.Count];
            foreach (var background in _backgrounds)
            {
                _backMat.Add(background.material);
            }
        }

        private void LateUpdate()
        {
            if (_move)
            {
                for (int i = 0; i < _backgrounds.Count; ++i)
                {
                    _dist[i] += _targetSpd * _spdCoef[i] * Time.deltaTime;
                    _backMat[i].SetTextureOffset("_MainTex", new Vector2(_dist[i], 0));
                }

                _move = false;
            }
        }
    }
}