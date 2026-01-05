using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Events;
public class DoctorsSink : MonoBehaviour, I_Interactable
{
    public UnityEvent SinkFill;
    public UnityEvent SinkEmpty;
    
    private bool sinkFull = false;
    
    private float fillSpeed = 0.22f;

    [SerializeField] private AudioClip fillSound;

    public delegate void SinkStateNotification(string sinkName);

    public event SinkStateNotification OnSinkFull;

    public event SinkStateNotification OnSinkEmpty;

    private float emptySpeed = 0.2f;

    [SerializeField] private AudioClip emptySound;

    [SerializeField]
    private GameObject water;

    [SerializeField]
    private GameObject topPosition;

    [SerializeField]
    private GameObject bottomPosition;

    private bool interactionInProgress = false;
    public new void HandleInteraction(CameraForwardsSampler playerCamSampler)
    {
        //check the player is interacting with me
        if (playerCamSampler.InteractableObjectInRangeRef != this.gameObject)
            return;

        if (this.interactionInProgress)
            return;
        
        
        if (this.sinkFull == false)
            this.FillSink();

        if (this.sinkFull == true)
            this.DrainSink();
    }
     private void FillSink()
    {
        SinkFill?.Invoke();
        this.interactionInProgress = true;
        this.bottomPosition.transform.position.LerpTo(this.topPosition.transform.position,
            7,
            value =>
            {
                this.water.transform.position = value;
            },
            pkg =>
            {
                this.interactionInProgress = false;
                this.sinkFull = true;
                this.OnSinkFull?.Invoke(this.gameObject.name);
            });

    }

    private void DrainSink()
    {
        SinkEmpty?.Invoke();
        this.interactionInProgress = true;
        
        this.topPosition.transform.position.LerpTo(this.bottomPosition.transform.position,
            6,
            value =>
            {
                this.water.transform.position = value;
            },
            pkg =>
            {
                this.interactionInProgress = false;
                this.sinkFull = false;
                this.OnSinkEmpty?.Invoke(this.gameObject.name);
            });

    }
}
