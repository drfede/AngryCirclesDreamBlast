using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.SceneManagement;

namespace AngryCirclesDreamBlast
{
    public class GameManager : Utilities.Singleton<GameManager>
    {

        [SerializeField]
        private RectTransform circlesSpawnPosition;
        [SerializeField]
        private Settings settings;
        [SerializeField]
        private CircleMap circleMap;
        [SerializeField]
        private LevelObject levelToLoad;

        [Header("Parameters")]
        [SerializeField]
        private int ballsToSpawnPerWave = 15;
        [SerializeField]
        private float delayBeforeSpawnNextWave = 0.45f;
        [SerializeField]
        private Vector2 spawnXRange = new(-30, 30);
        [SerializeField]
        private Vector2 spawnYRange = new(0, 20);
        public Settings Settings { get => settings; private set => settings = value; }


        private List<CircleLevelTarget> targetLevels;
        public UnityEvent<HashSet<StandardCircle>> onCircleTap;
        public UnityEvent onAfterPop;
        public UnityEvent onWin;
        public UnityEvent onLost;


        private int availableMoves = 0;

        public bool HasWon { get => TargetLevels.Where(x => x.TargetNumber != 0).Count() == 0; }
        public bool HasLost { get => !HasWon && AvailableMoves <= 0; }
        public int AvailableMoves { get => availableMoves; private set => availableMoves = value; }
        public List<CircleLevelTarget> TargetLevels { get => targetLevels; private set => targetLevels = value; }
        public CircleMap CircleMap { get => circleMap; set => circleMap = value; }

        public bool InputEnabled = true;

        public override void Awake()
        {
            if (levelToLoad != null)
            {
                onCircleTap.RemoveAllListeners();
                onCircleTap.AddListener(OnPop);
                Setup();
                StartCoroutine(StartLevel());
            }
        }

        public override void Start()
        {
            if (levelToLoad != null)
            {
                StartCoroutine(StartLevel());
            }
        }

        private void OnPop(HashSet<StandardCircle> poppedCircles)
        {
            var types = Enum.GetValues(typeof(StandardCircle.CircleType)).OfType<StandardCircle.CircleType>().ToList();
            types.Remove(StandardCircle.CircleType.NONE);
            types.Remove(StandardCircle.CircleType.BOMB);

            foreach(var type in types)
            {
                var targetNum = targetLevels.Find(x => x.Type == type);
                if (targetNum != null)
                {
                    int count = poppedCircles.Where(x => x.Type == type).Count();
                    targetNum.TargetNumber = Mathf.Max(0, targetNum.TargetNumber - count);
                }
            }
            //var targetNum = TargetLevels.Find(x => x.Type == poppedCircles.FirstOrDefault().Type);
            //if (targetNum != null)
            //{
            //    targetNum.TargetNumber = Mathf.Max(0, targetNum.TargetNumber - poppedCircles.Count);
            //}
            AvailableMoves--;
            onAfterPop?.Invoke();
            if (HasWon)
            {
                InputEnabled = false;
                onWin?.Invoke();
            }
            else if (HasLost)
            {
                InputEnabled = false;
                onLost?.Invoke();
            }
            else
            {
                GenerateRandomCircles(types, poppedCircles.Count);
            }
        }

        private void Setup()
        {
            AvailableMoves = levelToLoad.MovesLimit;
            TargetLevels = new();
            levelToLoad.LevelTargets.ForEach(x => TargetLevels.Add(new CircleLevelTarget() { TargetNumber = x.TargetNumber, Type = x.Type }));
        }

        private IEnumerator StartLevel()
        {
            var types = Enum.GetValues(typeof(StandardCircle.CircleType)).OfType<StandardCircle.CircleType>().ToList();
            types.Remove(StandardCircle.CircleType.NONE);
            types.Remove(StandardCircle.CircleType.BOMB);
            for (int i = 0; i < levelToLoad.StartingCircles; i += ballsToSpawnPerWave)
            {
                GenerateRandomCircles(types, ballsToSpawnPerWave);
                yield return new WaitForSeconds(delayBeforeSpawnNextWave);
            }
        }

        private void GenerateRandomCircles(List<StandardCircle.CircleType> types, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var circle = CirclesPooler.Instance.PoolCircle(types[UnityEngine.Random.Range(0, types.Count)]);
                circle.gameObject.SetActive(true);
                circle.transform.position = circlesSpawnPosition.position;
                circle.transform.localPosition += new Vector3(UnityEngine.Random.Range(spawnXRange.x, spawnXRange.y), UnityEngine.Random.Range(spawnYRange.x, spawnYRange.y));
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }
    }

}