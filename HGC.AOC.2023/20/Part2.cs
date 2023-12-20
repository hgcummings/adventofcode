using HGC.AOC.Common;

namespace HGC.AOC._2023._20;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var machines = new List<Machine>();
        Machine currentMachine = null;
        
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            if (line.StartsWith("broadcaster"))
            {
                continue;
            }
            
            if (line == "")
            {
                if (currentMachine != null)
                {
                    machines.Add(currentMachine);
                }

                currentMachine = new Machine();
                continue;
            }

            if (currentMachine == null)
            {
                throw new InvalidOperationException();
            }
            
            var parts = line.Trim().Split(" -> ");
            var targets = parts[1].Split(", ");
            
            if (parts[0].StartsWith('%'))
            {
                currentMachine.Modules[parts[0][1..]] = new FlipFlop(targets);
            }
            else if (parts[0].StartsWith('&'))
            {
                currentMachine.Modules[parts[0][1..]] = new Conjunction(targets);
            }
            else
            {
                throw new Exception("Unrecognised module type");
            }

            if (currentMachine.EntryPoint == null)
            {
                currentMachine.EntryPoint = parts[0][1..];
            }
        }

        machines.Add(currentMachine);

        foreach (var machine in machines)
        {
            foreach (var entry in machine.Modules.Where(e => e.Key != "entry"))
            {
                foreach (var targetName in entry.Value.Targets)
                {
                    if (machine.Modules.TryGetValue(targetName, out var target))
                    {
                        if (target is Conjunction conjunction)
                        {
                            conjunction.AddInput(entry.Key);
                        }
                    }
                }
            }
        }

        return machines
            .Select((m, id) => FindCycleLength(id, m))
            .Aggregate(Arithmetic.LeastCommonMultiple);
    }

    public class Machine
    {
        public readonly Dictionary<string, IModule> Modules = new();
        public string? EntryPoint = null;
    }

    public long FindCycleLength(int id, Machine machine)
    {
        var pulses = new Queue<Pulse>();

        for (var i = 1L;; ++i)
        {
            if (i % 100000000 == 0)
            {
                Console.WriteLine($"{id}: {i}");
            }
            
            pulses.Enqueue(new Pulse("broadcaster", machine.EntryPoint!, false));

            while (pulses.TryDequeue(out var input))
            {
                if (input is { To: "rx", Value: false })
                {
                    return i;
                }

                if (machine.Modules.TryGetValue(input.To, out var module))
                {
                    foreach (var output in module.Accept(input))
                    {
                        pulses.Enqueue(output);
                    }
                }
            }
        }
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
        public bool State => state;
    }

    public class Conjunction(string[] targets) : IModule
    {
        private readonly Dictionary<string, bool> state = new();
        private bool result = false;
        private readonly string[] targets = targets;

        public void AddInput(string name)
        {
            state[name] = false;
        }

        public IEnumerable<Pulse> Accept(Pulse pulse)
        {
            state[pulse.From] = pulse.Value;

            result = state.Any(entry => !entry.Value);
            
            return targets.Select(t => new Pulse(pulse.To, t, result));
        }

        public IEnumerable<string> Targets => targets;
        public bool State => result;
    }

    public class Broadcaster(string[] targets) : IModule
    {
        public IEnumerable<Pulse> Accept(Pulse pulse)
        {
            return targets.Select(t => new Pulse(pulse.To, t, pulse.Value));
        }

        public IEnumerable<string> Targets => targets;
        public bool State => false;
    }
    
    public interface IModule
    {
        public IEnumerable<Pulse> Accept(Pulse pulse);
        public IEnumerable<string> Targets { get; }
        public bool State { get; }
    }
}