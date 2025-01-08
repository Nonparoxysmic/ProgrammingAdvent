// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

using System.Collections;

namespace ProgrammingAdvent2023.Common;

internal class Permutatable<T>(T[] elements) : IEnumerable<T[]>
{
    private readonly Permutator<T> _permutator = new(elements);

    public IEnumerator<T[]> GetEnumerator()
    {
        _permutator.Reset();
        return _permutator;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class Permutator<T> : IEnumerator<T[]>
{
    private readonly T[] _elements;
    private readonly T[] _output;
    private readonly int[] _indices;
    private bool _hasMoveNexted;

    public Permutator(T[] elements)
    {
        _elements = elements;
        _output = new T[_elements.Length];
        _indices = new int[_elements.Length];
        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < _indices.Length; i++)
        {
            _indices[i] = i;
        }
        _hasMoveNexted = false;
    }

    public T[] Current
    {
        get
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                _output[i] = _elements[_indices[i]];
            }
            return _output;
        }
    }

    object IEnumerator.Current { get => Current; }

    public void Dispose() { }

    public bool MoveNext()
    {
        if (_hasMoveNexted)
        {
            return PermutateIndices();
        }
        else
        {
            _hasMoveNexted = true;
            return true;
        }
    }

    private bool PermutateIndices()
    {
        if (_indices.Length < 2)
        {
            return false;
        }

        // https://en.wikipedia.org/wiki/Permutation#Generation_in_lexicographic_order

        // 1a) Find the largest index k such that a[k] < a[k + 1].
        int kilo = -1;
        for (int i = _indices.Length - 2; i >= 0; i--)
        {
            if (_indices[i] < _indices[i + 1])
            {
                kilo = i;
                break;
            }
        }

        // 1b) If no such index exists, the permutation is the last permutation.
        if (kilo < 0)
        {
            return false;
        }

        // 2)  Find the largest index l greater than k such that a[k] < a[l].
        int lima = -1;
        for (int i = _indices.Length - 1; i > kilo; i--)
        {
            if (_indices[kilo] < _indices[i])
            {
                lima = i;
                break;
            }
        }

        // 3)  Swap the value of a[k] with that of a[l].
        _indices.Swap(kilo, lima);

        // 4)  Reverse the sequence from a[k + 1] up to and including the final element a[n].
        int start = kilo + 1;
        int end = _indices.Length - 1;
        while (end > start)
        {
            _indices.Swap(start++, end--);
        }

        return true;
    }
}
