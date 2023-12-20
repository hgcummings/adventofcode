using HGC.AOC.Common;

namespace HGC.AOC._2023._20;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var modules = new Dictionary<string, IModule>();

        foreach (var line in this.ReadInputLines("input.txt"))
        {
            var parts = line.Split(" -> ");
            var targets = parts[1].Split(", ");
            if (parts[0] == "broadcaster")
            {
                modules[parts[0]] = new Broadcaster(targets);
            }
            else if (parts[0].StartsWith('%'))
            {
                modules[parts[0][1..]] = new FlipFlop(targets);
            }
            else if (parts[0].StartsWith('&'))
            {
                modules[parts[0][1..]] = new Conjunction(targets);
            }
            else
            {
                throw new Exception("Unrecognised module type");
            }
        }

        foreach (var entry in modules)
        {
            foreach (var targetName in entry.Value.Targets)
            {
                if (modules.ContainsKey(targetName))
                {
                    var target = modules[targetName];
                    if (target is Conjunction conjunction)
                    {
                        conjunction.AddInput(entry.Key);
                    }
                }
            }
        }
        
        var pulses = new Queue<Pulse>();
        var hiCount = 0L;
        var lowCount = 0L;

        for (var i = 0; i < 1000; ++i)
        {
            pulses.Enqueue(new Pulse("button", "broadcaster", false));

            while (pulses.TryDequeue(out var input))
            {
                if (input.Value)
                {
                    hiCount++;
                }
                else
                {
                    lowCount++;
                }

                if (modules.ContainsKey(input.To))
                {
                    foreach (var output in modules[input.To].Accept(input))
                    {
                        pulses.Enqueue(output);
                    }
                }
            }
        }
        
        return hiCount * lowCount;
    }

    public struct Pulse
    {
        public Pulse(string from, string to, bool value)
        {
            From = from;
            To = to;
            Value = value;
        }

        public string From { get; }
        public string To { get; }
        public bool Value { get; }
    }

    public class FlipFlop(string[] targets) : IModule
    {
        private bool state = false;

        public IEnumerable<Pulse> Accept(Pulse pulse)
        {
            if (!pulse.Value)
            {
                state = !state;
                return targets.Select(t => new Pulse(pulse.To, t, state));
            }
            return Enumerable.Empty<Pulse>();
        }

        public IEnumerable<string> Targets => targets;
    }

    public class Conjunction(string[] targets) : IModule
    {
        private readonly Dictionary<string, bool> state = new();
        private readonly string[] targets = targets;

        public void AddInput(string name)
        {
            state[name] = false;
        }

        public IEnumerable<Pulse> Accept(Pulse pulse)
        {
            state[pulse.From] = pulse.Value;

            if (state.All(entry => entry.Value))
            {
                return targets.Select(t => new Pulse(pulse.To, t, false));
            }

            return targets.Select(t => new Pulse(pulse.To, t, true));
        }

        public IEnumerable<string> Targets => targets;
    }

    public class Broadcaster(string[] targets) : IModule
    {
        public IEnumerable<Pulse> Accept(Pulse pulse)
        {
            return targets.Select(t => new Pulse(pulse.To, t, pulse.Value));
        }

        public IEnumerable<string> Targets => targets;
    }
    
    public interface IModule
    {
        public IEnumerable<Pulse> Accept(Pulse pulse);
        public IEnumerable<string> Targets { get; }
    }
}