using System;
namespace Wordtris.GameCore.ItemObjects
{
    public class Dynamite : Item
    {
        public Dynamite() : base("Dynamite", 500) { }

        public override void UseItem()
        {
            // Logic to clear the tile it lands on
            ClearTile();
        }

        private void ClearTile()
        {
            // Implement the logic to clear a single tile
            Console.WriteLine("Tile cleared by Dynamite.");
        }
    }

    public class Bomb : Item
    {
        public Bomb() : base("Bomb", 5) { }

        public override void UseItem()
        {
            ClearColumn();
        }

        private void ClearColumn()
        {
            Console.WriteLine("Column cleared by Bomb.");
        }
    }

    public class Hammer : Item
    {
        public Hammer() : base("Hammer", 800) { }

        public override void UseItem()
        {
            DiscardTile();
        }

        private void DiscardTile()
        {
            Console.WriteLine("Row cleared by Hammer");
        }
    }

    public class Missile : Item
    {
        public Missile() : base("Missile", 1000) { }

        public override void UseItem()
        {
            ClearSameLetterTiles();
        }

        private void ClearSameLetterTiles()
        {
            Console.WriteLine("Tiles with the same letter cleared by Missile.");
        }
    }
}