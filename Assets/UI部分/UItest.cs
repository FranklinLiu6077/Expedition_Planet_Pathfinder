using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItest : MonoBehaviour
{
    public Animator ani;
    public Animator opani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            ani.SetTrigger("go");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
           opani.SetTrigger("go");
        }
    }
}
