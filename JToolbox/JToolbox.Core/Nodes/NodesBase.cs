using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JToolbox.Core.Nodes
{
    public abstract class NodesBase<T> : NotifyPropertyChanged
    {
        protected readonly List<Node<T>> nodes = new List<Node<T>>();
        internal readonly NodeEqualityComparer<T> nodeEqualityComparer = new NodeEqualityComparer<T>();

        public ReadOnlyCollection<Node<T>> Nodes => new ReadOnlyCollection<Node<T>>(nodes);

        public int NodesCount => nodes.Count;

        public int AllNodesCount => GetAllNodes().Count;

        internal abstract Node<T> CreateNewNode(T tag);

        internal abstract void AddNode(Node<T> node, bool raiseOnChanged);

        public Node<T> AddNewNode(T tag)
        {
            var node = CreateNewNode(tag);
            AddNode(node, true);
            return node;
        }

        public List<Node<T>> AddNewNodes(IEnumerable<T> tags)
        {
            var result = new List<Node<T>>();
            foreach (var tag in tags)
            {
                var node = CreateNewNode(tag);
                result.Add(node);
                AddNode(node, false);
            }
            OnNodesChanged();
            return result;
        }

        public void AddNode(Node<T> node)
        {
            AddNode(node, true);
        }

        public void AddNodes(IEnumerable<Node<T>> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node, false);
            }
            OnNodesChanged();
        }

        public void ClearNodes()
        {
            nodes.Clear();
            OnNodesChanged();
        }

        public List<Node<T>> GetAllNodes(List<Node<T>> result = null)
        {
            result = result ?? new List<Node<T>>();
            foreach (var node in nodes)
            {
                result.Add(node);
                node.GetAllNodes(result);
            }
            return result;
        }

        public void ForEachNode(Action<Node<T>> action)
        {
            GetAllNodes().ForEach(action);
        }

        public List<Node<T>> FindNodes(Func<Node<T>, bool> predicate)
        {
            return GetAllNodes()
                .Where(predicate)
                .ToList();
        }

        public Node<T> FindNode(Func<Node<T>, bool> predicate)
        {
            return FindNodes(predicate)
                .FirstOrDefault();
        }

        public void OnNodesChanged()
        {
            OnPropertyChanged(nameof(Nodes));
        }

        public void OnRecursiveNodesChanged()
        {
            OnPropertyChanged(nameof(Nodes));
            foreach (var node in nodes)
            {
                node.OnRecursiveNodesChanged();
            }
        }
    }
}