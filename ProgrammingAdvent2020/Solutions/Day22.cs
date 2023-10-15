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
        Queue<int> player1Deck = ReadPlayerDeck(1, input);
        Queue<int> player2Deck = ReadPlayerDeck(2, input);
        int partOneAnswer = CombatResult(player1Deck, player2Deck);
        return output.WriteAnswers(partOneAnswer, null);
    }

    private static Queue<int> ReadPlayerDeck(int player, string[] input)
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

    private static int CombatResult(Queue<int> player1StartingDeck, Queue<int> player2StartingDeck)
    {
        Queue<int> player1Deck = new(player1StartingDeck);
        Queue<int> player2Deck = new(player2StartingDeck);
        while (player1Deck.Any() && player2Deck.Any())
        {
            int player1Card = player1Deck.Dequeue();
            int player2Card = player2Deck.Dequeue();
            if (player1Card > player2Card)
            {
                player1Deck.Enqueue(Math.Max(player1Card, player2Card));
                player1Deck.Enqueue(Math.Min(player1Card, player2Card));
            }
            else
            {
                player2Deck.Enqueue(Math.Max(player1Card, player2Card));
                player2Deck.Enqueue(Math.Min(player1Card, player2Card));
            }
        }
        return WinnerScore(player1Deck, player2Deck);
    }

    private static int WinnerScore(Queue<int> deck1, Queue<int> deck2)
    {
        Queue<int> winner = deck1.Any() ? deck1 : deck2;
        int multiplier = winner.Count;
        int total = 0;
        while (winner.Any())
        {
            total += multiplier * winner.Dequeue();
            multiplier--;
        }
        return total;
    }
}
