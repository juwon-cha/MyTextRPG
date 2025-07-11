# MyTextRPG
## <a id="toc"></a>목차

1. [프로젝트 개요](#1-프로젝트-개요)
2. [클래스 구조](#2-클래스-구조)
3. [프로젝트 구조](#3-프로젝트-구조)
4. [핵심 로직 및 기능](#4-핵심-로직-및-기능)
5. [주요 클래스 상세 설명](#5-주요-클래스-상세-설명)
6. [개선 및 확장 방향](#6-개선-및-확장-방향)

---

## 1. 프로젝트 개요

C#을 사용하여 개발된 텍스트 기반의 롤플레잉 게임이다. 플레이어는 캐릭터를 생성하고, 마을에서 장비를 구매하거나 휴식을 취하며, 던전에 입장하여 몬스터와 싸우고 보상을 얻는 방식으로 게임을 진행한다. 이 문서는 프로젝트의 전체적인 구조, 핵심 로직, 데이터 흐름 등을 상세히 기술하여 개발 및 유지보수를 돕는 것을 목적으로 한다.

## 2. 클래스 구조

<img width="3840" height="2148" alt="TextRPGDiagram" src="https://github.com/user-attachments/assets/1e15f99b-2836-4aa6-8c87-d8994d107f35" />

[BackToTop](#toc)

## 3. 프로젝트 구조

프로젝트는 다음과 같은 주요 네임스페이스와 디렉토리로 구성되어 있다.

- **SpartaDungeon/**: 메인 프로젝트 디렉토리
    - **Program.cs**: 게임의 진입점(Entry Point). `Main` 함수가 포함되어 있으며, `Manager` 싱글톤 인스턴스를 통해 게임 루프를 실행한다.
    - **Controller/**: 게임 내 캐릭터(플레이어, 몬스터) 및 인벤토리의 로직을 제어하는 클래스들을 포함한다.
        - `CharacterController.cs`: 플레이어와 몬스터의 공통 속성(레벨, 이름, 체력 등)을 정의하는 기반 클래스이다.
        - `PlayerController.cs`: 플레이어의 고유 기능(직업, 능력치 계산, 아이템 관리, 골드 관리 등)을 담당한다.
        - `InventoryController.cs`: 플레이어의 인벤토리를 관리하며, 아이템 추가/제거/조회 기능을 제공한다.
        - `MonsterController.cs`: 몬스터 캐릭터를 위한 클래스 (현재는 기본 구조만 존재).
    - **Data/**: 게임 데이터(플레이어 스탯, 아이템, 던전 정보)를 정의하고, JSON 파일과의 직렬화/역직렬화를 관리한다.
        - `GameData.cs`: 게임에 필요한 각종 데이터 구조체, 클래스와 데이터 로더(`PlayerStatLoader`, `ItemLoader` 등)를 정의한다.
        - `SaveData.cs`: 게임 저장에 사용되는 `UserData` 클래스와 관련 데이터 구조를 정의한다.
    - **Items/**: 게임 내 아이템과 관련된 클래스들을 포함한다.
        - `Item.cs`: 모든 아이템의 기반이 되는 추상적인 `Item` 클래스와 파생 클래스(`Weapon`, `Armor`)를 정의한다. 아이템 생성(`MakeItem`) 메서드를 포함한다.
    - **Managers/**: 게임의 핵심 시스템(데이터, 씬, 아이템, 던전)을 관리하는 매니저 클래스들을 포함한다.
        - `Manager.cs`: 모든 매니저 클래스에 대한 접근을 제공하는 **싱글톤(Singleton)** 클래스이다. 게임 시작 시 모든 매니저를 초기화한다.
        - `GameManager.cs`: 게임의 전반적인 상태와 플레이어 데이터(`PlayerController`)를 관리하며, 게임 저장/로드 로직을 담당한다.
        - `DataManager.cs`: `Newtonsoft.Json`을 사용하여 JSON 형식의 게임 데이터를 불러오고 저장하는 정적(static) 클래스이다.
        - `SceneManager.cs`: 게임의 각 씬(마을, 상점, 던전 등)을 `Dictionary`로 관리하고, 씬 전환 로직을 처리한다.
        - `ItemManager.cs`: 게임에 존재하는 모든 아이템을 `Dictionary`로 관리하며, 상점 목록 제공 및 아이템 조회를 담당한다.
        - `DungeonManager.cs`: 던전 입장 시도, 성공/실패 판정, 보상 및 패널티 계산 로직을 처리한다.
    - **Scene/**: 게임의 각 화면(Scene)을 정의하는 클래스들을 포함한다.
        - `IDisplayable.cs`: 모든 씬 클래스가 구현해야 하는 인터페이스로, `Display()` 메서드를 통해 각 씬의 화면을 출력하고 사용자 입력을 처리한다.
        - `TitleScene.cs`, `TownScene.cs`, `StoreScene.cs` 등: 각 게임 화면의 UI 출력과 사용자 입력 처리를 담당하는 구체적인 씬 클래스들이다.
    - **Utils/**: 프로젝트 전반에서 사용되는 유틸리티 클래스들을 포함한다.
        - `StringConverter.cs`: `EClassType` 열거형을 문자열(예: "전사")로 변환하는 기능을 제공한다.
        - `StringFormatting.cs`: 한글/영문 혼합 문자열의 출력 길이를 계산하고, 콘솔에 텍스트를 정렬하여 출력하기 위한 포맷팅 기능을 제공한다.
- **Data/**: 게임 데이터 파일(JSON)을 저장하는 외부 디렉토리
    - **GameData/**: 기본적인 게임 데이터(아이템, 직업별 스탯, 던전 정보 등)
    - **SaveData/**: 플레이어의 게임 진행 상황 저장 데이터 (`UserData.json`)

[BackToTop](#toc)

## 4. 핵심 로직 및 기능

### 4.1. 게임 실행 흐름

1.  **초기화**: `Program.Main()`에서 `Manager.Instance`를 호출하여 싱글톤 객체를 생성하고, `Manager.Init()`을 통해 모든 하위 매니저(`DataManager`, `SceneManager` 등)를 초기화한다.
2.  **게임 루프**: `Program.Main()`의 `while` 루프가 `SceneManager.IsGameRunning()`을 확인하며, `SceneManager.DisplayScene()`을 반복 호출하여 현재 활성화된 씬의 `Display()` 메서드를 실행한다.
3.  **씬 처리**: 각 씬의 `Display()` 메서드는 해당 씬의 UI를 콘솔에 출력하고, `Console.ReadLine()`으로 사용자 입력을 받아 로직을 처리한다. 입력에 따라 `SceneManager.ChangeScene()`을 호출하여 다른 씬으로 전환한다.
4.  **게임 종료**: `TitleScene`이나 `TownScene`에서 '나가기'를 선택하면 `SceneManager.ExitGame()`이 호출되어 `IsGameRunning` 플래그가 `false`로 바뀌고, 게임 루프가 종료된다.

### 4.2. 데이터 관리

- **정적 데이터 로드**: `DataManager.LoadGameData()`는 게임 시작 시 `Data/GameData` 폴더의 `ItemData.json`, `PlayerStat.json`, `DungeonData.json` 파일을 읽어와 각각의 `Dictionary`에 정적 데이터로 저장한다. 이는 게임 내내 변하지 않는 기본 정보이다.
- **동적 데이터 (저장/로드)**:
    - **저장**: `SceneManager.ChangeScene()`이 호출(씬 전환)될 때마다 `GameManager.SaveGame()`이 실행된다. `GameManager`는 현재 `PlayerController`의 상태(`UserData`)를 `DataManager.SaveUserData()`를 통해 `Data/SaveData/UserData.json` 파일에 덮어쓴다.
    - **로드**: `TitleScene`에서 'Load Game'을 선택하면 `GameManager.LoadGame()`이 실행된다. `DataManager.LoadUserData()`로 JSON 파일을 읽고, `PlayerController.LoadPlayerSettings()`를 통해 플레이어의 상태를 복원한다.
- **JSON 처리**: `Newtonsoft.Json` 라이브러리를 사용하여 C# 객체와 JSON 문자열 간의 직렬화/역직렬화를 수행한다. `ILoader` 인터페이스와 제네릭 메서드 `LoadJson<T, Key, Value>()`를 통해 다양한 데이터 구조를 유연하게 로드한다.

### 4.3. 플레이어 및 아이템 시스템

- **능력치 계산**: `PlayerController`의 `Attack`과 `Defense` 속성은 기본 능력치(`BaseAttack/Defense`)와 장비로 인한 추가 능력치(`BonusAttack/Defense`)의 합으로 계산된다. `RecalculateStats()` 메서드는 아이템 장착/해제 시 인벤토리를 순회하며 보너스 능력치를 다시 계산하여 적용한다.
- **아이템 관리**:
    - `ItemManager`는 게임 시작 시 모든 아이템 정보를 `Items` 딕셔너리에 로드한다. 상점에서는 이 `Items` 딕셔너리를 기반으로 판매 목록을 표시한다.
    - `InventoryController`는 플레이어가 소유한 아이템을 `Inventory` 딕셔너리에서 관리한다.
    - **구매**: `StoreScene`에서 아이템을 구매하면 `PlayerController.BuyItem()`이 호출된다. 골드 차감 후 `InventoryController.AddItem()`으로 아이템을 인벤토리에 추가하고, 아이템의 `HasPurchased` 플래그를 `true`로 설정한다.
    - **판매**: `StoreScene`에서 아이템을 판매하면 `PlayerController.SellItem()`이 호출된다. `InventoryController.RemoveItem()`으로 아이템을 제거하고, 판매 가격만큼 골드를 획득한다. 만약 판매한 아이템이 장착 중이었다면 `ToggleEquipItem()`을 통해 장착 해제 및 능력치 재계산을 수행한다.
    - **장착**: `InventoryScene`에서 아이템을 장착/해제하면 `PlayerController.EquipItem()`이 호출된다. 아이템의 `HasEquipped` 상태를 변경하고, `RecalculateStats()`를 호출하여 능력치를 갱신한다.

### 4.4. 던전 시스템

- `DungeonScene`에서 던전 난이도를 선택하면 `DungeonManager.TryEnterDungeon()`이 호출된다.
- **성공/실패 판정**: 플레이어의 방어력(`Defense`)이 던전의 권장 방어력(`RecommendedDefense`)보다 낮을 경우, 40% 확률로 던전 공략에 실패하고 체력의 절반을 잃는다.
- **성공 시 로직**: 던전 공략에 성공하면 기본 피해량(20~35)에서 `(플레이어 방어력 - 권장 방어력)`만큼 감산된 최종 피해를 입는다. 이후 기본 보상 골드에 플레이어의 공격력에 비례하는 보너스 골드를 추가로 획득하고, 레벨이 1 오른다. 레벨업 시 캐릭터의 기본 공격력이 0.5, 방어력이 1 증가한다.
- **결과 처리**: `TryEnterDungeon()`은 `DungeonResult` 객체를 반환하며, `DungeonScene`은 이 객체의 `IsClear` 값에 따라 '던전 클리어' 또는 '공략 실패' UI를 표시한다.

[BackToTop](#toc)

## 5. 주요 클래스 상세 설명

- **`Manager` (Singleton)**: 모든 매니저에 대한 전역적인 접근점(`Manager.Instance`)을 제공하여 코드의 어느 곳에서든 `Manager.Instance.Scene`, `Manager.Instance.Game` 등으로 각 매니저의 기능에 접근할 수 있게 한다.
- **`PlayerController`**: 게임의 주인공. 플레이어의 모든 데이터를 가지고 있으며, 아이템 구매/판매, 장착, 휴식 등 대부분의 상호작용 로직을 직접 처리하는 핵심 클래스이다.
- **`DataManager`**: 모든 파일 I/O(읽기/쓰기)를 담당하는 정적 클래스. JSON 데이터를 C# 객체로 변환하거나 그 반대로 변환하는 역할을 전담하여 데이터 관리 로직을 중앙화한다.
- **`SceneManager`**: 상태 패턴(State Pattern)과 유사하게 작동한다. `mCurrentScene`을 현재 상태로 보고, `ChangeScene`을 통해 상태를 전환하며, 게임 루프는 현재 상태의 `Display`를 계속 호출하는 구조이다.
- **`StringFormatting`**: 콘솔 UI의 가독성을 높이는 유틸리티이다. `GetPrintableLength`로 한글을 2칸으로 계산하여 시각적 길이를 맞추고, `PadRightForMixedText`로 이를 이용해 텍스트를 정렬함으로써 표 형태의 UI를 구현한다.

[BackToTop](#toc)

## 6. 개선 및 확장 방향

- **저장/로드 슬롯 시스템**: 현재는 단일 `UserData.json` 파일만 사용한다. `SaveData.cs`의 `UserDataLoader`를 확장하여 여러 개의 `UserData`를 리스트 형태로 관리하고, 사용자에게 슬롯을 선택할 수 있는 UI를 제공하여 여러 게임을 저장하고 불러올 수 있도록 개선할 수 있을 것 같다.
- **전투 시스템 구체화**: `MonsterController`를 구현하고, 던전 진입 시 플레이어와 몬스터가 턴을 주고받으며 싸우는 상세한 전투 로직을 `DungeonScene` 또는 별도의 `BattleScene`에서 구현할 수 있을 것 같다.
- **스킬 및 아이템 다양화**: `PlayerController`의 `UseSkill()`, `UseItem()` 메서드를 구체적으로 구현하고, `ItemData.json`에 포션과 같은 소모성 아이템이나 다양한 효과를 가진 장비들을 추가하여 게임 플레이를 풍부하게 만들 수 있다.
- **코드 리팩토링**:
    - `StoreScene`과 `InventoryScene`의 `PrintItemList`, `PrintInventory` 메서드는 유사한 기능이므로 하나의 유틸리티 메서드로 통합하여 코드 중복을 줄일 수 있을 것 같다.
    - 각 씬의 `ProcessInput` 메서드 내 `Thread.Sleep()` 호출이 많아 게임 흐름이 지연될 수 있다. 사용자 경험을 고려하여 꼭 필요한 경우에만 사용해야 할 것 같다.

[BackToTop](#toc)

