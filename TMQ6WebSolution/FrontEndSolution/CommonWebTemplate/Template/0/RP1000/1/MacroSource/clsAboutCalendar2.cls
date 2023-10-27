VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "clsAboutCalendar2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'***************************************************************************************************
'   カレンダー及び日付処理(祝日含む)関連関数クラス              clsAboutCalendar2(Class)
'
'   ※本クラスの操作は全てカレンダー関連モジュール(modAboutCalendar2)で行ないます
'   本モジュールは祝日パラメータテーブルを作成してその祝日情報を元にカレンダーテーブルを作成します
'***************************************************************************************************
Option Explicit
'===================================================================================================
'  ↓↓↓ バージョン値、バージョン更新日は本クラスの変更を行なう時に必ず更新すること ↓↓↓
Private Const g_cnsCalendarVersion As String = "1.72"           ' 本クラスのバージョン
Private Const g_cnsCalendarVerUpdDate As Date = #5/20/2022#     ' バージョン更新日
'  ↑↑↑ バージョン値、バージョン更新日は本クラスの変更を行なう時に必ず更新すること ↑↑↑
'===================================================================================================
Private Const g_cnsTitle As String = "祝日判定処理"
Private Const g_cnsFuri As String = "(振替休日)"
Private Const g_cnsKyu2 As String = "国民の休日"
'---------------------------------------------------------------------------------------------------
' 祝日パラメータテーブル用ユーザー定義
Private Type g_typParamater
    Getsu As Long                                   ' 月
    SyoriKbn As Long                                ' 処理区分(0=固定日､1=HappyMonday､2=会社休日､9=特殊)
    Hiduke As Long                                  ' [固定日]日(Date型ではない)
    FurikaeKbn As Long                              ' [固定日]振替区分(0=通常､1=振替休日を行なわない)
    HmSyusu As Long                                 ' [HM]週数(第n週)
    HmYobi As Long                                  ' [HM]曜日(0=日､1=月､2=火～6=土)
    SyukuNm As String                               ' 祝日名
    StrYear As Long                                 ' 開始年(西暦)
    EndYear As Long                                 ' 終了年(西暦)
End Type
'---------------------------------------------------------------------------------------------------
' 祝日パラメータ月別配置テーブル用ユーザー定義
Private Type g_typParamMMPos
    StrIx As Long                                   ' 当月開始位置INDEX(祝日無しは-1)
    EndIx As Long                                   ' 当月終了位置INDEX(祝日無しは-1)
End Type
'---------------------------------------------------------------------------------------------------
' 振替休日・国民の休日判定対象テーブル用ユーザー定義
Private Type g_typFuriDIx
    CalIx As Long                                   ' カレンダーINDEX
    Syubetsu As Long                                ' 0=振替休日、1=国民の休日
End Type
'===================================================================================================
' 祝日パラメータシート（Holiday_Parameter）用フラグ
Private g_blnParaReaded As Boolean                  ' 読み込み済フラグ
Private g_blnParaChecked As Boolean                 ' チェック済フラグ
' 祝日パラメータテーブル(全件)
Private g_tblParamater() As g_typParamater          ' 祝日パラメータテーブル
'---------------------------------------------------------------------------------------------------
' 祝日パラメータ配置テーブル(このテーブルは月をINDEXとするため1オリジン)
Private g_tblParamMMPos(1 To 12) As g_typParamMMPos ' 祝日パラメータ月別配置テーブル

'***************************************************************************************************
'   ■■■ プロパティ(Friend) ■■■
'***************************************************************************************************
'   本クラスのバージョン(String)
'---------------------------------------------------------------------------------------------------
Friend Property Get Version() As String
    Version = g_cnsCalendarVersion
End Property

'===================================================================================================
'   バージョン更新日(Date)
'---------------------------------------------------------------------------------------------------
Friend Property Get VerUpdDate() As Date
    VerUpdDate = g_cnsCalendarVerUpdDate
End Property

'===================================================================================================
'   読み込み済フラグ(Boolean)
'---------------------------------------------------------------------------------------------------
Friend Property Get ParaReaded() As Boolean
    ParaReaded = g_blnParaReaded
End Property

'***************************************************************************************************
'   ■■■ 公開プロシージャ(Friend) ■■■
'***************************************************************************************************
'* 処理名　：MakeHoliParamater
'* 機能　　：祝日パラメータテーブル作成
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = チェック工程スキップ(Boolean)
'---------------------------------------------------------------------------------------------------
'* 機能説明：祝日パラメータテーブル作成だけを先行して行なう
'* 注意事項：本クラス初期化後に必ず呼び出すこと
'***************************************************************************************************
Friend Function MakeHoliParamater(ByVal blnOmitCheck As Boolean) As Boolean
    '-----------------------------------------------------------------------------------------------
    ' 祝日パラメータテーブルの取得
    MakeHoliParamater = FP_GetHoliParameter(blnOmitCheck)
End Function

'***************************************************************************************************
'* 処理名　：GetCalendarTable1
'* 機能　　：カレンダーテーブル作成(当月1ヶ月用)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル(Array:Structure)         ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Friend Sub GetCalendarTable1(ByVal lngY As Long, _
                             ByVal lngM As Long, _
                             ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    ReDim tblCalendar(0)
    ' カレンダーテーブル作成(当月分)
    Call GP_GetCalendarSub(lngY, lngM, -1, tblCalendar)
End Sub

'***************************************************************************************************
'* 処理名　：GetCalendarTable3
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
'* 注意事項：
'***************************************************************************************************
Friend Sub GetCalendarTable3(ByVal lngYear As Long, _
                             ByVal lngMonth As Long, _
                             ByRef tblCalendar() As g_typAboutCalendar2, _
                             Optional ByRef lngCurrStartIx As Long = -1, _
                             Optional ByRef lngCurrEndIx As Long = -1)
    '-----------------------------------------------------------------------------------------------
    Dim lngCalIxMax As Long                                         ' カレンダーテーブル要素上限
    Dim lngY As Long                                                ' 年WORK
    Dim lngM As Long                                                ' 月WORK
    ReDim tblCalendar(0)
    lngCalIxMax = -1
    ' 前月の年月を算出
    If lngMonth = 1 Then
        lngY = lngYear - 1
        lngM = 12
    Else
        lngY = lngYear
        lngM = lngMonth - 1
    End If
    ' カレンダーテーブル作成(前月分)
    Call GP_GetCalendarSub(lngY, lngM, lngCalIxMax, tblCalendar)
    '-----------------------------------------------------------------------------------------------
    ' 当月開始INDEX
    lngCurrStartIx = lngCalIxMax + 1
    '-----------------------------------------------------------------------------------------------
    ' カレンダーテーブル作成(当月分)
    Call GP_GetCalendarSub(lngYear, lngMonth, lngCalIxMax, tblCalendar)
    '-----------------------------------------------------------------------------------------------
    ' 当月終了INDEX
    lngCurrEndIx = lngCalIxMax
    '-----------------------------------------------------------------------------------------------
    ' 翌月の年月を算出
    If lngMonth = 12 Then
        lngY = lngYear + 1
        lngM = 1
    Else
        lngY = lngYear
        lngM = lngMonth + 1
    End If
    ' カレンダーテーブル作成(翌月分)
    Call GP_GetCalendarSub(lngY, lngM, lngCalIxMax, tblCalendar)
End Sub

