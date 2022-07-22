using TMPro;
using UnityEngine;

public class NumPlayers : MonoBehaviour
{
    public GameObject canvasNumber;
    public GameObject canvasPlayer;
    public GameObject playerPrefab;
    public DataPlayer[] players;
    private int numplayers;
    public int currentPlayer;
    public TextMeshPro nickText;
    public void selectNumPlayers(int num)
    {
        numplayers = num;
        canvasNumber.SetActive(false);

        for (var i = 0; i < num; i++)
        {
            players[i] = new DataPlayer(i);
        }

        currentPlayer = 0;
        
        canvasPlayer.SetActive(true);
    }

    public void playerReady()
    {
        players[currentPlayer].nickname = nickText.text;
        
    }
}
