using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{   
    public GameObject wholeObject;
    public GameObject fracturedObject;

    private void Start()
    {
        //wholeObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            FractureEffect();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            WholeEffect();
        }
    }

    public void FractureEffect()
    {
        Instantiate(fracturedObject, transform.position, transform.rotation, GameObject.Find("EarthSystem").transform);
        Destroy(gameObject);
    }

    public void WholeEffect()
    {
        Instantiate(wholeObject, transform.position, transform.rotation, GameObject.Find("EarthSystem").transform);
        Destroy(gameObject);
    }

}
