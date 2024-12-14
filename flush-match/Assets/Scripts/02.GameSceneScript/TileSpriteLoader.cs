using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class TileSpriteLoader : MonoBehaviour
{
    public Material outlineMaterial;
    private float pulseSpeed = 1.0f;
    private Color outlineColor = Color.white;

    private bool isPulsing = false;

    public Sprite LoadSprteFormPath(string fileName)
    {
        Sprite sprite = Resources.Load<Sprite>(fileName);
        if (sprite == null)//(!File.Exists(fileName))
        {
            Debug.LogError($"File not found at path : {fileName}"); //
            return null;
        }
        return sprite;
    }

    public void CreateOutline(GameObject target)
    {
        if (target == null) return;

        SpriteRenderer originalSprite = target.GetComponent<SpriteRenderer>();

        // create outline object
        GameObject outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(target.transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localScale = Vector3.one * 1.2f;

        SpriteRenderer outlineSprite = outlineObject.AddComponent<SpriteRenderer>();
        outlineSprite.sprite = originalSprite.sprite;
        outlineSprite.material = outlineMaterial;
        outlineSprite.color = Color.white;
        outlineSprite.sortingOrder = originalSprite.sortingOrder - 1;
    }

    public void RemoveOutline(GameObject target)
    {
        if (target == null) return;

        Transform outline = target.transform.Find("Outline");
        if (outline != null)
            Destroy(outline.gameObject);
    }

    /*public void StartOutlineEffect()
    {
        if (!isPulsing)
            StartCoroutine(PulseOutline());
    }

    public void StopOutlineEffect()
    {
        isPulsing = false;
        outlineMaterial.SetFloat("_Alpha", 0);
        StopAllCoroutines();
    }

    private IEnumerator PulseOutline()
    {
        isPulsing = true;
        float pulseTime = 0f;

        while (true)
        {
            pulseTime += Time.deltaTime * pulseSpeed;
            float alpha = Mathf.PingPong(pulseTime, 1.0f); // alpha value repeat pulseTime to 1.0f
            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_Alpha", alpha);

            yield return null;
        }
    }*/
}
