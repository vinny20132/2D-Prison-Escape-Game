using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState{ FreeRoam, Dialog}

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    

    GameState currentState;

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            currentState = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(currentState == GameState.Dialog)
            currentState = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        if(currentState == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }

        else if(currentState == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
