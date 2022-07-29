using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public static class ChangeScene
{
    public static DataPlayer[] players;
    public static bool server = false;

    public static void loadScene(string scene, DataPlayer[] dp)
    {
        players = dp;
        SceneManager.LoadScene(scene);
    }

    public static void loadScene(string scene, bool serv)
    {
        server = serv;
        SceneManager.LoadScene(scene);
    }
    
    public static void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
