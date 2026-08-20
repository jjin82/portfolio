# C# Game Server Portfolio

.NET 8 / C# 기반으로 구현한 **게임 서버 포트폴리오**입니다.

클라이언트의 게임 서버 진입을 관리하는 `FrontServer`, 실제 게임 플레이와 유저 콘텐츠를 처리하는 `GameServer`, 공용 네트워크/DB 모듈, 버전 관리 도구로 구성되어 있습니다.

서버 간 역할을 분리하고, TCP 기반 패킷 통신과 게임 서버 분산 진입, 유저/게임 세션 관리, MySQL 데이터 처리, 인증 및 외부 서비스 연동 등 실제 라이브 게임 서버에서 필요한 기능을 모듈 단위로 구성했습니다.

> 본 저장소는 포트폴리오 공개를 위해 실제 프로젝트의 서버 코드 일부를 정리한 저장소입니다.  
> 클라이언트 프로젝트, 일부 외부 라이브러리 및 서비스 모듈은 포함되어 있지 않아 저장소 단독으로 전체 프로젝트를 빌드하는 용도는 아닙니다.

\---

## 주요 기술

* C#
* .NET 8
* TCP/IP Socket Programming
* Multi-Threading
* Custom Network Framework (`HNET`)
* MySQL
* MySql.Data
* Windows Forms
* Newtonsoft.Json / System.Text.Json
* BouncyCastle
* Client ↔ FrontServer ↔ GameServer 구조
* Game Server Registry / Load Distribution
* User / Game Session Management
* 외부 인증 및 Web API 연동

\---

# Server Architecture

```text
                         ┌─────────────────┐
                         │     Client      │
                         └────────┬────────┘
                                  │
                         C2F : Server Query
                                  │
                                  ▼
                       ┌─────────────────────┐
                       │     FrontServer     │
                       │                     │
                       │ - GameServer 관리   │
                       │ - User Count 관리   │
                       │ - Server 선택       │
                       └──────────┬──────────┘
                                  │
                    GameServer Host / Port 반환
                                  │
                                  ▼
                         ┌─────────────────┐
                         │     Client      │
                         └────────┬────────┘
                                  │
                         C2G : Game Protocol
                                  │
                                  ▼
                 ┌───────────────────────────────┐
                 │          GameServer           │
                 │                               │
                 │ - Login / User               │
                 │ - Character / Item           │
                 │ - Currency / Shop            │
                 │ - Mission / Mail             │
                 │ - Ranking                    │
                 │ - Game Session               │
                 │ - IAP / Ads / Report         │
                 └─────────┬──────────┬──────────┘
                           │          │
                         MySQL      Web API
                           │          │
                           ▼          ▼
                    ┌──────────┐  ┌──────────────┐
                    │ Database │  │ Auth / IAP   │
                    │          │  │ External API │
                    └──────────┘  └──────────────┘


                  GameServer ── G2F ──► FrontServer
                       │
                       ├─ Server Info
                       ├─ User Count
                       └─ Server Status
```

클라이언트가 처음부터 특정 GameServer에 고정되는 구조가 아니라,  
FrontServer에서 현재 운영 중인 GameServer 정보를 관리하고 적절한 서버를 선택하여 접속 정보를 전달하는 구조입니다.

\---

# 프로젝트 구성

## FrontServer

게임 서버 진입과 GameServer 상태 관리를 담당합니다.

주요 역할:

* Client 연결 수락
* GameServer 연결 수락
* GameServer 등록 및 상태 관리
* GameServer별 접속자 수 관리
* Client에게 적절한 GameServer 주소 전달
* GameServer 연결/해제 상태 관리

```text
Client
   │
   │ RQ\_GAME\_SERVER\_ADDRESS\_GET
   ▼
FrontServer
   │
   │ ServerManager
   ▼
Connected Game Servers
   │
   │ 접속자 수 비교
   ▼
Least Loaded GameServer
   │
   │ Host / Port
   ▼
Client
```

\---

## GameServer

실제 유저 세션과 게임 콘텐츠를 처리하는 메인 게임 서버입니다.

