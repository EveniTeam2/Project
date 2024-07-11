using System;
using System.Collections;
using Unit.Blocks;
using Unit.Boards.Interfaces;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록의 이동을 처리하는 클래스입니다.
    /// </summary>
    public class BlockMover : IBlockMover
    {
        private readonly WaitForSeconds _swapDelay;
        private readonly float _duration;
        private readonly float _dropDurationPerUnit;
        private readonly float _bounceHeight;
        private readonly float _bounceDuration;
        private readonly MonoBehaviour _monoBehaviour;

        public BlockMover(float duration, float dropDurationPerUnit, float bounceHeight, float bounceDuration, WaitForSeconds progressTime, MonoBehaviour monoBehaviour)
        {
            _swapDelay = progressTime;
            _duration = duration;
            _bounceHeight = bounceHeight;
            _bounceDuration = bounceDuration;
            _dropDurationPerUnit = dropDurationPerUnit;
            _monoBehaviour = monoBehaviour;
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
            var currentBlockStartPos = currentBlock.transform.localPosition;
            var targetBlockStartPos = targetBlock.transform.localPosition;
            var elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                currentBlock.transform.localPosition = Vector3.Lerp(currentBlockStartPos, new Vector3(currentPos.Item1, currentPos.Item2, 0), elapsedTime / _duration);
                targetBlock.transform.localPosition = Vector3.Lerp(targetBlockStartPos, new Vector3(targetPos.Item1, targetPos.Item2, 0), elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.localPosition = new Vector3(currentPos.Item1, currentPos.Item2, 0);
            targetBlock.transform.localPosition = new Vector3(targetPos.Item1, targetPos.Item2, 0);

            yield return _swapDelay;
        }

        /// <summary>
        /// 블록을 목표 위치로 떨어뜨립니다.
        /// </summary>
        /// <param name="targetPos">목표 위치</param>
        /// <param name="currentBlock">현재 블록</param>
        public IEnumerator DropBlock(Tuple<float, float> targetPos, Block currentBlock)
        {
            var currentBlockStartPos = currentBlock.transform.localPosition;
            var targetPosition = new Vector3(targetPos.Item1, targetPos.Item2, 0);
            var distance = Vector3.Distance(currentBlockStartPos, targetPosition);
            var elapsedTime = 0f;

            while (elapsedTime < distance * _dropDurationPerUnit)
            {
                currentBlock.transform.localPosition = Vector3.Lerp(currentBlockStartPos, targetPosition, elapsedTime / (distance * _dropDurationPerUnit));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.localPosition = targetPosition;

            yield return _monoBehaviour.StartCoroutine(BounceBlock(targetPosition, currentBlock));
        }

        /// <summary>
        /// 블록이 바닥에 떨어진 후 튕기는 애니메이션을 처리합니다.
        /// </summary>
        /// <param name="targetPosition">목표 위치</param>
        /// <param name="currentBlock">현재 블록</param>
        private IEnumerator BounceBlock(Vector3 targetPosition, Block currentBlock)
        {
            var elapsedTime = 0f;
            var boundDuration = _bounceDuration / 2;
            var bounceTargetPosition = targetPosition + Vector3.up * _bounceHeight;

            while (elapsedTime < boundDuration)
            {
                currentBlock.transform.localPosition = Vector3.Lerp(targetPosition, bounceTargetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.localPosition = bounceTargetPosition;

            elapsedTime = 0f;
            while (elapsedTime < boundDuration)
            {
                currentBlock.transform.localPosition = Vector3.Lerp(bounceTargetPosition, targetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.localPosition = targetPosition;
        }
    }
}