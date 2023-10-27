rem BusinessLogic_Templateの「.editorconfig」ファイルを各プロジェクトへ配置するバッチ
rem このバッチファイル自体はシステムに必要ないので、リリースは不要です。

@echo off
set rule_file=%~dp0\BusinessLogic_Template\.editorconfig
echo %rule_file%
for /d %%a in (BusinessLogic_*) do (
  echo %%a
  copy /Y %rule_file% "%%a/.editorconfig"
)
pause