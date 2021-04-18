# UI Rotaion Class 설계
### 용어 설명
- 노드 : 하이어라키에 있는 하나의 UI 오브젝트를 뜻함
## 1. ComponentsNode
- 역할
    - 부모 노드에서 자식의 component 데이터를 트리 구조로 보관하기 위해 설계
### Field
- Name : string
    - 부모 노드의 이름
- ComponentInfos : List<ComponentInfo>
    - 부모 노드에 붙어있는 Component 정보를 보관
- Children : List<ComponentsNode>
    - 부모 노드 하위에 있는 자식 노드를 보관

## 2. ComponentInfo
- 역할
    - 노드에 붙어 있는  한 가지의 Component 정보를 보관하기 위해 설계
### Field 
- Name : string
    - Component의 이름
- Properties : List<PropertyNameValuePair>
    - 노드에 붙어있는 Component의 이름과 값을 보관
- PropertyNames : Dictionary<Type, string[]>
    - Component 타입별로 속성의 이름을 저장 -> 향후 저장이 될 속성 값을 별도로 명시
### Method
- ComponentInfo(string, Component)
    - ComponentInfo를 생성할 때 component의 타입에 따라 데이터 저장
- AddSerializeData(object) : void
    - Component 타입 별로 Component 정보를 저장
        - object : Component 타입
- SetPropertyValueByComponent(object) : void
    - Properties 저장되어 있던 Component 속성 값을 타겟 object에 할당
        - object : 값을 넣을 타겟

## 3. PropertyNameValuePair
- 역할
    - Component 속성의 이름과 값을 보관
### Field
- Name : string
    - Component 속성의 이름
- Value : string
    - Component 속성의 값

## 4. ComponentProperty
- 역할
    - Orientation 변경될 때마다 해당 Orientation에 대응하는 Component의 속성 값을 가지고 와서 현재 UI에 값을 할당하도록 설계
### Field
- Root : Transform
    - 최상단 노드
### Method
- Save : void
    - Root의 자식 노드를 탐색하여 트리 구조로 Component 값을 저장한 후 직렬화하여 Resources 폴더에 저장
- Load : void
    - Orientation 변경될 때마다 해당 Orientation에 대응하는 데이터를 역직렬화한 후 Root의 자식 노드에 앞의 데이터를 할당

## 5. ComponentsManager
- 역할
    - Orientation 변경될 때 모든 ComponentProperty의 Load 메서드를 Callback 하기 위해 설계
### Field
- OnLoadEventHandler : event
    - ComponentProperty의 Load 메서드를 저장
### Method
- Update() : void
    - Orientation 변경될 때 ComponentProperty의 Load 메서드를 callback
- OnLoadMenu() : void
    - 단축기를 통해 Load 메서드를 callback