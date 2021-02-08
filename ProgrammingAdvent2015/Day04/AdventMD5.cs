// Made for Advent of Code 2015, Day 4 puzzle
// https://adventofcode.com/2015/day/4
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;

namespace ProgrammingAdvent2015
{
    public static class AdventMD5
    {
        static readonly uint[] sines = { 0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee, 0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501, 0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be, 0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821, 0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa, 0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8, 0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed, 0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a, 0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c, 0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70, 0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05, 0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665, 0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039, 0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1, 0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1, 0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391 };
        static readonly int[] shifts = { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };

        public static byte[] ComputeHash(byte[] message)
        {
            int numOfBlocks = (message.Length + 8) / 64 + 1;
            byte[] paddedMessage = new byte[64 * numOfBlocks];
            byte[] appendedLengthBits = BitConverter.GetBytes((ulong)message.Length * 8);
            for (int i = 0; i < paddedMessage.Length; i++)
            {
                if (i < message.Length) paddedMessage[i] = message[i];
                else if (i == message.Length) paddedMessage[i] = 0b10000000;
                else if (i < paddedMessage.Length - 8) paddedMessage[i] = 0;
                else paddedMessage[i] = appendedLengthBits[i - paddedMessage.Length + 8];
            }

            uint stateA = 0x67452301;
            uint stateB = 0xefcdab89;
            uint stateC = 0x98badcfe;
            uint stateD = 0x10325476;

            for (int blockNum = 0; blockNum < numOfBlocks; blockNum++)
            {
                uint A = stateA;
                uint B = stateB;
                uint C = stateC;
                uint D = stateD;

                uint[] M = new uint[16];
                for (int i = 0; i < 16; i++)
                {
                    int firstByteIndex = 64 * blockNum + 4 * i;
                    M[i] = paddedMessage[firstByteIndex + 3];
                    M[i] = (M[i] << 8) + paddedMessage[firstByteIndex + 2];
                    M[i] = (M[i] << 8) + paddedMessage[firstByteIndex + 1];
                    M[i] = (M[i] << 8) + paddedMessage[firstByteIndex];
                }

                for (int operationNum = 0; operationNum < 64; operationNum++)
                {
                    uint F;
                    int g;
                    if (operationNum <= 15)
                    {
                        F = (B & C) | ((~B) & D);
                        g = operationNum;
                    }
                    else if (operationNum <= 31)
                    {
                        F = (D & B) | ((~D) & C);
                        g = (5 * operationNum + 1) % 16;
                    }
                    else if (operationNum <= 47)
                    {
                        F = B ^ C ^ D;
                        g = (3 * operationNum + 5) % 16;
                    }
                    else
                    {
                        F = C ^ (B | (~D));
                        g = (7 * operationNum) % 16;
                    }
                    F = F + A + sines[operationNum] + M[g];
                    A = D;
                    D = C;
                    C = B;
                    B += ((F << shifts[operationNum]) | (F >> (32 - shifts[operationNum])));
                }
                stateA += A;
                stateB += B;
                stateC += C;
                stateD += D;
            }
            byte[] output = new byte[16];
            byte[] outA = BitConverter.GetBytes(stateA);
            byte[] outB = BitConverter.GetBytes(stateB);
            byte[] outC = BitConverter.GetBytes(stateC);
            byte[] outD = BitConverter.GetBytes(stateD);
            for (int i = 0; i < 4; i++)
            {
                output[i] = outA[i];
                output[i + 4] = outB[i];
                output[i + 8] = outC[i];
                output[i + 12] = outD[i];
            }
            return output;
        }
    }
}
