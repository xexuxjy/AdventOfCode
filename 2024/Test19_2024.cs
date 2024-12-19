using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static Test13_2024;

public class Test19_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 19;
    }



    public override void Execute()
    {
        List<string> availablePatterns = new List<string>();
        List<string> targetPatterns = new List<string>();

        string[] tokens = m_dataFileContents[0].Split(',');
        foreach (string token in tokens)
        {
            availablePatterns.Add(token.Trim());
        }
        for (int i = 2; i < m_dataFileContents.Count; i++)
        {
            targetPatterns.Add(m_dataFileContents[i].Trim());
        }

        List<string> sortedPatterns = new List<string>();
        sortedPatterns.AddRange(availablePatterns.OrderBy(x => x.Length));



        List<string> reducedList = new List<string>();
        // remove the patterns that can be made from other patterns?
        foreach (string availablePattern in sortedPatterns)
        {
            List<string> patternCopy = new List<string>();
            patternCopy.AddRange(sortedPatterns);
            patternCopy.Remove(availablePattern);
            if (!TestPossibility(availablePattern, patternCopy))
            {
                reducedList.Add(availablePattern);
            }

        }



        List<string> possiblePatterns = new List<string>();

        int count = 0;
        int score = 0;
        foreach (string targetPattern in targetPatterns)
        {
            bool foundPossible = TestPossibility(targetPattern, reducedList);
            if (foundPossible)
            {
                possiblePatterns.Add(targetPattern);
                score++;
            }
            DebugOutput($"Testing pattern {count}  valid {foundPossible} ");
            count++;
        }


        DebugOutput($"{score} out of {targetPatterns.Count}  are possible.");
        int ibreak = 0;


        if (IsPart2)
        {

            ConcurrentBag<long> bagResults = new ConcurrentBag<long>();
            long totalScore = 0;
            Parallel.ForEach(possiblePatterns, new ParallelOptions { MaxDegreeOfParallelism = 6 }, pattern =>
            {
                DateTime startTime = DateTime.Now;
                DebugOutput($"Testing {pattern}");

                //long localScore = TestPossibility2(pattern, sortedPatterns);
                Trie trie = new Trie();
                foreach (string s in sortedPatterns)
                {
                    char[] arr = s.ToCharArray();
                    Array.Reverse(arr);

                    trie.Insert(new string(arr));
                }

                long localScore = trie.WaysOfFormingString(pattern);

                bagResults.Add(localScore);
                DateTime endTime = DateTime.Now;
                double bpElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                DebugOutput("Elapsed = " + bpElapsed + " ms");

                DebugOutput($"Pattern {pattern} can be made in {localScore} ways");
            });

            foreach (long b in bagResults)
            {
                totalScore += b;
            }

            //var tasks = new List<Task<long>>(possiblePatterns.Count);
            //foreach (string pattern in possiblePatterns)
            //{
            //    tasks.Add(Task<long>.Factory.StartNew(() =>
            //    {
            //        DateTime startTime = DateTime.Now;
            //        DebugOutput($"Testing {pattern}");
            //        long localScore = TestPossibility2(pattern, sortedPatterns);
            //        DebugOutput($"Pattern {pattern} can be made in {localScore} ways");
            //        DateTime endTime = DateTime.Now;
            //        double bpElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            //        DebugOutput("Elapsed = " + bpElapsed + " ms");
            //        return localScore;
            //    }));
            //}



            //foreach (Task<long> task in tasks)
            //{
            //    totalScore += task.Result;
            //}



            DebugOutput("Total score is " + totalScore);

        }


    }
    public long TestPossibility2(string targetPattern, List<string> availablePatterns)
    {
        long count = 0;
        if (targetPattern == "")
        {
            return 1;
        }

        List<string> reducedList = new List<string>();

        foreach (string s in availablePatterns)
        {
            if (targetPattern.Contains(s))
            {
                reducedList.Add(s);
            }
        }

        bool hasMatch = false;
        foreach (string s in reducedList)
        {
            if (s.Length > targetPattern.Length)
            {
                return count;
            }

            //DebugOutput($"Testing {targetPattern} with {s} matches {targetPattern.StartsWith(s)}");
            if (targetPattern.StartsWith(s))
            {
                count += TestPossibility2(targetPattern.Substring(s.Length), reducedList); ;
            }
        }
        return count;
    }



    // to start with, don;t need to know what the pattern is, just that one exists
    public bool TestPossibility(string targetPattern, List<string> availablePatterns)
    {
        if (targetPattern == "")
        {
            return true;
        }

        bool hasMatch = false;
        foreach (string s in availablePatterns)
        {
            if (s.Length > targetPattern.Length)
            {
                return false;
            }

            //DebugOutput($"Testing {targetPattern} with {s} matches {targetPattern.StartsWith(s)}");
            if (targetPattern.StartsWith(s))
            {
                bool match = TestPossibility(targetPattern.Substring(s.Length), availablePatterns); ;
                if (match)
                {
                    return true;
                }
            }
        }
        return hasMatch;
    }


    // need to reduce the set of possible types.

    //https://www.geeksforgeeks.org/number-of-ways-to-form-a-given-string-from-the-given-set-of-strings/
    public class TrieNode
    {
        public bool endOfWord = false;
        public TrieNode[] children = new TrieNode[26];
    }

    public class Trie
    {
        private TrieNode root = new TrieNode();

        // Insert a string into the trie
        public void Insert(string s)
        {
            TrieNode prev = root;
            foreach (char c in s)
            {
                int index = c - 'a';
                if (prev.children[index] == null)
                    prev.children[index] = new TrieNode();
                prev = prev.children[index];
            }
            prev.endOfWord = true;
        }

        // Find the number of ways to form the given string
        // using the strings in the trie
        public long WaysOfFormingString(string str)
        {
            int n = str.Length;
            long[] count = new long[n];

            // For each index i in the input string
            for (int i = 0; i < n; i++)
            {
                TrieNode ptr = root;
                // Check all possible substrings of str ending
                // at index i
                for (int j = i; j >= 0; j--)
                {
                    char ch = str[j];
                    int index = ch - 'a';
                    if (ptr.children[index] == null)
                        break;
                    ptr = ptr.children[index];
                    if (ptr.endOfWord)
                        // If the substring ending at index j is
                        // in the trie, update the count
                        count[i] += j > 0 ? count[j - 1] : 1;
                }
            }

            // The final count is the number of ways to form the
            // entire string
            return count[n - 1];
        }
    }
}