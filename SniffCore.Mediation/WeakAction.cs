using System;
using System.Reflection;

namespace SniffCore.Mediation
{
    internal class WeakAction<T>
    {
        private readonly MethodInfo _method;
        private readonly WeakReference _targetReference;

        public WeakAction(Action<T> action)
        {
            _targetReference = new WeakReference(action.Target);
            _method = action.Method;
        }

        public bool IsAlive => _targetReference.IsAlive;

        public bool HasAction(Action<T> action)
        {
            return _method.Equals(action.Method);
        }

        public Action<T> GetAction()
        {
            if (!IsAlive)
                return null;
            try
            {
                return Delegate.CreateDelegate(typeof(Action<T>), _targetReference.Target, _method.Name) as Action<T>;
            }
            catch
            {
                return null;
            }
        }
    }
}