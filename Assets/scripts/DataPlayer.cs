using UnityEngine;
public class DataPlayer : ScriptableObject
{
    public int id;
    public string nickname;
    public KeyCode[] keys;
    public Sprite skin;

    public DataPlayer(int id)
    {
        this.id = id;
    }
}