'***************************************************************************************************
'* 処理名　：SumEigyoNissu
'* 機能　　：営業日数算出(土日祝日を除外)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 期間開始日(Date)
'* 　　　　　Arg2 = 期間終了日(Date)
'* 　　　　　Arg3 = 営業日数(Long)                              ※Ref参照
'* 　　　　　Arg4 = 歴日数(Long)                                ※Ref参照(Option)
'* 　　　　　Arg5 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：開始日､終了日自体も営業日判断に適用される
'***************************************************************************************************
Friend Sub SumEigyoNissu(ByVal dteDateF As Date, _
                         ByVal dteDateT As Date, _
                         ByRef lngCntEigyo As Long, _
                         Optional ByRef lngCntReki As Long = 0)
    '-----------------------------------------------------------------------------------------------
    lngCntEigyo = 0
    lngCntReki = 0
    ' 時刻の除去
    dteDateF = DateSerial(Year(dteDateF), Month(dteDateF), Day(dteDateF))
    dteDateT = DateSerial(Year(dteDateT), Month(dteDateT), Day(dteDateT))
    ' 開始日>終了日(逆転)の場合は処理なし
    If dteDateF > dteDateF Then Exit Sub
    '-----------------------------------------------------------------------------------------------
    Dim dteTmp As Date                                              ' 作業日
    Dim currY As Long                                               ' 現在年
    Dim currM As Long                                               ' 現在月
    Dim prevY As Long                                               ' 直前年
    Dim prevM As Long                                               ' 直前月
    Dim swSyuku As Boolean                                          ' 祝日判定
    Dim lngIxS As Long                                              ' カレンダーテーブルINDEX
    Dim lngIxSMax As Long                                           ' カレンダーテーブルINDEX上限
    Dim tblCalendar() As g_typAboutCalendar2                        ' カレンダーテーブル
    dteTmp = dteDateF
    currY = Year(dteTmp)
    currM = Month(dteTmp)
    '===============================================================================================
    ' 終了日までループ
    Do While dteTmp <= dteDateT
        '-------------------------------------------------------------------------------------------
        ' ■月あたり前処理
        lngIxS = 0
        prevY = currY
        prevM = currM
        lngIxSMax = -1
        ReDim tblCalendar(0)
        ' カレンダーテーブル作成(当月1ヶ月用)
        Call GP_GetCalendarSub(prevY, prevM, lngIxSMax, tblCalendar)
        '-------------------------------------------------------------------------------------------
        ' ■月あたり主処理(月替わりか終了日までループ)
        Do While ((dteTmp <= dteDateT) And (currY = prevY) And (currM = prevM))
            '----------------------------------------------
            ' 1日単位処理
            swSyuku = False
            lngCntReki = lngCntReki + 1                         ' 歴日数を加算
            '----------------------------------------------
            ' カレンダーテーブルをサーチ
            Do While lngIxS <= lngIxSMax
                ' 日付発見か
                If tblCalendar(lngIxS).Hiduke = dteTmp Then
                    ' 祝日か判定
                    swSyuku = tblCalendar(lngIxS).Syuku <> 0
                    Exit Do
                End If
                ' 次へ
                lngIxS = lngIxS + 1
            Loop
            '----------------------------------------------
            ' 土日祝日でなければ営業日数を加算
            If ((Weekday(dteTmp) <> vbSunday) And _
                (Weekday(dteTmp) <> vbSaturday) And _
                (Not swSyuku)) Then
                lngCntEigyo = lngCntEigyo + 1                   ' 営業日数を加算
            End If
            '----------------------------------------------
            ' 翌日を設定
            dteTmp = dteTmp + 1
            currY = Year(dteTmp)
            currM = Month(dteTmp)
        Loop
    Loop
End Sub

'***************************************************************************************************
'* 処理名　：SumEigyoBi
'* 機能　　：営業日数経過後営業日算出(土日祝日を除外)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 起算日(Date)
'* 　　　　　Arg2 = 経過日数(Long)                              ※±可能
'* 　　　　　Arg3 = 営業日数経過後営業日(Date)                  ※Ref参照(Option)
'---------------------------------------------------------------------------------------------------
'* 機能説明：経過日数は翌日(翌営業日)を｢1｣として算出される(前営業日は｢-1｣)
'* 注意事項：経過日数がゼロの場合は起算日をそのまま返す(土日祝判断なし)
'***************************************************************************************************
Friend Sub SumEigyoBi(ByVal dteDateF As Date, _
                      ByVal lngCntKeika As Long, _
                      ByRef dteDateT As Date)
    '-----------------------------------------------------------------------------------------------
    dteDateT = dteDateF                                         ' 一旦、起算日とする
    ' 経過日数がゼロなら非処理 ⇒ 起算日をそのまま返す(正常処理)
    If lngCntKeika = 0 Then Exit Sub
    '-----------------------------------------------------------------------------------------------
    Dim lngSign As Long                                             ' ループ時の加算日数
    Dim lngCntKeikaABS As Long                                      ' 経過日数の絶対値
    Dim dteTmp As Date                                              ' 作業日
    Dim currY As Long                                               ' 現在年
    Dim currM As Long                                               ' 現在月
    Dim prevY As Long                                               ' 直前年
    Dim prevM As Long                                               ' 直前月
    Dim lngCntEigyo As Long                                         ' 営業日数
    Dim swSyuku As Boolean                                          ' 祝日判定
    Dim lngIxS As Long                                              ' カレンダーテーブルINDEX
    Dim lngIxSMax As Long                                           ' カレンダーテーブルINDEX上限
    Dim tblCalendar() As g_typAboutCalendar2                        ' カレンダーテーブル
    dteTmp = dteDateF
    ' 経過日数の絶対値及び符号判定
    If lngCntKeika > 0 Then
        ' プラスは後方に向かって判定
        lngSign = 1
        ' 翌日から判定を始める(起算日は0なので含めない)
        dteTmp = dteTmp + 1
    Else
        ' マイナスは前方に向かって判定
        lngSign = -1
        ' 前日から判定を始める(起算日は0なので含めない)
        dteTmp = dteTmp - 1
    End If
    lngCntKeikaABS = lngCntKeika * lngSign
    currY = Year(dteTmp)
    currM = Month(dteTmp)
    '===============================================================================================
    ' 経過日数が経過するまでループ
    Do While lngCntEigyo < lngCntKeikaABS
        '-------------------------------------------------------------------------------------------
        ' ■月あたり前処理
        prevY = currY
        prevM = currM
        lngIxSMax = -1
        ReDim tblCalendar(0)
        ' カレンダーテーブル作成(当月1ヶ月用)
        Call GP_GetCalendarSub(prevY, prevM, lngIxSMax, tblCalendar)
        ' 経過日数がプラスか
        If lngCntKeika > 0 Then
            lngIxS = 0
            '---------------------------------------------------------------------------------------
            ' ■月あたり主処理(月替わりか終了日までループ) ※プラス時
            Do While ((lngCntEigyo < lngCntKeikaABS) And (currY = prevY) And (currM = prevM))
                '----------------------------------------------
                ' 1日単位処理
                swSyuku = False
                '----------------------------------------------
                'カレンダーテーブルをサーチ
                Do While lngIxS <= lngIxSMax
                    ' 日付発見か
                    If tblCalendar(lngIxS).Hiduke = dteTmp Then
                        ' 祝日か判定
                        swSyuku = tblCalendar(lngIxS).Syuku <> 0
                        Exit Do
                    End If
                    ' 次へ
                    lngIxS = lngIxS + 1
                Loop
                '----------------------------------------------
                ' 土日祝日でなければ営業日数を加算
                If ((Weekday(dteTmp) <> vbSunday) And _
                    (Weekday(dteTmp) <> vbSaturday) And _
                    (Not swSyuku)) Then
                    lngCntEigyo = lngCntEigyo + 1               ' 営業日数を加算
                    dteDateT = dteTmp                           ' 現在日をセット
                End If
                '----------------------------------------------
                ' 翌日を設定
                dteTmp = dteTmp + 1
                currY = Year(dteTmp)
                currM = Month(dteTmp)
            Loop
        Else
            lngIxS = lngIxSMax
            '---------------------------------------------------------------------------------------
            ' ■月あたり主処理(月替わりか終了日までループ) ※マイナス時
            Do While ((lngCntEigyo < lngCntKeikaABS) And (currY = prevY) And (currM = prevM))
                '----------------------------------------------
                ' 1日単位処理
                swSyuku = False
                '----------------------------------------------
                'カレンダーテーブルをサーチ(後ろから)
                Do While lngIxS >= 0
                    ' 日付発見か
                    If tblCalendar(lngIxS).Hiduke = dteTmp Then
                        ' 祝日か判定
                        swSyuku = tblCalendar(lngIxS).Syuku <> 0
                        Exit Do
                    End If
                    ' 次へ
                    lngIxS = lngIxS - 1
                Loop
                '----------------------------------------------
                ' 土日祝日でなければ営業日数を加算
                If ((Weekday(dteTmp) <> vbSunday) And _
                    (Weekday(dteTmp) <> vbSaturday) And _
                    (Not swSyuku)) Then
                    lngCntEigyo = lngCntEigyo + 1               ' 営業日数を加算
                    dteDateT = dteTmp                           ' 現在日をセット
                End If
                '----------------------------------------------
                ' 前日を設定
                dteTmp = dteTmp - 1
                currY = Year(dteTmp)
                currM = Month(dteTmp)
            Loop
        End If
    Loop
