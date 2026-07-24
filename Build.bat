@echo off

:: UTF-8 인코딩 설정
chcp 65001 >nul

:: Visual Studio 2022 환경 변수 설정
call "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"

:: 솔루션 경로 설정
set SOLUTION_PATH=Server.sln

:: Rebuild 실행
:: 에러만 출력
echo Rebuild 중... 솔루션: %SOLUTION_PATH%
msbuild "%SOLUTION_PATH%" /t:Rebuild /p:Configuration=Release /m /nologo /consoleloggerparameters:ErrorsOnly

if %ERRORLEVEL% neq 0 (
    echo Rebuild에 실패했습니다.
    pause
    exit /b %ERRORLEVEL%
)

echo Rebuild가 성공적으로 완료되었습니다.
pause
exit /b 0
