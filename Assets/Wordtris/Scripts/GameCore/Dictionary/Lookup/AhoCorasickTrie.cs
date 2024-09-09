using System;
using System.Collections.Generic;

namespace Wordtris.GameCore.Functions.Lookup
{
    public class AhoCorasickTrie
    {
        private class Node
        {
            public Dictionary<char, Node> Children { get; } = new Dictionary<char, Node>();
            public Node FailureLink { get; set; }
            public HashSet<string> Output { get; } = new HashSet<string>();
        }

        private readonly Node root = new Node();

        // Add a word to the Trie
        public void AddWord(string word)
        {
            var currentNode = root;
            foreach (var character in word)
            {
                if (!currentNode.Children.ContainsKey(character))
                {
                    currentNode.Children[character] = new Node();
                }
                currentNode = currentNode.Children[character];
            }
            currentNode.Output.Add(word);
        }

        // Build the Failure links
        public void Build()
        {
            var queue = new Queue<Node>();
            foreach (var child in root.Children.Values)
            {
                child.FailureLink = root;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                foreach (var kvp in currentNode.Children)
                {
                    var childNode = kvp.Value;
                    var failNode = currentNode.FailureLink;

                    while (failNode != null && !failNode.Children.ContainsKey(kvp.Key))
                    {
                        failNode = failNode.FailureLink;
                    }

                    childNode.FailureLink = failNode == null ? root : failNode.Children[kvp.Key];
                    childNode.Output.UnionWith(childNode.FailureLink.Output);

                    queue.Enqueue(childNode);
                }
            }
        }

        // Search the Trie for matches
        public List<string> Search(string text)
        {
            var currentNode = root;
            var results = new List<string>();

            foreach (var character in text)
            {
                while (currentNode != null && !currentNode.Children.ContainsKey(character))
                {
                    currentNode = currentNode.FailureLink;
                }

                if (currentNode == null)
                {
                    currentNode = root;
                    continue;
                }

                currentNode = currentNode.Children[character];

                if (currentNode.Output.Count > 0)
                {
                    results.AddRange(currentNode.Output);
                }
            }

            return results;
        }
    }

}
