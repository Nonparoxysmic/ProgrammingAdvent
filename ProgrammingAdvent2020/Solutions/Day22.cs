// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day22 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (input.Length < 5)
        {
            errorMessage = "Input is too short.";
            return false;
        }
        if (input[0] != "Player 1:")
        {
            errorMessage = $"Expected \"Player 1:\", found \"{input[0].Left(20, true)}\".";
            return false;
        }
        int i = 1;
        HashSet<int> player1Cards = new();
        for (; i < input.Length - 2; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
            if (!int.TryParse(input[i], out int card))
            {
                errorMessage = $"Line \"{input[i].Left(20, true)}\" is not a valid card.";
                return false;
            }
            if (!player1Cards.Add(card))
            {
                errorMessage = "Player 1's deck contains duplicate cards.";
                return false;
            }
        }
        if (player1Cards.Count == 0)
        {
            errorMessage = "Player 1's deck contains no cards.";
            return false;
        }
        i++;
        if (input[i] != "Player 2:")
        {
            errorMessage = $"Expected \"Player 2:\", found \"{input[i].Left(20, true)}\".";
            return false;
        }
        i++;
        HashSet<int> player2Cards = new();
        for (; i < input.Length; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
            if (!int.TryParse(input[i], out int card))
            {
                errorMessage = $"Line \"{input[i].Left(20, true)}\" is not a valid card.";
                return false;
            }
            if (!player2Cards.Add(card))
            {
                errorMessage = "Player 2's deck contains duplicate cards.";
                return false;
            }
        }
        if (player2Cards.Count == 0)
        {
            errorMessage = "Player 2's deck contains no cards.";
            return false;
        }
        if (player1Cards.Intersect(player2Cards).Any())
        {
            errorMessage = "Input contains duplicate cards.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        Queue<int> player1Cards = ReadPlayerCards(1, input);
        Queue<int> player2Cards = ReadPlayerCards(2, input);
        int totalCardCount = player1Cards.Count + player2Cards.Count;
        Deck player1Deck = new(player1Cards, totalCardCount);
        Deck player2Deck = new(player2Cards, totalCardCount);
        int partOneAnswer = CombatWinnerScore(player1Cards, player2Cards);
        int partTwoAnswer = RecursiveCombatWinnerScore(player1Deck, player2Deck);
        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Queue<int> ReadPlayerCards(int player, string[] input)
    {
        string title = $"Player {player}:";
        int i = 0;
        for (; i < input.Length; i++)
        {
            if (input[i] == title)
            {
                break;
            }
        }
        i++;
        Queue<int> output = new();
        for (; i < input.Length; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
            output.Enqueue(int.Parse(input[i]));
        }
        return output;
    }

    private static int CombatWinnerScore(Queue<int> player1Deck, Queue<int> player2Deck)
    {
        while (player1Deck.Any() && player2Deck.Any())
        {
            int player1Card = player1Deck.Dequeue();
            int player2Card = player2Deck.Dequeue();
            if (player1Card > player2Card)
            {
                player1Deck.Enqueue(player1Card);
                player1Deck.Enqueue(player2Card);
            }
            else
            {
                player2Deck.Enqueue(player2Card);
                player2Deck.Enqueue(player1Card);
            }
        }
        Queue<int> winner = player1Deck.Any() ? player1Deck : player2Deck;
        int multiplier = winner.Count;
        int total = 0;
        while (winner.Any())
        {
            total += multiplier * winner.Dequeue();
            multiplier--;
        }
        return total;
    }

    private static int RecursiveCombatWinnerScore(Deck player1Deck, Deck player2Deck)
    {
        RecursiveCombatGame game = new(player1Deck, player2Deck);
        game.Play();
        return game.WinnerScore;
    }

    private class Deck
    {
        public int Count => _len;

        private int _pos;
        private int _len;
        private readonly int[] _cards;

        public Deck(Queue<int> deck, int size)
        {
            _cards = new int[size];
            _pos = 0;
            _len = deck.Count;
            for (int i = 0; i < _len; i++)
            {
                _cards[i] = deck.Dequeue();
                deck.Enqueue(_cards[i]);
            }
        }

        public Deck(Deck original, int count)
        {
            _pos = original._pos;
            _len = count;
            _cards = new int[original._cards.Length];
            Array.Copy(original._cards, _cards, _cards.Length);
        }

        public bool Any()
        {
            return _len > 0;
        }

        public int Draw()
        {
            int card = _cards[_pos % _cards.Length];
            _pos++;
            _len--;
            return card;
        }

        public void Bury(int card)
        {
            _cards[(_pos + _len) % _cards.Length] = card;
            _len++;
        }

        public int Score()
        {
            int multiplier = _len;
            int total = 0;
            for (int i = _pos; i < _pos + _len; i++)
            {
                total += multiplier * _cards[i % _cards.Length];
                multiplier--;
            }
            return total;
        }
    }

    private class RecursiveCombatGame
    {
        public int Winner { get; private set; }
        public int WinnerScore { get; private set; }

        private readonly Deck _player1Deck;
        private readonly Deck _player2Deck;

        public RecursiveCombatGame(Deck player1Deck, Deck player2Deck)
        {
            _player1Deck = player1Deck;
            _player2Deck = player2Deck;
        }

        public void Play()
        {
            HashSet<ulong> seenStates = new()
            {
                State(_player1Deck, _player2Deck)
            };
            int timeout = 100_000;
            while (timeout-- > 0)
            {
                int player1Card = _player1Deck.Draw();
                int player2Card = _player2Deck.Draw();
                if (_player1Deck.Count >= player1Card && _player2Deck.Count >= player2Card)
                {
                    Deck recursiveDeck1 = new(_player1Deck, player1Card);
                    Deck recursiveDeck2 = new(_player2Deck, player2Card);
                    RecursiveCombatGame game = new(recursiveDeck1, recursiveDeck2);
                    game.Play();
                    if (game.Winner == 1)
                    {
                        _player1Deck.Bury(player1Card);
                        _player1Deck.Bury(player2Card);
                    }
                    else if (game.Winner == 2)
                    {
                        _player2Deck.Bury(player2Card);
                        _player2Deck.Bury(player1Card);
                    }
                    else
                    {
                        // Recursive game not resolved.
                        return;
                    }
                }
                else
                {
                    if (player1Card > player2Card)
                    {
                        _player1Deck.Bury(player1Card);
                        _player1Deck.Bury(player2Card);
                    }
                    else
                    {
                        _player2Deck.Bury(player2Card);
                        _player2Deck.Bury(player1Card);
                    }
                }

                if (!_player1Deck.Any())
                {
                    Winner = 2;
                    WinnerScore = _player2Deck.Score();
                    return;
                }
                else if (!_player2Deck.Any())
                {
                    Winner = 1;
                    WinnerScore = _player1Deck.Score();
                    return;
                }

                ulong state = State(_player1Deck, _player2Deck);
                if (!seenStates.Add(state))
                {
                    Winner = 1;
                    WinnerScore = _player1Deck.Score();
                    return;
                }
            }
        }

        private static ulong State(Deck player1Deck, Deck player2Deck)
        {
            return ((ulong)player1Deck.Score() << 32) | (uint)player2Deck.Score();
        }
    }
}
