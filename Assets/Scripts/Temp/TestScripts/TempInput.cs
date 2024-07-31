using Unit.GameScene.Manager.Units.GameSceneManagers;
using Unit.GameScene.Manager.Units.StageManagers;
using UnityEngine;

namespace Temp.TestScripts
{
    public class TempInput : MonoBehaviour
    {
        public KeyCode exit;
        public float waitTime;
        public int limitCount;

        private int count;
        private float passed;

        public KeyCode levelup;
        public int cardCount;
        public GameSceneManager gameSceneManager;

        private void Awake()
        {
            count = 0;
            gameSceneManager = gameObject.GetComponent<GameSceneManager>();
        }

        void Update()
        {
            if (Input.GetKeyDown(exit))
            {
                passed = 0;
                if (count > limitCount)
                    Application.Quit();
                else
                    ++count;
            }
            else if (Input.GetKeyDown(levelup))
            {
                gameSceneManager.CardManager.GetCards(cardCount);
            }
            else
            {
                if (passed > waitTime)
                    count = 0;
                else
                    passed += Time.deltaTime;
            }

        }
    }
}
