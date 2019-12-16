using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.FSM
{
    public class FSMState
    {
        private List<FSMAction> actions;
        private readonly string name;
        private FSM owner;
        private readonly Dictionary<string, FSMState> transitionMap;

        ///<summary>
        ///Initializes a new Instance of the <see cref="Common.FSM.FSMState"/> class
        ///</summary>
        ///<param name="name">Name.</param>
        ///<param owner="owner">Owner.</param>
        
        public FSMState (string name, FSM owner)
        {
            this.name = name;
            this.owner = owner;
            this.transitionMap = new Dictionary<string, FSMState>();
            this.actions = new List<FSMAction>();
        }

        //Adds the transition

        public void AddTransition(string id, FSMState destinationState)
        {
            if (transitionMap.ContainsKey(id))
            {
                Debug.LogError(string.Format("State {0} already contains transition for {1}", this.name, id));
                return;
            }

            transitionMap[id] = destinationState;
        }

        //Gets the transition
        public FSMState GetTransition (string eventID)
        {
            if (transitionMap.ContainsKey (eventID))
            {
                return transitionMap[eventID];
            }

            return null;
        }

        //Adds the action
        public void AddAction (FSMAction action)
        {
            if(actions.Contains(action))
            {
                Debug.LogWarning("This State already contains " + actions);
                return;
            }

            if(action.GetOwner() != this)
            {
                Debug.LogWarning("This state doesn't own " + actions);
            }

            actions.Add(action);
        }

        //Gets the action of the state
        public IEnumerable<FSMAction> GetActions()
        {
            return actions;
        }

        //Send the Event
        public void SendEvent (string eventId)
        {
            this.owner.SendEvent(eventId);
            
        }
    }
}