using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public static void ClickLocalButton()
    {
        ChangeScene.loadScene("Local");
    }
    public static void ClickCreateMatchButton()
    {
        ChangeScene.loadScene("GameOnline", true);
    }
    public static void ClickJoinMatchButton()
    {
        ChangeScene.loadScene("GameOnline", false);
    }
}
