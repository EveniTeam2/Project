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
        private readonly float _bounceHeight; // 튕길 높이
        private readonly float _bounceDuration; // 튕기는 시간
        private readonly MonoBehaviour _monoBehaviour; // MonoBehaviour 참조

        public BlockMover(float duration, float dropDurationPerUnit, float bounceHeight, float bounceDuration, WaitForSeconds progressTime, MonoBehaviour monoBehaviour)
        {
            _swapDelay = progressTime;
            _duration = duration;
            _bounceHeight = bounceHeight;
            _bounceDuration = bounceDuration;
            _dropDurationPerUnit = dropDurationPerUnit;
            _monoBehaviour = monoBehaviour; // MonoBehaviour 초기화
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
            while (elapsedTime < distance * _dropDurationPerUnit)
            {
                currentBlock.transform.position = Vector3.Lerp(currentBlockStartPos, targetPosition, elapsedTime / (distance * _dropDurationPerUnit));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = targetPosition;

            // 블록이 바닥에 떨어진 후 튕기는 애니메이션 추가
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

            // 위로 튕기기
            while (elapsedTime < boundDuration)
            {
                currentBlock.transform.position = Vector3.Lerp(targetPosition, bounceTargetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = bounceTargetPosition;

            // 다시 아래로 내려오기
            elapsedTime = 0f;
            while (elapsedTime < boundDuration)
            {
                currentBlock.transform.position = Vector3.Lerp(bounceTargetPosition, targetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = targetPosition;
        }
    }
}