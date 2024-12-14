using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Row { get; private set; }
    public int Col { get; private set; }

    public TileType Type;
    public GameManager gameManager;
    private BoardManager boardManager;
    public SpriteRenderer spriteRenderer;
    public TileData tileDatabase;
    private TileSpriteLoader tileSpriteLoader;

    public void Initialize(GameManager gameManager, BoardManager boardManager, TileSpriteLoader tileSpriteLoader)
    {
        // gameManager = GameManager.instance; // cashing
        this.gameManager = gameManager;
        this.boardManager = boardManager;
        this.tileSpriteLoader = tileSpriteLoader;

        spriteRenderer = GetComponent<SpriteRenderer>();
        tileDatabase = new TileData();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        boardManager.DestoryTile(this);
    }

    private void OnDestroy()
    {
        boardManager.DestoryTile(this);
    }

    // display tile's color(or img?)
    public void UpdateTileDisplay(TileType type, int r, int c)
    {
        Type = type;
        Row = r;
        Col = c;
        
        UpdateTileDisplay(type);
    }
    public void UpdateTileDisplay(TileType type)
    {
        Type = type;

        string spritePath = tileDatabase.GetSprite(type);
        /*if (string.IsNullOrEmpty(spritePath))
        {
            Debug.LogWarning($"No sprte path found for TileType {type}");
            return;
        }*/

        Sprite newSprite = tileSpriteLoader.LoadSprteFormPath(spritePath);
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }

        /*string colorHex = tileData.GetColor(Type);

        if (ColorUtility.TryParseHtmlString(colorHex, out var color))
        {
            Material newMaterial = new Material(tileRenderer.sharedMaterial) { color = color };
            tileRenderer.material = newMaterial;
            tileRenderer.material.color = color;
        }*/
    }

    // click event
    void OnMouseDown()
    {
        if (gameManager != null)
        {
            if (gameManager.GetIsPause())
                return;
            else
                gameManager.OnTileClicked(this);
        }
    }
}
