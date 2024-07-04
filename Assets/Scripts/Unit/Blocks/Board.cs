using System;
using System.Collections.Generic;
using Manager;
using ScriptableObjects.Scripts.Blocks;
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
        
        private List<NewBlock> _blockInfos;
        private BlockGenerator _blockGenerator;
        private GameObject _blockPrefab;
        
        private Dictionary<Tuple<float, float>, GameObject> _tiles;

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;
            _blockInfos = GameManager.Instance.blockInfos;
            _blockPrefab = GameManager.Instance.tilePrefab;
            
            _tiles = new Dictionary<Tuple<float, float>, GameObject>();
            
            _blockGenerator = GetComponent<BlockGenerator>();
            _blockGenerator.Initialize(_width, _height);
            
            _tiles = _blockGenerator.GenerateAllBlocks(_blockPrefab, _blockInfos, true);
        }
    }
}