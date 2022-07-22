
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumPlayers : MonoBehaviour
{
    public GameObject canvasNumber;
    public GameObject canvasPlayer;
    public TextMeshProUGUI currentText;
    public DataPlayer[] players;
    private int numplayers;
    private int currentPlayer;
    public GameObject nickText;
    private int currentKey = -1;
    public GameObject warning;
    public TextMeshProUGUI[] keys;
    public void selectNumPlayers(int num)
    {
        numplayers = num;
        canvasNumber.SetActive(false);
        players = new DataPlayer[num];
        
        for (var i = 0; i < num; i++)
        {
            players[i] = new DataPlayer(i);
        }

        currentPlayer = 0;
        canvasPlayer.SetActive(true);
        currentText.text = "Player " + (int)(currentPlayer + 1);
    }

    public void playerReady()
    {
        //nick
        players[currentPlayer].nickname = nickText.GetComponent<TMP_InputField>().text;
        
        //skin
        if (currentPlayer == numplayers-1)
        {
            ChangeScene.loadScene("Game");
        }else{
            nickText.GetComponent<TMP_InputField>().Select();
            nickText.GetComponent<TMP_InputField>().text = "";
            for (var i = 0; i < 4; i++)
            {
                players[currentPlayer].keys[i] = (KeyCode)Enum.Parse(typeof(KeyCode), keys[i].text);
                keys[i].text = "-";
            }
            currentPlayer++;
            currentText.text = "Player " + (int)(currentPlayer + 1);
        }
    }

    public void listenerKeys(int key)
    {
        currentKey = key;
    }
    
    void OnGUI()
    {
        if (currentKey!=-1)
        {
            warning.SetActive(true);
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown && e.keyCode != KeyCode.None)
            {
                if (players[currentPlayer].keys == null)
                {
                    players[currentPlayer].keys = new KeyCode[4];
                }
                players[currentPlayer].keys[currentKey] = e.keyCode;
                keys[currentKey].text = e.keyCode.ToString();
                currentKey = -1;
                warning.SetActive(false);
            }
        }
 
    }
}
