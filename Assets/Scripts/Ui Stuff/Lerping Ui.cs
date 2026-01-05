using TMPro;
using UnityEngine;

public class LerpingUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField] private AnimationCurve animCurve;

    private float scaleTarget = 125.0f;
    private bool centerTextLerping = false;

    // Update is called once per frame
    void Update()
    {
        bool readyToLerp = Input.GetKeyUp(KeyCode.Space) && !this.centerTextLerping;
        if (readyToLerp)
        {
            //set a flag variable
            this.centerTextLerping = true;
            
            
            //do the lerp
            text.fontSize.LerpTo(scaleTarget, 1, 
                value =>
            {
                text.fontSize = value;
            }, 
                pkg => 
                {
                    scaleTarget = scaleTarget == 70.0f? 125.0f : 70.0f;
                    this.centerTextLerping = false;
                },
                this.animCurve);
            
            // OMG LERPING

            int a = 0;
            
            print(a);
        }
    }
}