End Sub

'***************************************************************************************************
'* 処理名　：FP_CheckSyukuParameter
'* 機能　　：祝日パラメータシート（Holiday_Parameter）チェック
'---------------------------------------------------------------------------------------------------
'* 返り値　：チェック成否(Boolean)
'* 引数　　：Arg1 = 祝日パラメータシート(Object)                ※Ref参照
'* 　　　　　Arg2 = 祝日パラメータ最終行(Long)                  ※Ref参照(Option)
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Friend Function FP_CheckSyukuParameter(ByRef objParaSheet As Worksheet, _
                                       Optional ByRef lngParaRowMax As Long = 0) As Boolean
    '-----------------------------------------------------------------------------------------------
    Dim lngRow As Long                                              ' 行INDEX
    Dim lngRow2 As Long                                             ' 行INDEX
    Dim lngCurrEnd As Long                                          ' 当回終了年
    Dim lngNextStr As Long                                          ' 次回開始年
    Dim lngPrevM As Long                                            ' 直前行の月
    Dim strMSG As String                                            ' エラーメッセージ
    FP_CheckSyukuParameter = False
    '-----------------------------------------------------------------------------------------------
    ' 祝日パラメータシート（Holiday_Parameter）チェック
    lngRow = 3                                                  ' データ先頭行
    With objParaSheet
        .Unprotect
        ' フィルタ解除
        If .FilterMode Then .ShowAllData
        ' 最終行取得
        lngParaRowMax = .Range("$A$" & .Rows.Count).End(xlUp).row
        ' 有効内容無し
        If lngParaRowMax < 3 Then
            strMSG = "「" & g_cnsParaSheet & "」シートに有効内容が登録されていません。"
            MsgBox strMSG, vbCritical, g_cnsTitle
            Exit Function
        End If
        '-------------------------------------------------------------------------------------------
        ' 各行を巡回してチェック
        Do While lngRow <= lngParaRowMax
            ' 祝日パラメータテーブルチェック(行あたり)
            Call GP_CheckSyukuParameterSUB(objParaSheet, lngRow, lngPrevM, strMSG)
            ' 次の行へ
            lngRow = lngRow + 1
        Loop
        '-------------------------------------------------------------------------------------------
        ' ここまででエラーがあれば終了
        If strMSG <> "" Then
            MsgBox strMSG, vbCritical, g_cnsTitle
            Exit Function
        End If
        '-------------------------------------------------------------------------------------------
        lngRow = 3                                              ' データ先頭行
        ' 有効年期間重複チェック等(同一月日(固定日)のみ)
        Do While lngRow <= lngParaRowMax
            lngRow2 = lngRow + 1
            ' 同じ月か
            If .Cells(lngRow2, 1).Value = .Cells(lngRow, 1).Value Then
                ' 処理区分が固定日で同一日か
                If (((.Cells(lngRow2, 2).Value = 0) Or (.Cells(lngRow2, 2).Value = 2)) And _
                    ((.Cells(lngRow, 2).Value = 0) Or (.Cells(lngRow, 2).Value = 2)) And _
                    (.Cells(lngRow2, 3).Value = .Cells(lngRow, 3).Value)) Then
                    lngCurrEnd = .Cells(lngRow, 9).Value
                    lngNextStr = .Cells(lngRow2, 8).Value
                    ' 重複か
                    If lngNextStr <= lngCurrEnd Then
                        Dim strTemp As String                       ' テキストWORK
                        strTemp = CStr(lngRow2) & "行目："
                        strTemp = strTemp & "「有効年期間」が前行と重複しています。"
                        Call GP_AppendMessage(strTemp, strMSG)
                    End If
                End If
            End If
            ' 次の行へ
            lngRow = lngRow + 1
        Loop
        .Protect UserInterfaceOnly:=True
    End With
    '-----------------------------------------------------------------------------------------------
    If strMSG <> "" Then
        MsgBox strMSG, vbCritical, g_cnsTitle
    Else
        FP_CheckSyukuParameter = True
    End If
End Function

