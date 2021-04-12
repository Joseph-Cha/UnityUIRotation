# UI Rotaion Class 설계
## 1. ComponentsNode
- 역할
    - Root에서 자식들의 component 데이터를 트리 구조로 보관하기 위해 설계
### Field
- Name : string
    - Component가 붙어 있는 GameObject 이름
- ComponentInfos : List<ComponentInfo>
    - 하나의 대상 게임오브젝트에 붙어있는 Component들의 정보를 보관
- Children : List<ComponentsNode>
    - 해당 노드의 자식 노드를 보관

## 2. ComponentInfo
- 역할
    - GameObject에 붙어 있는  한 가지의 Component 정보를 보관하기 위해 설계
### Field 
- Name : string
    - Component의 이름
- Properties : List<PropertyNameValuePair>
    - GameObject에 붙어있는 Component들의 이름과 값을 보관
- (#region)PropertyNames
    - Component 속성 값 중 저장하기를 원하는 속성의 이름
### Method
- ComponentInfo(string, object) 
    - ComponentInfo을 생성할 때 component의 타입에 따라 데이터 저장
- AddPropertiesByPropertyNames(string, object) 
    - Component 타입 별로 Component 정보를 저장
    - string : 해당 타입의 속성의 배열
    - object : 타겟이 되는 component
- SetPropertyValueByComponent(Component)
    - Component 타입에 따라 Properties 저장되에 있던 Component 값 셋팅
    - Component : 값을 넣을 타겟

## 3. PropertyNameValuePair
- 역할
    - 한 가지 Component 속성들의 이름과 값을 보관
### Field
- Name : string
    - Component 속성의 이름
- Value : string
    - Component 속성의 값

## 4. ComponentProperty
- 역할
    - Orientation 변경될 때마다 해당 Orientation에 대응하는 Components의 속성 값으로 할당하도록 설계
### Field
- Root : Transform
### Method
- Save
    - Root의 자식 Object를 탐색하여 ComponentsNode에 트리 구조로 Component 값을 저장 후 직렬화하여 Resources 폴더에 저장
- Load
    - Orientation 변경될 때마다 해당 Orientation에 대응하는 데이터를 역직렬화한 후 Root의 자식 Object를 탐색하여 각 Component에 값을 할당

## 5. ComponentsManager
- 역할
    - Orientation 변경될 때 모든 ComponentProperty의 Load 메서드를 callback 하기위해 설계
### Field
- OnLoadEventHandler : event
    - ComponentProperty의 Load 메서드를 저장
### Method
- Update()
    - Orientation 변경될 때 ComponentProperty의 Load 메서드를 callback
- OnLoadMenu()
    - 단축기를 통해 Load 메서드를 callback