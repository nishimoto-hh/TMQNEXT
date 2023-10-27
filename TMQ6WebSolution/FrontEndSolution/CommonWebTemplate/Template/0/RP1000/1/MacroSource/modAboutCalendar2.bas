Attribute VB_Name = "modAboutCalendar2"
'***************************************************************************************************
'   カレンダー関連関数(クラス呼び出し部分)                      modAboutCalendar2(Module)
'***************************************************************************************************
'   ※カレンダー関連関数の主要部分はクラス(clsAboutCalendar2)側に実装されている
'***************************************************************************************************
Option Explicit
Option Private Module
'===================================================================================================
'★↓↓↓2022/11/16 祝日パラメータシート名変更 by ATTS↓↓↓★
'Public Const g_cnsParaSheet As String = "祝日パラメータ"
Public Const g_cnsParaSheet As String = "Holiday_Parameter"
'★↑↑↑2022/11/16 祝日パラメータシート名変更 by ATTS↑↑↑★
'---------------------------------------------------------------------------------------------------
' 祝日含むカレンダーテーブル用ユーザー定義(公開)
Public Type g_typAboutCalendar2
    Hiduke As Date                                  ' 日付
    Yobi As Long                                    ' 曜日(0=日､1=月､2=火～6=土)
    Syusu As Long                                   ' 週数(1始まりで日曜日発見時に1加算)
    HmSyusu As Long                                 ' 週数(0始まりで月曜日発見時に1加算) ※祝日法(HM)対応
    Syuku As Long                                   ' 祝日判定(0=通常､1=祝日､2=振替休日､3=会社休日)
    SyukuNm As String                               ' 祝日名称
    FurikaeKbn As Long                              ' 振替区分(0=通常､1=振替休日を行なわない)
End Type
'---------------------------------------------------------------------------------------------------
' カレンダー関連関数クラス
Public g_clsAboutCalendar As clsAboutCalendar2      ' カレンダー関連関数クラス
Private g_blnInitAboutCalendar As Boolean           ' カレンダー関連関数クラス初期化判定
' [注]VBAではモジュール保持変数の｢保持｣は保証されない

