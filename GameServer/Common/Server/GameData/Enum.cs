
public enum GLOBAL_VALUE_TYPE
{
    NONE                             = 0,   // 없음

    DATA_VERSION                     = 1,   // 데이터 버전
    LANGUAGE                         = 2,   // 언어
    NEWBIE_SUPPORT                   = 9,   // 신규 유저 지원
    CHAT_BANNED_FORBIDDEN_WORD_LIMIT = 20,  // 금지어 횟수 제한으로 채팅 금지
    BETA_SERVICE_REWARD_MAIL         = 22,  // 베타 서비스 감사 보상 메일
    TIME_REWARD_INTERVAL             = 33,  // 시간 보상 간격
    MAX_SCORE                        = 39,  // 최대 점수
    PVP_LIMIT_TIME                   = 41,  // 대전 제한 시간(분)
    GAME_COUNTDOWN                   = 42,  // 게임 카운트 다운

    MAX                              = 43,  // 최대
}

public enum REWARD_TYPE
{
    NONE                                           = 0,     // 없음

    MOBILE_DATA                                    = 1,     // 모바일 데이터
    DIAMOND                                        = 2,     // 다이아
    ITEM_GAME_TICKET                               = 3,     // 게임 티켓
    ITEM_GAME                                      = 4,     // 게임 아이템
    SHOP_MOBILE_DATA_4000                          = 5,     // 상점 다이아 4000개
    SHOP_MOBILE_DATA_13000                         = 6,     // 상점 다이아 13000개
    SHOP_MOBILE_DATA_45000                         = 7,     // 상점 다이아 45000개
    SHOP_DIA_120                                   = 8,     // 상점 다이아 120개
    SHOP_DIA_750                                   = 9,     // 상점 다이아 750개
    SHOP_DIA_1800                                  = 10,    // 상점 다이아 1800개
    SHOP_AD_MOBILE_DATA                            = 11,    // 광고 모바일 데이터
    SHOP_AD_DIA                                    = 12,    // 광고 다이아
    SHOP_AD_GAME_TICKET                            = 13,    // 광고 게임 티켓
    SHOP_AD_GAME_ITEM                              = 14,    // 광고 게임 아이템
    SHOP_ITEM_GAME_TICKET                          = 15,    // 게임 티켓
    SHOP_ITEM_WATERMELON_PUNCH                     = 16,    // 수박 아이템 펀치
    SHOP_ITEM_WATERMELON_CHANGE                    = 17,    // 수박 아이템 변경
    SHOP_ITEM_WATERMELON_STONE                     = 18,    // 수박 아이템 돌
    SHOP_ITEM_WATERMELON_ICE                       = 19,    // 수박 아이템 얼음
    SHOP_ITEM_WATERMELON_SPEED                     = 20,    // 수박 아이템 속도 증가
    SCENARIO_DIA_TO_MOBILE_DATA                    = 21,    // 다이아로 데이터 교환 보상
    SCENARIO_MOBILE_DATA                           = 22,    // 시나리오 모바일 데이터
    SCENARIO_DIA                                   = 23,    // 시나리오 다이아
    FIRST_SUPPORT                                  = 24,    // 최초 지원
    MISSION_LOGIN_DAILY                            = 100,   // 매일 로그인하고 보상 받기
    MISSION_LOGIN_STREAK_3_TIMES                   = 101,   // 연속 로그인으로 보너스 획득
    MISSION_LOGIN_STREAK_7_TIMES                   = 102,   // 연속 로그인으로 보너스 획득
    MISSION_LOGIN_STREAK_30_TIMES                  = 103,   // 연속 로그인으로 보너스 획득
    MISSION_LOGIN_AT_12                            = 104,   // 정각 접속! 낮 12시 들어오기
    MISSION_LOGIN_AT_17                            = 105,   // 정각 접속! 저녁 5시 들어오기
    MISSION_LOGIN_AT_21                            = 106,   // 정각 접속! 밤 9시 들어오기
    MISSION_GAME_PLAY                              = 200,   // 게임을 즐기고 보상 얻기
    MISSION_GAME_PLAY_30_TIMES                     = 201,   // 게임 10번 즐기고 보상 받기
    MISSION_GAME_PLAY_SINGLE                       = 202,   // 혼자 게임하며 보상 얻기
    MISSION_GAME_PLAY_SINGLE_30_TIMES              = 203,   // 혼자 게임 10회 달성하고 보너스
    MISSION_GAME_PLAY_MULTI                        = 204,   // 친구와 게임하고 보상 얻기
    MISSION_GAME_PLAY_MULTI_30_TIMES               = 205,   // 친구와 게임 10회 즐기고 보너스
    MISSION_CHAT_USER                              = 300,   // 유저와 채팅하며 포인트 얻기
    MISSION_CHAT_AI                                = 301,   // AI 친구와 대화하며 보상 받기
    MISSION_RANK                                   = 400,   // 랭킹에 올라 보너스 획득
    MISSION_RANK_TOP_10                            = 401,   // 랭킹 Top 10 달성하고 특별 보상
    MISSION_PHOTO_UNLOCK                           = 500,   // 사진첩
    MISSION_PHOTO_UNLOCK_50_TIMES                  = 501,   // 사진첩 50
    MISSION_SCENARIO_PLAY                          = 600,   // 시나리오 경험하기
    MISSION_TING_CREATE                            = 700,   // 팅 만들기
    MISSION_DM_SEND                                = 800,   // DM 보내기
    MISSION_STORE_REVIEW                           = 1000,  // 리뷰 작성 해주기
    MISSION_CHANGE_NICKNAME                        = 1001,  // 닉네임 변경하기
    MISSION_SHOP_BUY_ONE_PLUS_ONE_DIA_120          = 1002,  // 1+1 상점 구매 이벤트 
    MISSION_SHOP_BUY_ONE_PLUS_ONE_MOBILE_DATA_4000 = 1003,  // 1+1 상점 구매 이벤트 

