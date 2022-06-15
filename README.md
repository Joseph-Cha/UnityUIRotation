# Unity UI Rotaion 
Screen이 가로 모드 또는 세로 모드일 때 UI의 배치 및 크기가 달라지는 것을 대응하기 위한 모듈입니다.
가로 해상도로 UI 작업을 완료한 후 저장을 하고 마찬가지로 세로 해상도에서 UI 작업을 완료한 후 저장을 해주면
화면이 회전 되었을 때 자동으로 가로 세로 해상도에 맞는 UI를 셋팅합니다.

# 사용 방법

## 사전 준비
- 태그에서 "Ignore" 태그 추가
- 빈 오브젝트에 `ComponentsManager` 스크립트 부착

## UI 데이터 저장
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

## 샘플
![샘플](https://github.com/Joseph-Cha/UnityRotateUI/blob/main/GIF/Sample.gif?raw=true)
