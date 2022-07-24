using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public static void ClickLocalButton()
    {
        ChangeScene.loadLocalConfig("Local");
    }
}
