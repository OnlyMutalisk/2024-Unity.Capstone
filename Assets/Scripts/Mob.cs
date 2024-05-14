using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
    public Slider HP_slider;
    public static List<Mob> Mobs;
    public virtual float HP { get; set; }
    public virtual int i { get; set; }
    public virtual int j { get; set; }

    private void Start() { Mobs.Add(this); }


}
