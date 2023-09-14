using System.IO;
using System;
using UnityEngine;

public class FileProviderLevelPack : IProviderLevelPack
{
    private const string PACK_PATH = "Fillwords/pack_0";
    public string[] LoadLevels()
    {
        TextAsset packAsset = Resources.Load<TextAsset>(PACK_PATH);

        if (packAsset == null)
        {
            throw new FileNotFoundException($"The file with path {PACK_PATH} was not found");
        }

        string[] levelList = packAsset.text.Split("\r\n");

        return levelList;
    }
}
