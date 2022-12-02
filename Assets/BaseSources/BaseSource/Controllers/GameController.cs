using System.Collections;
using System.Collections.Generic;
using CubeGames.Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : ControllerBaseModel
{
    public static GameController Instance;
    public static bool IsPlayerWin;
    private GameStates currentState;
    public static GameStates CurrentState
    {
        get
        {
            return Instance.currentState;
        }
    }

    [SerializeField] ControllerBaseModel[] controllers;
    public GameStateEventModel OnStateChange;

    public override void Initialize()
    {
        base.Initialize();

        if (Instance == null)
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = -1;
        }
        else
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void Update()
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].ControllerUpdate(currentState);
        }
    }

    public static void ChangeState(GameStates state)
    {
        Instance.currentState = state;
        Instance.OnStateChange?.Invoke(state);
        for (int i = 0; i < Instance.controllers.Length; i++)
        {
            Instance.controllers[i].OnStateChange(state);
        }

        switch (state)
        {
            case GameStates.Main:
                break;
            case GameStates.Game:
                //AnalyticController.OnLevelStarted(PlayerDataModel.Data.Level);
                break;
            case GameStates.Win:
                //AnalyticController.OnLevelCompleted(PlayerDataModel.Data.Level);
                break;
            case GameStates.Lose:
                //AnalyticController.OnLevelFailed(PlayerDataModel.Data.Level);
                break;
            default:
                break;
        }
    }

    public static void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Test() { Debug.Log("Coin++"); }
}