```text
GameServer
│
├─ Network
│  ├─ ClientAcceptor
│  └─ FrontConnector
│
├─ Domain
│  ├─ User
│  ├─ Game
│  ├─ Ranking
│  ├─ Shop
│  ├─ Notice
│  ├─ Push
│  └─ TimeEvent
│
├─ DB
│  ├─ Account
│  ├─ Character
│  ├─ Currency
│  ├─ Item
│  ├─ Mail
│  ├─ Mission
│  ├─ Ranking
│  ├─ IAP
│  └─ User
│
└─ Web
   └─ Service
      ├─ Auth
      ├─ IAP
      ├─ Report
      └─ External Service
```

\---

## VersionHub

버전 관련 정보를 관리하기 위한 별도 도구 프로젝트입니다.

```text
VersionHub
│
├─ Network
├─ VersionManager
└─ Windows Forms UI
```

게임 서버와 별개로 버전 정보를 관리할 수 있도록 독립 프로젝트로 분리되어 있습니다.

\---

## \_Common

서버들이 공통으로 사용하는 기능을 분리한 영역입니다.

```text
\_Common
│
├─ HNetwork
│  ├─ HNet
│  ├─ HNetAcceptor
│  ├─ HNetConnector
│  ├─ HNetSession
│  ├─ HNetIo
│  ├─ HNetPacket
│  └─ HNetFunctor
│
├─ Packet
├─ MySql
├─ ServerConfig
├─ IniFile
├─ Logger
└─ Utility
```

네트워크, 패킷, DB, 설정 등 여러 서버에서 공통적으로 필요한 기능을 하나의 공용 계층으로 구성했습니다.

\---

# 핵심 구현

## 1\. FrontServer 기반 GameServer 분산 진입

Client는 FrontServer에 접속하여 실제 플레이에 사용할 GameServer 주소를 요청합니다.

```csharp
void RQ\_GAME\_SERVER\_ADDRESS\_GET(
    int netId,
    C2F.RQ\_GAME\_SERVER\_ADDRESS\_GET packet)
{
    ServerInfo? gameServerInfo =
        ServerManager.Instance
            .GetConnectGameServerInfo();

    if (null == gameServerInfo)
    {
        Send(
            netId,
            new C2F.RS\_RESULT\_CODE(
                RESULT\_CODE.FAIL\_SERVER\_MAINTENANCE));

        return;
    }

    Network.Send(
        netId,
        new C2F.RS\_GAME\_SERVER\_ADDRESS\_GET
        {
            host = gameServerInfo.\_host,
            port = gameServerInfo.\_port,
        });
}
```

FrontServer의 `ServerManager`는 현재 연결된 GameServer의 상태와 접속자 수를 관리합니다.

```text
GameServer #1 : 1,250 users
GameServer #2 :   780 users
GameServer #3 : 1,050 users

          │
          ▼

FrontServer
    │
    └─ 가장 접속자가 적은 서버 선택
          │
          ▼

GameServer #2
```

이를 통해 Client가 특정 서버 주소를 직접 알고 있을 필요 없이 FrontServer를 통해 현재 운영 상태에 맞는 GameServer로 진입할 수 있도록 구성했습니다.

\---

## 2\. GameServer Registry

GameServer는 FrontServer와 연결되면 자신의 서버 정보를 전달합니다.

```text
GameServer
    │
    │ Connected / Reconnected
    ▼
FrontServer
    │
    └─ GameServer Information 등록
```

GameServer 측 `FrontConnector`는 FrontServer 연결 또는 재연결 시 서버 정보를 전송합니다.

FrontServer는 다음 정보를 기반으로 GameServer 목록을 관리합니다.

* Server ID
* Host / Port
* Connection State
* Current User Count
* Server Information

서버 목록은 `ConcurrentDictionary` 기반으로 관리하여 여러 네트워크 이벤트에서 서버 상태를 조회하고 변경할 수 있도록 구성했습니다.

\---

## 3\. TCP Packet Dispatch

GameServer의 `ClientAcceptor`에서 Client 연결과 Packet 처리를 담당합니다.

