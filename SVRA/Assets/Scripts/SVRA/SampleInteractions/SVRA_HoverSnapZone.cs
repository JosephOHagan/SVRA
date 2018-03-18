using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_HoverSnapZone : MonoBehaviour {

    [System.Serializable]
    public class HoverCharacteristics
    {
        [Tooltip("Enable the object to oscillation on the x-axis")]
        public bool xAxisHover = false;

        [Tooltip("Enable the object to oscillate on the y-axis")]
        public bool yAxisHover = false;

        [Tooltip("Enable the object to oscillate on the z-axis")]
        public bool zAxisHover = false;

        [Tooltip("The osciallation speed")]
        public float oscillationSpeed = 0.3f;

        [Tooltip("The oscillation distance")]
        public float oscillationDistance = 0.01f;
    }

    public HoverCharacteristics hoverCharacteristics;

    [HideInInspector]
    public bool inZone = false;

    [HideInInspector]
    public bool seated = true;

    private Vector3 seatedPosition;

    private const float FLOAT_SPEED = 3f;
    private const float FLOAT_DISTANCE = 0.01f;

    void Start()
    {
        seatedPosition = transform.position;

        if (seated)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }

    }

    void Update()
    {
        if (!seated)
        {
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (seated)
        {
            Vector3 floatPosition = transform.position;

            floatPosition.x = seatedPosition.x + ( Mathf.Sin(Time.time * hoverCharacteristics.oscillationSpeed) * hoverCharacteristics.oscillationDistance * convertBoolToInt(hoverCharacteristics.xAxisHover) );
            floatPosition.y = seatedPosition.y + ( Mathf.Sin(Time.time * hoverCharacteristics.oscillationSpeed) * hoverCharacteristics.oscillationDistance * convertBoolToInt(hoverCharacteristics.yAxisHover) );
            floatPosition.z = seatedPosition.z + ( Mathf.Sin(Time.time * hoverCharacteristics.oscillationSpeed) * hoverCharacteristics.oscillationDistance * convertBoolToInt(hoverCharacteristics.zAxisHover) );
            
            transform.position = floatPosition;
        }
        
    }

    int convertBoolToInt(bool input)
    {
        return input ? 1 : 0;
    }

    void SVRAGrabStart()
    {
        seated = false;
    }

    void SVRAGrabStop()
    {
        seated = inZone;

        if (seated)
        {
            Reseat();
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void Reseat()
    {
        transform.position = seatedPosition;
        transform.rotation = Quaternion.identity;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void OnCollisionEnter()
    {
        seated = false;
    }
}
