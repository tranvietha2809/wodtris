using NUnit.Framework;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

[TestFixture]
public class BoardPlayModeTests
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
        var grid = boardGameObject.AddComponent<Grid>();
        tilemap = boardGameObject.AddComponent<Tilemap>();
        var tilemapRenderer = boardGameObject.AddComponent<TilemapRenderer>();

        // Use reflection to set the Tilemap if needed
        var tilemapField = typeof(Board).GetField("Tilemap", BindingFlags.NonPublic | BindingFlags.Instance);
        if (tilemapField != null)
        {
            tilemapField.SetValue(board, tilemap);
        }

        // Create a sample Tile to use in the test
        testTile = ScriptableObject.CreateInstance<Tile>();
        testTile.name = "TestTile";
    }

    [Test]
    public void IsLineHaveWords_WithValidWord_FindsWord()
    {
        // Arrange
        var word = "TEST";
        PlaceWordInTilemap(word, 0, 0);

        // Act
        var result = board.IsLineHaveWords(0);

        // Assert
        Assert.IsTrue(result);
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
