using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour 
{ 
    public int w, h, x, y;
    public int id;
    public bool[,] hwalls, vwalls;
 
    public override void OnNetworkSpawn() {
        if (!IsOwner) Destroy(this);
    }
    void Update()
    {
        var dirs = new[]
        {
            (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
            (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
            (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
            (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
        };
        if (id == 2)
        {
            dirs = new[]
            {
                (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.LeftArrow),
                (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.RightArrow),
                (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.DownArrow),
                (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.UpArrow),
            };
        }
        foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
            if (Input.GetKeyDown(k))
                if (wall[wx, wy])
                    transform.position = Vector3.Lerp(transform.position, new Vector3(nx, ny), 0.1f);
                else (x, y) = (nx, ny);

        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y), Time.deltaTime * 12);
    }

    public void setPlayer(int w, int h, int x, int y, bool[,] hwalls, bool[,] vwalls)
    {
        this.w = w;
        this.h = h;
        this.x = x;
        this.y = y;
        this.hwalls = hwalls;
        this.vwalls = vwalls;
    }
}