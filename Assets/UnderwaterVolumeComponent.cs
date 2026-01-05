using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnderwaterVolumeComponent : VolumeComponent, IPostProcessComponent
{
    [SerializeField] private BoolParameter enabled = new BoolParameter(false);
    public bool IsActive()
    {
        return this.enabled.value;
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}