
# UI Rotaion 사용법 
## 목적
Screen이 가로 모드 또는 세로 모드일 때 UI의 배치 및 크기가 달라지는 것을 대응하기 위한 모듈입니다.
GameView에서 가로 해상도로 UI 작업을 완료한 후 저장을 하고 마찬가지로 세로 해상도에서 UI 작업을 완료한 후 저장한 다음에
화면이 회전이 되면 각 해상도에 맞게 불러오는 작업을 수행합니다.

## 사전 준비
- 태그에서 "Dynamic" 태그 추가
- 빈 오브젝트에 `ComponentsManager` 스크립트 부착
- Newtonsoft.Json Package 다운로드
  - 에셋스토어에서 **JSON .NET For Unity** 다운로드[[링크](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)]

## 저장
### UI 데이터 저장
![화면 캡처 2021-04-19 094043](https://user-images.githubusercontent.com/75019048/115168549-4da52680-a0f6-11eb-9644-b65024b0d5c2.jpg)
1. Target이 될 Canvas에 `ComponentProperty` 스크립트 부착
2. 가로 또는 세로 해상도에서 UI 화면 작업 진행
3. ComponentProperty의 컴포넌트 메뉴에서 `Save` 버튼 클릭(또는 `shift + s` 단축키 입력)
4. Assets/Resources/JsonData/{Orientation}/{SceneName}/{CanvasName} 경로에 저장 완료

## 불러오기
### 에디터 모드에서 불러오기
1. 가로 또는 세로 해상도로 변경
2. `ComponentsManager` 스크립트가 부착된 오브젝트를 하이어라키 창에서 클릭 후 'shift + l` 단축키 입력
### 플레이 모드 중 불러오기
1. 플레이 버튼 클릭
2. 해상도를 바꾸면 자동으로 해당 해상도로 작업했던 UI 화면이 불러와 짐

# UI Rotaion Class 설계
### 용어 설명

- 노드 : 하이어라키에 있는 하나의 UI 게임 오브젝트

### [ComponentsNode.cs](Assets/UIRotation/Script/ComponentsNode.cs)

- 역할

    : 부모 노드에서 자식의 component 데이터를 보관

**[Propertises]**

- Name : string

    : 부모 노드의 이름

- ComponentInfos : List<ComponentInfo>

    : 부모 노드에 붙어있는 Component들의 정보

- Children : List<ComponentsNode>

    : 부모 노드 하위에 있는 자식 노드들

### [ComponentInfo.cs](Assets/UIRotation/Script/ComponentInfo.cs)

- 역할

    : 노드에 붙어 있는 한가지의 Component 정보를 보관

**[Propertises]**

- string Name

    : Component의 이름

- List<PropertyNameValuePair> Properties

    : 노드에 붙어있는 Component의 이름과 값을 보관

- Dictionary<Type, string[]> PropertyNames

    : Component 타입별로 속성의 이름을 저장 -> 향후 저장이 될 속성 값을 별도로 명시

**[Methods]**

- ComponentInfo(string, Component)

    : ComponentInfo를 생성할 때 component의 타입에 따라 데이터 저장

- AddSerializeData(object) : void

    : Component 타입 별로 Component 정보를 저장

    : object : Component 타입

- SetPropertyValueByComponent(object) : void

    : Properties 저장되어 있던 Component 속성 값을 타겟 object에 할당

    : object : 값을 넣을 타겟

### [PropertyNameValuePair.cs](Assets/UIRotation/Script/PropertyNameValuePair.cs)

- 역할

    : Component 속성의 이름과 값 저장

**[Propertises]**

- Name : string

    : Component 속성의 이름

- Value : string

    : Component 속성의 값

### [ComponentProperty.cs](Assets/UIRotation/Script/ComponentProperty.cs)

- 역할

    : Orientation 변경될 때마다 해당 Orientation에 대응하는 Component의 속성 값을 가지고 와서 현재 UI에 값을 할당

**[Propertises]**

- Root : Transform
    - 최상단 노드

**[Methods]**

- Save : void

    : Root의 자식 노드를 탐색하여 트리 구조로 Component 값을 저장한 후 직렬화하여 Resources 폴더에 저장

- Load : void

    : Orientation 변경될 때마다 해당 Orientation에 대응하는 데이터를 역직렬화한 후 Root의 자식 노드에 앞의 데이터를 할당

### [ComponentsManager.cs](Assets/UIRotation/Script/ComponentsManager.cs)

- 역할

    : Orientation 변경될 때 모든 ComponentProperty의 Load 메서드를 Callback 하기 위해 설계

**[Property]**

- OnLoadEventHandler : event

    : ComponentProperty의 Load 메서드를 저장

**[Methods]**

- Update() : void

    : Orientation 변경될 때 ComponentProperty의 Load 메서드를 callback

- OnLoadMenu() : void

    : 단축기를 통해 Load 메서드를 callback