using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts.Blocks;
using Unit.Blocks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    public class BlockGenerator : MonoBehaviour
    {
        private int _width;
        private int _height;

        public void Initialize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// 랜덤한 타일을 생성합니다.
        /// </summary>
        /// <param name="blockPrefab">blockPrefab 게임 오브젝트</param>
        /// <param name="blockInfos">blockInfo 스크립터블 오브젝트</param>
        /// <param name="mergeLock">true - 초반 머지가 불가능하도록 생성합니다. / false - 초반 머지가 가능하도록 생성합니다.</param>
        public Dictionary<Tuple<float, float>, GameObject> GenerateBlocks(GameObject blockPrefab, List<NewBlock> blockInfos, bool mergeLock)
        {
            var blocksPositions = CalculateEachBlockPosition();

            return GenerateRandomBlocks(blockPrefab, blockInfos, blocksPositions);
        }

        private IEnumerable<Tuple<float, float>> CalculateEachBlockPosition()
        {
            //TODO: 이후 로직 다시 수정
            
            Debug.Log($"가로 {_width} / 세로 {_height}");
            
            var positions = new List<Tuple<float, float>>();

            for (var i = 1; i <= _width / 2 ; i++)
            {
                for (var j = 1; j <= _height / 2 ; j++)
                {
                    var x = 0.5f + 1 * (i - 1);
                    var y = 0.5f + 1 * (j - 1);
                    
                    positions.Add(new Tuple<float, float>(x, y));
                    positions.Add(new Tuple<float, float>(-x, -y));
                    positions.Add(new Tuple<float, float>(-x, y));
                    positions.Add(new Tuple<float, float>(x, -y));
                }
            }
            
            Debug.Log($"우웅 우리 안한 게 모가 이찌? positions 몇 개인지 확인하기? 우웅 마쟈 죵답 >-< {positions.Count}");
            
            // var isWidthEvenNum = _width % 2 == 0 ? true : false;
            // var isHeightEvenNum = _height % 2 == 0 ? true : false;

            return positions;
        }
        
        private Dictionary<Tuple<float, float>, GameObject> GenerateRandomBlocks(GameObject blockPrefab, IReadOnlyList<NewBlock> blockInfos, IEnumerable<Tuple<float, float>> blockPositions)
        {
            var blockDic = new Dictionary<Tuple<float, float>, GameObject>();

            foreach (var blockPosition in blockPositions)
            {
                var position = new Vector3(blockPosition.Item1, blockPosition.Item2, 0);
                
                var block = Instantiate(blockPrefab, position, transform.rotation);
                
                block.GetComponent<Block>().Initialize(blockInfos[Random.Range(0, blockInfos.Count)]);
                
                blockDic.Add(new Tuple<float, float>(blockPosition.Item1, blockPosition.Item2), block);
            }

            return blockDic;
        }
    }
}