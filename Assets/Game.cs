// Made from scratch in just 8 minutes 23 seconds
// Watch this video to see how:
// https://youtu.be/aP9eKrnyxe4
// Play online here:
// https://nns2009.itch.io/just-maze

using Cinemachine;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Game : NetworkBehaviour
{
    public Player[] players;
    public float holep;
    public int w, h, x, y;
    public bool[,] hwalls, vwalls;
    public Transform level, goal;
    public GameObject floor, wall;
    public CinemachineVirtualCamera cam;
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) Destroy(this);
    }

    void Start()
    {
        foreach (Transform child in level)
            Destroy(child.gameObject);

        hwalls = new bool[w + 1, h];
        vwalls = new bool[w, h + 1];
        var st = new int[w, h];

        void dfs(int x, int y)
        {
            st[x, y] = 1;
            Instantiate(floor, new Vector3(x, y), Quaternion.identity, level);

            var dirs = new[]
            {
                (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
                (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
                (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
                (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
            };
            foreach (var (nx, ny, bwall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
                if (!(0 <= nx && nx < w && 0 <= ny && ny < h) || (st[nx, ny] == 2 && Random.value > holep))
                {
                    bwall[wx, wy] = true;
                    Instantiate(wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), level);
                }
                else if (st[nx, ny] == 0) dfs(nx, ny);
            st[x, y] = 2;
        }
        dfs(0, 0);

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        
        goal.position = new Vector3(x, y);
        foreach (Player p in players)
        {
            var localx = 0;
            var localy = 0;
            do
            {
                localx = Random.Range(0, w);
                localy = Random.Range(0, h);
                p.transform.position = new Vector3(localx, localy);
            } 
            while (Vector3.Distance( p.transform.position, goal.position) < (w + h) / 4);
            p.setPlayer(localx, localy, hwalls, vwalls);
        }
        
        cam.m_Lens.OrthographicSize = Mathf.Pow(w / 3 + h / 2, 0.7f) + 1;
    }

    void Update()
    {
        foreach (Player p in players)
        {
            if (Vector3.Distance(p.transform.position, goal.position) < 0.12f)
            {
                if (Random.Range(0, 5) < 3) w++;
                else h++;
                Start();
            }
        }
    }
}