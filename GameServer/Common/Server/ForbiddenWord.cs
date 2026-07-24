using System;
using System.Collections.Generic;

public class TrieNode
{
    public bool IsEndOfWord { get; set; }
    public Dictionary<char, TrieNode> Children { get; set; }

    public TrieNode()
    {
        IsEndOfWord = false;
        Children = new Dictionary<char, TrieNode>();
    }
}

public class Trie
{
    private TrieNode root;
    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word)
    {
        TrieNode currentNode = root;

        foreach (char c in word)
        {
            if (!currentNode.Children.ContainsKey(c))
                currentNode.Children[c] = new TrieNode();

            currentNode = currentNode.Children[c];
        }

        currentNode.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        TrieNode currentNode = root;

        foreach (char c in word)
        {
            if (!currentNode.Children.ContainsKey(c))
                return false;

            currentNode = currentNode.Children[c];
        }

        return currentNode.IsEndOfWord;
    }
}


public class ForbiddenWord
{
    private static readonly Lazy<ForbiddenWord> _instance = new Lazy<ForbiddenWord>(() => new ForbiddenWord());
    public static ForbiddenWord Get() { return _instance.Value; }

    public void Load()
    {
        foreach (var e in T_ForbiddenWord.GetAll())
        {
            trie.Insert(e.word.ToUpper());
        }
    }

    public bool Check(string str)
    {
        var strUpper = str.ToUpper();

        for (int i = 0; i < strUpper.Length; i++)
        {
            for (int j = i + 1; j <= strUpper.Length; j++)
            {
                string substring = strUpper.Substring(i, j - i);
                if (trie.Search(substring))
                    return true;
            }
        }

        return false;
    }

    public string GetMessage(string message, string convertWord, out bool hasForbiddenWord)
    {
        hasForbiddenWord = false;

        string[] words = message.Split(" ");
        for( int i = 0; i < words.Length; i++)
        {
            if (Check(words[i]) == true)
            {
                hasForbiddenWord = true;

                words[i] = convertWord;
            }
        }

        string outMessage = "";

        foreach(var iter in words)
        {
            outMessage += iter;
            outMessage += " ";
        }

        return outMessage.Trim();
    }

    public string GetMessage(string message, string convertWord)
    {
        return GetMessage(message, convertWord, out var result);
    }

    public Trie trie = new Trie();
}
