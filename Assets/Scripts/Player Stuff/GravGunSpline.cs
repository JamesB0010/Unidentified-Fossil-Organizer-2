using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UFO_PlayerStuff;
using UFO_UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class GravGunSpline : MonoBehaviour
{
    [FormerlySerializedAs("MySpline")] [FormerlySerializedAs("spline")] [SerializeField] private SplineContainer SplineContainer;

    private Spline spline;

    [SerializeField] private GameObject particleSystem;

    private ParticleSystem particleSystemSystem;

    [SerializeField] private CameraForwardsSampler forwardsSampler;

    [SerializeField]
    private bool objectIsPickedUp = false;

    [SerializeField] private Transform targetTransform;

    private SplineExtrude spineExtrudeComp;

    private Transform lastHeldObjectTransform;

    private GameObject PickedUpObject;
    
    [SerializeField] private PlayerUi playerUi;
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.particleSystemSystem = this.particleSystem.GetComponent<ParticleSystem>();
        this.particleSystemSystem.Stop();
        this.spineExtrudeComp = this.SplineContainer.gameObject.GetComponent<SplineExtrude>();
        this.spineExtrudeComp.Range = new Vector2(0, 0);
        this.spline = this.SplineContainer.Spline;
        this.spineExtrudeComp.enabled = false;
    }

    private void Update()
    {
        if (this.objectIsPickedUp == false)
            return;
        
        var firstKnot = spline.ToArray()[0];
        

        firstKnot.Position = this.SplineContainer.transform.InverseTransformPoint(targetTransform.position);
        
        spline.SetKnot(0, firstKnot);

        this.particleSystem.transform.position = this.targetTransform.position;
        this.particleSystem.transform.rotation = this.targetTransform.rotation;

        if (this.objectIsPickedUp)
        {
            var thirdKnot = spline.ToArray()[2];
            var secondKnot = spline.ToArray()[1];
            secondKnot.Position = (firstKnot.Position + thirdKnot.Position) / 2;
            secondKnot.Position.y -= 0.1f;
            secondKnot.Position.x -= 0.1f;
            spline.SetKnot(1, secondKnot);

            
            thirdKnot.Position =
                this.SplineContainer.transform.InverseTransformPoint(this.PickedUpObject.transform.position);

            //thirdKnot.Position.y = firstKnot.Position.y;
            
            
            
            spline.SetKnot(2, thirdKnot);
        }
    }
    
    public void OnPickup()
    {
        StopCoroutine(nameof(setObjectDropped));
        this.particleSystemSystem.Play();
        this.objectIsPickedUp = true;
        this.spineExtrudeComp.enabled = true;
        this.lastHeldObjectTransform = this.forwardsSampler.ObjectInRange.transform;
        this.PickedUpObject = this.forwardsSampler.ObjectInRange;
        0.0f.LerpTo(1, 0.3f, value =>
        {
            this.spineExtrudeComp.Range = new Vector2(0, value);
        });
    }

    IEnumerator setObjectDropped()
    {
        yield return new WaitForSeconds(1);
        this.objectIsPickedUp = false;
        this.spineExtrudeComp.enabled = false;
    }
    public void OnDrop()
    {
        this.particleSystemSystem.Stop();
        this.particleSystemSystem.Clear();
        StartCoroutine(nameof(this.setObjectDropped));
        
        var thirdKnot = spline.ToArray()[2];
        
        0.0f.LerpTo(1.0f, 0.3f, value =>
        {
            this.spineExtrudeComp.Range = new Vector2(value, 1);
            
            thirdKnot.Position =
                this.SplineContainer.transform.InverseTransformPoint(this.lastHeldObjectTransform.position);
            
            spline.SetKnot(2, thirdKnot);
        }, 
            pkg =>
            {
            });
    }
}