    MAX                                            = 1004,  // 최대
}

public enum MISSION_TYPE
{
    NONE                  = 0,     // 없음

    LOGIN_DAILY           = 100,   // 매일 출석하고 보상 받기
    LOGIN_STREAK_TIMES    = 101,   // 연속 로그인으로 보너스 획득
    LOGIN_AT_TIME         = 102,   // n시 땡! 하고 접속하기
    GAME_PLAY             = 200,   // 게임을 즐기고 보상 얻기
    GAME_PLAY_SINGLE      = 201,   // 혼자 게임하며 보상 얻기
    GAME_PLAY_MULTI       = 202,   // 친구와 게임하고 보상 얻기
    CHAT_USER             = 300,   // 유저와 채팅하며 포인트 얻기
    CHAT_AI               = 301,   // AI 친구와 대화하며 보상 받기
    RANK                  = 400,   // 랭킹에 올라 보너스 획득
    RANK_TOP_N            = 401,   // 랭킹 Top N 달성하고 특별 보상
    PHOTO_UNLOCK          = 500,   // 사진첩에 사진 잠금 해제하기
    SCENARIO_PLAY         = 600,   // 시나리오 플레이
    TING_CREATE           = 700,   // 팅 만들기
    DM_SEND               = 800,   // DM 보내기
    STORE_REVIEW          = 1000,  // 리뷰 작성 해주기
    CHANGE_NICKNAME       = 1001,  // 닉네임 변경하기
    SHOP_BUY_ONE_PLUS_ONE = 1002,  // 1+1 상점 구매 이벤트 

    MAX                   = 1003,  // 최대
}

public enum MISSION_RESET_TYPE
{
    NONE   = 0,  // 없음

    DAILY  = 1,  // 매일
    WEEKLY = 2,  // 주간
    ONCE   = 3,  // 업적

    MAX    = 4,  // 최대
}

public enum NAVI_TYPE
{
    NONE          = 0,  // 없음

    PANEL_SHOP    = 1,  // 상점 탭
    GAME_BUTTON   = 2,  // 게임 버튼
    STORE_REVIEW  = 3,  // 스토어 리뷰
    POPUP_SETTING = 4,  // 팝업 세팅

    MAX           = 5,  // 최대
}

public enum OPEN_STAUS_TYPE
{
    NONE      = 0,  // 없음

    OPEN      = 1,  // 오픈
    CLOSE     = 2,  // 클로즈
    OPEN_SOON = 3,  // 오픈 예정

    MAX       = 4,  // 최대
}

public enum SKILL_TYPE
{
    NONE              = 0,  // 없음