```text
Client Socket
     │
     ▼
ClientAcceptor
     │
     ▼
Packet Type
     │
     ▼
Registered Message Handler
     │
     ▼
User / Game / Service Logic
```

서버 시작 시 처리할 Packet Handler를 등록합니다.

```csharp
public override void OnRegMessage()
{
    RegMessage<C2G.RQ\_KEEP\_ALIVE>(RQ\_KEEP\_ALIVE);

    RegMessage<C2G.RQ\_LOGIN>(RQ\_LOGIN);
    RegMessage<C2G.RQ\_LOGIN\_OK>(RQ\_LOGIN\_OK);

    RegMessage<C2G.RQ\_USER\_PROFILE>(
        RQ\_USER\_PROFILE);

    RegMessage<C2G.RQ\_SHOP\_IAP>(
        RQ\_SHOP\_IAP);

    RegMessage<C2G.RQ\_GAME\_CREATE>(
        RQ\_GAME\_CREATE);

    RegMessage<C2G.RQ\_GAME\_JOIN>(
        RQ\_GAME\_JOIN);

    RegMessage<C2G.RQ\_GAME\_LEAVE>(
        RQ\_GAME\_LEAVE);

    RegMessage<C2G.RQ\_GAME\_SYNC>(
        RQ\_GAME\_SYNC);

    RegMessage<C2G.RQ\_RANKING>(
        RQ\_RANKING);

    RegMessage<C2G.RQ\_MISSION\_REWARD>(
        RQ\_MISSION\_REWARD);
}
```

네트워크 I/O 계층과 실제 콘텐츠 로직을 분리하고 Packet Type별 Handler를 등록하는 형태로 구성했습니다.

\---

## 4\. Client / Server Network 역할 분리

GameServer의 Network 계층은 크게 두 방향의 통신을 담당합니다.

```text
               ┌──────────────┐
               │  FrontServer │
               └──────▲───────┘
                      │
               FrontConnector
                      │
                      │ G2F
                      │
              ┌───────┴────────┐
              │   GameServer   │
              └───────▲────────┘
                      │
                ClientAcceptor
                      │
                      │ C2G
                      │
               ┌──────┴───────┐
               │    Client    │
               └──────────────┘
```

* `ClientAcceptor`

  * Client → GameServer 연결
  * C2G Packet 수신 및 처리
  * Client Disconnect 처리
* `FrontConnector`

  * GameServer → FrontServer 연결
  * GameServer 정보 등록
  * FrontServer와의 서버 간 Message 처리
  * 연결 및 재연결 처리

Client 통신과 Server-to-Server 통신의 역할을 분리했습니다.

\---

# Game Domain

## 5\. Game Session Lifecycle

게임 세션은 `Game` 객체를 중심으로 관리합니다.

```text
Create
  │
  ▼
OnCreate
  │
  ├─ Game State 초기화
  ├─ Player Join
  └─ GameManager 등록
  │
  ▼
Update
  │
  ├─ State Update
  ├─ Player 상태 확인
  └─ Game Over 판정
  │
  ▼
Destroy
  │
  ├─ Player Leave
  └─ GameManager 제거
```

Game 객체에서 게임 생성, Player 참가/이탈, 상태 갱신, 결과 처리, Game Over 판단과 Game 제거 등의 라이프사이클을 처리합니다.

게임 콘텐츠를 네트워크 연결 객체에 직접 구현하지 않고 독립된 Domain 객체에서 관리하도록 구성했습니다.

\---

## 6\. Concurrent Game Management

`GameManager`에서는 생성된 Game 객체들을 관리합니다.

```csharp
ConcurrentDictionary<uint, Game> \_games;
```

```text
GameManager
│
├─ Game Allocate
├─ Game Join
├─ Game Find
├─ Game Add
├─ Game Delete
└─ User Logout → Game Out
```

Game Session과 User Session을 분리하면서 User가 로그아웃하거나 연결이 종료될 때 Game 상태도 함께 정리할 수 있도록 구성했습니다.

\---

# User Domain

