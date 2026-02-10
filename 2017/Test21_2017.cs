using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test21_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 21;
    }

    public override void Execute()
    {
        List<EnhanceRule> rules = new List<EnhanceRule>();
        int lineCount = 0;
        foreach (string line in m_dataFileContents)
        {
            EnhanceRule rule = new EnhanceRule();
            //rule.BuildVariations(line,false);
            rule.BuildVariations(line);
            foreach (var result in rule.Variations)
            {
                if (lineCount == -1)
                {
                    DebugOutput($"Rule {lineCount} {result.Item1}");
                    DebugOutput(Helper.DrawGridHash(result.Item2, rule.PatternDims, rule.PatternDims));
                }
            }

            lineCount++;
            rules.Add(rule);
        }

        List<bool> startList = new List<bool>();

        /*
         *  .#.
            ..#
            ###
         */
        startList.Add(false);
        startList.Add(true);
        startList.Add(false);
        startList.Add(false);
        startList.Add(false);
        startList.Add(true);
        startList.Add(true);
        startList.Add(true);
        startList.Add(true);


        int numIterations = IsTestInput ? 2 : 5;

        if (IsPart2)
        {
            numIterations = 18;
        }
        
        //numIerations = 0;
        List<bool> currentList = startList;


        for (int iteration = 0; iteration < numIterations; iteration++)
        {
            int originalWidth = (int)Math.Sqrt(currentList.Count);
            List<List<bool>> brokenBlockList = null;
            int blockSize = 2;

            if (originalWidth % 2 == 0)
            {
                brokenBlockList = BreakBlock(currentList, 2);
                blockSize = 2;
            }
            else if (originalWidth % 3 == 0)
            {
                brokenBlockList = BreakBlock(currentList, 3);
                blockSize = 3;
            }

            Debug.Assert(brokenBlockList != null);

            List<bool> resultList = new List<bool>();

            foreach (var block in brokenBlockList)
            {
                bool foundMatch = false;
                foreach (EnhanceRule rule in rules)
                {
                    if (rule.PatternDims == blockSize)
                    {
                        foreach (var variation in rule.Variations)
                        {
                            if (Enumerable.SequenceEqual(block, variation.Item2))
                            {
                                foreach (bool b in rule.TransformedVersion)
                                {
                                    resultList.Add(b);
                                }

                                foundMatch = true;
                                goto FoundRule;
                            }
                        }
                    }
                }

                if (!foundMatch)
                {
                    DebugOutput($"Failed to find match for {string.Join(',', block)}");
                }

                FoundRule:
                int stuff;
            }

            int transformedWidth = (int)Math.Sqrt(resultList.Count);

            // need to merge this
            bool[] mergedResult = new bool[resultList.Count];

            int blockX = 0;
            int blockY = 0;

            int stride = transformedWidth;

            int numBlocks = brokenBlockList.Count;
            int blockWidth = blockSize + 1;

            int blocksPerSide = (int)Math.Sqrt(numBlocks);

            int count = 0;
            for (int i = 0; i < numBlocks; i++)
            {
                for (int y = 0; y < blockWidth; y++)
                {
                    for (int x = 0; x < blockWidth; x++)
                    {
                        int index = (blockY * blockWidth * stride) + (y * stride) + ((blockX * blockWidth) + x);
                        mergedResult[index] = resultList[count++];
                        // DebugOutput($"Block {blockX},{blockY},{x},{y},{index},{count}");
                        // DebugOutput(Helper.DrawGridHash(mergedResult,transformedWidth,transformedWidth));
                    }
                }

                blockX += 1;
                if (blockX == blocksPerSide)
                {
                    blockX = 0;
                    blockY += 1;
                }
            }

            currentList = new List<bool>();
            currentList.AddRange(mergedResult);

            if (iteration == numIterations - 1)
            {
                int lightsOn = currentList.FindAll(x => x == true).Count;
                DebugOutput($"Iteration {iteration} on {lightsOn}");
                DebugOutput(Helper.DrawGridHash(mergedResult, transformedWidth, transformedWidth));
            }
        }

        int ibreak = 0;
    }

    public List<List<bool>> BreakBlock(List<bool> input, int breakSize)
    {
        int numBlocks = input.Count / (breakSize * breakSize);
        int width = (int)Math.Sqrt(numBlocks);

        int blockX = 0;
        int blockY = 0;

        int stride = width * breakSize;

        List<List<bool>> splitBlocksList = new List<List<bool>>();

        for (int i = 0; i < numBlocks; i++)
        {
            List<bool> newBlock = new List<bool>();
            splitBlocksList.Add(newBlock);
            for (int y = 0; y < breakSize; y++)
            {
                for (int x = 0; x < breakSize; x++)
                {
                    int index = (blockY * breakSize * stride) + (y * stride) + ((blockX * breakSize) + x);
                    newBlock.Add(input[index]);
                }
            }

            blockX += 1;
            if (blockX == width)
            {
                blockX = 0;
                blockY += 1;
            }
        }

        return splitBlocksList;
    }
}

