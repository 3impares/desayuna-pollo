using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    public bool locked;
    public bool chosen;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        chosen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(locked){
            GetComponent<Button>().interactable = false;
        }
        

    }

    public void chooseSkin()
    {
        chosen = true;
    }

}
