using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HudController : MonoBehaviour
{
    public static HudController instacia;
    public Image lifebar;
    void Awake()
    {
        instacia = this;
        lifebar = GameObject.FindWithTag("Life").GetComponent<Image>();
    }
    
}
