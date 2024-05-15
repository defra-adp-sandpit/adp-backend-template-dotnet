using ADP.Backend.Template.Core.Entities;

namespace ADP.Backend.Template.Core.Interfaces
{
    public interface IItemService
    {
        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns>List of all items</returns>
        public Task<List<Item>> GetAllItems();

        /// <summary>
        /// Get item by id property
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>Item if present</returns>
        public Task<Item> GetItemById(int id);

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item">Item object to be created</param>
        /// <returns>Created Item object</returns>
        public Task<Item> CreateItem(Item item);

        /// <summary>
        /// Update existing item
        /// </summary>
        /// <param name="item">Item object to be updated</param>
        /// <returns>Updated Item object</returns>
        public Task<Item> UpdateItem(Item item);

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Id of the item to be deleted</param>
        /// <returns>Delted item</returns>
        public Task<Item> DeleteItem(int id);

    }
}
