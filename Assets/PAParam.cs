using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAParam : MonoBehaviour
{


    [SerializeField] public float time;
    public static int selectionIndex;

    void Start()
    {
        StartCoroutine(Playing(time));
    }

    private IEnumerator Playing(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            selectionIndex = Random.Range(0, 12);
            //Debug.Log("we invoking with this one");
        }
    }
}