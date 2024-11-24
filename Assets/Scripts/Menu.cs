using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject message_mid;
    public GameObject panel_menu;

    public void OpenMsg_Menu()
    {
        message_mid.SetActive(true);
        panel_menu.SetActive(true);
    }
}
