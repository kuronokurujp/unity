using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progresstion", menuName = "Stats/New Progresstion", order = 0)]
    public class Progresstion : ScriptableObject
    {
        [SerializeField] 
        private ProgressionCharacterStats[] characterStats = null;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;

        public float GetStats(Stats stats, CharacterClass character, int level)
        {
            this.BuildLookupTable();

            var levels = this.lookupTable[character][stats];
            level = Mathf.Clamp(level - 1, 0, levels.Length - 1);

            return levels[level];
        }

        public int GetLevels(Stats stats, CharacterClass character)
        {
            this.BuildLookupTable();

            var levels = this.lookupTable[character][stats];
            return levels.Length;
        }

        private void BuildLookupTable()
        {
            if (this.lookupTable != null) return;

            this.lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();
            foreach (var progressionCharacterStats in this.characterStats)
            {
                var startStatsDictionary = new Dictionary<Stats, float[]>();
                foreach (var progressionStats in progressionCharacterStats.stats)
                {
                    startStatsDictionary[progressionStats.stats] = progressionStats.levels;
                }
                this.lookupTable[progressionCharacterStats.characterClass] = startStatsDictionary; 
            }
        }

        [System.Serializable]
        private class ProgressionCharacterStats
        {
            public CharacterClass characterClass;
            public ProgressionStats[] stats;
        }

        [System.Serializable]
        private struct ProgressionStats
        {
            public Stats stats;
            public float[] levels;
        }
    }
}
