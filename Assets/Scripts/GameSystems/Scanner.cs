using System.Collections;
using System.Collections.Generic;
using UFO_PickupStuff;
using UnityEditor;
using UnityEngine;

//credit for remapping https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/  Jessy
public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

public class Scanner : MonoBehaviour
{
    private List<Bone> bones = new List<Bone>();

    [SerializeField] private float MaxRed;

    [SerializeField] private float MaxGreen;

    [SerializeField] private float sensitivityMin = 1;

    [SerializeField] private float sensitivityMax = 25;

    [SerializeField] private RectTransform gradientTransform;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Bone bone in FindObjectsOfType<Bone>())
        {
            bones.Add(bone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float closestMagnitude = FindClosestMagnitude();
        
        //clamp closestMagnitude to be between sensitivityMin and Sensitivity Max
        closestMagnitude = Mathf.Clamp(closestMagnitude, this.sensitivityMin, this.sensitivityMax);
        
        //remap value to be between Max Red and Max Green
        float remappedMagnitude = closestMagnitude.Remap(this.sensitivityMin, sensitivityMax, MaxRed, MaxGreen);
        this.gradientTransform.anchoredPosition = new Vector3(-remappedMagnitude, 2.3724f, 0);
    }

    private float FindClosestMagnitude()
    {
        float closestMagnitude = float.MaxValue;
        for (int i = 0; i < this.bones.Count; i++)
        {
            //if this bone is on the skelly stand skip past it
            if (bones[i].IsEnabled == false)
            {
                continue;
            }
            
            //find the magnitude of this bone and update closest magnitude if need be
            float currentMagnitude = (bones[i].gameObject.transform.position - gameObject.transform.position).magnitude;
            if (currentMagnitude < closestMagnitude)
            {
                closestMagnitude = currentMagnitude;
            }
        }

        return closestMagnitude;
    }
}
