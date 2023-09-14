using System.IO;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private const string LEVEL_DIRECTORY_PATH = "WordSearch/Levels/";
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string pathToFile = LEVEL_DIRECTORY_PATH + levelIndex;

            TextAsset levelAsset = Resources.Load<TextAsset>(pathToFile);

            if (levelAsset == null)
            {
                throw new FileNotFoundException($"The file with path {pathToFile} was not found");
            }

            string jsonLevelText = levelAsset.text;

            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonLevelText);

            return levelInfo;
        }
    }
}