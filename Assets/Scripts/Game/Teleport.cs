using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Teleport : MonoBehaviour
{
    private GameObject player;
    public string destinationGameObjectName;
    public GameObject destination;
    public float time;
    private float teleportTime;

    [HideInInspector] public static bool teleporting;
    private Material material;
    private Color originalColor;

    private float initTime;

    // Start is called before the first frame update
    public void InitTeleporter()
    {
        player = Marble.instance.gameObject;
        material = Marble.instance.teleportMesh.GetComponent<MeshRenderer>().sharedMaterial;
        material.color = Color.white;
        originalColor = Color.white;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out var movement))
        {
            if (destination)
            {
                teleporting = true;

                teleportTime = initTime = time;
                Invoke("TeleportMarble", time);

                Marble.instance.teleportSound.volume = PlayerPrefs.GetFloat("Audio_SoundVolume", 0.5f);
                Marble.instance.teleportSound.Play();

                if(teleportTime >= 2)
                    GameUIManager.instance.SetBottomText("Teleporter has been activated, please wait.", teleportTime);
                else
                    GameUIManager.instance.SetBottomText("Teleporter has been activated.", teleportTime);

                SetTransparent();
                StartCoroutine(TeleportFade());
            }
            else
            {
                GameUIManager.instance.SetBottomText("There's no destination specified! Please check the .mis file.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out var movement))
        {
            teleporting = false;

            SetOpaque();

            Marble.instance.teleportSound.DOFade(0, 0.5f);

            GameUIManager.instance.TeleportFadeOutBottomText();

            CancelInvoke("TeleportMarble");
        }
    }

    void TeleportMarble()
    {
        Marble.instance.teleportSound.Stop();
        GameManager.instance.PlaySpawnAudio();

        teleporting = false;

        BoxCollider bc = destination.GetComponent<BoxCollider>();
        GameObject cameraPos = destination.transform.Find("CameraPos").gameObject;

        Vector3 targetPos = destination.transform.position + new Vector3(0, bc.size.z, 0);

        cameraPos.transform.position = targetPos;
        player.GetComponent<Movement>().SetPosition(targetPos);
        Camera.main.GetComponent<CameraController>().SetCameraPosition(cameraPos.transform.GetChild(0).position, cameraPos.transform.GetChild(1).position);
        
        SetOpaque();
        material.color = originalColor;
    }

    IEnumerator TeleportFade()
    {
        while (teleporting)
        {
            teleportTime -= Time.deltaTime;
            float alpha = Mathf.Clamp01(teleportTime / time);

            material.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }
    }

    void SetTransparent()
    {
        Marble.instance.normalMesh.SetActive(false);
        Marble.instance.teleportMesh.SetActive(true);
    }

    void SetOpaque()
    {
        Marble.instance.normalMesh.SetActive(true);
        Marble.instance.teleportMesh.SetActive(false);
    }
}