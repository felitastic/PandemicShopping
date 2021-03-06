﻿public enum eItemLocation { shelf, cart, ground }
public enum eShelfFaceDirection { down, right, up, left }
public enum eGameState { loading, running, paused, cutscene}
public enum eModeBool { apocalypse, fixedAmount, randomSpawn}

public enum eItemType 
{
    Toiletpaper,
    Soup,
    CannedFruit
}

public enum eUI_Input
{
    pause = 0,
    toTitle = 1,
    exit = 2,
    gameMode = 3,
}

public enum eUIMenu
{
    pause = 0,
    main = 1,
    receipt = 2,
    gamemode = 3
}

public enum eCutscene
{
    intro,
    checkout,
    timeout
}

