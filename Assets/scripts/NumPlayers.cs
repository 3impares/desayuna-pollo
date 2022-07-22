
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

    private string[] filesSkins;
    public GameObject skinPrefab;


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
        //nick
        players[currentPlayer].nickname = nickText.GetComponent<TMP_InputField>().text;

        //keys
        
        
        //skin
        Skin[] listaSkins;
        GameObject div_skins = GameObject.Find("skins");

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
            ChangeScene.loadScene("Game");
        }else{
            nickText.GetComponent<TMP_InputField>().Select();
            nickText.GetComponent<TMP_InputField>().text = "";
            
            currentPlayer++;
            currentText.text = "Player " + (int)(currentPlayer + 1);
        }
    }
    

    private void loadSkins(){

        string path = @"Assets/Resources/skins/";
        
        filesSkins = System.IO.Directory.GetFiles(path, "*.asset");
        print(filesSkins[0]); //Aquí sí lo coge
        
        GameObject div_skins = GameObject.Find("skins");

        foreach(string filePath in filesSkins){
            GameObject newObj = Instantiate(skinPrefab, div_skins.transform);
            //div_skins.AddComponent(typeof(Image));
            print(filePath.Replace("Assets/Resources/","").Replace(".asset",""));
            Sprite spriteLoad = Resources.Load<Sprite>(filePath.Replace("Assets/Resources/","").Replace(".asset",""));
            newObj.GetComponent<Image>().sprite = spriteLoad;

            
        }

    }



}