    WATERMELON_PUNCH  = 1,  // 펀치
    WATERMELON_RANDOM = 2,  // 랜덤
    WATERMELON_STONE  = 3,  // 돌맹이
    WATERMELON_ICE    = 4,  // 탕후루
    WATERMELON_SPEED  = 5,  // 찌릿슈즈

    MAX               = 6,  // 최대
}

public enum ORIENTATION
{
    NONE      = 0,  // 없음

    PORTRAIT  = 1,  // 세로
    LANDSCAPE = 2,  // 가로

    MAX       = 3,  // 최대
}

public enum PUSH_MESSAGE
{
    NONE             = 0,  // 없음

    GIFT_MOBILE_DATA = 1,  // 모바일 데이터 선물
    FRIEND_REQUEST   = 2,  // 친구 초대 푸시 메시지
    FRIEND_ACCEPT    = 3,  // 친구 수락 푸시 메시지
    SEND_DM          = 4,  // DM 메시지

    MAX              = 5,  // 최대
}

public enum CURRENCY_TYPE
{
    NONE        = 0,  // 없음

    CASH        = 1,  // 현금
    DIA         = 2,  // 다이아
    MOBILE_DATA = 3,  // 모바일 데이터
    MILEAGE     = 4,  // 마일리지

    MAX         = 5,  // 최대
}

public enum TIME_EVENT_TYPE
{
    NONE             = 0,  // 없음

    SQUARE_CHAT_FREE = 1,  // 광장 채팅 무료
    SHOP_DISCOUNT    = 2,  // 상점 할인

    MAX              = 3,  // 최대
}

public enum SHOP_CATEGORY_TYPE
{
    NONE   = 0,  // 없음

    PAKAGE = 1,  // 패키지
    GAME   = 2,  // 게임 아이템

    MAX    = 3,  // 최대
}

public enum SHOP_TYPE
{
    NONE            = 0,  // 없음

    MOBILE_DATA     = 1,  // 데이터
    DIA             = 2,  // 다이아
    GOODS           = 3,  // 굿즈
    CHARACTER       = 4,  // 캐릭터
    AD              = 5,  // 광고
    GAME_TICKET     = 6,  // 게임 티켓
    GAME_WATERMELON = 7,  // 수박 아이템
    GAME_3MATCH     = 8,  // 3매치 아이템

    MAX             = 9,  // 최대
}

public enum AD_TYPE
{
    NONE                  = 0,  // 없음

    AD_REWARD_DIA         = 1,  // 다이아 지급
    AD_REWARD_MOBILE_DATA = 2,  // 포인트 지급
    AD_REWARD_GAME_TICKET = 3,  // 게임 티켓
    AD_REWARD_GAME_ITEM   = 4,  // 게임 아이템
    AD_BANNER             = 5,  // 배너

    MAX                   = 6,  // 최대
}

public enum EFFECT_TYPE
{
    NONE            = 0,  // 없음

    ADD_MOBILE_DATA = 1,  // 모바일 데이터
    ADD_DIAMOND     = 2,  // 다이아몬드
    ADD_ITEM        = 4,  // 아이템

    MAX             = 5,  // 최대
}

public enum CHAR_TYPE
{
    NONE = 0,  // 없음

    PC   = 1,  // PC
    NPC  = 2,  // NPC

    MAX  = 3,  // 최대
}

public enum LOGIN_TYPE
{
    NONE   = 0,  // 없음

    GUEST  = 1,  // 게스트
    GOOGLE = 2,  // 구글
    APPLE  = 3,  // 애플

    MAX    = 4,  // 최대
}

public enum LANGUAGE_TYPE
{
    NONE = 0,  // 없음

    KO   = 1,  // 한국어
    EN   = 2,  // 영어

    MAX  = 3,  // 최대
}

public enum PRODUCT_TYPE
{
    NONE           = 0,  // 없음

    CONSUMABLE     = 1,  // 소비
    NON_CONSUMABLE = 2,  // 비소비
    SUBSCRIPTION   = 3,  // 구독

    MAX            = 4,  // 최대
}

public enum AUDIO_TYPE
{
    NONE  = 0,  // 없음

    BGM   = 1,  // BGM
    SFX   = 2,  // SFX
    VOICE = 3,  // VOICE

    MAX   = 4,  // 최대
}
