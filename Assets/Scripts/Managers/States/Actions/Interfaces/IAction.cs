using System;

namespace Managers.States.Actions.Interfaces {
    /// <summary>
    /// Represent an action that can be performed during some state of the game (e.g. MouseClickAction)
    /// </summary>
    public interface IAction{
        /// <summary>
        /// The strategy method called to execute the action.
        /// </summary>
       void Execute();
    }
}