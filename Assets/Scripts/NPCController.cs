using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    public void interact()
    {
        DialogManager.Instance.ShowDialog(dialog);
    }
}
