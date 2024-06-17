using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Voronov : MonoBehaviour
{
    [SerializeField] private int gridSize = 10;
    [SerializeField] private Color[] availableColors;

    private int _imageSize;
    [SerializeField] private RawImage image;
    private int _pixelPerCell;
    private Vector2Int[,] _pointsPosition;
    private Color[,] _colors;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        _imageSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
    }

    private void Start()
    {
        GeneratePoints();
        // GenerateDiagram();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenerateDiagram();
        }
    }

    private void GenerateDiagram()
    {
        Texture2D texture = new Texture2D(_imageSize, _imageSize)
        {
            filterMode = FilterMode.Point
        };
        _pixelPerCell = _imageSize / gridSize;

        for (int i = 0; i < _imageSize; i++)
        {
            for (int j = 0; j < _imageSize; j++)
            {
                texture.SetPixel(i, j, Color.white);
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                texture.SetPixel(_pointsPosition[i, j].x, _pointsPosition[i, j].y, Color.black);
            }
        }

        // texture.Apply();
        // image.texture = texture;


        for (int i = 0; i < _imageSize; i++)
        {
            for (int j = 0; j < _imageSize; j++)
            {
                int gridX = i / _pixelPerCell;
                int gridY = j / _pixelPerCell;
                
                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();
                
                for (int k = -1; k <= 2; k++)
                {
                    for (int l = -1; l <= 2; l++)
                    {
                        int x = gridX + k;
                        int y = gridY + l;

                        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), _pointsPosition[x, y]);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(x, y);
                        }
                    }
                }

                texture.SetPixel(i, j, _colors[nearestPoint.x, nearestPoint.y]);
            }
        }

        Debug.Log("Done");
        texture.Apply();
        image.texture = texture;
    }

    private void GeneratePoints()
    {
        _pixelPerCell = _imageSize / gridSize;
        _pointsPosition = new Vector2Int[gridSize, gridSize];
        _colors = new Color[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                _pointsPosition[i, j] = new Vector2Int(i * _pixelPerCell + Random.Range(0, _pixelPerCell),
                    j * _pixelPerCell + Random.Range(0, _pixelPerCell));
                _colors[i, j] = availableColors[Random.Range(0, availableColors.Length)];
            }
        }
    }
}