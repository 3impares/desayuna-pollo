using UnityEngine.SceneManagement;


public static class ChangeScene
{
    public static DataPlayer[] players;

    public static void loadScene(string scene, DataPlayer[] dp)
    {
        players = dp;
        SceneManager.LoadScene(scene);
    }
}
