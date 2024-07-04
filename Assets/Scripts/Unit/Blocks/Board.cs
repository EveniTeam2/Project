using Manager;
using UnityEngine;

namespace Unit.Blocks
{
    /// <summary>
    /// Board 클래스 : 보드판과 블록을 생성하고, 이후 블록과 관련된 로직을 처리합니다.
    /// </summary>
    public class Board : MonoBehaviour
    {
        private int _width;
        private int _height;
        
        private Block[,] _tiles;
        private BlockGenerator _blockGenerator;

        [SerializeField] private GameObject blockPrefab;

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;
            
            _tiles = new Block[_width, _height];
            _blockGenerator = new BlockGenerator(_width, _height);
            
            _tiles = _blockGenerator.GenerateBlocks(blockPrefab, true);
        }
    }
}