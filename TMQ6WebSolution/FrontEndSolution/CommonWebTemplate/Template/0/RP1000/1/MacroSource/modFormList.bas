Attribute VB_Name = "modFormList"
'***************************************************************************************************
'   選択リスト画面  ※呼び出しプロシージャ    modFormList(Module)
'
'***************************************************************************************************

Private Const g_cnsCaption = "選択リスト"                         ' デフォルトのCaption

' フォーム位置制御関連
Private Const LOGPIXELSX As Long = 88
Private Const LOGPIXELSY As Long = 90
Private Const SM_CYSCREEN As Long = 1
Private Const SM_XVIRTUALSCREEN As Long = 76
Private Const SM_YVIRTUALSCREEN As Long = 77
Private Const SM_CXVIRTUALSCREEN As Long = 78
Private Const SM_CYVIRTUALSCREEN As Long = 79
Private Const SPI_GETWORKAREA As Long = 48
'---------------------------------------------------------------------------------------------------
' GetWindowRect用ユーザー定義
Private Type g_typRect
    Left As Long
    Top As Long
    Right As Long
    Bottom As Long
End Type

#If VBA7 Then
' ■GetDC(API)
Private Declare PtrSafe Function GetDC Lib "user32.dll" (ByVal hwnd As LongPtr) As LongPtr
' ■ReleaseDC(API)
Private Declare PtrSafe Function ReleaseDC Lib "user32.dll" _
    (ByVal hwnd As LongPtr, ByVal hdc As LongPtr) As Long
' ■GetDeviceCaps(API)
Private Declare PtrSafe Function GetDeviceCaps Lib "gdi32.dll" _
    (ByVal hdc As LongPtr, ByVal nIndex As Long) As Long
' ■GetSystemMetrics(API)
Private Declare PtrSafe Function GetSystemMetrics Lib "user32.dll" (ByVal nIndex As Long) As Long
' ■SystemParametersInfo(API)
Private Declare PtrSafe Function SystemParametersInfo Lib "user32.dll" _
    Alias "SystemParametersInfoA" ( _
    ByVal uAction As Long, _
    ByVal uParam As Long, _
    ByRef lpvParam As g_typRect, _
    ByVal fuWinIni As Long) As Long
#Else
' ■GetDC(API)
Private Declare Function GetDC Lib "user32.dll" (ByVal hwnd As Long) As Long
' ■ReleaseDC(API)
Private Declare Function ReleaseDC Lib "user32.dll" (ByVal hwnd As Long, ByVal hdc As Long) As Long
' ■GetDeviceCaps(API)
Private Declare Function GetDeviceCaps Lib "gdi32.dll" (ByVal hdc As Long, ByVal nIndex As Long) As Long
' ■GetSystemMetrics(API)
Private Declare Function GetSystemMetrics Lib "user32.dll" (ByVal nIndex As Long) As Long
' ■SystemParametersInfo(API)
Private Declare Function SystemParametersInfo Lib "user32.dll" _
    Alias "SystemParametersInfoA" ( _
    ByVal uAction As Long, _
    ByVal uParam As Long, _
    ByRef lpvParam As g_typRect, _
    ByVal fuWinIni As Long) As Long
#End If

'* 処理名　：ShowFormListRange2
'* 機能　　：セル(Range)から表示させる
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = セル(Object) ※単一セル又は結合した日付用セル
'* 　　　　　Arg2 = フォームのCaption(String)
'---------------------------------------------------------------------------------------------------
'* 機能説明：当該セルの下にフォームが表示される
'* 注意事項：
'***************************************************************************************************
'Public Sub ShowFormListRange2(ByRef objRange As Range, _
'                                  ByRef objRange_ID As Range, _
'                                  Optional ByVal strCaption As String = g_cnsCaption)
'Public Sub ShowFormListRange2(ByRef objRange As Range, _
'                                  Optional ByVal strCaption As String = g_cnsCaption)
Public Sub ShowFormListRange2(ByRef objRange As Range, _
                                  ByVal longIdCol As Long, _
                                  ByVal strName As String, _
                                  Optional ByVal strCaption As String = g_cnsCaption)

    '-----------------------------------------------------------------------------------------------
    Dim lngLeft As Long                                             ' 横位置
    Dim lngTop As Long                                              ' 縦位置
    ' 非結合のセル範囲を選択している時は処理しない
    If objRange.Count > 1 Then
        ' 単一結合セルはOK とする
        If objRange.Address <> objRange.Cells(1).MergeArea.Address Then Exit Sub
    End If
    '-----------------------------------------------------------------------------------------------
    ' ユーザーフォーム表示位置取得
''    Call FP_GetFormPosition(objRange, UF_FormList.Width, UF_FormList.Height, lngLeft, lngTop)
'    Call modCalendar5.FP_GetFormPosition(objRange, UF_FormList.Width, UF_FormList.Height, lngLeft, lngTop)
    '-----------------------------------------------------------------------------------------------
    ' 選択フォーム
    With UF_FormList
        .prpTitle = strCaption & "：" & strName             ' タイトル + ：+項目名
        .prpEntMode = 0                                     ' 入力モード(0=セル、1=TextBox)
        Set .prpRange = objRange                            ' 対象セル
        .prpIdCol = longIdCol                               ' 対象セルID列
        '.prpStrName = strName                               ' 対象名称
        ' フォーム表示位置の確認
        If ((lngLeft <> 0) Or (lngTop <> 0)) Then
            ' 指定がある場合はマニュアル指定
            .StartUpPosition = 0
            .Left = lngLeft
            .Top = lngTop
        Else
            ' 指定がない場合はスクリーンの中央
            .StartUpPosition = 2
        End If
        ' フォームを表示
        .Show
    End With
End Sub



