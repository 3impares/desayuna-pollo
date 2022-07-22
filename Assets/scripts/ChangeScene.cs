using UnityEngine.SceneManagement;

public static class ChangeScene
{
    public static void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
