using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            Dictionary<char, int> characterNumberMapResult = new Dictionary<char, int>();

            foreach (var word in words)
            {
                Dictionary<char, int> tempCharacterNumberMap = GetCharacterNumberMap(word);

                foreach (var characterNumberPair in tempCharacterNumberMap)
                {
                    if (characterNumberMapResult.ContainsKey(characterNumberPair.Key))
                    {
                        int numberCharacter = characterNumberPair.Value;

                        if (characterNumberMapResult[characterNumberPair.Key] < numberCharacter)
                        {
                            characterNumberMapResult[characterNumberPair.Key] = numberCharacter;
                        }
                    }
                    else
                    {
                        characterNumberMapResult.Add(characterNumberPair.Key, characterNumberPair.Value);
                    }
                }
            }

            List<char> result = new List<char>();

            foreach (var characterNumberPair in characterNumberMapResult)
            {
                int numberCharacter = characterNumberPair.Value;

                for (int i = 0; i < numberCharacter; i++)
                {
                    result.Add(characterNumberPair.Key);
                }
            }

            return result;
        }

        private Dictionary<char, int> GetCharacterNumberMap(string word)
        {
            Dictionary<char, int> characterNumberMap = new Dictionary<char, int>();

            for (int i = 0; i < word.Length; i++)
            {
                if (characterNumberMap.ContainsKey(word[i]))
                {
                    characterNumberMap[word[i]]++;
                }
                else
                {
                    characterNumberMap.Add(word[i], 1);
                }
            }

            return characterNumberMap;
        }
    }
}