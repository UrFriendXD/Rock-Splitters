using UnityEngine;
using UnityEngine.Tilemaps;

namespace RockSystem.Chunks
{
    public class Chunk
    {
        private readonly ChunkDescription chunkDescription;
        private ChunkStructure chunkStructure;

        private readonly Vector3Int position;
        public Vector3Int Position => position;
        public Vector2Int FlatPosition => (Vector2Int) position;
        
        private int currentHealth;
        public int Health => currentHealth;
        public int MaxHealth => chunkDescription.Health;

        internal Chunk(ChunkDescription chunkDescription, Vector3Int position)
        {
            this.chunkDescription = chunkDescription;
            this.position = position;
            
            int healthVariation = Random.Range(-chunkDescription.HealthVariation, chunkDescription.HealthVariation + 1);
            currentHealth = chunkDescription.Health + healthVariation;

            if (currentHealth < 1)
                currentHealth = 1;
        }

        internal void AttachTo(ChunkStructure chunkStructure)
        {
            this.chunkStructure = chunkStructure;
        }
        
        internal TileBase CreateTile()
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = chunkDescription.Sprite;
            tile.transform = Matrix4x4.Translate(new Vector3(chunkDescription.Offset.x, chunkDescription.Offset.y, 0));
            return tile;
        }

        /// <summary>
        /// Deals the given amount of damage to the chunk and returns the damage taken.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>The amount of damage taken.</returns>
        internal int DamageChunk(int amount)
        {
            if (amount <= 0) return 0;

            int damageTaken = Mathf.Min(amount, currentHealth);
            
            // Debug.Log($"Damaging by {amount} with chunk at layer {position.z}");
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                chunkStructure.Clear(FlatPosition);
            }

            return damageTaken;
        }
    }
}