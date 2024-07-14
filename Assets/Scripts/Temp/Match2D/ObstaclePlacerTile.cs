using UnityEngine;
using UnityEngine.Tilemaps;

namespace Match2D {
    [CreateAssetMenu(fileName = "ObstaclePlacerTile", menuName = "Tile/Obstacle Placer Tile")]
    public class ObstaclePlacerTile : TileBase {
        public Sprite PreviewEditorSprite;
        //public Obstacle ObstaclePrefab;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //var newObstacle = Instantiate(ObstaclePrefab);
            //newObstacle.Init(position);
            return true;
        }
    }
}
