using System;
using System.Collections;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록의 이동을 처리하는 클래스입니다.
    /// </summary>
    public class BlockMover
    {
        private readonly WaitForSeconds _swapDelay;
        private readonly float _duration;
        private readonly float _dropDurationPerUnit;

        public BlockMover(float duration, float dropDurationPerUnit, WaitForSeconds progressTime)
        {
            _swapDelay = progressTime;
            _duration = duration;
            _dropDurationPerUnit = dropDurationPerUnit;
        }

        /// <summary>
        /// 두 블록을 스왑합니다.
        /// </summary>
        /// <param name="currentBlock">현재 블록</param>
        /// <param name="targetBlock">목표 블록</param>
        /// <param name="currentPos">현재 블록의 위치</param>
        /// <param name="targetPos">목표 블록의 위치</param>
        public IEnumerator SwapBlock(Block currentBlock, Block targetBlock, Tuple<float, float> currentPos, Tuple<float, float> targetPos)
        {
            var currentBlockStartPos = currentBlock.transform.position;
            var targetBlockStartPos = targetBlock.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                // 블록의 위치를 선형 보간하여 스왑
                currentBlock.transform.position = Vector3.Lerp(currentBlockStartPos, new Vector3(currentPos.Item1, currentPos.Item2, 0), elapsedTime / _duration);
                targetBlock.transform.position = Vector3.Lerp(targetBlockStartPos, new Vector3(targetPos.Item1, targetPos.Item2, 0), elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = new Vector3(currentPos.Item1, currentPos.Item2, 0);
            targetBlock.transform.position = new Vector3(targetPos.Item1, targetPos.Item2, 0);

            yield return _swapDelay;
        }

        /// <summary>
        /// 블록을 목표 위치로 떨어뜨립니다.
        /// </summary>
        /// <param name="targetPos">목표 위치</param>
        /// <param name="currentBlock">현재 블록</param>
        public IEnumerator DropBlock(Tuple<float, float> targetPos, Block currentBlock)
        {
            var currentBlockStartPos = currentBlock.transform.position;
            var targetPosition = new Vector3(targetPos.Item1, targetPos.Item2, 0);
            var distance = Vector3.Distance(currentBlockStartPos, targetPosition);
            var elapsedTime = 0f;

            // 블록의 위치를 선형 보간하여 떨어뜨림
            while (elapsedTime < distance * _dropDurationPerUnit) // 수정된 부분
            {
                currentBlock.transform.position = Vector3.Lerp(currentBlockStartPos, targetPosition, elapsedTime / (distance * _dropDurationPerUnit)); // 수정된 부분
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = targetPosition;
        }
    }
}