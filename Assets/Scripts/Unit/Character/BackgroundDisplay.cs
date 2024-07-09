using System.Collections.Generic;
using UnityEngine;
namespace Unit.Character {
    public class BackgroundDisplay : MonoBehaviour, IShowable {
        [SerializeField] List<SpriteRenderer> _backgrounds;
        List<Material> _backMat;
        [SerializeField] List<float> _spdCoef;
        private float _targetSpd;
        private bool _move;
        private float[] _dist;

        public void Move(IRunnable target) {
            _move = true;
            _targetSpd = target.Speed;
        }

        public void Stop() {
            _move = false;
        }
        private void Awake() {
            _backMat = new List<Material>();
            _dist = new float[_backgrounds.Count];
            foreach (var background in _backgrounds) {
                _backMat.Add(background.material);
            }
        }
        void LateUpdate() {
            if (_move) {
                for (int i = 0; i < _backgrounds.Count; ++i) {
                    _dist[i] += _targetSpd * _spdCoef[i];
                    _backMat[i].SetTextureOffset("_MainTex", new Vector2(_dist[i], 0));
                }
                _move = false;
            }
        }
    }
}