'***************************************************************************************************
'   ■■■ サブ処理(Private) ■■■
'***************************************************************************************************
'* 処理名　：GP_GetCalendarSub
'* 機能　　：カレンダーテーブル作成(単月分)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル要素上限(Long)            ※Ref参照
'* 　　　　　Arg4 = カレンダーテーブル(Array:Structure)         ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：本処理ではカレンダーテーブルの初期化は行なわずに要素が追加される
'***************************************************************************************************
Private Sub GP_GetCalendarSub(ByVal lngY As Long, _
                              ByVal lngM As Long, _
                              ByRef lngCalIxMax As Long, _
                              ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngCalStrIx As Long                                         ' 当月開始位置INDEX
    '-----------------------------------------------------------------------------------------------
    lngCalStrIx = lngCalIxMax + 1
    ' カレンダーテーブル作成(当月分：追加モード)
    Call GP_MakeCalendarTable(lngY, lngM, lngCalIxMax, tblCalendar)
    '-----------------------------------------------------------------------------------------------
    ' 当月に祝日がない時は終了
    If g_tblParamMMPos(lngM).EndIx < 0 Then Exit Sub
    '-----------------------------------------------------------------------------------------------
    Dim lngFuriDIxMax As Long                                       ' 振替休日判定日INDEXテーブル上限
    Dim tblFuriDIx() As g_typFuriDIx                                ' 振替休日判定日INDEXテーブル
    lngFuriDIxMax = -1
    ReDim tblFuriDIx(0)
    ' カレンダーテーブルに祝日情報を更新(この時点では振替休日の処置はしない)
    Call GP_UpdateHolidayToCalendar(lngY, _
                                    lngM, _
                                    lngCalStrIx, _
                                    lngFuriDIxMax, _
                                    tblFuriDIx, _
                                    tblCalendar)
    '-----------------------------------------------------------------------------------------------
    ' 振替休日判定が必要か
    If lngFuriDIxMax >= 0 Then
        ' 振替休日・国民の休日設定
        Call GP_UpdateFuriKokuminKyu(tblFuriDIx, tblCalendar)
    End If
End Sub

'***************************************************************************************************
'* 処理名　：GP_MakeCalendarTable
'* 機能　　：カレンダーテーブル作成(当月分：追加モード)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル要素上限(Long)            ※Ref参照
'* 　　　　　Arg4 = カレンダーテーブル(Array:Structure)         ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：カレンダーテーブルの日付要素の追加を行なう
'* 注意事項：本処理ではカレンダーテーブルの初期化は行なわずに要素が追加される
'***************************************************************************************************
Private Sub GP_MakeCalendarTable(ByVal lngY As Long, _
                                 ByVal lngM As Long, _
                                 ByRef lngCalIxMax As Long, _
                                 ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim dteStrDate As Date                                          ' 月初日
    Dim dteEndDate As Date                                          ' 月末日
    Dim dteDate As Date                                             ' 作業日付
    Dim lngSyusu As Long                                            ' 週数(1始まり､日曜日発見で1加算)
    Dim lngHmSyusu As Long                                          ' HM週数(0始まり､月曜日発見で1加算)
    Dim lngCalIxMax2 As Long                                        '
    dteStrDate = DateSerial(lngY, lngM, 1)
    dteEndDate = DateSerial(lngY, lngM + 1, 0)
    ' カレンダーテーブルの要素を追加(当月の日数分)
    lngCalIxMax2 = lngCalIxMax + Day(dteEndDate)
    ReDim Preserve tblCalendar(lngCalIxMax2)
    dteDate = dteStrDate
    ' 1日が日曜日の時に週数が2にならないように対応
    If Weekday(dteStrDate, vbSunday) = vbSunday Then
        lngSyusu = 0
    Else
        lngSyusu = 1
    End If
    ' 月初日から月末日まで繰り返す
    Do While dteDate <= dteEndDate
        lngCalIxMax = lngCalIxMax + 1
        With tblCalendar(lngCalIxMax)
            .Hiduke = dteDate
            .Yobi = Weekday(dteDate, vbSunday) - 1      ' ←日曜日=0とする値(VBAのWeekdayは1になる)
            ' 日曜日なら週数を加算
            If .Yobi = 0 Then lngSyusu = lngSyusu + 1
            ' 月曜日ならHM週数を加算
            If .Yobi = 1 Then lngHmSyusu = lngHmSyusu + 1
            .Syusu = lngSyusu
            .HmSyusu = lngHmSyusu
        End With
        ' 次の日へ
        dteDate = dteDate + 1
    Loop
End Sub

'***************************************************************************************************
'* 処理名　：GP_UpdateHolidayToCalendar
'* 機能　　：カレンダーテーブルに祝日情報を更新
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = 月(Long)
'* 　　　　　Arg3 = カレンダーテーブル当月開始位置INDEX(Long)
'* 　　　　　Arg4 = 振替休日判定日INDEXテーブル上限INDEX(Long)      ※Ref参照
'* 　　　　　Arg5 = 振替休日判定日INDEXテーブル(Array:Structure)    ※Ref参照
'* 　　　　　Arg6 = カレンダーテーブル(Array:Structure)             ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：この時点では振替休日の処置はしない
'***************************************************************************************************
Private Sub GP_UpdateHolidayToCalendar(ByVal lngY As Long, _
                                       ByVal lngM As Long, _
                                       ByVal lngCalStrIx As Long, _
                                       ByRef lngFuriDIxMax As Long, _
                                       ByRef tblFuriDIx() As g_typFuriDIx, _
                                       ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngIxS_Str As Long                                          ' 祝日テーブル当月開始位置
    Dim lngIxS_End As Long                                          ' 祝日テーブル当月終了位置
    Dim lngIxSyuku As Long                                          ' 祝日テーブルテーブルINDEX
    ' 祝日パラメータテーブルの当月範囲を得る
    lngIxS_Str = g_tblParamMMPos(lngM).StrIx
    lngIxS_End = g_tblParamMMPos(lngM).EndIx
    ' カレンダーテーブルに祝日情報を更新(この時点では振替休日の判断はしない)
    For lngIxSyuku = lngIxS_Str To lngIxS_End
        With g_tblParamater(lngIxSyuku)
            ' 祝日パラメータの有効年範囲を判定
            If ((.StrYear <= lngY) And (.EndYear >= lngY)) Then
                ' 処理区分の判定
                Select Case .SyoriKbn
                    Case 0                          ' 固定日
                        ' 祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
                        Call GP_UpdateHolidaySUB1(.Hiduke, _
                                                  .SyukuNm, _
                                                  1, _
                                                  .FurikaeKbn, _
                                                  lngCalStrIx, _
                                                  lngIxSyuku, _
                                                  lngFuriDIxMax, _
                                                  tblFuriDIx, _
                                                  tblCalendar)
                    Case 1                          ' HappyMonday
                        ' 祝日情報のカレンダーテーブル更新(サブ処理：週数＋曜日で判定)
                        Call GP_UpdateHolidaySUB2(.HmSyusu, _
                                                  .HmYobi, _
                                                  .SyukuNm, _
                                                  1, _
                                                  .FurikaeKbn, _
                                                  lngCalStrIx, _
                                                  lngIxSyuku, _
                                                  lngFuriDIxMax, _
                                                  tblFuriDIx, _
                                                  tblCalendar)
                    Case 2                          ' 固定日(会社休日)
                        ' 祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
                        Call GP_UpdateHolidaySUB1(.Hiduke, _
                                                  .SyukuNm, _
                                                  3, _
                                                  .FurikaeKbn, _
                                                  lngCalStrIx, _
                                                  lngIxSyuku, _
                                                  lngFuriDIxMax, _
                                                  tblFuriDIx, _
                                                  tblCalendar)
                    Case 9                          ' 特殊処理
                        ' 3月か
                        If lngM = 3 Then
                            ' 春分の日の算出(簡易計算方式)
                            Call GP_GetSyunbun(lngY, _
                                               lngCalStrIx, _
                                               lngIxSyuku, _
                                               lngFuriDIxMax, _
                                               tblFuriDIx, _
                                               tblCalendar)
                        ElseIf lngM = 9 Then
                            ' 秋分の日の算出(簡易計算方式)
                            Call GP_GetSyuubun(lngY, _
                                               lngCalStrIx, _
                                               lngIxSyuku, _
                                               lngFuriDIxMax, _
                                               tblFuriDIx, _
                                               tblCalendar)
                        Else ' 他月は無視
                        End If
                End Select
            End If
        End With
    Next lngIxSyuku
End Sub

'***************************************************************************************************
'* 処理名　：GP_UpdateHolidaySUB1
'* 機能　　：祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 日(Long:1～31)
'* 　　　　　Arg2 = 祝日名称(String)
'* 　　　　　Arg3 = 祝日判定(Long:0=通常､1=祝日､2=振替休日､3=会社休日)
'* 　　　　　Arg4 = 振替区分(Long:0=通常､1=振替休日を行なわない)
'* 　　　　　Arg5 = カレンダー当月開始位置INDEX(Long)
'* 　　　　　Arg6 = 祝日パラメータテーブルINDEX(Long)
'* 　　　　　Arg7 = 振替休日判定日INDEXテーブルINDEX(Long)          ※Ref参照
'* 　　　　　Arg8 = 振替休日判定日INDEXテーブル(Array:Structure)    ※Ref参照
'* 　　　　　Arg9 = カレンダーテーブル(Array:Structure)             ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Sub GP_UpdateHolidaySUB1(ByVal lngDay As Long, _
                                 ByVal strName As String, _
                                 ByVal lngSyuku As Long, _
                                 ByVal lngFurikaeKbn As Long, _
                                 ByVal lngCalStrIx As Long, _
                                 ByVal lngIxSyuku As Long, _
                                 ByRef lngFuriDIxMax As Long, _
                                 ByRef tblFuriDIx() As g_typFuriDIx, _
                                 ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngIx As Long                                           ' テーブルINDEX
    Dim lngIxMax As Long                                        ' テーブルINDEX上限
    Dim lngIxA1 As Long                                         ' テーブルINDEX(1日後)
    Dim lngIxA2 As Long                                         ' テーブルINDEX(2日後)
    Dim lngIxB1 As Long                                         ' テーブルINDEX(1日前)
    Dim lngIxB2 As Long                                         ' テーブルINDEX(2日前)
    Dim lngIxSD As Long                                         ' 振替休日判定日INDEXテーブルINDEX
    Dim lngIxSyukuN As Long                                     ' 次の祝日パラメータテーブルINDEX
    Dim lngYearN As Long                                        ' 次の年
    Dim lngMonthN As Long                                       ' 次の月
    ' ｢日｣から該当するカレンダーテーブル上のINDEXを算出
    lngIx = lngCalStrIx + lngDay - 1
    lngIxMax = UBound(tblCalendar)
    '-------------------------------------------------------
    ' 当該日が既に祝日(又は振替休日、国民の休日)だったら｢会社休日｣は適用しない
    ' ※この判定には｢会社休日｣の対する振替区分の判断は行なっていません
    If ((lngSyuku = 3) And (tblCalendar(lngIx).Syuku <> 0)) Then
        Exit Sub
    End If
    '-------------------------------------------------------
    ' 前後の日付位置INDEXを算出
    lngIxA1 = lngIx + 1
    lngIxA2 = lngIx + 2
    lngIxB1 = lngIx - 1
    lngIxB2 = lngIx - 2
    ' 祝日情報を反映
    With tblCalendar(lngIx)
        .Syuku = lngSyuku
        .SyukuNm = strName
        .FurikaeKbn = lngFurikaeKbn
    End With
    ' 振替休日を行なわない時は終了
    If lngFurikaeKbn = 1 Then Exit Sub
    '-----------------------------------------------------------------------------------------------
    ' 日曜日又は前後判定で｢国民の休日｣対象なら振替休日判定日INDEXテーブルに追加
    If tblCalendar(lngIx).Yobi = 0 Then
        lngFuriDIxMax = lngFuriDIxMax + 1
        ReDim Preserve tblFuriDIx(lngFuriDIxMax)
        tblFuriDIx(lngFuriDIxMax).CalIx = lngIx
        tblFuriDIx(lngFuriDIxMax).Syubetsu = 0
        Exit Sub
    End If
    ' 2日前が祝日で1日前が祝日でない⇒｢国民の休日｣対象
    If lngIxB2 >= 0 Then
        If ((tblCalendar(lngIxB2).Syuku <> 0) And _
            (tblCalendar(lngIxB1).Syuku = 0) And _
            (tblCalendar(lngIxB2).FurikaeKbn = 0)) Then
            lngFuriDIxMax = lngFuriDIxMax + 1
            ReDim Preserve tblFuriDIx(lngFuriDIxMax)
            tblFuriDIx(lngFuriDIxMax).CalIx = lngIxB2
            tblFuriDIx(lngFuriDIxMax).Syubetsu = 1
            Exit Sub
        End If
    End If
    ' 2日後が祝日で1日後が祝日でない⇒｢国民の休日｣対象
    If lngIxA2 <= lngIxMax Then
        If ((tblCalendar(lngIxA2).Syuku <> 0) And _
            (tblCalendar(lngIxA1).Syuku = 0) And _
            (tblCalendar(lngIxA2).FurikaeKbn = 0)) Then
            lngFuriDIxMax = lngFuriDIxMax + 1
            ReDim Preserve tblFuriDIx(lngFuriDIxMax)
            tblFuriDIx(lngFuriDIxMax).CalIx = lngIx
            tblFuriDIx(lngFuriDIxMax).Syubetsu = 1
            Exit Sub
        End If
    End If
    ' 月末日の1日前⇒翌月1日が祝日か判定
    If lngIx = lngIxMax - 1 Then
        ' 次の祝日パラメータテーブルINDEX
        lngIxSyukuN = lngIxSyuku + 1
        ' 次の祝日が1日か
        If ((g_tblParamater(lngIxSyukuN).Hiduke = 1) And _
            (g_tblParamater(lngIxSyukuN).FurikaeKbn = 0)) Then
            lngYearN = Year(tblCalendar(lngIx).Hiduke)
            lngMonthN = Month(tblCalendar(lngIx).Hiduke) + 1     ' 次の月
            ' 次の月が有効か
            If lngMonthN > 12 Then
                lngYearN = lngYearN + 1
                lngMonthN = 1
            End If
            With g_tblParamater(lngIxSyukuN)
                ' 有効な祝日か
                If ((.Getsu = lngMonthN) And _
                    (.StrYear <= lngYearN) And _
                    (.EndYear >= lngYearN)) Then
                    ' 月末日が｢国民の休日｣対象
                    lngFuriDIxMax = lngFuriDIxMax + 1
                    ReDim Preserve tblFuriDIx(lngFuriDIxMax)
                    tblFuriDIx(lngFuriDIxMax).CalIx = lngIx
                    tblFuriDIx(lngFuriDIxMax).Syubetsu = 1
                    Exit Sub
                End If
            End With
        End If
    End If
End Sub

'***************************************************************************************************
'* 処理名　：GP_UpdateHolidaySUB2
'* 機能　　：祝日情報のカレンダーテーブル更新(サブ処理：週数＋曜日で判定)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = HM週数(Long)
'* 　　　　　Arg2 = 曜日(Long)
'* 　　　　　Arg3 = 祝日名称(String)
'* 　　　　　Arg4 = 祝日判定(0=通常､1=祝日､2=振替休日､3=会社休日)
'* 　　　　　Arg5 = 振替区分(0=通常､1=振替休日を行なわない)
'* 　　　　　Arg6 = カレンダー当月開始位置INDEX(Long)
'* 　　　　　Arg7 = 祝日パラメータテーブルINDEX(Long)
'* 　　　　　Arg8 = 振替休日判定日INDEXテーブルINDEX(Long)          ※Ref参照
'* 　　　　　Arg9 = 振替休日判定日INDEXテーブル(Array:Structure)    ※Ref参照
'* 　　　　　Arg10= カレンダーテーブル(Array:Structure)             ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Sub GP_UpdateHolidaySUB2(ByVal lngSyusu As Long, _
                                 ByVal lngYobi As Long, _
                                 ByVal strName As String, _
                                 ByVal lngHoli As Long, _
                                 ByVal lngFurikaeKbn As Long, _
                                 ByVal lngCalStrIx As Long, _
                                 ByVal lngIxSyuku As Long, _
                                 ByRef lngFuriDIxMax As Long, _
                                 ByRef tblFuriDIx() As g_typFuriDIx, _
                                 ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngIx As Long                                           ' テーブルINDEX
    lngIx = lngCalStrIx
    ' カレンダーテーブルから該当週数＋曜日を探す
    Do While lngIx <= UBound(tblCalendar)
        ' HM週数＋曜日発見か
        If ((tblCalendar(lngIx).HmSyusu = lngSyusu) And (tblCalendar(lngIx).Yobi = lngYobi)) Then
            ' 祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
            Call GP_UpdateHolidaySUB1(Day(tblCalendar(lngIx).Hiduke), _
                                      strName, _
                                      lngHoli, _
                                      lngFurikaeKbn, _
                                      lngCalStrIx, _
                                      lngIxSyuku, _
                                      lngFuriDIxMax, _
                                      tblFuriDIx, _
                                      tblCalendar)
            Exit Do
        End If
        ' 次へ
        lngIx = lngIx + 1
    Loop
End Sub

'***************************************************************************************************
'* 処理名　：GP_GetSyunbun
'* 機能　　：春分の日の算出(簡易計算方式)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = カレンダー当月開始位置INDEX(Long)
'* 　　　　　Arg3 = 祝日パラメータテーブルINDEX(Long)
'* 　　　　　Arg4 = 振替休日判定日INDEXテーブルINDEX上限(Long)      ※Ref参照
'* 　　　　　Arg5 = 振替休日判定日INDEXテーブル(Array:Structure)    ※Ref参照
'* 　　　　　Arg6 = カレンダーテーブル(Array:Structure)             ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：本計算方法は1948～2150年まで算出可能
'* 注意事項：本処理内では振替休日判定は行なわない
'***************************************************************************************************
Private Sub GP_GetSyunbun(ByVal lngY As Long, _
                          ByVal lngCalStrIx As Long, _
                          ByVal lngIxSyuku As Long, _
                          ByRef lngFuriDIxMax As Long, _
                          ByRef tblFuriDIx() As g_typFuriDIx, _
                          ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngD As Long                                                ' 算出日
    Dim lngY2 As Long                                               ' 年算出WORK
    ' 祝日法施行(1947年)以前,2151年以降(簡易計算不可)は無視
    lngY2 = lngY - 1980
    Select Case lngY
        Case Is <= 1979
            lngD = Int(20.8357 + (0.242194 * lngY2) - Int(lngY2 / 4))
        Case Is <= 2099
            lngD = Int(20.8431 + (0.242194 * lngY2) - Int(lngY2 / 4))
        Case Else
            lngD = Int(21.851 + (0.242194 * lngY2) - Int(lngY2 / 4))
    End Select
    ' 祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
    Call GP_UpdateHolidaySUB1(lngD, _
                              "春分の日", _
                              1, _
                              0, _
                              lngCalStrIx, _
                              lngIxSyuku, _
                              lngFuriDIxMax, _
                              tblFuriDIx, _
                              tblCalendar)
End Sub

'***************************************************************************************************
'* 処理名　：GP_GetSyuubun
'* 機能　　：秋分の日の算出(簡易計算方式)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 年(Long)
'* 　　　　　Arg2 = カレンダー当月開始位置INDEX(Long)
'* 　　　　　Arg3 = 祝日パラメータテーブルINDEX(Long)
'* 　　　　　Arg4 = 振替休日判定日INDEXテーブルINDEX上限(Long)      ※Ref参照
'* 　　　　　Arg5 = 振替休日判定日INDEXテーブル(Array:Structure)    ※Ref参照
'* 　　　　　Arg6 = カレンダーテーブル(Array:Structure)             ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：本計算方法は1948～2150年まで算出可能
'* 注意事項：本処理内では振替休日判定は行なわない
'***************************************************************************************************
Private Sub GP_GetSyuubun(ByVal lngY As Long, _
                          ByVal lngCalStrIx As Long, _
                          ByVal lngIxSyuku As Long, _
                          ByRef lngFuriDIxMax As Long, _
                          ByRef tblFuriDIx() As g_typFuriDIx, _
                          ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngD As Long                                                ' 算出日
    Dim lngY2 As Long                                               ' 年算出WORK
    ' 祝日法施行(1947年)以前,2151年以降(簡易計算不可)は無視
    lngY2 = lngY - 1980
    Select Case lngY
        Case Is <= 1979
            lngD = Int(23.2588 + (0.242194 * lngY2) - Int(lngY2 / 4))
        Case Is <= 2099
            lngD = Int(23.2488 + (0.242194 * lngY2) - Int(lngY2 / 4))
        Case Else
            lngD = Int(24.2488 + (0.242194 * lngY2) - Int(lngY2 / 4))
    End Select
    ' 祝日情報のカレンダーテーブル更新(サブ処理：日付で判定)
    Call GP_UpdateHolidaySUB1(lngD, _
                              "秋分の日", _
                              1, _
                              0, _
                              lngCalStrIx, _
                              lngIxSyuku, _
                              lngFuriDIxMax, _
                              tblFuriDIx, _
                              tblCalendar)
End Sub

'***************************************************************************************************
'* 処理名　：GP_UpdateFuriKokuminKyu
'* 機能　　：振替休日・国民の休日設定
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 振替休日判定日INDEXテーブル(Array:Structure)
'* 　　　　　Arg2 = カレンダーテーブル(Array:Structure)         ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Sub GP_UpdateFuriKokuminKyu(tblFuriDIx() As g_typFuriDIx, _
                                    ByRef tblCalendar() As g_typAboutCalendar2)
    '-----------------------------------------------------------------------------------------------
    Dim lngIxF As Long                                              ' 振替休日判定日テーブルINDEX
    Dim lngIxC As Long                                              ' カレンダーテーブルINDEX
    Dim lngIxFMax As Long                                           ' 振替休日判定日テーブルINDEX上限
    Dim lngIxCMax As Long                                           ' カレンダーテーブルINDEX上限
    lngIxFMax = UBound(tblFuriDIx)
    lngIxCMax = UBound(tblCalendar)
    '-----------------------------------------------------------------------------------------------
    Do While lngIxF <= lngIxFMax
        ' 翌日のカレンダー位置を算出
        lngIxC = tblFuriDIx(lngIxF).CalIx + 1
        ' 振替休日か
        If tblFuriDIx(lngIxF).Syubetsu = 0 Then
            ' 翌日以降の最初の平日(又は会社休日)を｢振替休日｣とする
            Do While lngIxC <= lngIxCMax
                ' 祝日以外(又は会社休日)を発見
                If ((tblCalendar(lngIxC).Syuku = 0) Or (tblCalendar(lngIxC).Syuku = 3)) Then
                    ' 振替休日とする(会社休日は振替休日に置き換える)
                    tblCalendar(lngIxC).Syuku = 2
                    tblCalendar(lngIxC).SyukuNm = g_cnsFuri         ' 振替休日
                    Exit Do
                ElseIf tblCalendar(lngIxC).SyukuNm = g_cnsKyu2 Then
                    ' 既に国民の休日がある場合は処置中止(国民の休日のままにする)
                    Exit Do
                End If
                ' 次の日へ
                lngIxC = lngIxC + 1
            Loop
        Else    ' 国民の休日
            ' 翌日を｢国民の休日｣とする
            tblCalendar(lngIxC).Syuku = 1
            tblCalendar(lngIxC).SyukuNm = g_cnsKyu2                 ' 国民の休日
        End If
        ' 次へ
        lngIxF = lngIxF + 1
    Loop
End Sub

'***************************************************************************************************
'* 処理名　：FP_GetHoliParameter
'* 機能　　：祝日パラメータテーブルの取得
'---------------------------------------------------------------------------------------------------
'* 返り値　：処理成否(Boolean)
'* 引数　　：Arg1 = チェック工程スキップ(Boolean)               ※Option
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：｢祝日パラメータ｣シートの設定に不備がある場合はエラーを表示して処理を打ち切ります
'***************************************************************************************************
Private Function FP_GetHoliParameter(Optional ByVal blnOmitCheck As Boolean = False) As Boolean
    '-----------------------------------------------------------------------------------------------
    Dim lngIx As Long                                               ' テーブルINDEX
    Dim lngRow As Long                                              ' 行INDEX
    Dim lngRowMax As Long                                           ' 行INDEX上限
    Dim lngCurrM As Long                                            ' 現在月
    Dim lngPrevM As Long                                            ' 直前月
    Dim objParaSheet As Worksheet                                   ' 祝日パラメータシート
    FP_GetHoliParameter = False
    g_blnParaReaded = False
    g_blnParaChecked = False
    '-----------------------------------------------------------------------------------------------
    On Error Resume Next
    Set objParaSheet = ThisWorkbook.Worksheets(g_cnsParaSheet)
    ' 参照失敗
    If Err.Number <> 0 Then
        MsgBox "「" & g_cnsParaSheet & "」シートが参照できません。", vbCritical, g_cnsTitle
        On Error GoTo 0
        Exit Function
    End If
    On Error GoTo 0
    '-----------------------------------------------------------------------------------------------
    If Not g_blnParaChecked And Not blnOmitCheck Then
        '★↓↓↓2022/11/16 祝日パラメータチェックをスキップ by ATTS↓↓↓★
        ' 祝日パラメータシートチェック
'        If Not FP_CheckSyukuParameter(objParaSheet, lngRowMax) Then Exit Function
        lngRowMax = 0
        '★↑↑↑2022/11/16 祝日パラメータチェックをスキップ by ATTS↑↑↑★
    Else
        ' 最終行を取得
        With objParaSheet
            ' 最終行取得
            lngRowMax = .Range("$A$" & .Rows.Count).End(xlUp).row
        End With
    End If
    '★↓↓↓2022/11/16 祝日パラメータチェックをスキップ by ATTS↓↓↓★
    If lngRowMax > 3 Then
        ReDim g_tblParamater(lngRowMax - 3)
    End If
    '★↑↑↑2022/11/16 祝日パラメータチェックをスキップ by ATTS↑↑↑★
    '-----------------------------------------------------------------------------------------------
    ' 祝日パラ月別範囲テーブルを一旦初期化
    For lngIx = 1 To 12
        With g_tblParamMMPos(lngIx)
            .StrIx = -1
            .EndIx = -1
        End With
    Next lngIx
    lngIx = 0
    lngRow = 3
    '-----------------------------------------------------------------------------------------------
    ' 祝日パラメータから内容をモジュールレベルテーブルに格納する
    With objParaSheet                                                 ' ｢祝日パラメータ｣シート
        lngCurrM = .Cells(lngRow, 1).Value
        ' 祝日パラメータの各行をループ
        Do While lngRow <= lngRowMax
            '-----------------------------------------------
            ' 月単位[前]処理
            lngPrevM = lngCurrM
            ' 月開始INDEXを祝日パラ月別範囲テーブルにセット
            g_tblParamMMPos(lngPrevM).StrIx = lngIx
            '-----------------------------------------------
            ' 月単位[主]処理
            Do While lngCurrM = lngPrevM
                With g_tblParamater(lngIx)
                    .Getsu = lngPrevM                               ' 月
                    .SyoriKbn = FP_GetNumber(objParaSheet.Cells(lngRow, 2))     ' 処理区分
                    .Hiduke = FP_GetNumber(objParaSheet.Cells(lngRow, 3))       ' [固定日]日
                    .FurikaeKbn = FP_GetNumber(objParaSheet.Cells(lngRow, 4))   ' [固定日]振替区分
                    .HmSyusu = FP_GetNumber(objParaSheet.Cells(lngRow, 5))      ' [HM]週数
                    .HmYobi = FP_GetNumber(objParaSheet.Cells(lngRow, 6))       ' [HM]曜日
                    .SyukuNm = Trim(objParaSheet.Cells(lngRow, 7).Value)        ' 祝日名
                    .StrYear = FP_GetNumber(objParaSheet.Cells(lngRow, 8))      ' 開始年
                    .EndYear = FP_GetNumber(objParaSheet.Cells(lngRow, 9))      ' 終了年
                End With
                ' 次へ
                lngIx = lngIx + 1
                lngRow = lngRow + 1
                ' 次の月をセット
                If lngRow <= lngRowMax Then
                    lngCurrM = .Cells(lngRow, 1).Value
                Else
                    lngCurrM = 99
                End If
            Loop
            '---0--------------------------------------------
            ' 月単位[後]処理
            ' 月終了INDEXを祝日パラ月別範囲テーブルにセット
            g_tblParamMMPos(lngPrevM).EndIx = lngIx - 1
        Loop
    End With
    g_blnParaReaded = True
    g_blnParaChecked = True
    FP_GetHoliParameter = True
End Function

'***************************************************************************************************
'* 処理名　：GP_CheckSyukuParameterSUB
'* 機能　　：祝日パラメータテーブルチェック(行あたり)
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 祝日パラメータシート(Object)                ※Ref参照
'* 　　　　　Arg2 = 現在行(Long)
'* 　　　　　Arg3 = 直前行の月(Long)                            ※Ref参照
'* 　　　　　Arg4 = 累積メッセージ(String)                      ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Sub GP_CheckSyukuParameterSUB(ByRef objParaSheet As Worksheet, _
                                      ByVal lngRow As Long, _
                                      ByRef lngPrevM As Long, _
                                      ByRef strMSG As String)
    '-----------------------------------------------------------------------------------------------
    Const cnsError01 As String = "」が入力されていません。"
    Const cnsError02 As String = "」が数値ではありません。"
    Const cnsError03 As String = "」の値が有効範囲外です。"
    Const cnsError04 As String = "」は入力不要です。"
    Dim blnRowErr As Boolean                                        ' 行あたりエラー有無
    Dim lngValue As Long                                            ' 数値WORK
    Dim lngGatsu As Long                                            ' 月
    Dim lngSyoriKbn As Long                                         ' 処理区分
    Dim lngStrYear As Long                                          ' 開始年
    Dim lngEndYear As Long                                          ' 開始年
    Dim strRow As String                                            ' メッセージ補助
    strRow = CStr(lngRow) & "行目："
    With objParaSheet
        ' 月
        If Not FP_CheckNumber(.Cells(lngRow, 1), lngGatsu) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「月" & cnsError02, strMSG)
        ElseIf ((lngGatsu < 1) Or (lngGatsu > 12)) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「月" & cnsError03, strMSG)
        ElseIf lngGatsu < lngPrevM Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「月」が昇順ではありません。", strMSG)
        Else
            lngPrevM = lngGatsu
        End If
        ' 処理区分
        If Not FP_CheckNumber(.Cells(lngRow, 2), lngSyoriKbn) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「処理区分" & cnsError02, strMSG)
        ElseIf (((lngSyoriKbn < 0) Or (lngSyoriKbn > 2)) And (lngSyoriKbn <> 9)) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「処理区分" & cnsError03, strMSG)
        End If
        ' エラーがなければ処理区分による判断
        If Not blnRowErr Then
            ' ｢固定値｣か
            If ((lngSyoriKbn = 0) Or (lngSyoriKbn = 2)) Then    ' ｢固定値｣
                ' 日
                If Not FP_CheckNumber(.Cells(lngRow, 3), lngValue) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError02, strMSG)
                ElseIf lngValue < 1 Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError03, strMSG)
                Else
                    ' 月による最終日チェック
                    Select Case lngGatsu
                        Case 2
                            If lngValue > 28 Then
                                blnRowErr = True
                                Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError03, strMSG)
                            End If
                        Case 4, 6, 9, 11
                            If lngValue > 30 Then
                                blnRowErr = True
                                Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError03, strMSG)
                            End If
                        Case Else
                            If lngValue > 31 Then
                                blnRowErr = True
                                Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError03, strMSG)
                            End If
                    End Select
                End If
                ' 振替
                If Not FP_CheckNumber(.Cells(lngRow, 4), lngValue) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]振替" & cnsError02, strMSG)
                ElseIf ((lngValue < 0) Or (lngValue > 1)) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]振替" & cnsError03, strMSG)
                End If
                ' HM項目(入力不要)
                If .Cells(lngRow, 5).Value <> "" Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]週数" & cnsError04, strMSG)
                End If
                If .Cells(lngRow, 6).Value <> "" Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]曜日" & cnsError04, strMSG)
                End If
            ElseIf lngSyoriKbn = 1 Then                         ' ｢HM｣
                ' 週数(現状は2又は3であるが本来の有効範囲でチェック)
                If Not FP_CheckNumber(.Cells(lngRow, 5), lngValue) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]週数" & cnsError02, strMSG)
                ElseIf ((lngValue < 1) Or (lngValue > 5)) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]週数" & cnsError03, strMSG)
                End If
                ' 曜日(現状は1のみであるが本来の有効範囲でチェック)
                If Not FP_CheckNumber(.Cells(lngRow, 6), lngValue) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]曜日" & cnsError02, strMSG)
                ElseIf ((lngValue < 0) Or (lngValue > 6)) Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[HM]曜日" & cnsError03, strMSG)
                End If
                ' 固定値項目(入力不要)
                If .Cells(lngRow, 3).Value <> "" Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]日" & cnsError04, strMSG)
                End If
                If .Cells(lngRow, 4).Value <> "" Then
                    blnRowErr = True
                    Call GP_AppendMessage(strRow & "「[固定値]振替" & cnsError04, strMSG)
                End If
            End If
        End If
        ' 祝日名
        If Trim(.Cells(lngRow, 7).Value) = "" Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「祝日名" & cnsError01, strMSG)
        End If
        ' 開始年
        If Not FP_CheckNumber(.Cells(lngRow, 8), lngStrYear) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「開始年" & cnsError02, strMSG)
        ElseIf ((lngStrYear < 0) Or (lngStrYear > (Year(Date) + 3))) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「開始年" & cnsError03, strMSG)
        End If
        ' 終了年
        If Not FP_CheckNumber(.Cells(lngRow, 9), lngEndYear) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「終了年" & cnsError02, strMSG)
        ElseIf ((lngEndYear < 1948) Or (lngEndYear > 9999)) Then
            blnRowErr = True
            Call GP_AppendMessage(strRow & "「終了年" & cnsError03, strMSG)
        End If
        ' 開始年・終了年逆転チェック
        If Not blnRowErr Then
            ' 逆転か
            If lngEndYear < lngStrYear Then
                blnRowErr = True
                Call GP_AppendMessage(strRow & "「有効年期間」が逆転しています。", strMSG)
            End If
        End If
    End With
