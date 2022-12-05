using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelController : ControllerBaseModel
{
    public LevelModel ActiveLevel;
    [SerializeField] private List<LevelModel> levels;
    [SerializeField] private WorldObjectController worldObjectController;

    public override void Initialize()
    {
        base.Initialize();
        loadLevel();
        worldObjectController.Initialize();
    }

    private void loadLevel()
    {
        ActiveLevel = levels[PlayerDataModel.Data.LevelIndex];
    }

    public void NextLevel()
    {
        PlayerDataModel.Data.Level++;
        PlayerDataModel.Data.LevelIndex = PlayerDataModel.Data.LevelIndex + 1 < levels.Count ? PlayerDataModel.Data.LevelIndex + 1 : 0;
        PlayerDataModel.Data.Save();
    }

    [EditorButton]
    public void E_SaveLevel()
    {
#if UNITY_EDITOR
        LevelModel level = getActiveLevel();

        var path = EditorUtility.SaveFilePanel("Save Level", "Assets", "", "asset");
        if (path.Length > 0)
        {
            AssetDatabase.CreateAsset(level, path.Remove(0, path.IndexOf("Assets")));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        ActiveLevel = null;
#endif
    }
    private LevelModel getActiveLevel()
    {
        LevelModel levelData = ScriptableObject.CreateInstance<LevelModel>();

        GirlModel[] girls = FindObjectsOfType<GirlModel>();
        levelData.GirlData = new GirlDataModel[girls.Length];
        for (int i = 0; i < girls.Length; i++)
        {
            levelData.GirlData[i] = girls[i].GetDataModel();
            girls[i].SetDeactive();
        }

        DiamondModel[] collectables = FindObjectsOfType<DiamondModel>();
        levelData.CollectableData = new CollectableDataModel[collectables.Length];
        for (int i = 0; i < collectables.Length; i++)
        {
            levelData.CollectableData[i] = collectables[i].GetDataModel();
            collectables[i].SetDeactive();
        }

        ObstacleModel[] obstacles = FindObjectsOfType<ObstacleModel>();
        levelData.ObstacleData = new ObstacleDataModel[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++)
        {
            levelData.ObstacleData[i] = obstacles[i].GetDataModel();
            obstacles[i].SetDeactive();
        }

        return levelData;
    }
}
