using UnityEngine;

namespace Unit.GameScene.Stages.Backgrounds
{
    public class CameraController : MonoBehaviour
    {
        private Transform _target;

        private void LateUpdate()
        {
            if (_target == null) return;

            var newPos = _target.transform.position;
            transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
        }

        public void Initialize(Transform target)
        {
            _target = target;
        }
    }
}