## 7\. User 기능의 Partial Class 분리

User 객체에 기능을 모두 집중시키지 않고 기능 단위의 Partial Class로 분리했습니다.

```text
User
│
├─ User.cs
├─ User.Character.cs
├─ User.Currency.cs
├─ User.Item.cs
├─ User.Mail.cs
├─ User.Mission.cs
├─ User.Reward.cs
├─ User.Shop.cs
├─ User.Game.cs
├─ User.Friend.cs
├─ User.DM.cs
├─ User.Ad.cs
└─ User.TimeReward.cs
```

하나의 User Entity를 유지하면서도 콘텐츠별 구현 파일을 분리하여 대형 User 클래스의 유지보수 복잡도를 낮추는 형태로 구성했습니다.

\---

# Game Contents

서버 코드에서 다음과 같은 게임 서비스 기능을 확인할 수 있습니다.

```text
Account / Login
Character
Currency
Item
Friend
Mail
Mission
Reward
Shop
IAP
Advertisement
Ranking
Direct Message
Push
Notice
Time Event
Game Session
Game Result
Report
```

Network Packet, Domain Logic, DB Access를 영역별로 분리하여 콘텐츠가 추가되더라도 관련 모듈 단위로 확장할 수 있도록 구성했습니다.

\---

# Database

## 8\. MySQL 공용 데이터 계층

공용 `MySql` 클래스에서 `MySql.Data` 기반 DB 접근 기능을 제공합니다.

```csharp
public enum DB\_KIND
{
    GF\_ACCOUNT,
    GF\_COMMON,
    GF\_GAME01,
    GF\_RANKING = 100,
}
```

DB 역할을 구분하여 Account / Common / Game / Ranking 데이터를 서로 다른 Connection 설정으로 사용할 수 있도록 구성했습니다.

기본 DB 처리 흐름은 다음과 같습니다.

```text
DB Connection
     │
     ▼
PREPARE(Query)
     │
     ▼
SET\_PARAM(...)
     │
     ▼
EXECUTE / EXECUTE\_UPDATE
     │
     ▼
FETCH
     │
     ▼
GET\_DATA<T>
```

Query Parameter를 별도로 전달할 수 있도록 구성하여 문자열 조립 대신 Parameter 기반 Query 처리를 지원합니다.

\---

## 9\. 기능별 DBManager 분리

DB 접근 코드는 하나의 거대한 클래스에 집중시키지 않고 Partial Class를 이용해 기능 단위로 분리되어 있습니다.

```text
DBManager
│
├─ DBManager.Account.cs
├─ DBManager.Ad.cs
├─ DBManager.Character.cs
├─ DBManager.Currency.cs
├─ DBManager.DM.cs
├─ DBManager.IAP.cs
├─ DBManager.Item.cs
├─ DBManager.Mail.cs
├─ DBManager.Mission.cs
├─ DBManager.PushMessge.cs
├─ DBManager.Ranking.cs
├─ DBManager.Report.cs
└─ DBManager.User.cs
```

Domain 영역과 DB 구현 영역을 분리하여 게임 로직과 데이터 영속성 코드를 독립적으로 관리할 수 있도록 구성했습니다.

\---

# External Service

## 10\. 인증 및 외부 Web Service 연동

`WebManager` 계층을 통해 게임 서버에서 필요한 외부 서비스를 분리하여 처리합니다.

```text
GameServer
    │
    ▼
WebManager
    │
    ├─ Auth
    ├─ IAP
    ├─ Report
    └─ External Service
```

인증 처리에서는 외부 인증 결과를 기반으로 PID와 Access Token 등의 유효성을 확인하고 User Login 흐름으로 연결합니다.

또한 중복 로그인 및 재로그인 상황을 고려한 User Session 처리 구조가 포함되어 있습니다.

\---

# 개발 환경

```text
Language
  C#

Framework
  .NET 8

Platform
  Windows x64

Network
  TCP/IP
  Custom HNET Framework

Database
  MySQL
  MySql.Data

Serialization / Data
  Newtonsoft.Json
  System.Text.Json

Security / Crypto
  BouncyCastle.Cryptography

Server Tool
  Windows Forms
```

