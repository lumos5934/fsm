# FSM
조건 기반 Transition을 지원하는 FSM 구현입니다.<br>
State 별 생명주기 관리와 조건에 따른 상태 전환을 지원합니다. <br>

<br>
<br>
<br>

## 🔧Usage

State를 구현하고 StateMachine에 Transition을 등록하여 사용합니다.<br>
StateMachine은 현재 State를 관리하며, Update 호출 시 등록된 조건을 검사하여 조건이 만족되면 자동으로 State를 전환합니다.

<br>
<br>

#### State 구현
`IState`를 상속하여 State를 구현합니다. <br>

```cs
public class IdleState : IState
{
    public void OnEnter()
    {
        // State 진입 시 호출
    }

    public void OnUpdate()
    {
        // Update 호출마다 실행
    }

    public void OnFixedUpdate()
    {
        // FixedUpdate 호출마다 실행
    }

    public void OnExit()
    {
        // State 종료 시 호출
    }
}
```

<br> 
<br>

#### State 등록
```cs
var idle = new IdleState();
var run = new RunState();

StateMachine stateMachine = new();

stateMachine.SetState(idle);
```

<br>
<br>

#### Transition 등록

특정 State에서 다른 State로 전환되는 조건을 등록합니다. <br>

```cs

stateMachine.AddTransition(
    idle,
    run,
    () => speed > 0
);

```

현재 State가 Idle이고 speed > 0 조건이 만족되면 Run State로 전환됩니다.<br>

<br>
<br>

#### Any Transition

현재 State와 관계없이 동작하는 Transition을 등록합니다.<br>

```cs
stateMachine.AddAnyTransition(
    dead,
    () => hp <= 0
);
```

모든 State에서 조건이 만족되면 해당 State로 전환됩니다. <br>

<br> 
<br>

#### Update

StateMachine의 Update를 호출하여 상태 전환 및 State Update를 처리합니다.<br>

```cs
void Update()
{
    stateMachine.Update();
}

void FixedUpdate()
{
    stateMachine.FixedUpdate();
}
```

* **동작 순서:**

```cs
1. Transition 조건 검사
2. 조건 만족 시 State 변경
3. Current State Update 실행
```

<br> 
<br>

#### State 변경 이벤트
State 변경 시 이전 State와 현재 State를 전달받을 수 있습니다. <br>

```cs
stateMachine.OnStateChanged += (from, to) =>
{
    Debug.Log($"{from} -> {to}");
};
```

<br> 
<br> 
<br>


## 📖API

#### StateMachine
**`SetState(IState state)`** : 현재 State를 변경합니다. 기존 State가 존재하면 `OnExit()`가 호출되고, 새로운 State는 `OnEnter()`가 호출됩니다.<br>
**`Update()`** : Transition 조건을 검사하고 조건이 만족되면 State를 변경한 뒤 현재 State의 `OnUpdate()`를 실행합니다.<br>
**`FixedUpdate()`** : 현재 State의 `OnFixedUpdate()`를 실행합니다.<br>
**`AddTransition(IState from, IState to, Func<bool> condition)`** : 특정 State에서 조건 만족 시 다른 State로 전환되는 Transition을 추가합니다.<br>
**`AddAnyTransition(IState to, Func<bool> condition)`** : 현재 State와 관계없이 조건 만족 시 전환되는 Transition을 추가합니다.<br>
**`CurrentState`** : 현재 활성화된 State를 반환합니다.<br>
**`OnStateChanged`** : State 변경 시 호출되는 이벤트입니다. 이전 State와 변경된 State를 전달합니다.<br>


<br>


#### IState
**`OnEnter()`** : State 진입 시 호출됩니다.<br>
**`OnUpdate()`** : StateMachine의 `Update()` 호출 시 현재 State에서 실행됩니다.<br>
**`OnFixedUpdate()`** : StateMachine의 `FixedUpdate()` 호출 시 현재 State에서 실행됩니다.<br>
**`OnExit()`** : State 종료 시 호출됩니다.<br>


<br>

#### Transition
**`TargetState`** : Transition 조건 만족 시 이동할 State입니다.<br>
**`Condition()`** : Transition 실행 여부를 판단하는 조건입니다.<br>


