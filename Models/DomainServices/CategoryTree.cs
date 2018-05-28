using System.Collections.Generic;
using System.Linq;

namespace FiveDevsShop.Models.DomainServices
{
    public class CategoryTree
    {
        public class Node
        {
            public Category Category { get; }
            public Node Parent { get; set; }
            public List<Node> Children { get; } = new List<Node>();

            public Node(Category category)
            {
                Category = category;
            }

            private void Dfs(List<Category> resultList)
            {
                resultList.Add(Category);
                foreach (var child in Children)
                    child.Dfs(resultList);
            }

            public List<Category> CategoriesInSubtree()
            {
                var list = new List<Category>();
                Dfs(list);
                return list;
            }
        }

        private Dictionary<int, Node> Categories { get; }
        public IEnumerable<Node> RootCategories { get; }

        public CategoryTree(IEnumerable<Category> categories)
        {
            Categories = categories.Select(c => new Node(c)).ToDictionary(c => c.Category.Id);
            var root = new List<Node>();
            foreach (var node in Categories.Values)
            {
                if (node.Category.Parent_id == null)
                    root.Add(node);
                else
                {
                    var parent = Categories[node.Category.Parent_id.Value];
                    Categories[node.Category.Id].Parent = parent;
                    parent.Children.Add(node);
                }
            }
            RootCategories = root;
        }

        public Node FindCategoryNode(int id)
        {
            return Categories[id];
        }

        public IEnumerable<Node> Subtrees(int? rootCategoryId)
        {
            if (rootCategoryId == null)
            {
                return RootCategories;
            }
            else
            {
                return Categories[rootCategoryId.Value].Children;
            }
        }
    }
}
