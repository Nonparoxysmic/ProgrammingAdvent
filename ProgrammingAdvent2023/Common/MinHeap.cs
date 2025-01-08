// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

namespace ProgrammingAdvent2023.Common;

internal class MinHeap<T> where T : MinHeapNode<T>
{
    public int Count { get; private set; }

    private readonly List<T> _nodes = [];

    public void Insert(T node)
    {
        if (Count < _nodes.Count)
        {
            _nodes[Count] = node;
        }
        else
        {
            _nodes.Add(node);
        }
        UpHeap(Count);
        Count++;
    }

    public T PullMin()
    {
        T min = FindMin();
        DeleteMin();
        return min;
    }

    public T FindMin()
    {
        return _nodes[0];
    }

    public void DeleteMin()
    {
        if (Count > 0)
        {
            Count--;
            if (Count > 0)
            {
                _nodes[0] = _nodes[Count];
                DownHeap(0);
            }
        }
    }

    public void PriorityUpdated(T node)
    {
        for (int i = 0; i < Count; i++)
        {
            if (node.Equals(_nodes[i]))
            {
                UpHeap(i);
                return;
            }
        }
    }

    private void UpHeap(int index)
    {
        if (index == 0)
        {
            return;
        }
        int parentIndex = (index - 1) / 2;
        if (_nodes[parentIndex].Score > _nodes[index].Score)
        {
            _nodes.Swap(index, parentIndex);
            UpHeap(parentIndex);
        }
    }

    private void DownHeap(int index)
    {
        int leftChildIndex = 2 * index + 1;
        int rightChildIndex = 2 * index + 2;
        int lowest = index;
        if (leftChildIndex < Count && _nodes[leftChildIndex].Score < _nodes[lowest].Score)
        {
            lowest = leftChildIndex;
        }
        if (rightChildIndex < Count && _nodes[rightChildIndex].Score < _nodes[lowest].Score)
        {
            lowest = rightChildIndex;
        }
        if (lowest != index)
        {
            _nodes.Swap(index, lowest);
            DownHeap(lowest);
        }
    }
}

internal abstract class MinHeapNode<T>
{
    public bool Open { get; set; }
    public int CostFromStart { get; set; } = int.MaxValue / 2;
    public int EstimatedCostToGoal { get; set; }
    public int Score => CostFromStart + EstimatedCostToGoal;

    public abstract IEnumerable<(T, int)> NeighborsAndCosts();

    public abstract bool Equals(MinHeapNode<T>? other);
}
