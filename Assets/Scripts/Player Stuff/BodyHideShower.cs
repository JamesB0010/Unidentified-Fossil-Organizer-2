using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHideShower : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;

    private bool locked = false;
    
    public void ReactToLookingUp()
    {
        if (this.locked)
            return;
     
        this.DisableBodyMeshRenderer();
    }

    public void ReactToLookingDown()
    {
        if (this.locked)
            return;
        
        EnableBodyMeshRenderer();
    }

    public void LockToEnabled()
    {
        this.locked = false;
        this.EnableBodyMeshRenderer();
        this.locked = true;
    }

    public void LockToDisabled()
    {
        this.locked = false;
        this.DisableBodyMeshRenderer();
        this.locked = true;
    }

    public void EnableBodyMeshRenderer() => this.bodyMeshRenderer.enabled = true;

    public void DisableBodyMeshRenderer() => this.bodyMeshRenderer.enabled = false;
}
