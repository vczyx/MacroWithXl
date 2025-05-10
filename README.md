# XlMacro 3.3

> 이 프로그램을 사용하여 발생할 수 있는 모든 법적·재정적 책임 및 불이익은 전적으로 사용자에게 있으며, 이에 대해 제작자는 어떠한 책임도 지지 않습니다.

[최신 버젼 다운로드](https://github.com/vczyx/ipmcr/releases)

## 개요

**엑셀(\*.xlsx, \*.xls, \*.xlsm 등) 파일을 활용**하여 **매크로 명령을 실행** 시킬 수 있는 프로그램 입니다.
직관적인 UI/UX를 채택하여, 사용자 친화적인 프로그램이 될 수 있도록 제작하였습니다.

![image](https://github.com/user-attachments/assets/c76e0946-93d5-4439-be16-cea9cc1239cd)
![image](https://github.com/user-attachments/assets/06e87de9-2fcd-4560-9873-9a858692517e)

## 사용 방법

### 준비 단계

먼저 매크로를 실행시키기 위해서는 준비 작업이 필요합니다. 이 준비 작업에는 다음과 같은 프로그램이 필요합니다.

> Microsoft Office Excel

매크로를 실행시키기 위한 엑셀 파일(매크로 파일)을 **제작**하고 **수정**하는 것이 준비 작업입니다.

더욱 빠르고 쉽게 제작할 수 있는 템플릿을 제공하므로, 이를 기반하여 시트를 수정하면 됩니다. (`/Data/MacroOR.xlsx`)

#### 시트 구성

| 이름   | 역할                                                             | 비고                                                |
| ------ | ---------------------------------------------------------------- | --------------------------------------------------- |
| macro  | 실질적인 매크로 명령들을 작성하는 시트                           | 시트 이름 뒤에 index를 붙여 여러 매크로 시트를 작성 |
| view   | 프로그램을 실행할 때, 사용자를 위한 도움말을 삽입할 수 있는 시트 |                                                     |
| config | 매크로 파일의 설정을 작성하는 시트                               |                                                     |
| help   | 매크로 파일 작성자를 위한 도움말                                 |                                                     |

- macro
  이름 규칙 : `macro<index>` ※ `<index>`는 1부터 시작합니다.

  `macro.maxCount` 옵션에 맞게 시트 수를 생성/복제하여야 합니다.

  예시로는 다음과 같습니다.

  ![image](https://github.com/user-attachments/assets/7e759a28-9937-41d6-abab-2c575ce08729)
  ![image](https://github.com/user-attachments/assets/20c0b62f-24a8-41a5-8954-d06709c1acd6)

  매크로의 실행 순서는, 행 -> 열 입니다. (왼쪽에서 오른쪽으로, 위에서 아래로)

  - 행
    `C2:~2` 범위에 값을 삽입하면, 행이 추가됩니다. 필요한 행에 따라 가변적으로 설정하시기 바랍니다.

    ![image](https://github.com/user-attachments/assets/a2444988-b591-4f68-923a-116b7be12be8)

  - 열
    `C3:~~` 범위에는 **실질적으로 매크로가 실행되는 구간**입니다. 설정 해 놓은 행에 따라 값을 입력을 하시면 됩니다. (수식 이용 가능)

    이 범위에 입력한 만큼 매크로가 로딩됩니다.

    ![image](https://github.com/user-attachments/assets/c56fc658-ba77-481a-91f4-a018ee8086fc)

    각 셀에 입력하여 사용할 수 있는 명령들은 다음과 같습니다.

    명령 사용 방법 : `$<식별자>:<매개변수>` 또는 `<keys>`를 입력합니다. (식별자 확인이 안될 시 에는 기본값인 `$key:<셀값>`으로 실행됩니다.)

    | 식별자        | 매개변수       | 실행 시                                                                            |
    | ------------- | -------------- | ---------------------------------------------------------------------------------- |
    | click         | \<mouseevent\> | \<mouseevent\>에 해당하는 마우스 클릭 이벤트를 발생합니다.                         |
    | cursor        | \<x\>,\<y\>    | \<x\>, \<y\>에 해당하는 위치로 마우스 커서를 이동합니다.                           |
    | key           | \<keys\>       | \<keys\>에 해당하는 키들을 순차적으로 입력합니다. 이 명령은 SendKeys를 이용합니다. |
    | wait          | \<milisec\>    | \<milisec\>밀리초 동안 작업을 잠시 멈춥니다.                                       |
    | sh            | \<command\>    | 백그라운드에서 \<command\>에 해당하는 cmd 명령을 실행합니다.                       |
    | vsh           | \<command\>    | \<command\>에 해당하는 cmd 명령을 실행합니다.                                      |
    | util.mcrlist  |                | 현재 사용 가능한 명령 식별자들에 대한 설명을 출력합니다.                           |
    | util.log      | \<message\>    | \<message\>를 출력합니다.                                                          |
    | util.gcursor  |                | 현재 마우스 커서 위치를 출력합니다.                                                |
    | clipboard.set | \<text\>       | \<text\>를 클립보드에 저장합니다.                                                  |

    - `<mouseevent>` 매개변수 입력 값
    - |                     | LEFT     | MIDDLE     | RIGHT     |
      | ------------------- | -------- | ---------- | --------- |
      | **클릭(DOWN > UP)** | left     | middle     | right     |
      | **DOWN**            | leftdown | middledown | rightdown |
      | **UP**              | leftup   | middleup   | rightup   |
    - `keys` 매개변수 입력 값
    - 키보드에 있는 키에 해당하는 값을 입력하면 됩니다. 예를 들어 A글자를 입력하고 싶다면, `A` 입력

      하지만 시스템 제어키 혹은 조합키는 다음 규칙을 따라야 합니다.

      - 조합키
        | 키 | 코드 |
        |---|---|
        | Shift | + |
        | Ctrl | ^ |
        | Alt | % |
      - 제어키
        | 키 | 코드 |
        |---|---|
        | ←BackSpace | {BACKSPACE} or {BS} or {BKSP} |
        | Break | {BREAK} |
        | CapsLock | {CAPSLOCK} |
        | Delete | {DELETE} or {DEL} |
        | ↑ | {UP} |
        | ↓ | {DOWN} |
        | ← | {LEFT} |
        | → | {RIGHT} |
        | Esc | {ESC} |
        | Help | {HELP} |
        | Home | {HOME} |
        | Insert | {INSERT} or {INS} |
        | NumLock | {NUMLOCK} |
        | PageDown | {PGDN} |
        | PageUp | {PGUP} |
        | PrintScreen | {PRTSC} |
        | ScrollLock | {SCROLLLOCK} |
        | Tab | {TAB} |
        | F1~F16 | {F1} ~ {F16} |
        | KEYPAD + | {ADD} |
        | KEYPAD - | {SUBTRACT} |
        | KEYPAD \* | {MULTIPLY} |
        | KEYPAD / | {DIVIDE} |
        | End | {END} |
        | Enter | {ENTER} or ~ |

    ex1) 마우스 커서를 (200, 500)으로 이동시키고, 마우스 왼쪽 버튼 클릭을 실행

    ![image](https://github.com/user-attachments/assets/4a821fdf-211f-4386-a711-0466d1f422ed)

    ex2) "TEST"를 입력하고, Enter키 입력 (key 명령에 한해서 `TEST{Enter}`로 입력해도 정상적으로 작동됩니다.)

    ![image](https://github.com/user-attachments/assets/ee8c00cf-2046-4fd6-a29a-5234ce9b656b)

- view

  이 프로그램에서 Step2 페이지와 Step4 페이지에 표시될 항목들을 삽입하는 시트입니다.

  다양한 이미지, 텍스트 등을 사용하여, 사용자 친화적인 프로그램을 제작합시다.

  페이지에 표시할 항목들을 범위로 선택한 다음, 이름을 지정하고, `config` 시트에 `step2View.imageRange` 혹은 `step4View.imageRange`에 값으로 입력합니다.

- config

  매크로 설정 옵션들을 조정할 수 있는 시트입니다. 옵션에 대한 값들은 다음과 같습니다.

  ※ 해당 시트가 존재하지 않거나 오류가 나는 경우, 프로그램에서 파일을 인식할 수 없으니 주의하시기 바랍니다.

  | 옵션 이름              | 값                                                                   | 타입    |
  | ---------------------- | -------------------------------------------------------------------- | ------- |
  | macro.interval         | 매크로 명령 실행 간격 밀리초 설정                                    | int     |
  | macro.maxCount         | macro 시트 개수                                                      | int     |
  | step2view.width        | step2 화면에서 변경 될 가로 크기 (기본 창 크기보다 작을 경우 변경 X) | double  |
  | step2view.height       | step2 화면에서 변경 될 세로 크기 (기본 창 크기보다 작을 경우 변경 X) | double  |
  | step2view.descriptions | step2 화면에서 표시될 설명 문구 작성                                 | string  |
  | step2view.imageRange   | step2 화면에서 표시될 이미지 범위 설정 (view 시트)                   | string  |
  | step4view.width        | step4 화면에서 변경 될 가로 크기 (기본 창 크기보다 작을 경우 변경 X) | double  |
  | step4view.height       | step4 화면에서 변경 될 세로 크기 (기본 창 크기보다 작을 경우 변경 X) | double  |
  | step4view.descriptions | step4 화면에서 표시될 설명 문구 작성                                 | string  |
  | step4view.imageRange   | step4 화면에서 표시될 이미지 범위 설정 (view 시트)                   | string  |
  | closeAfterLoad         | 매크로 파일을 불러올 때, 수정 한 매크로 파일을 닫을 지 여부          | boolean |
  | forceLoad              | 매크로 파일을 바로 불러올지에 대한 여부                              | boolean |

### 사용 단계

준비 작업을 끝마쳤다면, 이제 매크로를 사용할 수 있습니다. 사용 방법은 다음 절차를 따릅니다.

#### Step 0. 프로그램 실행

`InputMacro3.exe` 파일을 실행합니다.

#### Step 1. 매크로 파일 선택

준비 작업에서 제작한 매크로 파일을 선택하고, 다음 버튼을 클릭합니다. (더블 클릭으로도 선택 가능)

![image](https://github.com/user-attachments/assets/615c2f33-9966-4beb-b46b-7ca49f8772e7)

#### Step 2. 매크로 파일 수정 / 저장

만약 `forceLoad` 옵션이 `false`일 경우 사용자는 직접 매크로 파일의 일부분을 수정하고 저장해서 매크로를 불러와야 합니다. 다음 화면이 켜지고 엑셀 편집 프로그램이 실행 됩니다.

![image](https://github.com/user-attachments/assets/6fcd82d2-9e9e-4e26-a319-79e8d4abe203)

다음과 같이 엑셀 편집 프로그램이 실행되었다면, 필요한 부분을 수정 후 저장합니다. (저장하지 않고 불러오고 싶은 경우, InputMacro 프로그램에서 `다음`버튼을 클릭합니다.)

※ 엑셀 편집 프로그램이 실행되지 않는다면, InputMacro 프로그램을 재시작 하십시오.
※ 엑셀 편집 프로그램이 설치되어 있지 않으면, 매크로를 불러올 수 없습니다.

![image](https://github.com/user-attachments/assets/e9b538c7-82a3-4b7b-bfdf-b129bed764e7)

#### Step 3. 매크로 불러오기

이 페이지에서 사용자는 그저 매크로가 불러오는 작업이 완료되기를 기다리면 됩니다.

![image](https://github.com/user-attachments/assets/050fcf73-01ed-4afd-a410-37211e2ccd9d)

#### Step 4. 매크로 사용

이제 모든 준비가 완료되었습니다! `F9` 키를 눌러 사전에 등록한 매크로 명령들을 순차적으로 실행시킬 수 있습니다.

`처음으로` 버튼을 클릭하여, 매크로 선택 화면으로 돌아갑니다.

![image](https://github.com/user-attachments/assets/17f0cbbc-fd69-4fc6-8a46-3dff8faf0407)

#### 그 외

정상적으로 모든 매크로 명령이 불러와졌는지 확인이 필요한 경우, Step 4 페이지에서 `매크로` 글자를 클릭하면 다음과 같은 페이지로 이동하여, 불러온 매크로를 한 눈에 볼 수 있습니다.

`이전` 버튼을 클릭하면, Step 4 페이지로 돌아갑니다.

![image](https://github.com/user-attachments/assets/97b8c011-d781-4797-9b01-175fb821928d)

## Requires

- [.NET Framework v4.8](https://dotnet.microsoft.com/ko-kr/download/dotnet-framework/net48)
- [Microsoft Office Excel](https://www.microsoft.com/ko-kr/microsoft-365/get-office-and-microsoft-365-oem-download-page)

## License

- [MIT License](https://github.com/vczyx/ipmcr/blob/master/LICENSE)

## 사용 IDE 및 언어

- Jetbrains Rider
- C# 7.3 WPF
