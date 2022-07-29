// Made from scratch in just 8 minutes 23 seconds
// Watch this video to see how:
// https://youtu.be/aP9eKrnyxe4
// Play online here:
// https://nns2009.itch.io/just-maze

using System;
using Cinemachine;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameOnline : NetworkBehaviour
{
    private GameObject[] players;
    public float holep;
    public int w, h, x, y;
    public bool[,] hwalls, vwalls;
    public Transform level, goal;
    public GameObject floor, wall, playerPrefab;
    public CinemachineVirtualCamera cam;

    //contador de jugadores, provisional mientras se automatiza el nombre de los players
    public int numPlayers = 0;

    public Text scores;

    public int currentLevel = 0;
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) Destroy(this);
    }

    void generatePlayerOnline()
    {
        GameObject div_players = GameObject.Find("Players");
        GameObject player = Instantiate(playerPrefab, div_players.transform);
        //Player p = player.GetComponent<Player>(); //Faltara una pantalla de inicialización que se le pase un DataPlayer
        //p.initPlayer(dataPlayers[i]);
        Array.Resize(ref players, players.Length+1);
        players[players.Length-1]=player;
    }


    void Start()
    {

        print("Server:");
        print(ChangeScene.server);
        if (currentLevel == 0 && !ChangeScene.server)
        {
            players = new GameObject[0];
            //generatePlayerOnline();
            NetworkManager.StartClient();
        }
        
        if (currentLevel == 0 && ChangeScene.server)
        {
            NetworkManager.StartServer();
        }
        
        currentLevel++;

        updateScores();
        
        foreach (Transform child in level)
            Destroy(child.gameObject);

        hwalls = new bool[w + 1, h];
        vwalls = new bool[w, h + 1];
        //en realidad hwalls guarda los muros verticales y vwalls los horizontales
        var st = new int[w, h];
        //st es una matriz que representa las casillas para cada casilla el valor es:
        //0:casilla no inicializada
        //1:casilla inicializada pero no terminada
        //2: casilla con las 4 paredes hechas

        void dfs(int x, int y)
        {
            st[x, y] = 1;
            Instantiate(floor, new Vector3(x, y), Quaternion.identity, level);
            //la matriz dirs se estructura de las siguiente manera:
            //(nx, ny, bwall, wx, wy, sh, ang, k)
            //nx y ny te dan las coordenadas de la casilla adyacente en esa posicion
            //bwall te indica que matriz de muros hay que mirar hwalls (muros verticales) para movimientos en horizontal y viceversa
            //wx y wy cooordenadas del muro dentro de su matriz
            //sh y ang sirven para colocar el gameobject del muro en la posicion que toca
            //k es la tecla para mover en esa direccion que no se usa aqui y la voy a quitar
            var dirs = new[]
            {
                (x - 1, y, hwalls, x, y, Vector3.right, 90),//derecha
                (x + 1, y, hwalls, x + 1, y, Vector3.right, 90),//izquierda
                (x, y - 1, vwalls, x, y, Vector3.up, 0),//abajo
                (x, y + 1, vwalls, x, y + 1, Vector3.up, 0),//arriba
            };

            foreach (var (nx, ny, bwall, wx, wy, sh, ang) in dirs.OrderBy(d => Random.value))
            {
                if ((nx < 0 || nx >= w || ny < 0 || ny >= h) || (st[nx, ny] == 2 && Random.value > holep))
                {//la primera condicion es para ver si es un borde del tablero siempre pone muro
                    //la segunda solo pone si la casilla esta completa ya
                    bwall[wx, wy] = true;
                    Instantiate(wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), level);
                }
                else if (st[nx, ny] == 0)
                {
                    dfs(nx, ny);
                }
            }
            st[x, y] = 2;
        }
        dfs(0, 0);

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        
        goal.position = new Vector3(x, y);
        if (players != null)
        {
            foreach (GameObject p in players)
            {
                //contador de jugadores, provisional mientras se automatiza el nombre de los players
                numPlayers++;

                var localx = 0;
                var localy = 0;
                do
                {
                    localx = Random.Range(0, w);
                    localy = Random.Range(0, h);
                    p.transform.position = new Vector3(localx, localy);
                } while (Vector3.Distance(p.transform.position, goal.position) < (w + h) / 4);

                print("walls");
                p.GetComponent<Player>().setPlayer(localx, localy, hwalls, vwalls);
            }
        }

        cam.m_Lens.OrthographicSize = Mathf.Pow(w / 3 + h / 2, 0.7f) + 1;
    }

    void Update()
    {
        if (players != null)
        {
            foreach (GameObject p in players)
            {
                if (Vector3.Distance(p.gameObject.transform.position, goal.position) < 0.12f)
                {
                    if (Random.Range(0, 5) < 3) w++;
                    else h++;

                    //Se actualiza la puntuación al ganador
                    p.GetComponent<Player>().victory();
                    Debug.Log("Nivel " + this.currentLevel + " terminado: El jugador " +
                              p.GetComponent<Player>().getNickname() + " gana un punto y ahora tiene " +
                              p.GetComponent<Player>().getScore() + " puntos");

                    Start();
                }
            }
        }


    }

    void updateScores() {
        
        scores.text = "";

        if (players != null)
        {
            foreach (GameObject p in players)
            {
                print(p.GetComponent<Player>().getNickname());
                scores.text += p.GetComponent<Player>().getNickname().PadRight(12) + "\t" +
                               p.GetComponent<Player>().getScore() + "\n";
            }
        }

    }

}