﻿using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour 
{ 
    public int x, y;
    public int id;
    public bool[,] hwalls, vwalls;
    private int currentScore;
    public string nickname;
    public KeyCode[] keys;
    public Sprite skin;

    public override void OnNetworkSpawn() {
        if (!IsOwner) Destroy(this);
    }
    void Update()
    {
        var dirs = new[]
        {
            (x - 1, y, hwalls, x, y, Vector3.right, 90, keys[0]),
            (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, keys[3]),
            (x, y - 1, vwalls, x, y, Vector3.up, 0, keys[1]),
            (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, keys[2]),
        };
        foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
            if (Input.GetKeyDown(k))
                (x, y) = (nx, ny);
                //if (wall[wx, wy])
                 //   transform.position = Vector3.Lerp(transform.position, new Vector3(nx, ny), 0.1f);
                //else (x, y) = (nx, ny);

        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y), Time.deltaTime * 12);
    }

    public void setPlayer(int x, int y, bool[,] hwalls, bool[,] vwalls)
    {
        this.x = x;
        this.y = y;
        this.hwalls = hwalls;
        this.vwalls = vwalls;
    }

    public int getScore() {
        return this.currentScore;
    }

    public string getNickname() {
        return this.nickname;
    }

    public void victory() {
        this.currentScore += 1;
    }

    public void initPlayer (DataPlayer dp) {
        //Al crearse el jugador cada partida, la puntuación es cero.
        this.currentScore=0;
        nickname = dp.nickname;
        keys = dp.keys;
        skin = dp.skin;
        print(nickname);

        GetComponent<SpriteRenderer>().sprite = skin;
    }

   

    public bool isParentActive(){
        return true;//transform.parent.activeSelf;
    }
}