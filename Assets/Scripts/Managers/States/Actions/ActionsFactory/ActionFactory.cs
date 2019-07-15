using System;
using System.Collections;
using Managers.States.Actions.Interfaces;

namespace Managers.States.Actions.ActionsFactory {
    public class ActionFactory {
        private static readonly Hashtable Instances = new Hashtable();

        private ActionFactory() { }

        public static IAction GetAction<T>() where T : IAction, new() {
            var instance = Instances[typeof(T)];

            if (instance == null) {
                instance = Activator.CreateInstance(typeof(T));
                Instances[typeof(T)] = instance;
            }

            return instance as IAction;
        }
    }
}