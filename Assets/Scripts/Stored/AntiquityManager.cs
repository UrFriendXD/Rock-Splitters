using Managers;
using UnityEngine;

namespace Stored
{
    public class AntiquityManager : Manager
    {
        public Inventory Inventory { get; } = new Inventory(100);
        public override bool PersistBetweenScenes => true;
        public float TotalIncome { private set;  get; }
        public float TotalCapacity { private set;  get; }
        public AntiquitySetDatabase antiquitySetDatabase;

        protected override void Start()
        {
            base.Start();
            ValidateAllSets();
        }

        public bool AddItem(Antiquity item)
        {
            bool success;
            if (!Inventory.HasSpace()) return false;
            
            if (Inventory.Contains(item))
            {
                // Don't need to update set as it already counts it unless we want to validate constantly
                success = Inventory.AddItem(item); 
            }
            else
            {
                Debug.Log("No item");
                success = Inventory.AddItem(item);
                item.AntiquitySet.ValidateSet(Inventory);
                CalculateStats();
            }
            return success;
        }
        
        // TODO: Figure if you can sell and how to sell it
        public bool RemoveItem(Antiquity item)
        {
            if (!Inventory.Contains(item))
            {
                Debug.Log("Unable to remove item. Doesn't exist in inventory");
                return false;
            }
            
            bool success = Inventory.RemoveItem(item); 

            // Don't think is is support so not sure why I have it. Player can't go below 1 of an item.
            // Update the item set as it no longer contains the item.
            if (Inventory.Contains(item)) return success;
            item.AntiquitySet.ValidateSet(Inventory);
            CalculateStats();

            return success;
        }

        public void CalculateStats()
        {
            float income = 0f;
            float capacity = 0f;
            foreach (var set in antiquitySetDatabase.Items)
            {
                income += set.CurrentSetIncomeRate;
                capacity += set.CurrentSetCapacity;
            }

            TotalCapacity = capacity;
            TotalIncome = income;
        }

        /// <summary>
        /// Check all sets in game if the player has the sets items then adds the income rate and capacity to the total.
        /// Should be called on startup or when we need to validate all sets.
        /// Can use context menu for a manual call.
        /// </summary>
        [ContextMenu("Validate all AntiquitySets")]
        public void ValidateAllSets()
        {
            foreach (var set in antiquitySetDatabase.Items)
            {
                set.ValidateSet(Inventory);
                CalculateStats();
            }
        }

        protected override void OnValidate()
        {
            if (ReferenceEquals(antiquitySetDatabase, null))
            {
                Debug.Log("Database is empty. Please manually assign!");
            }
        }
    }
}