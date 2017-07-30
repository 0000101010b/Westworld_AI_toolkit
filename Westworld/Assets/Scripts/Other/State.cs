abstract public class State <T> {

	abstract public void Enter (T agent);
	abstract public void Execute (T agent);
	abstract public void Exit (T agent);
}