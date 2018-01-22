/* The purpose of this script is to make some of the functions for modifying
 * the play area more accessible and modifiable by a developer from within the HMD:
 *   1. First iteration maps the modifications to the keyboard
 *
 *   2. Second iteration maps the modifcations to the controller allowing the 
 *      developer to customise the world from within the HMD 
 * 
 * Todo Idea List:
 *  - Setup and load different modified settings (toggle between them easily)
 *  - Map onto controllers for full within HMD modification
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_PlayAreaModifier : MonoBehaviour
{   
    private SteamVR_PlayArea playArea;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    [Tooltip("The rotation of the xyz axis on which to rotate the play area")]
    public Vector3 fixedRotationXYZ = new Vector3(0, 180, 0);

    [Tooltip("The rotation of the xyz axis on which to rotate the play area")]
    public Vector3 incrementalRotationXYZ = new Vector3(0, 90, 0);

    [Tooltip("The scale of the xyz vector on which to scale the world size")]
    public Vector3 fixedWorldScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("The scale of the xyz vector on which to scale the world size")]
    public Vector3 incrementalWorldScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("The scale of the xyz vector on which to move the world position")]
    public Vector3 fixedWorldMovement = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("The scale of the xyz vector on which to move the world position")]
    public Vector3 incrementalWorldMovement = new Vector3(0.1f, 0.1f, 0.1f);
    
    private void Awake()
    {
        playArea = GetComponentInParent<SteamVR_PlayArea>();
        originalPosition = playArea.transform.position;
        originalRotation = playArea.transform.rotation;
        originalScale = playArea.transform.localScale;
    }

    private void Update()
    {
        // Press 'A' key to toggle the rotate to some fixed rotation XYZ
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (playArea.transform.rotation == originalRotation)
            {
                playArea.transform.rotation = Quaternion.Euler(fixedRotationXYZ);
            }
            else
            {
                playArea.transform.rotation = originalRotation;
            }
        }

        // Press 'Z' key to rotate play area +N degrees
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playArea.transform.Rotate(incrementalRotationXYZ);            
        }

        // Press 'X' key to rotate play area -N degrees
        if (Input.GetKeyDown(KeyCode.X))
        {
            playArea.transform.Rotate(-incrementalRotationXYZ);
        }

        // Press 'C' key to reset the play area to the original
        if (Input.GetKeyDown(KeyCode.C))
        {
            playArea.transform.rotation = originalRotation;
        }

        // Press 'V' key to toggle the play area scale by a fixed value
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (playArea.transform.localScale == originalScale)
            {
                playArea.transform.localScale = fixedWorldScale;
            }
            else
            {
                playArea.transform.localScale = originalScale;
            }
        }

        // Press 'B' and 'N' keys to change the play area scale by an incremental value
        if (Input.GetKeyDown(KeyCode.B))
        {
            playArea.transform.localScale -= incrementalWorldScale;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            playArea.transform.localScale += incrementalWorldScale;
        }

        // Press 'M' key to reset the play area to its default scale
        if (Input.GetKeyDown(KeyCode.M))
        {
            playArea.transform.localScale = originalScale;
        }

        // Press 'H' key to toggle the play area position by a fixed value
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (playArea.transform.position == originalPosition)
            {
                playArea.transform.position = fixedWorldMovement;
            }
            else
            {
                playArea.transform.position = originalPosition;
            }            
        }

        // Press 'J' and 'K' keys to move the play area position by the fixed value
        if (Input.GetKeyDown(KeyCode.J))
        {
            playArea.transform.position -= incrementalWorldMovement;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            playArea.transform.position += incrementalWorldMovement;
        }

        // Press 'L' key to reset the play area position to its default value
        if (Input.GetKeyDown(KeyCode.L))
        {
            playArea.transform.position = originalPosition;
        }
    }
}