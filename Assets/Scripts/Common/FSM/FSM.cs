using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.FSM {

    
    ///<summary>
    ///This is the main engine of our FSM, without this, you won't be
    ///able to use FSM States and FSM Actions.
    ///</summary>
    
    public class FSM
    {

        private readonly string name;
        private FSMState currentState;
        private readonly Dictionary<string, FSMState> stateMap;

        private delegate void StateActionProcessor(FSMAction action);

        public string Name
        {
           get
            {
                return name;
            }
        }

        //FSM Constructor, will initialize the FSM and give it a name.
        public FSM (string name)
        {
            this.name = name;
            this.currentState = null;
            stateMap = new Dictionary<string, FSMState>();

        }

        //Initializes the FSM. We can indicate the starting State of the Object
        public void Start(string stateName)
        {
            if(!stateMap.ContainsKey (stateName))
            {
                Debug.LogWarning("The FSM doesn't contain: " + stateName);
                return;
            }

            ChangeToState(stateMap[stateName]);
        }

        //Method to change the state of the Object. Will call the Exit state before going to the next
        public void ChangeToState(FSMState state)
        {
            if(this.currentState != null)
            {
                ExitState(this.currentState);
            }

            this.currentState = state;
            EnterState(this.currentState);
        }

        //Method to change the state of the Object. Not advisable to use this
        public void EnterState(FSMState state)
        {
            ProcessStateAction(state, delegate (FSMAction action)
           {
               action.OnEnter();
           });

        }

        //Exits the state
        private void ExitState(FSMState state)
        {
            FSMState currentStateOnInvoke = this.currentState;

            ProcessStateAction(state, delegate (FSMAction action)
           {

               if (this.currentState != currentStateOnInvoke)
                   Debug.LogError("State cannot be changed on exit of the specified state");

               action.OnExit();
           });
        }

        //Call this under a MonoBehaviour's Update 
        public void Update()
        {
            if (this.currentState == null)
                return;

            ProcessStateAction(this.currentState, delegate (FSMAction action) {
                action.OnUpdate();
            });
        }

        //Handles the events that are bound to a state and changes the state
        public void SendEvent(string eventId)
        {
            FSMState transitionState = ResolveTransition(eventId);

            if (transitionState == null)
                Debug.LogWarning("The current state has no transition for event " + eventId);
            else
                ChangeToState(transitionState);
        }

        private void ProcessStateAction (FSMState state, StateActionProcessor actionProcessor)
        {

            FSMState currentStateOnInvoke = this.currentState;
            IEnumerable<FSMAction> actions = state.GetActions();

            foreach (FSMAction action in actions)
            {
                if (this.currentState != currentStateOnInvoke)
                {
                    break;
                }

                actionProcessor(action);
            }
        }

        private FSMState ResolveTransition (string eventId)
        {
            FSMState transitionState = this.currentState.GetTransition(eventId);

            if (transitionState == null)
                return null;
            else
                return transitionState;
        }

        public FSMState AddState (string name)
        {
            if(stateMap.ContainsKey(name))
            {
                Debug.LogWarning("The FSM already contains: " + name);
                return null;
            }

            FSMState newState = new FSMState(name, this);
            stateMap[name] = newState;
            return newState;
        }
       
    }


}
