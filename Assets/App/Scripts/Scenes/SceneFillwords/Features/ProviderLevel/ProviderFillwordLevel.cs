using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private const char WORD_SEPARATOR = ' ';
        private const char INDEXES_SEPARATOR = ';';

        private readonly IProviderLevelPack _providerLevelPack;
        private readonly IProviderWorldList _providerWorldList;
        public ProviderFillwordLevel(IProviderLevelPack providerLevelPack, IProviderWorldList providerWorldList)
        {
            _providerLevelPack = providerLevelPack;
            _providerWorldList = providerWorldList;
        }

        public GridFillWords LoadModel(int level)
        {
            int indexLevel = level - 1;

            string[] levels = _providerLevelPack.LoadLevels();
            string[] wordList = _providerWorldList.LoadWorldList();

            string levelLine = GetLevelLine(indexLevel, levels);

            if (!TryGetWordIndexesMap(levelLine, wordList, out var wordIndexesMap, out var size))
            {
                return null;
            }

            GridFillWords gridFillWords = CreateGridFillWords(wordIndexesMap, size);

            return gridFillWords;
        }

        private static bool TryGetWordIndexesMap(string levelLine, string[] wordList, out Dictionary<string, int[]> wordIndexesMap, out int size)
        {
            int maxCharIndex = -1;
            int sumWordsLength = 0;

            wordIndexesMap = new Dictionary<string, int[]>();
            size = -1;

            string[] wordIndexesString = levelLine.Split(WORD_SEPARATOR);
            ISet<int> closedInexes = new HashSet<int>();

            for (int i = 0; i < wordIndexesString.Length; i += 2)
            {
                List<int> indexes = new List<int>();

                string[] indexesString = wordIndexesString[i + 1].Split(INDEXES_SEPARATOR);

                for (int j = 0; j < indexesString.Length; j++)
                {
                    if (int.TryParse(indexesString[j], out int charIndex) && closedInexes.Contains(charIndex))
                    {
                        return false;
                    }

                    if (maxCharIndex < charIndex)
                    {
                        maxCharIndex = charIndex;
                    }

                    closedInexes.Add(charIndex);
                    indexes.Add(charIndex);
                }

                if (int.TryParse(wordIndexesString[i], out int wordIndex) && wordIndex >= wordList.Length)
                {
                    return false;
                }

                string word = wordList[wordIndex];

                if (word.Length != indexes.Count)
                {
                    return false;
                }

                wordIndexesMap.Add(word, indexes.ToArray());
                sumWordsLength += indexes.Count;
            }

            if (sumWordsLength != maxCharIndex + 1)
            {
                return false;
            }

            size = (int)Math.Sqrt(maxCharIndex + 1);

            if (size * size != sumWordsLength)
            {
                return false;
            }


            return true;
        }

        private static string GetLevelLine(int indexLevel, string[] levels)
        {
            string levelLine = null;
            while ((string.IsNullOrEmpty(levelLine) || string.IsNullOrWhiteSpace(levelLine)))
            {
                if (indexLevel >= levels.Length)
                {
                    throw new Exception();
                }

                levelLine = levels[indexLevel];
                indexLevel++;
            }

            return levelLine;
        }

        private static GridFillWords CreateGridFillWords(Dictionary<string, int[]> wordIndexesMap, int size)
        {
            GridFillWords gridFillWords = new GridFillWords(new Vector2Int(size, size));

            foreach (var pair in wordIndexesMap)
            {
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    int x, y;
                    x = pair.Value[i] / size;
                    y = pair.Value[i] % size;
                    gridFillWords.Set(x, y, new CharGridModel(pair.Key[i]));
                }
            }

            return gridFillWords;
        }
    }
}