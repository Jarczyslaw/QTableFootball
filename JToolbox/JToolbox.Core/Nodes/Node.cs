using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Core.Nodes
{
    public class Node<T> : NodesBase<T>
    {
        internal Node(NodesCollection<T> collection, Node<T> parent, T tag)
        {
            Collection = collection;
            Parent = parent;
            Tag = tag;
        }

        public NodesCollection<T> Collection { get; }

        public Node<T> Parent { get; internal set; }

        public T Tag { get; set; }

        internal override void AddNode(Node<T> node, bool raiseOnChanged)
        {
            if (!nodes.Contains(node))
            {
                if (node.Parent == null)
                {
                    Collection.RemoveNode(node);
                }
                else
                {
                    node.Parent.RemoveNode(node);
                }

                node.Parent = this;
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
                node.Parent = null;
                OnNodesChanged();
            }
        }

        public List<Node<T>> GetAllParents()
        {
            var result = new List<Node<T>>();
            var currentNode = this;
            while (currentNode.Parent != null)
            {
                result.Insert(0, currentNode.Parent);
                currentNode = currentNode.Parent;
            }
            return result;
        }

        internal override Node<T> CreateNewNode(T tag)
        {
            return new Node<T>(Collection, this, tag);
        }

        public void Map<TItem>(TItem item, Func<TItem, IEnumerable<TItem>> childrenSelector, Func<TItem, T> tagSelector)
        {
            var tag = tagSelector(item);
            var children = childrenSelector(item);

            Tag = tag;
            foreach (var child in children)
            {
                var childTag = tagSelector(child);
                var childNode = CreateNewNode(childTag);
                AddNode(childNode, false);
                childNode.Map(child, childrenSelector, tagSelector);
            }
            OnNodesChanged();
        }

        public bool CompareTo(Node<T> other)
        {
            if (!nodeEqualityComparer.Equals(this, other))
            {
                return false;
            }

            var allNodes = GetAllNodes();
            var otherAllNodes = other.GetAllNodes();
            return Enumerable.SequenceEqual(allNodes, otherAllNodes, nodeEqualityComparer);
        }
    }
}