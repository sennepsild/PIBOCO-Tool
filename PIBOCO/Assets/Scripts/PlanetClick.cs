using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetClick : MonoBehaviour
{
    public Animator anim;
    public string Trigger;
    private void OnMouseDown()
    {
        anim.SetTrigger(Trigger);


    }
}
