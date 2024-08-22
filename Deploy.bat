@echo off
SET enviroments=dev Dev prod Prod uat UAT

IF "%1%"=="" GOTO=Error

for %%A in (%enviroments%) do (   
   IF "%1"=="%%A" (
      GOTO Deploy
   )
)

GOTO Error

:Deploy
c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild Deploy.xml /p:Configuration=%1%;AutoParameterizationWebConfigConnectionStrings=false
GOTO End

:Error
echo.
ECHO Debe indicar el entorno: [Dev] [Prod] [UAT]
ECHO.

:End