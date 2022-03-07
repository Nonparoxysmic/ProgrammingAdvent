// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day20 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            List<Particle> initialParticles = new List<Particle>();
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (!Regex.IsMatch(inputLines[i],
                    @"^p=<-?\d+,-?\d+,-?\d+>, v=<-?\d+,-?\d+,-?\d+>, a=<-?\d+,-?\d+,-?\d+>$"))
                {
                    output.WriteError($"Invalid line in input: {inputLines[i]}", sw);
                    return output;
                }
                MatchCollection matches = Regex.Matches(inputLines[i], @"(?<=[<,])-?\d+(?=[,>])");

                Vector3Int pos = new Vector3Int(int.Parse(matches[0].Value),
                    int.Parse(matches[1].Value), int.Parse(matches[2].Value));
                Vector3Int vel = new Vector3Int(int.Parse(matches[3].Value),
                    int.Parse(matches[4].Value), int.Parse(matches[5].Value));
                Vector3Int acc = new Vector3Int(int.Parse(matches[6].Value),
                    int.Parse(matches[7].Value), int.Parse(matches[8].Value));
                initialParticles.Add(new Particle(i, pos, vel, acc));
            }

            int partOneAnswer = PartOneAnswer(initialParticles);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int PartOneAnswer(List<Particle> initialParticles)
        {
            // Copy the list of particles to an array so the list can be used for Part Two.
            Particle[] particles = new Particle[initialParticles.Count];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(initialParticles[i]);
            }

            // Return the ID of the particle with the smallest acceleration
            // if there is exactly one.
            List<Particle> slowestAccelParticles = new List<Particle>();
            int smallestAcceleration = int.MaxValue;
            foreach (Particle p in particles)
            {
                int acceleration = p.Acceleration.TaxicabMagnitude();
                if (acceleration < smallestAcceleration)
                {
                    slowestAccelParticles.Clear();
                }
                if (acceleration <= smallestAcceleration)
                {
                    slowestAccelParticles.Add(p);
                    smallestAcceleration = acceleration;
                }
            }
            if (slowestAccelParticles.Count == 1) { return slowestAccelParticles[0].ID; }

            // If multiple particles are found with the smallest acceleration,
            // tick the simulation until no particles are accelerating
            // toward the origin in any dimension.
            bool particlesAwayFromOrigin = false;
            while (!particlesAwayFromOrigin)
            {
                particlesAwayFromOrigin = true;
                foreach (Particle p in slowestAccelParticles)
                {
                    p.Tick();
                    if (!p.IsAwayFromOrigin())
                    {
                        particlesAwayFromOrigin = false;
                    }
                }
            }
            
            // Return the ID of the slowest of the particles with the smallest acceleration
            // if there is exactly one.
            List<Particle> slowestParticles = new List<Particle>();
            int slowestSpeed = int.MaxValue;
            foreach (Particle p in slowestAccelParticles)
            {
                int speed = p.Velocity.TaxicabMagnitude();
                if (speed < slowestSpeed)
                {
                    slowestParticles.Clear();
                }
                if (speed <= slowestSpeed)
                {
                    slowestParticles.Add(p);
                    slowestSpeed = speed;
                }
            }
            if (slowestParticles.Count == 1) { return slowestParticles[0].ID; }

            // Return the ID of the closest to the origin of the slowest of the particles
            // with the smallest acceleration. There should only be one.
            List<Particle> closestParticles = new List<Particle>();
            int closestDistance = int.MaxValue;
            foreach (Particle p in slowestParticles)
            {
                int distance = p.Position.TaxicabMagnitude();
                if (distance < closestDistance)
                {
                    closestParticles.Clear();
                }
                if (distance <= closestDistance)
                {
                    closestParticles.Add(p);
                    closestDistance = distance;
                }
            }
            return closestParticles[0].ID;
        }

        private class Particle
        {
            internal int ID { get; }
            internal Vector3Int Position { get; set; }
            internal Vector3Int Velocity { get; set; }
            internal Vector3Int Acceleration { get; }

            public Particle(int id, Vector3Int position,
                Vector3Int velocity, Vector3Int acceleration)
            {
                ID = id;
                Position = position;
                Velocity = velocity;
                Acceleration = acceleration;
            }

            public Particle(Particle particleToCopy)
            {
                ID = particleToCopy.ID;
                Position = new Vector3Int(particleToCopy.Position);
                Velocity = new Vector3Int(particleToCopy.Velocity);
                Acceleration = new Vector3Int(particleToCopy.Acceleration);
            }

            internal void Tick()
            {
                Velocity.X += Acceleration.X;
                Velocity.Y += Acceleration.Y;
                Velocity.Z += Acceleration.Z;
                Position.X += Velocity.X;
                Position.Y += Velocity.Y;
                Position.Z += Velocity.Z;
            }

            internal bool IsAwayFromOrigin()
            {
                return SignsMatch(Position.X, Velocity.X, Acceleration.X)
                    && SignsMatch(Position.Y, Velocity.Y, Acceleration.Y)
                    && SignsMatch(Position.Z, Velocity.Z, Acceleration.Z);
            }

            private bool SignsMatch(int position, int velocity, int acceleration)
            {
                if (acceleration == 0)
                {
                    if (velocity == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return Math.Sign(position) == Math.Sign(velocity);
                    }
                }
                else
                {
                    return Math.Sign(position) == Math.Sign(velocity)
                        && Math.Sign(velocity) == Math.Sign(acceleration);
                }
            }
        }
    }
}
