using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Core.Nodes
{
    public class NodesCollection<T> : NodesBase<T>
    {
        internal override void AddNode(Node<T> node, bool raiseOnChanged)
        {
            if (!nodes.Contains(node))
            {
                node.Parent?.RemoveNode(node);
                nodes.Add(node);

                if (raiseOnChanged)
                {
                    OnNodesChanged();
                }
            }
        }

        public void RemoveNode(Node<T> node)
        {
            if (nodes.Remove(node))
            {
                OnNodesChanged();
                return;
            }

            node.Parent?.RemoveNode(node);
        }

        internal override Node<T> CreateNewNode(T tag)
        {
            return new Node<T>(this, null, tag);
        }

        public void Map<TItem>(IEnumerable<TItem> items, Func<TItem, IEnumerable<TItem>> childrenSelector, Func<TItem, T> tagSelector)
        {
            foreach (var item in items)
            {
                var tag = tagSelector(item);
                var newNode = CreateNewNode(tag);
                AddNode(newNode, false);
                newNode.Map(item, childrenSelector, tagSelector);
            }
            OnNodesChanged();
        }

        public void Map<TItem>(IEnumerable<TItem> items, Func<TItem, int> idSelector, Func<TItem, int> parentIdSelector, Func<TItem, T> tagSelector)
        {
            var lookup = new Dictionary<int, Node<T>>();
            var itemsCopy = new List<TItem>(items);

            while (itemsCopy.Count > 0)
            {
                var item = itemsCopy[0];
                var id = idSelector(item);
                var parentId = parentIdSelector(item);
                var tag = tagSelector(item);

                Node<T> newNode;
                if (parentId <= 0)
                {
                    newNode = CreateNewNode(tag);
                    AddNode(newNode, false);
                }
                else
                {
                    lookup.TryGetValue(parentId, out Node<T> parentNode);
                    if (parentNode == null)
                    {
                        continue;
                    }

                    newNode = parentNode.CreateNewNode(tag);
                    parentNode.AddNode(newNode, false);
                }

                if (!lookup.ContainsKey(id))
                {
                    lookup.Add(id, newNode);
                }
                itemsCopy.Remove(item);
            }
            OnRecursiveNodesChanged();
        }

        public bool CompareTo(NodesCollection<T> other)
        {
            if (NodesCount != other.NodesCount)
            {
                return false;
            }

            var allNodes = GetAllNodes();
            var otherAllNodes = other.GetAllNodes();
            return Enumerable.SequenceEqual(allNodes, otherAllNodes, nodeEqualityComparer);
        }
    }
}