
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
    private int currentPlayer=-1;
    public GameObject nickText;

    private GameObject div_skins;

    private string[] filesSkins;
    public GameObject skinPrefab;

    private int currentKey = -1;
    public GameObject warning;
    public TextMeshProUGUI[] keys;
    public Button done;
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

        loadSkins();
    }

    public void playerReady()
    {

        players[currentPlayer].nickname = new String(nickText.GetComponent<TMP_InputField>().text.ToString());

        for (var i = 0; i < 4; i++)
        {
            players[currentPlayer].keys[i] = (KeyCode)Enum.Parse(typeof(KeyCode), keys[i].text);
        }
        
        Skin[] listaSkins;
        div_skins = GameObject.Find("skins");

        listaSkins = div_skins.GetComponentsInChildren<Skin>();
        foreach(Skin s in listaSkins){
            if(s.chosen){
                print("Seleccionado la skin: " + s.img.sprite);
                s.locked = true;
                s.chosen = false;
                players[currentPlayer].skin = s.img.sprite;
                break;
            } //incompleto, falta la comunicación a DataPLayer
        }
        print("Seleccionada la skin: " + players[currentPlayer].skin);
        
        if (currentPlayer == numplayers-1)
        {
            ChangeScene.loadScene("Game",players);
        }else{
            print(players[0].nickname);
            nickText.GetComponent<TMP_InputField>().Select();
            nickText.GetComponent<TMP_InputField>().text = "";
            for (var i = 0; i < 4; i++)
            {
                keys[i].text = "-";
            }
            currentKey = -1;
            currentPlayer++;
            currentText.text = "Player " + (int)(currentPlayer + 1);
            print(players[0].nickname);
        }
    }

    private void loadSkins(){

        string path = @"Assets/Resources/skins/";
        
        filesSkins = System.IO.Directory.GetFiles(path, "*.asset");
        
        div_skins = GameObject.Find("skins");

        foreach(string filePath in filesSkins){
            GameObject newObj = Instantiate(skinPrefab, div_skins.transform);
            Sprite spriteLoad = Resources.Load<Sprite>(filePath.Replace("Assets/Resources/","").Replace(".asset",""));
            newObj.GetComponent<Image>().sprite = spriteLoad;
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
                if (currentKey < 3 && incorrectKeys())
                    currentKey++;
                else
                    currentKey = -1;
                warning.SetActive(false);
            }
        }
    }

    private void Update()
    {
        done.interactable = valid();
    }

    public void inputNick()
    {
        players[currentPlayer].nickname = nickText.GetComponent<TMP_InputField>().text;
    }

    private bool valid()
    {
        if (currentPlayer==-1 || players==null || players[currentPlayer] == null)
        {
            return false;
        }
        bool nickvoid = players[currentPlayer].nickname == null || players[currentPlayer].nickname == "";
        bool badkeys = incorrectKeys();
        bool skinChosen = false;
        
        Skin [] listaSkins = div_skins.GetComponentsInChildren<Skin>();
        foreach (Skin s in listaSkins)
        {
            if (s.chosen)
            {
                skinChosen = true;
            } 
        }


        return !nickvoid && !badkeys && skinChosen;
    }

    private bool incorrectKeys()
    {
        bool incorrect = false;
        if (players[currentPlayer].keys != null)
        {
            foreach (var key in players[currentPlayer].keys)
            {
                if (key == KeyCode.None || key == KeyCode.Minus)
                {
                    incorrect = true;
                }
            }
        }
        else
        {
            incorrect = true;
        }

        return incorrect;
    }
}
