@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

SET DIR=%~dp0

CALL Tools\CodeGenerator\CodeGenerator.exe -i "!DIR:~0,-1!" -o "!DIR!Assets\Generated"

PAUSE