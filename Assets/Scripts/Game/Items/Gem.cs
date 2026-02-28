using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] Texture[] gemColors;
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip pickupLastGem;
    [SerializeField] Color selectedColor;

    void Start()
    {
        int randomGemColor = Random.Range(0, gemColors.Length);
        smr.materials[0].mainTexture = gemColors[randomGemColor];
        Debug.Log(randomGemColor);
        switch (randomGemColor)
        {
            case 0:
                selectedColor = new Color(1f, 1f, 1f);
                break;
            case 1:
                selectedColor = new Color(0f, 0f, 0f);
                break;
            case 2:
                selectedColor = new Color(0f, 0f, 1f);
                break;
            case 3:
                selectedColor = new Color(1f, 1f, 1f);
                break;
            case 4:
                selectedColor = new Color(0.69f, 0f, 1f);
                break;
            case 5:
                selectedColor = new Color(1f, 0f, 0f);
                break;
            case 6:
                selectedColor = new Color(0.9f, 0.46f, 0.06f);
                break;
            case 7:
                selectedColor = new Color(0f, 1f, 0f);
                break;
            case 8:
                selectedColor = new Color(1f, 1f, 1f);
                break;
            case 9:
                selectedColor = new Color(1f, 1f, 1f);
                break;
            default:
                Debug.Log("Invalid Gem Color");
                selectedColor = new Color(1f, 1f, 1f);
                break;
        }
        this.gameObject.GetComponent<Target>().setTargetColor(selectedColor);
        
        //Target indicatorTarget = gameObject.
        //indicatorTarget
        //gameObject.AddCom
    }

    private void FixedUpdate()
    {
        var rot = transform.Find("Mesh").rotation;
        transform.Find("Mesh").rotation = Quaternion.AngleAxis(Time.fixedDeltaTime * 120f, rot * Vector3.up) * rot;
    }

    public void PickupItem()
    {
        GameManager.onCollectGem?.Invoke(GameManager.instance.currentGems + 1);

        if (GameManager.instance.CheckForAllGems())
            GameManager.instance.PlayAudioClip(pickupLastGem);
        else
            GameManager.instance.PlayAudioClip(pickupSound);

        GameManager.instance.recentGems.Add(gameObject);
        gameObject.SetActive(false);
    }
}