\---

# 디렉터리 구조

```text
portfolio
│
├─ FrontServer
│  ├─ Network
│  │  ├─ ClientAcceptor.cs
│  │  ├─ GameAcceptor.cs
│  │  └─ Network.cs
│  │
│  ├─ ServerManager
│  │  └─ ServerManager.cs
│  │
│  └─ FrontServerForm.\*
│
├─ GameServer
│  ├─ Network
│  │  ├─ ClientAcceptor.cs
│  │  ├─ ClientAcceptor.Message.cs
│  │  ├─ ClientAcceptor.Functor.cs
│  │  ├─ FrontConnector.cs
│  │  └─ Network.cs
│  │
│  ├─ Domain
│  │  ├─ Game
│  │  ├─ User
│  │  ├─ Ranking
│  │  ├─ Shop
│  │  ├─ Notice
│  │  ├─ Push
│  │  └─ TimeEvent
│  │
│  ├─ DB
│  ├─ Web
│  │  └─ Service
│  └─ GameServerForm.\*
│
├─ VersionHub
│  ├─ Network
│  ├─ VersionManager.cs
│  └─ VersionHubForm.\*
│
├─ \_Common
│  ├─ HNetwork
│  ├─ Packet
│  ├─ MySql.cs
│  ├─ ServerConfig.cs
│  ├─ IniFile.cs
│  └─ Util.cs
│
├─ Dummy
├─ Server.sln
└─ Build.bat
```

\---

# 코드에서 확인할 수 있는 개발 경험

### Game Server Architecture

* FrontServer / GameServer 역할 분리
* Client 진입 서버와 실제 게임 서버 분리
* GameServer Registry
* GameServer 상태 관리
* 접속자 수 기반 서버 선택
* Server-to-Server 통신

### Network Programming

* C# TCP/IP 서버
* Acceptor / Connector 구조
* Client Session 관리
* Packet Type 기반 Message Dispatch
* Client ↔ GameServer Protocol
* GameServer ↔ FrontServer Protocol
* Disconnect / Reconnect 처리

### Game Server Contents

* Account / Login
* User Session
* Character
* Currency
* Item
* Friend
* Mail
* Mission
* Reward
* Shop
* IAP
* Advertisement
* Ranking
* Game Session
* Game Result
* Push / Notice / Time Event

### Database

* MySQL
* 다중 DB Connection 구성
* Parameter 기반 Query 처리
* 기능별 DB Layer 분리
* Account / Game / Ranking 데이터 영역 분리

### Server Operation

* Windows Forms 기반 Server Tool
* GameServer 상태 표시
* User Count 관리
* Server Connection 상태 관리
* Config 기반 서버 실행 환경
* Version 관리 도구 분리

\---

# Repository 범위

이 저장소는 실제 서비스 프로젝트 전체를 공개한 것이 아니라  
**게임 서버 설계 및 구현 경험을 보여주기 위한 포트폴리오 코드 발췌본**입니다.

일부 코드에서는 저장소 외부의 Client 및 Library 프로젝트를 참조하고 있기 때문에 현재 Repository만으로 전체 솔루션을 그대로 Build하는 것을 목적으로 하지 않습니다.

공개 범위에서 중점적으로 보여주고자 하는 부분은 다음과 같습니다.

```text
1. FrontServer / GameServer의 역할 분리

2. GameServer 등록 및 접속자 수 기반 서버 선택

3. TCP Packet 기반 Client / Server 통신

4. Game / User Session의 Domain 구조

5. 게임 콘텐츠와 DB 계층의 기능별 분리

6. MySQL 기반 데이터 처리

7. 인증 / IAP 등 외부 서비스 연동 구조
```

즉, 이 저장소는 단순 기능 구현 모음이 아니라

> \*\*C# 기반의 실제 서비스 게임 서버를 어떤 계층과 역할로 분리하고, 네트워크·게임 로직·DB·외부 서비스를 어떻게 구성했는지를 보여주는 포트폴리오입니다.\*\*

