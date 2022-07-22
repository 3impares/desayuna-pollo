
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

        //keys
        
        
        //skin
        
        
        if (currentPlayer == numplayers-1)
        {
            ChangeScene.loadScene("Game");
        }else{
            nickText.GetComponent<TMP_InputField>().Select();
            nickText.GetComponent<TMP_InputField>().text = "";
            
            currentPlayer++;
            currentText.text = "Player " + (int)(currentPlayer + 1);
        }
    }
}
