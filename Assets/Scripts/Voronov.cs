using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Voronov : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private Color[] availableColors;

    private int _imageSize;
    private int _pixelPerCell;
    private Vector2Int[,] _pointsPosition;
    private Color[,] _colors;

    private void Awake()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }

        _imageSize = Mathf.RoundToInt(rawImage.GetComponent<RectTransform>().sizeDelta.x);
        _pixelPerCell = _imageSize / gridSize;
    }

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        GeneratePoints();
        GenerateDiagram();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Generate();
        }
    }

    private void GeneratePoints()
    {
        _pointsPosition = new Vector2Int[gridSize, gridSize];
        _colors = new Color[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                _pointsPosition[i, j] = new Vector2Int(
                    i * _pixelPerCell + Random.Range(0, _pixelPerCell),
                    j * _pixelPerCell + Random.Range(0, _pixelPerCell)
                );
                _colors[i, j] = availableColors[Random.Range(0, availableColors.Length)];
            }
        }
    }

    private void GenerateDiagram()
    {
        Texture2D texture = new Texture2D(_imageSize, _imageSize)
        {
            filterMode = FilterMode.Point
        };

        for (int i = 0; i < _imageSize; i++)
        {
            for (int j = 0; j < _imageSize; j++)
            {
                int gridX = i / _pixelPerCell;
                int gridY = j / _pixelPerCell;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();

                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                    {
                        int x = gridX + k;
                        int y = gridY + l;

                        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), _pointsPosition[x, y]);
                        if (distance < nearestDistance)
                        {
                            Debug.Log("nearestPoint");
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(x, y);
                        }
                    }
                }
                texture.SetPixel(i, j, _colors[nearestPoint.x, nearestPoint.y]);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }
}