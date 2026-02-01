using System.Collections.Generic;

namespace ProgressGraph
{
    public static class Progress
    {
        private static ProgressState _state = new ProgressState();

        // --- Flag Registration ---

        public static void DefineFlag(string id, bool oneWay = true)
        {
            if (_state.Flags.ContainsKey(id))
                return;

            _state.Flags[id] = new ProgressFlag(id, oneWay);
        }

        // --- Dependencies ---
        public static void Require(string targetFlag, string[] requiredFlags)
        {
            foreach (string flag in requiredFlags)
            {
                Require(targetFlag, flag);
            }
        }

        public static void Require(string targetFlag, string requiredFlag)
        {
            _state.Dependencies.Add(new ProgressDependency
            {
                TargetFlag = targetFlag,
                RequiredFlag = requiredFlag
            });
        }

        // --- Queries ---

        public static bool TrySet(string id)
        {
            if (CanSet(id))
            {
                Set(id);
                return true;
            }
            return false;
        }

        public static bool IsSet(string id)
        {
            return _state.Flags.TryGetValue(id, out var flag) && flag.IsSet;
        }

        public static bool CanSet(string id)
        {
            if (!_state.Flags.TryGetValue(id, out var flag))
                return false;

            foreach (var dep in _state.Dependencies)
            {
                if (dep.TargetFlag != id)
                    continue;

                if (!IsSet(dep.RequiredFlag))
                    return false;
            }

            return true;
        }

        // --- Mutation ---

        public static bool Set(string id)
        {
            if (!CanSet(id))
                return false;

            var flag = _state.Flags[id];
            flag.IsSet = true;
            return true;
        }

        public static bool Reset(string id)
        {
            if (!_state.Flags.TryGetValue(id, out var flag))
                return false;

            if (flag.IsOneWay)
                return false;

            flag.IsSet = false;
            return true;
        }
        public static List<string> GetUnmetRequirements(string id)
        {
            var unmet = new List<string>();

            if (!_state.Flags.ContainsKey(id))
                return unmet;

            foreach (var dep in _state.Dependencies)
            {
                if (dep.TargetFlag != id)
                    continue;

                if (!IsSet(dep.RequiredFlag))
                    unmet.Add(dep.RequiredFlag);
            }

            return unmet;
        }



        public static ProgressSnapshot Export()
        {
            var snapshot = new ProgressSnapshot();

            foreach (var flag in _state.Flags.Values)
            {
                if (flag.IsSet)
                    snapshot.SetFlags.Add(flag.Id);
            }

            return snapshot;
        }

        public static void Import(ProgressSnapshot snapshot)
        {
            foreach (var id in snapshot.SetFlags)
            {
                if (_state.Flags.TryGetValue(id, out var flag))
                    flag.IsSet = true;
            }
        }
        public static void ResetAll()
        {
            foreach (var flag in _state.Flags.Values)
            {
                flag.IsSet = false;
            }
        }

    }

}