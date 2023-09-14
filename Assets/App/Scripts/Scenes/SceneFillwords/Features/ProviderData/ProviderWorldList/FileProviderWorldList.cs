using System;
using System.IO;
using UnityEngine;

public class FileProviderWorldList : IProviderWorldList
{
    private const string WORLD_LIST_PATH = "Fillwords/words_list";
    public string[] LoadWorldList()
    {
        TextAsset worldListAsset = Resources.Load<TextAsset>(WORLD_LIST_PATH);

        if (worldListAsset == null )
        {
            throw new FileNotFoundException($"The file with path {WORLD_LIST_PATH} was not found");
        }

        string[] worldList = worldListAsset.text.Split("\r\n");

        return worldList;
    }
}
