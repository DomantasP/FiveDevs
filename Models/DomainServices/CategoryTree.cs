using System.Collections.Generic;
using System.Linq;

namespace FiveDevsShop.Models.DomainServices
{
    public class CategoryTree
    {
        private Dictionary<int, Category> Categories { get; }
        private HashSet<int> Visited { get; } = new HashSet<int>();
        private HashSet<int> InSubtree { get; } = new HashSet<int>();

        private CategoryTree(IEnumerable<Category> categories)
        {
            Categories = categories.ToDictionary(c => c.Id);
        }

        private void WalkCategory(int id)
        {
            if (Visited.Contains(id))
                return;

            Visited.Add(id);

            var parent = Categories[id].Parent_id;
            if (parent == null)
                return;

            WalkCategory(parent.Value);

            if (InSubtree.Contains(parent.Value))
                InSubtree.Add(id);
        }

        private void WalkAll()
        {
            foreach (var categoryId in Categories.Keys)
            {
                WalkCategory(categoryId);
            }
        }

        /// <summary>
        /// Find ids of categories that are descendants of
        /// given category (including the given category).
        /// </summary>
        /// <param name="categories">All categories in the system</param>
        /// <param name="subtreeRoot">Subtree root</param>
        /// <returns></returns>
        public static HashSet<int> CategoriesInSubtree(IEnumerable<Category> categories, int subtreeRoot)
        {
            var tree = new CategoryTree(categories);
            tree.Visited.Add(subtreeRoot);
            tree.InSubtree.Add(subtreeRoot);
            tree.WalkAll();
            return tree.InSubtree;
        }
    }
}
