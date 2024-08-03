using UnityEngine;

namespace Unit.GameScene.Units.Panels.Modules.StageModules
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private float _adjustX;
        private float _adjustY;
        private float _adjustZ;

        private void LateUpdate()
        {
            if (target == null) return;

            var position = transform.position;
            var newPos = target.transform.position;
            
            position = new Vector3(newPos.x + _adjustX, position.y + _adjustY, position.z + _adjustZ);
            transform.position = position;
        }

        public void Initialize(Transform target, Vector3 cameraSpawnPosition)
        {
            this.target = target;
            _adjustX = cameraSpawnPosition.x;
            _adjustY = cameraSpawnPosition.y;
            _adjustZ = cameraSpawnPosition.z;
        }
    }
}