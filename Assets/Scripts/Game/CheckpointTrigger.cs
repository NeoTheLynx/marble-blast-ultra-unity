using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Checkpoint baseCheckpoint;
    private MeshRenderer thisRenderer;
    private void OnTriggerEnter(Collider other)
    {
        Movement movement = null;
        if (other.gameObject.TryGetComponent<Movement>(out movement))
        {
            TriggerEnter();
        }
    }

    void TriggerEnter()
    {
        CancelInvoke();
        GameManager.onReachCheckpoint?.Invoke(baseCheckpoint.spawn, baseCheckpoint.checkpointGravityDir);
        GameObject MBUCheckPad = baseCheckpoint.transform.parent.gameObject.transform.parent.gameObject.transform.Find("Mesh").transform.Find("mbucheck").gameObject;
        //MBUCheckPad.SetActive(false);
        thisRenderer = MBUCheckPad.GetComponent<MeshRenderer>();
        thisRenderer.sharedMaterial = baseCheckpoint.visualStates[1];
        thisRenderer.sharedMaterial.EnableKeyword("_EMISSION");
        StartCoroutine(FadeEmissionRoutine());
    }

    IEnumerator FadeEmissionRoutine()
    {
        float timer = 0f;
        while (timer < 2.0f)
        {
            // Calculate the interpolation value
            float currentIntensity = Mathf.Lerp(0f, 3f, timer / 2.0f);
            float t = timer / 2.0f;
            // Use Color.Lerp to fade the color
            Color currentColor = Color.Lerp(Color.black, Color.white, t) * currentIntensity;
            // Set the material's emission color
            thisRenderer.sharedMaterial.SetColor("_EmissionColor", currentColor);
            
            timer += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final color is set precisely
        thisRenderer.sharedMaterial.SetColor("_EmissionColor", Color.white * 3.0f);
    }
}
    