using System;
using System.Collections.Generic;

namespace JToolbox.Core.Nodes
{
    internal class NodeEqualityComparer<T> : IEqualityComparer<Node<T>>
    {
        public bool Equals(Node<T> node1, Node<T> node2)
        {
            if (!EqualityComparer<T>.Default.Equals(node1.Tag, node2.Tag))
            {
                return false;
            }

            if ((node1.Parent == null && node2.Parent != null)
                || (node2.Parent == null && node1.Parent != null))
            {
                return false;
            }

            if (node1.Parent != null && node2.Parent != null
                && !EqualityComparer<T>.Default.Equals(node1.Parent.Tag, node2.Parent.Tag))
            {
                return false;
            }

            if (node1.NodesCount != node2.NodesCount)
            {
                return false;
            }

            return true;
        }

        public int GetHashCode(Node<T> obj)
        {
            throw new NotImplementedException();
        }
    }
}