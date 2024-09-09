using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

[TestFixture]
public class BoardPerformanceTests
{
    private Board board;
    private Tilemap tilemap;
    private Tile testTile;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject with a Board component
        var boardGameObject = new GameObject("Board");
        board = boardGameObject.AddComponent<Board>();

        // Create and assign a Tilemap component to the Board
        var gridGameObject = new GameObject("Grid");
        gridGameObject.transform.parent = boardGameObject.transform;
        var grid = gridGameObject.AddComponent<Grid>();
        tilemap = gridGameObject.AddComponent<Tilemap>();
        gridGameObject.AddComponent<TilemapRenderer>();

        // Use reflection to set the Tilemap if needed
        var tilemapField = typeof(Board).GetField("Tilemap", BindingFlags.NonPublic | BindingFlags.Instance);
        if (tilemapField != null)
        {
            tilemapField.SetValue(board, tilemap);
        }

        // Create a sample Tile to use in the test
        testTile = ScriptableObject.CreateInstance<Tile>();
        testTile.name = "TestTile";

        // Initialize the Board
        board.tetrominoes = new TetrominoData[7]; // Assuming there are 7 Tetromino types
        for (int i = 0; i < board.tetrominoes.Length; i++)
        {
            board.tetrominoes[i] = new TetrominoData();
            board.tetrominoes[i].Initialize();
        }

        // Set other necessary properties
        board.boardSize = new Vector2Int(10, 20);
        board.spawnPosition = new Vector3Int(-1, 8, 0);
        board.holdiPosition = new Vector3Int(-4, 10, 0);
        board.previewPosition = new Vector3Int(3, 11, 0);
        board.previewiPosition = new Vector3Int(2, 10, 0);

        // Manually call Start to initialize the board
        board.Start();
    }

    [Test]
    public void GenerateTetromino_Performance()
    {
        MeasurePerformance(() =>
        {
            MethodInfo setNextPieceMethod = typeof(Board).GetMethod("SetNextPiece", BindingFlags.NonPublic | BindingFlags.Instance);
            setNextPieceMethod.Invoke(board, null);
        }, "GenerateTetromino");
    }

    [Test]
    public void GenerateRandomText_Performance()
    {
        MeasurePerformance(() =>
        {
            string randomText = GenerateRandomText(50, 50);
        }, "GenerateRandomText");
    }

    [Test]
    public void IsLineHaveWords_Performance()
    {
        // Arrange
        var word = "TEST";
        PlaceWordInTilemap(word, 0, 0);

        MeasurePerformance(() =>
        {
            board.IsLineHaveWords(0);
        }, "IsLineHaveWords");
    }

    private void MeasurePerformance(System.Action action, string testName)
    {
        int warmupCount = 5;
        int measurementCount = 10;

        // Warm-up
        for (int i = 0; i < warmupCount; i++)
        {
            action();
        }

        // Measure
        Stopwatch stopwatch = new Stopwatch();
        long totalTime = 0;

        for (int i = 0; i < measurementCount; i++)
        {
            stopwatch.Reset();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            totalTime += stopwatch.ElapsedMilliseconds;
        }

        UnityEngine.Debug.Log($"{testName} - Average Time: {totalTime / measurementCount} ms");
    }

    private string GenerateRandomText(int rows, int cols)
    {
        var random = new System.Random();
        var text = new char[rows * cols];
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = (char)('a' + random.Next(26));
        }
        return new string(text);
    }

    private void PlaceWordInTilemap(string word, int row, int startCol)
    {
        for (int i = 0; i < word.Length; i++)
        {
            Vector3Int position = new Vector3Int(startCol + i, row, 0);
            TileBase tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = word[i].ToString();
            tilemap.SetTile(position, tile);
        }
    }
}
