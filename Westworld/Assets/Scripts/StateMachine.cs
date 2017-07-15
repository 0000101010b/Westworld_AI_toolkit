public class StateMachine <T> {
	
	private T agent;
	private State<T> current_state;
    private State<T> global_state;
    


	public void Awake () {
		this.current_state = null;
	}

	public void Init (T agent, State<T> startState, State<T> globalState) {
		this.agent = agent;
  
		this.current_state = startState;
        this.current_state.Enter(this.agent);
   
        this.global_state = globalState;
        this.global_state.Enter(this.agent);
    }

	public void Update () {
        if (this.global_state != null) this.global_state.Execute(this.agent);

		if (this.current_state != null) this.current_state.Execute(this.agent);
	}
	
	public void ChangeState (State<T> nextState) {
		if (this.current_state != null) this.current_state.Exit(this.agent);
		this.current_state = nextState;
		if (this.current_state != null) this.current_state.Enter(this.agent);
	}
}