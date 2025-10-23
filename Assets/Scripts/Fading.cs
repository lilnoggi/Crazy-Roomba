using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        Color c = GetComponentInChildren<MeshRenderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            GetComponentInChildren<MeshRenderer>().material.color = c;
                yield return null;
        }
    }
}