public class EnhanceRule
{
    public int PatternDims;
    public List<(string, bool[])> Variations = new List<(string, bool[])>();

    public bool[] TransformedVersion;

    public void BuildVariations(string line, bool yx = true)
    {
        string[] sides = line.Split("=>");
        List<bool> lhs = new List<bool>();
        List<bool> rhs = new List<bool>();

        foreach (char c in sides[0])
        {
            if (c == '.')
            {
                lhs.Add(false);
            }
            else if (c == '#')
            {
                lhs.Add(true);
            }
        }

        foreach (char c in sides[1])
        {
            if (c == '.')
            {
                rhs.Add(false);
            }
            else if (c == '#')
            {
                rhs.Add(true);
            }
        }

        BuildVariations(lhs.ToArray(), rhs.ToArray(), yx);
    }

    public void BuildVariations(bool[] pattern, bool[] transformed, bool yx = true)
    {
        PatternDims = (int)Math.Sqrt(pattern.Length);
        TransformedVersion = transformed;

        int[,] matrix = new int[PatternDims, PatternDims];
        int count = 0;
        for (int x = 0; x < PatternDims; x++)
        {
            for (int y = 0; y < PatternDims; y++)
            {
                if (yx)
                {
                    matrix[y, x] = count++;
                }
                else
                {
                    matrix[x, y] = count++;
                }
            }
        }

        List<(string, int[,])> rotations = new List<(string, int[,])>();

        int[,] matrix90 = Helper.RotateMatrixCounterClockwise(matrix);
        int[,] matrix180 = Helper.RotateMatrixCounterClockwise(matrix90);
        int[,] matrix270 = Helper.RotateMatrixCounterClockwise(matrix180);

        rotations.Add(("Original", matrix));
        rotations.Add(("Rotate90", matrix90));
        rotations.Add(("Rotate180", matrix180));
        rotations.Add(("Rotate270", matrix270));

        foreach (var rotation in rotations)
        {
            BuildFlips(rotation, pattern);
        }
    }

    public void BuildFlips((string, int[,]) originalMatrix, bool[] originalData)
    {
        bool[] flattened = new bool[PatternDims * PatternDims];
        bool[] flattenedX = new bool[PatternDims * PatternDims];
        bool[] flattenedY = new bool[PatternDims * PatternDims];

        int[,] mirrorX = Helper.MirrorMatrixX(originalMatrix.Item2);
        int[,] mirrorY = Helper.MirrorMatrixY(originalMatrix.Item2);

        for (int x = 0; x < PatternDims; x++)
        {
            for (int y = 0; y < PatternDims; y++)
            {
                flattened[originalMatrix.Item2[x, y]] = originalData[(y * PatternDims) + x];
                flattenedX[mirrorX[x, y]] = originalData[(y * PatternDims) + x];
                flattenedY[mirrorY[x, y]] = originalData[(y * PatternDims) + x];
            }
        }

        UniqueAdd((originalMatrix.Item1, flattened));
        UniqueAdd((originalMatrix.Item1 + "-X", flattenedX));
        UniqueAdd((originalMatrix.Item1 + "-Y", flattenedY));
    }

    private void UniqueAdd((string, bool[]) variation)
    {
        foreach (var existing in Variations)
        {
            if (Enumerable.SequenceEqual(variation.Item2, existing.Item2))
            {
                return;
            }
        }
        Variations.Add(variation);
    }
}