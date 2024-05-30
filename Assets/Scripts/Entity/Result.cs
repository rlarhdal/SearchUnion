using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private UIInventory inventory;
    private int cnt;
    private bool isSuccess;

    public GameObject resultImg;
    public TextMeshProUGUI resultText;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckItem();
            GameResult();
            CharacterManager.Instance.player.controller.Result();
        }
    }

    private void GameResult()
    {
        resultImg.SetActive(true);
        if (isSuccess)
        {
            resultText.text = "성공!";
        }
        else
        {
            resultText.text = "실패...";
        }
    }

    private void CheckItem()
    {
        for (int i = 0; i < CharacterManager.Instance.player.inventoryItems.Count; i++)
        {
            if (CharacterManager.Instance.player.inventoryItems[i].type == ItemType.Quest)
            {
                cnt++;
            }
        }

        if(cnt == 3)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }
}