'***************************************************************************************************
'   ■■■ 公開プロシージャ(Public) ■■■
'***************************************************************************************************
'* 処理名　：FP_InitAboutCalendar
'* 機能　　：カレンダー関連関数クラス初期化
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = チェック工程スキップ(Boolean)               ※Option
'* 　　　　　Arg2 = 強制再初期化スイッチ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：初期化済みの場合はスキップされる
'***************************************************************************************************
Public Function FP_InitAboutCalendar(Optional ByVal blnOmitCheck As Boolean = False, _
                                     Optional ByVal blnForcedInit As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' 強制再初期化判定
    If blnForcedInit Then
        g_blnInitAboutCalendar = False
    End If
    ' 未初期化なら初期化を行なう
    If Not g_blnInitAboutCalendar Then
        ' カレンダー関連関数クラスを初期化
        Set g_clsAboutCalendar = New clsAboutCalendar2
        ' 祝日パラメータテーブル作成
        g_blnInitAboutCalendar = g_clsAboutCalendar.MakeHoliParamater(blnOmitCheck)
    End If
    FP_InitAboutCalendar = g_blnInitAboutCalendar
End Function

'***************************************************************************************************
'* 処理名　：FP_GetCalendarTable1
'* 機能　　：カレンダーテーブル作成(当月1ヶ月用)
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル(Array:Structure)         ※Ref参照
'* 　　　　　Arg4 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Public Function FP_GetCalendarTable1(ByVal lngY As Long, _
                                     ByVal lngM As Long, _
                                     ByRef tblCalendar() As g_typAboutCalendar2, _
                                     Optional ByVal blnOmitCheck As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' カレンダー関連関数クラス初期化
    If Not FP_InitAboutCalendar(blnOmitCheck) Then
        FP_GetCalendarTable1 = False
        Exit Function
    End If
    ' カレンダーテーブル作成(当月分)
    Call g_clsAboutCalendar.GetCalendarTable1(lngY, lngM, tblCalendar)
    FP_GetCalendarTable1 = True
End Function

'***************************************************************************************************
'* 処理名　：GP_GetCalendarTable1
'* 機能　　：カレンダーテーブル作成(当月1ヶ月用)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル(Array:Structure)         ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：カレンダー関連関数クラスが初期化済であること(本処理内では判定しない)
'***************************************************************************************************
Public Sub GP_GetCalendarTable1(ByVal lngY As Long, _
                                ByVal lngM As Long, _
                                ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    ' カレンダーテーブル作成(当月分)
    Call g_clsAboutCalendar.GetCalendarTable1(lngY, lngM, tblCalendar)
End Sub

'***************************************************************************************************
'* 処理名　：FP_GetCalendarTable3
'* 機能　　：カレンダーテーブル作成(当月+前後の3ヶ月用)
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル(Array:Structure)         ※Ref参照
'* 　　　　　Arg4 = カレンダーテーブル当月開始INDEX(Long)       ※Ref参照(Option)
'* 　　　　　Arg5 = カレンダーテーブル当月終了INDEX(Long)       ※Ref参照(Option)
'* 　　　　　Arg6 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Public Function FP_GetCalendarTable3(ByVal lngYear As Long, _
                                     ByVal lngMonth As Long, _
                                     ByRef tblCalendar() As g_typAboutCalendar2, _
                                     Optional ByRef lngCurrStartIx As Long = -1, _
                                     Optional ByRef lngCurrEndIx As Long = -1, _
                                     Optional ByVal blnOmitCheck As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' カレンダー関連関数クラス初期化
    If Not FP_InitAboutCalendar(blnOmitCheck) Then
        FP_GetCalendarTable3 = False
        Exit Function
    End If
    ' カレンダーテーブル作成(当月+前後の3ヶ月用)
    Call g_clsAboutCalendar.GetCalendarTable3(lngYear, _
                                              lngMonth, _
                                              tblCalendar, _
                                              lngCurrStartIx, _
                                              lngCurrEndIx)
    FP_GetCalendarTable3 = True
End Function

'***************************************************************************************************
'* 処理名　：GP_GetCalendarTable3
'* 機能　　：カレンダーテーブル作成(当月+前後の3ヶ月用)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル(Array:Structure)         ※Ref参照
'* 　　　　　Arg4 = カレンダーテーブル当月開始INDEX(Long)       ※Ref参照(Option)
'* 　　　　　Arg5 = カレンダーテーブル当月終了INDEX(Long)       ※Ref参照(Option)
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：カレンダー関連関数クラスが初期化済であること(本処理内では判定しない)
'***************************************************************************************************
Public Sub GP_GetCalendarTable3(ByVal lngYear As Long, _
                                ByVal lngMonth As Long, _
                                ByRef tblCalendar() As g_typAboutCalendar2, _
                                Optional ByRef lngCurrStartIx As Long = -1, _
                                Optional ByRef lngCurrEndIx As Long = -1)
    '-----------------------------------------------------------------------------------------------
    ' カレンダーテーブル作成(当月+前後の3ヶ月用)
    Call g_clsAboutCalendar.GetCalendarTable3(lngYear, _
                                              lngMonth, _
                                              tblCalendar, _
                                              lngCurrStartIx, _
                                              lngCurrEndIx)
End Sub

'***************************************************************************************************
'* 処理名　：FP_SumEigyoNissu
'* 機能　　：営業日数算出(土日祝日を除外)
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = 期間開始日(Date)
'* 　　　　　Arg2 = 期間終了日(Date)
'* 　　　　　Arg3 = 営業日数(Long)                              ※Ref参照
'* 　　　　　Arg4 = 歴日数(Long)                                ※Ref参照(Option)
'* 　　　　　Arg5 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：開始日､終了日自体も営業日判断に適用される
'***************************************************************************************************
Public Function FP_SumEigyoNissu(ByVal dteDateF As Date, _
                                 ByVal dteDateT As Date, _
                                 ByRef lngCntEigyo As Long, _
                                 Optional ByRef lngCntReki As Long = 0, _
                                 Optional ByVal blnOmitCheck As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' カレンダー関連関数クラス初期化
    If Not FP_InitAboutCalendar(blnOmitCheck) Then
        FP_SumEigyoNissu = False
        Exit Function
    End If
    ' 営業日数算出(土日祝日を除外)
    Call g_clsAboutCalendar.SumEigyoNissu(dteDateF, dteDateT, lngCntEigyo, lngCntReki)
    FP_SumEigyoNissu = True
End Function

'***************************************************************************************************
'* 処理名　：FP_SumEigyoBi
'* 機能　　：営業日数経過後営業日算出(土日祝日を除外)
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = 起算日(Date)
'* 　　　　　Arg2 = 経過営業日数(Long)                          ※±可能
'* 　　　　　Arg3 = 営業日数経過後営業日(Date)                  ※Ref参照(Option)
'* 　　　　　Arg4 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：経過日数は翌日(翌営業日)を｢1｣として算出される(前営業日は｢-1｣)
'* 注意事項：経過日数がゼロの場合は起算日をそのまま返す(土日祝判断なし)
'***************************************************************************************************
Public Function FP_SumEigyoBi(ByVal dteDateF As Date, _
                              ByVal lngCntKeika As Long, _
                              ByRef dteDateT As Date, _
                              Optional ByVal blnOmitCheck As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' カレンダー関連関数クラス初期化
    If Not FP_InitAboutCalendar(blnOmitCheck) Then
        FP_SumEigyoBi = False
        Exit Function
    End If
    ' 営業日数経過後営業日算出(土日祝日を除外)
    Call g_clsAboutCalendar.SumEigyoBi(dteDateF, lngCntKeika, dteDateT)
    FP_SumEigyoBi = True
End Function

'***************************************************************************************************
'   ■■■ 以下はサポート用処理(削除可) ■■■
'***************************************************************************************************
'* 処理名　：GP_SortHoliParameter
'* 機能　　：祝日パラメータシート（Holiday_Parameter）の並替え
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 祝日パラメータシート(Worksheet)
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Public Sub GP_SortHoliParameter(ByRef objParaSheet As Worksheet)
    '-----------------------------------------------------------------------------------------------
    Dim lngRow As Long                                              ' 行INDEX
    Dim lngRowMax As Long                                           ' 行INDEX上限
    With objParaSheet
        ' フィルタモード解除
        If .FilterMode Then .ShowAllData
        ' 最終行取得
        lngRowMax = .Range("$A$" & .Rows.Count).End(xlUp).row
        ' 作業列クリア
        .Columns("$K").ClearContents
        lngRow = 3
        '-------------------------------------------------------------------------------------------
        ' 作業列に月以下の並替え要素をセット
        Do While lngRow <= lngRowMax
            ' 処理区分の判定
            Select Case .Cells(lngRow, 2).Value
                Case 0, 2                                   ' 固定日
                    .Cells(lngRow, 11).Value = .Cells(lngRow, 3).Value
                Case 1                                      ' HappyMonday
                    .Cells(lngRow, 11).Value = .Cells(lngRow, 5).Value * 7
                Case Else                                   ' 特殊
                    If .Cells(lngRow, 1).Value = 3 Then
                        .Cells(lngRow, 11).Value = 20
                    Else
                        .Cells(lngRow, 11).Value = 23
                    End If
            End Select
            ' 次行へ
            lngRow = lngRow + 1
        Loop
        '-------------------------------------------------------------------------------------------
        ' 並替え
        .Range("$A$3:$K$" & lngRowMax).Sort _
            Key1:=.Range("$A$3"), Order1:=xlAscending, _
            Key2:=.Range("$K$3"), Order1:=xlAscending, _
            Header:=xlNo, Orientation:=xlTopToBottom, SortMethod:=xlCodePage
        '-------------------------------------------------------------------------------------------
        ' 作業列クリア
        .Columns("$K").ClearContents
    End With
End Sub

'***************************************************************************************************
'* 処理名　：GP_ResetFormatConditions
'* 機能　　：祝日パラメータシート（Holiday_Parameter）条件付き書式再セット
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 祝日パラメータシート(Worksheet)
'---------------------------------------------------------------------------------------------------
'* 機能説明：行の挿入・削除で列単位の条件付き書式が分断されてしまうものを復旧させる
'* 注意事項：対象シートは非保護であること
'***************************************************************************************************
Public Sub GP_ResetFormatConditions(ByRef objParaSheet As Worksheet)
    '-----------------------------------------------------------------------------------------------
    Dim intReferenceStyleSV As Integer                              ' セル参照形式退避
    intReferenceStyleSV = Application.ReferenceStyle
    Application.ReferenceStyle = xlR1C1
    With objParaSheet
        ' フィルタモード解除
        If .FilterMode Then .ShowAllData
        '-------------------------------------------------------------------------------------------
        ' 現在の条件付き書式をクリア(シート全体)
        .Cells.FormatConditions.Delete
        '-------------------------------------------------------------------------------------------
        ' 罫線設定
        With .Columns("A:J")
            .FormatConditions.Add Type:=xlExpression, Formula1:="=AND(ROW()>3,RC1<>R[-1]C1)"
            With .FormatConditions(.FormatConditions.Count)
                With .Borders(xlTop)
                    .LineStyle = xlContinuous
                    .ColorIndex = xlAutomatic
                    .TintAndShade = 0
                    .Weight = xlThin
                End With
                .StopIfTrue = True
            End With
        End With
    End With
    With Application
        ' セル参照形式を復旧
        If .ReferenceStyle <> intReferenceStyleSV Then
            .ReferenceStyle = intReferenceStyleSV
        End If
    End With
End Sub

'----------------------------------------<< End of Source >>----------------------------------------
