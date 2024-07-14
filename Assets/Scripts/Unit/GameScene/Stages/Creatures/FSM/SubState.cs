namespace Unit.GameScene.Stages.Creatures.FSM {
    //public class SubState : StateMachine, IState {
    //    protected StateMachine _parent;
    //    protected Dictionary<string, Func<BaseCharacter, bool>> conditions = new Dictionary<string, Func<BaseCharacter, bool>>();
    //    protected IState _defaultState;
    //    protected Action<IState> _onEnter;
    //    protected Action<IState> _onExit;
    //    protected Action<IState> _onUpdate;
    //    protected Action<IState> _onFixedUpdate;
    //    protected Func<BaseCharacter, bool> _transitionCondition;
    //    protected string _name;
    //    protected int _parameterHash;
    //    public string StateName => _name;
    //    public int ParameterHash => _parameterHash;
    //    public StateMachine StateMachine => _parent;
    //    public SubState(StateMachine sm, IState defaultState, string name, int aniHash, Action<IState> onEnter = null, Action<IState> onExit = null, Action<IState> onUpdate = null, Action<IState> onFixedUpdate = null, Func<BaseCharacter, bool> condition = null) : base(sm.Target) {
    //        _name = name;
    //        _parameterHash = aniHash;
    //        _onEnter = onEnter;
    //        _onExit = onExit;
    //        _onUpdate = onUpdate;
    //        _onFixedUpdate = onFixedUpdate;
    //        _transitionCondition = condition;

    //        _parent = sm;
    //        _defaultState = defaultState;
    //    }
    //    void IState.Enter(BaseCharacter self) {
    //        _onEnter?.Invoke(this);
    //        foreach (var (name, condition) in conditions) {
    //            if (condition.Invoke(self)) {
    //                _current = _states[name];
    //                _current.Enter(self);
    //                break;
    //            }
    //        }
    //        _current = _defaultState;
    //        _defaultState.Enter(self);
    //    }
    //    void IState.Exit(BaseCharacter target) {
    //        _onExit?.Invoke(this);
    //        _current.Exit(target);
    //    }

    //    public override bool TryAddState(string name, IState state) {
    //        if (base.TryAddState(name, state)) {
    //            conditions.Add(name, state.CanTransitionToThis);
    //            return true;
    //        }
    //        else
    //            return false;
    //    }

    //    bool IState.CanTransitionToThis(BaseCharacter target) {
    //        if (!_transitionCondition.Invoke(target))
    //            return false;

    //        foreach (var (name, condition) in conditions) {
    //            if (condition.Invoke(target)) {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    public override void FixedUpdate(BaseCharacter target) {
    //        _onFixedUpdate?.Invoke(this);
    //        base.FixedUpdate(target);
    //    }

    //    public override void Update(BaseCharacter target) {
    //        _onUpdate?.Invoke(this);
    //        base.Update(target);
    //    }
    //}
}