End Sub

'***************************************************************************************************
'* 処理名　：FP_GetNumber
'* 機能　　：数値取り出し
'---------------------------------------------------------------------------------------------------
'* 返り値　：数値(Long)
'* 引数　　：Arg1 = 対象セル(Range)
'---------------------------------------------------------------------------------------------------
'* 機能説明：念のため､一旦文字列で受け取ってから数値に変換する(ブランクセルはゼロを充当)
'* 注意事項：
'***************************************************************************************************
Private Function FP_GetNumber(ByRef objRange As Range) As Long
    '-----------------------------------------------------------------------------------------------
    Dim strText1 As String                                          ' 文字列WORK
    Dim strText2 As String                                          ' 文字列WORK
    strText1 = objRange.Value
    strText2 = Trim(strText1)
    ' ブランクセルはゼロを充当
    If strText2 = "" Then strText2 = "0"
    FP_GetNumber = CLng(strText2)
End Function

'***************************************************************************************************
'* 処理名　：FP_CheckNumber
'* 機能　　：数値チェック(整数)
'---------------------------------------------------------------------------------------------------
'* 返り値　：チェック成否(Boolean)
'* 引数　　：Arg1 = 対象セル(Range)
'* 　　　　　Arg2 = 数値(Long)                                  ※Ref参照(Option)
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Function FP_CheckNumber(ByRef objRange As Range, _
                                Optional ByRef lngValue As Long = -1) As Boolean
    '-----------------------------------------------------------------------------------------------
    Dim strText1 As String                                          ' 文字列WORK
    Dim strText2 As String                                          ' 文字列WORK
    strText1 = CStr(objRange.Value)
    strText2 = Trim(strText1)
    ' ブランクはゼロとする
    If strText2 = "" Then
        lngValue = 0
        FP_CheckNumber = True
        Exit Function
    End If
    ' 数値チェック(整数転記エラー判定で)
    On Error Resume Next
    lngValue = CLng(strText2)
    ' チェック結果判定
    If Err.Number = 0 Then
        FP_CheckNumber = True
    Else
        FP_CheckNumber = False
    End If
    On Error GoTo 0
End Function

'***************************************************************************************************
'* 処理名　：GP_AppendMessage
'* 機能　　：エラーメッセージ累積
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：Arg1 = 今回メッセージ(String)
'* 　　　　　Arg2 = 累積メッセージ(String)                      ※Ref参照
'---------------------------------------------------------------------------------------------------
'* 機能説明：改行を付加して追加
'* 注意事項：
'***************************************************************************************************
Private Sub GP_AppendMessage(ByVal strAddMSG As String, ByRef strMSG As String)
    '-----------------------------------------------------------------------------------------------
    If strMSG <> "" Then strMSG = strMSG & vbCrLf
    strMSG = strMSG & strAddMSG
End Sub

'----------------------------------------<< End of Source >>----------------------------------------


