VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "ThisWorkbook"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
'***************************************************************************************************
'   カレンダーフォーム4(日付入力部品)                       ※ThisWorkbookイベント(Class)
'***************************************************************************************************
Option Explicit

'***************************************************************************************************
'   ■■■ ワークブックイベント ■■■
'***************************************************************************************************
'* 処理名　：Workbook_Open
'* 機能　　：開くイベント
'---------------------------------------------------------------------------------------------------
'* 返り値　：(なし)
'* 引数　　：(既定)
'---------------------------------------------------------------------------------------------------
'* 機能説明：
'* 注意事項：
'***************************************************************************************************
Private Sub Workbook_Open()
    '-----------------------------------------------------------------------------------------------
    ' カレンダー関連関数クラス初期化(祝日パラメータシートがチェック済なら引数Trueを追加) ※初回
    If modAboutCalendar2.FP_InitAboutCalendar Then
        ' カレンダーフォームの表示シート
        With Worksheets(1)
            '2023/01/17 ATTS Mod Start
            '.Protect UserInterfaceOnly:=True
            'シート保護
            '＜許可するもの＞
            '  ・マクロによる操作
            '  ・ロックされたセル範囲の選択
            '  ・ロックされていないセル範囲の選択
            '  ・セルの書式設定
            '  ・列の書式設定
            '  ・オートフィルターの使用
            '  ・オブジェクトの編集
            .Protect UserInterfaceOnly:=True, DrawingObjects:=False, Contents:=True, _
                Scenarios:=True, AllowFormattingCells:=True, AllowFormattingColumns:=True, _
                AllowFiltering:=True
            '2023/01/17 ATTS Mod End
        End With
    End If
    
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    '対象シート番号を取得
    Dim targetSheetNo  As Integer
    targetSheetNo = defineWs.Range(Define_Target_SheetNo).Value
    If targetSheetNo = SheetNo_Maintenance Then
        ' 保全活動シートの場合
        Call Worksheets(1).ChangeColumnDisplay
    ElseIf targetSheetNo = SheetNo_HistoryFailure Then
        ' 保全活動_故障情報シートの場合
        Call Worksheets(1).ChangeColumnDisplay
    End If

    'アクティブシートのインデックス取得
    Dim activeIndex As Integer
    activeIndex = ActiveSheet.Index
    
    '一番左のシートを表示
    Sheets(1).Select
      
    'アクティブセル列の1行目にフォーカスを当てる
    Cells(1, ActiveCell.Column).Select
    
    'ブックを開いたときにアクティブだったシートを表示
    Sheets(activeIndex).Select

    ' 保存済みにする
    ThisWorkbook.Saved = True
End Sub

'----------------------------------------<< End of Source >>----------------------------------------


