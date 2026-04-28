using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShengLi : MonoBehaviour
{
    public GameObject hit;
    public GameObject Player;
    public GameObject shengli;
    public void Update()
    {
        float dis = Vector3.Distance(transform.position, Player.transform.position);

        if (dis > 1)
        {
            hit.SetActive(false );
        }
        else
        {
            hit.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.GetComponent<Player>().enabled = false;
                shengli.gameObject.SetActive(true );
            }
        }
    }
}
