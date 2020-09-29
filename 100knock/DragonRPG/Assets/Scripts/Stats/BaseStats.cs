using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99), SerializeField]
        private int startingLevel = 1;
        [SerializeField]
        private CharacterClass charcterClass = CharacterClass.Grunt;
        [SerializeField]
        private Progresstion progression = null;
        [SerializeField]
        private GameObject levelUpEffectPrefab = null;
        [SerializeField]
        private bool useAdditiveModifer = false;

        private SafeValue<int> currentLevel = null;

        public event Action onLevelupEvent;

        private Experience experience = null;

        private void Awake()
        {
            this.experience = this.GetComponent<Experience>();

            this.currentLevel = new SafeValue<int>(() => this.CalculateLevel());
        }

        private void Start()
        {
            this.currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (this.experience != null)
            {
                this.experience.OnExperienceGaineed += this.UpdateLevelup;
            }
        }

        private void OnDisable()
        {
            if (this.experience != null)
            {
                this.experience.OnExperienceGaineed -= this.UpdateLevelup;
            }
        }

        private void UpdateLevelup()
        {
            // レベルが更新しているかチェックして更新しているのであれば最新に更新
            var newLevel = this.CalculateLevel();
            if (newLevel > this.currentLevel.Value)
            {
                this.currentLevel.Value = newLevel;
                Debug.Assert(this.levelUpEffectPrefab);
                GameObject.Instantiate(this.levelUpEffectPrefab, this.transform);

                this.onLevelupEvent();
            }
        }

        public int GetLevel()
        {
            return this.currentLevel.Value;
        }

        public float GetStats(Stats stats)
        {
            var statsValue = this.progression.GetStats(stats, this.charcterClass, this.GetLevel());
            // タイプに応じて補正値を与える
            // 補正値はinterfaceを継承した側で決める
            statsValue += this.GetAdditiveModifier(stats);
            statsValue = statsValue * (1 + (this.GetPrecentageModifer(stats) / 100f));
            return statsValue;
        }

        private int CalculateLevel()
        {
            // プレイヤー以外(敵とか)は経験値コンポーネントがないので初期設定値を返す
            var experience = this.GetComponent<Experience>();
            if (experience == null) return this.startingLevel;

            // 最大レベルの一つ前のレベル値を取得
            var penultimateLevel = this.progression.GetLevels(Stats.ExperienctToLevelup, this.charcterClass);
            for (int level = 1; level <= penultimateLevel; ++level)
            {
                var compExperience = this.progression.GetStats(Stats.ExperienctToLevelup, this.charcterClass, level);
                if (compExperience > experience.GetPoints())
                {
                    return level;
                }
            }

            // 経験値リストに該当しないのが見つからない場合は最大レベルを返す
            return penultimateLevel + 1;
        }

        private float GetAdditiveModifier(Stats stats)
        {
            if (!this.useAdditiveModifer) return 0f;

            float total = 0f;
            foreach (var modifiterProvider in this.GetComponents<IModifiterProvider>())
            {
                foreach (var addValue in  modifiterProvider.GetAdditiveModifiters(stats))
                {
                    total += addValue;
                }
            }

            return total;
        }

        private float GetPrecentageModifer(Stats stats)
        {
            if (!this.useAdditiveModifer) return 0f;

            float total = 0f;
            foreach (var modifiterProvider in this.GetComponents<IModifiterProvider>())
            {
                foreach (var addValue in  modifiterProvider.GetPercentageModifiters(stats))
                {
                    total += addValue;
                }
            }

            return total;
        }
    }
}

