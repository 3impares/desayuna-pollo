using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyOnline : MonoBehaviour
{
    public DataPlayer[] players = new DataPlayer[4]; //lista de data players que manega el host ya ctualiza al local cada X tiempo o evento
    
    //public string username;
    public TextMeshProUGUI currentPlayer;

    private GameObject playersList;
    private TextMeshProUGUI ownPlayer_playerList;

    private string[] filesSkins;
    private GameObject div_skins;
    public GameObject skinPrefab;

    private DataPlayer player;

    public GameObject nickText;

    private int currentKey = -1;
    public GameObject warning;
    public TextMeshProUGUI[] keys;

    public Button done;
    private TextMeshProUGUI doneButtonText;
    private string playerState = "";

    // Start is called before the first frame update
    void Start()
    {   
        playerState = "No preparado";
        
        player = new DataPlayer(Random.Range(10000,99999));
        currentPlayer.text = "Player ID: " + player.id;

        loadCurrentPlayerInList();

        loadSkins();

    }

    // Update is called once per frame
    void Update()
    {
        checkSkins();
    }

    private void checkSkins(){
        bool skinChosen = false;
        Skin [] listaSkins = div_skins.GetComponentsInChildren<Skin>();
        foreach (Skin s in listaSkins)
        {
            if (s.chosen)
            {
                skinChosen = true;
            } 
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
        print(currentKey);
    }
    
    void OnGUI()
    {   
        if (currentKey!=-1)
        {
            warning.SetActive(true);
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown && e.keyCode != KeyCode.None)
            {
                if (player.keys == null)
                {
                    player.keys = new KeyCode[4];
                    print("inicializo el keycodes");
                }
                player.keys[currentKey] = e.keyCode;
                keys[currentKey].text = e.keyCode.ToString();
                if (currentKey < 3 && incorrectKeys())
                    currentKey++;
                else
                    currentKey = -1;
                warning.SetActive(false);
            }
        }
    }

    private bool incorrectKeys()
    {
        bool incorrect = false;
        if (player.keys != null)
        {
            foreach (var key in player.keys)
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

    public void inputNick()
    {
        player.nickname = nickText.GetComponent<TMP_InputField>().textComponent.text;
        //Actualiza la etiqueta de la lista de jugadores
        ownPlayer_playerList.text = player.id + " : " + (player.nickname ?? "Nick not assigned") + "\n" + playerState;
    }

    public void OnClickReady(){
        doneButtonText = done.GetComponentInChildren<TextMeshProUGUI>();
        if (doneButtonText.text == "¡Listo!"){
            done.GetComponentInChildren<TextMeshProUGUI>().text = "¡No estoy listo!";
            playerState = "Preparado";
        }else{
            done.GetComponentInChildren<TextMeshProUGUI>().text = "¡Listo!";
            playerState = "No preparado";
        }
        ownPlayer_playerList.text = player.id + " : " + (player.nickname ?? "Nick not assigned") + "\n" + playerState;
        printDataPlayer();
    }

    public void loadCurrentPlayerInList(){
        playersList = GameObject.Find("PlayersList");
        /*playersList.AddComponent<TextMeshProUGUI>;
        playersList.GetComponent<TextMeshProUGUI>;
        ownPlayer_playerList.text = player.id + " : " + player.nickname;*/

        GameObject[] playersListArray = new GameObject[4];


        for (int i = 0; i < 4; i++)
        {
            // Giving the objects a nice and logical name taking into account their position in the 2d "grid".
            playersListArray[i] = new GameObject("playersList: [" + i + "]");

            // They need to be moved under a parent in the UI.
            playersListArray[i].transform.parent = playersList.transform;

            // Changes their transform type (UI uses rect transform).
            playersListArray[i].AddComponent<RectTransform>();

            // Moves current looped object to position i & j. If you use i * 10 their position "i" will have 10 units of space apart, etc.
            //playersListArray[i].transform.localPosition = new Vector3(i*10, j, 1f);

            // Now we can finally add the TMProUI component.
            playersListArray[i].AddComponent<TextMeshProUGUI>();

            // You can modify each property of any component of this current object being looped.
            // Here is its text being changed as an example.
            playersListArray[i].GetComponent<TextMeshProUGUI>().fontSize = 30;
            playersListArray[i].GetComponent<TextMeshProUGUI>().text = "Player" + (i+1) + " : " + "Nick not assigned" + "\n" + "No preparado" ; //(players[i].nickname ?? "Not assigned"

            // Now that you know this, you can change their pivot, size, anchors and etc and better adjust them to your own project.
        }
        
        ownPlayer_playerList = playersListArray[0].GetComponent<TextMeshProUGUI>();
        ownPlayer_playerList.text = player.id + " : " + (player.nickname ?? "Nick not assigned") + "\n" + playerState;
    }

    public void printDataPlayer(){
        print("id: " + player.id);
        print("nickname: " + player.nickname);
        print("keys[0]: " + player.keys[0]);
        print("keys[1]: " + player.keys[1]);
        print("keys[2]: " + player.keys[2]);
        print("keys[3]: " + player.keys[3]);
        print("skin: " + player.skin);
    }

}
