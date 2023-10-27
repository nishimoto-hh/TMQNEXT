Attribute VB_Name = "modCommon"
Option Explicit
 
#If VBA7 And Win64 Then
Private Declare PtrSafe Function OpenClipboard Lib "user32" (ByVal hwnd As LongPtr) As Long
Private Declare PtrSafe Function GetClipboardData Lib "user32" (ByVal wFormat As Long) As LongPtr
Private Declare PtrSafe Function RegisterClipboardFormat Lib "user32" Alias "RegisterClipboardFormatA" (ByVal lpString As String) As Long
Private Declare PtrSafe Function CloseClipboard Lib "user32" () As Long
Private Declare PtrSafe Function GlobalSize Lib "kernel32" (ByVal hMem As LongPtr) As LongPtr
Private Declare PtrSafe Function GlobalLock Lib "kernel32" (ByVal hMem As LongPtr) As LongPtr
Private Declare PtrSafe Function MoveMemory Lib "kernel32.dll" Alias "RtlMoveMemory" (ByRef Destination As Any, ByRef Source As Any, ByVal Length As LongPtr) As LongPtr
Private Declare PtrSafe Function GlobalUnlock Lib "kernel32" (ByVal hMem As LongPtr) As Long
#Else
Private Declare Function OpenClipboard Lib "user32" (ByVal hwnd As Long) As Long
Private Declare Function GetClipboardData Lib "user32" (ByVal wFormat As Long) As Long
Private Declare Function RegisterClipboardFormat Lib "user32" Alias "RegisterClipboardFormatA" (ByVal lpString As String) As Long
Private Declare Function CloseClipboard Lib "user32" () As Long
Private Declare Function GlobalSize Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Sub MoveMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal Destination As Long, ByVal Source As Long, ByVal Length As Long)
Private Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Long) As Long
#End If
 
'***************************************
' 定数定義
'***************************************

' シート名-レイアウト定義シート
Public Const SheetName_Define As String = "Sheet_Define"
' シート名-選択アイテムシート
Public Const SheetName_Item As String = "Sheet_Item"
' シート名-メッセージ翻訳シート
Public Const SheetName_Message As String = "Sheet_Message"
' シート名-エラー情報
Public Const SheetName_ErrorInfoEn As String = "Sheet_Error"
Public Const SheetName_ErrorInfo   As String = "エラー情報"
' シート名-選択リスト
Public Const SheetName_SelectList As String = "Sheet_Form"
' シート番号-入力シート
Public Const SheetNo_Input As Integer = 1

' 送信時処理ID-登録
Public Const SendId_Insert As String = "1"
' 送信時処理ID-内容更新
Public Const SendId_Update As String = "2"
'★2023/03/01
' 送信時処理ID-削除
Public Const SendId_Delete As String = "9"

' 送信時処理ID-エラー(登録・内容更新・削除でない場合　入力チェックで使用)
Public Const SendId_Error As String = "-1"

' 入力シート行番号-データ開始行
Public Const Input_RowNo_Start As Long = 4

' セル種類-なし
Public Const CellType_None As String = "0"
' セル種類-文字列（改行なし）
Public Const CellType_Text As String = "1"
' セル種類-数値
Public Const CellType_Number As String = "2"
' セル種類-年月日
Public Const CellType_Date As String = "3"
' セル種類-時刻
Public Const CellType_Time As String = "4"
' セル種類-コンボボックス
Public Const CellType_ComboBox As String = "5"
' セル種類-複数選択リストボックス
Public Const CellType_MultiListBox As String = "6"
' セル種類-チェックボックス
Public Const CellType_CheckBox As String = "7"
' セル種類-選択リスト画面
Public Const CellType_FormList As String = "8"
' セル種類-文字列（改行あり）
Public Const CellType_Text_NewLine As String = "9"

' 列区分-KEY
Public Const ColumnDivision_Key As String = "1"
' 列区分-送信時処理ID
Public Const ColumnDivision_SnedId As String = "2"
' 列区分-エラー有無
Public Const ColumnDivision_ErrorUmu As String = "3"
' 列区分-工場ID
Public Const ColumnDivision_FactoryId As String = "4"

' 定義シート列番号-シート番号
Public Const Define_ColNo_SheetNo As Long = 1
' 定義シート列番号-対象列番号
Public Const Define_ColNo_ColNo As Long = 2
' 定義シート列番号-シート番号_列番号
Public Const Define_ColNo_SheetNoAndColNo As Long = 3
' 定義シート列番号-列種類
Public Const Define_ColNo_Type As Long = 4
' 定義シート列番号-列区分
Public Const Define_ColNo_ColumnDivision As Long = 5
' 定義シート列番号-ヘッダ表示名
Public Const Define_ColNo_Name As Long = 6
' 定義シート列番号-選択ID値格納先列番号
Public Const Define_ColNo_Val As Long = 7
' 定義シート列番号-連動元列番号
Public Const Define_ColNo_Link As Long = 8
' 定義シート列番号-自動表示拡張列番号
Public Const Define_ColNo_AutoNo As Long = 9
' 定義シート列番号-選択項目グループID
Public Const Define_ColNo_GrpId As Long = 10
' 定義シート列番号-必須項目フラグ
Public Const Define_Hissu_Flg As Long = 11
' 定義シート列番号-最大桁数
Public Const Define_Max_Length As Long = 12
' 定義シート列番号-最小値
Public Const Define_Min_Value As Long = 13
' 定義シート列番号-最大値
Public Const Define_Max_Value As Long = 14
' 定義シート列番号-書式
Public Const Define_Format As Long = 15
' 定義シート-項目数
Public Const Define_Max As Long = 15
' 定義シート-対象シート番号
Public Const Define_Target_SheetNo As String = "O4"

' 定義シート行番号-データ開始行
Public Const Define_RowNo_Start As Long = 6

' 選択アイテムシート列番号-選択項目グループID
Public Const Item_ColNo_GrpId As Long = 1
' 選択アイテムシート列番号-ID
Public Const Item_ColNo_Id As Long = 2
' 選択アイテムシート列番号-親ID
Public Const Item_ColNo_Parent As Long = 3
' 選択アイテムシート列番号-表示文字列（名称）
Public Const Item_ColNo_Text As Long = 4
' 選択アイテムシート列番号-工場ID
Public Const Item_ColNo_FactoryId As Long = 5
' 選択アイテムシート列番号-拡張項目1
Public Const Item_ColNo_Kakucho01 As Long = 6
' 選択アイテムシート列番号-拡張項目2
Public Const Item_ColNo_Kakucho02 As Long = 7
' 選択アイテムシート列番号-拡張項目3
Public Const Item_ColNo_Kakucho03 As Long = 8
' 選択アイテムシート列番号-拡張項目4
Public Const Item_ColNo_Kakucho04 As Long = 9
' 選択アイテムシート列番号-拡張項目5
Public Const Item_ColNo_Kakucho05 As Long = 10
' 選択アイテムシート列番号-拡張項目6
Public Const Item_ColNo_Kakucho06 As Long = 11
' 選択アイテムシート列番号-拡張項目7
Public Const Item_ColNo_Kakucho07 As Long = 12
' 選択アイテムシート列番号-拡張項目8
Public Const Item_ColNo_Kakucho08 As Long = 13
' 選択アイテムシート列番号-拡張項目9
Public Const Item_ColNo_Kakucho09 As Long = 14
' 選択アイテムシート列番号-拡張項目10
Public Const Item_ColNo_Kakucho10 As Long = 15
' 選択アイテムシート列番号-拡張項目11
Public Const Item_ColNo_Kakucho11 As Long = 16
' 選択アイテムシート列番号-拡張項目12
Public Const Item_ColNo_Kakucho12 As Long = 17
' 選択アイテムシート列番号-拡張項目13
Public Const Item_ColNo_Kakucho13 As Long = 18
' 選択アイテムシート列番号-拡張項目14
Public Const Item_ColNo_Kakucho14 As Long = 19
' 選択アイテムシート列番号-拡張項目15
Public Const Item_ColNo_Kakucho15 As Long = 20
' 選択アイテムシート列番号-拡張項目16
Public Const Item_ColNo_Kakucho16 As Long = 21
' 選択アイテムシート列番号-拡張項目17
Public Const Item_ColNo_Kakucho17 As Long = 22
' 選択アイテムシート列番号-拡張項目18
Public Const Item_ColNo_Kakucho18 As Long = 23
' 選択アイテムシート列番号-拡張項目19
Public Const Item_ColNo_Kakucho19 As Long = 24
' 選択アイテムシート列番号-拡張項目20
Public Const Item_ColNo_Kakucho20 As Long = 25
' 選択アイテムシート列番号-標準アイテム未使用工場ID
Public Const Item_ColNo_UnuseFactoryId As Long = 26

' 選択アイテムシート行番号-データ開始行
Public Const Item_RowNo_Start As Long = 3
' 選択アイテムシート-親ID-標準
Public Const Item_ColNo_Parent_Default As Long = 0

' エラー情報
' 入力チェック-エラー有無-文言(NG時に表示）
Public Error_Umu_Ari As String
' エラー情報-シート名
Public Const ErrorInfo_SheetNo As Long = 1
' エラー情報-行
Public Const ErrorInfo_Row As Long = 2
' エラー情報-列
Public Const ErrorInfo_Col As Long = 3
' エラー情報-処理区分
Public Const ErrorInfo_Kubun As Long = 4
' エラー情報-エラー情報
Public Const ErrorInfo_Info As Long = 5

' エラー情報行番号-データ開始行
Public Const ErrorInfo_RowNo_Start As Long = 3
' エラー情報-エラー背景色（薄い赤）
Enum BackColorNg
    Red = 255
    Green = 153
    Blue = 153
End Enum

' エラー情報-出力用構造体
Type typeErrorInfo
    sOutName As String    '出力　：シート名
    lOutRow As Long       '出力　：行
    lOutCol As Long       '出力　：列
    sErrName As String    'エラー：シート名
    lErrRow As Long       'エラー：行
    lErrCol As Long       'エラー：列
    sErrRange As String   'エラー：レンジ(String)
    sErrColName As String 'エラー：ヘッダ表示名
    sErrKubun As String   '処理区分
    sErrMsg As String     'メッセージ
End Type

' 複数選択リストボックス-ID区切り文字
Public Const DelimiterId As String = "|"
' 複数選択リストボックス-名称区切り文字
Public Const DelimiterName As String = ","
' 保護パスワード
Public Const ProtectPassword As String = "aec"

'グローバル変数
Public G_SendIdData() As Variant    '定義情報シートの送信時処理ID情報（送信時処理の件数分）
Public G_ErrorUmuData() As Variant  '定義情報シートのエラー有無情報（送信時処理の件数分）
Public G_FactoryIdData() As Variant '定義情報シートの工場ID情報（送信時処理の件数分）
Public G_SyoriIdCnt As Integer      '定義情報シートの送信時処理ID件数
Public G_ErrorUmuCnt As Integer     '定義情報シートのエラー有無件数
Public G_FactoryIdCnt As Integer    '定義情報シートの工場ID件数

' シート番号-長期計画_対象機器別管理基準
Public Const SheetNo_ChokeiKanri As Integer = 4
' シート番号-保全活動
Public Const SheetNo_Maintenance As Integer = 5
' シート番号-保全活動_故障情報
Public Const SheetNo_HistoryFailure As Integer = 6
' シート番号-マスタアイテム並び順設定
Public Const SheetNo_MasterItemSort As Integer = 14
' シート番号-場所階層
Public Const SheetNo_Bashokaiso As Integer = 20
' シート番号-部門（工場・部門）
Public Const SheetNo_Bumon As Integer = 63
' シート番号-予備ロケーション
Public Const SheetNo_YobiLocation As Integer = 62
' シート番号-職種・機種
Public Const SheetNo_ShokushuKishu As Integer = 56

' Double最大値
Public Const G_DoubleMax As Double = (2 ^ 53) - 1
' Double最小値
Public Const G_DoubleMin As Double = 1 - (2 ^ 53) - 1

'■長期計画_対象機器別管理基準
'長期計画_対象機器別管理基準：スケジュールの選択項目グループID
Public Const ChokeiKanri_Schedule_GrpId As Integer = 2
'長期計画_対象機器別管理基準：スケジュール文字列の使用ID（〇は使用）
Public Const ChokeiKanri_Schedule_OnId As String = "1"  'フィルターで使用する為String型

' 標準ID
Public Const G_DefaultID As String = "0"

' 工場ID(コンボボックス選択時にレコードの工場IDが設定される、拡張項目設定時に使用)
Public G_FactoryId As String

'送信時処理「登録」
Public Const SendName_Insert = "登録"

'送信時処理「内容更新」
Public Const SendName_Update = "内容更新"

'送信時処理「削除」
Public Const SendName_Delete = "削除"


'***************************************
' 定義値の取得
'***************************************
Public Function GetDefineVal(ByVal SheetNo As Integer _
                           , ByVal Target As Range _
                           , ByVal ColNo As Long) As String
On Error GoTo ErrHandler
    GetDefineVal = ""
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    
    If Target Is Nothing Then
        Exit Function
    End If
    
    If Target.row < Input_RowNo_Start Then
        Exit Function
    End If
    
    Dim targetRange As Range
    If Target.Count > 1 Then
        Set targetRange = Target(1)
    Else
        Set targetRange = Target
    End If
    
    With defineWs.Range("A1")
        'VLockup関数のエラーを無視する
        On Error Resume Next
        
        'シート番号_列番号で検索
        Dim key As String
        Dim targetCell As Range
        key = SheetNo & "_" & Target.Column
        Dim lastRow  As Long
        lastRow = defineWs.Cells(defineWs.Rows.Count, Define_ColNo_ColNo).End(xlUp).row
        
        GetDefineVal = WorksheetFunction.VLookup(key, _
            defineWs.Range(defineWs.Cells(Define_RowNo_Start, Define_ColNo_SheetNoAndColNo), _
            defineWs.Cells(lastRow, ColNo)), _
            ColNo - Define_ColNo_SheetNoAndColNo + 1, False)

        'これ以降エラーを表示する
        On Error GoTo 0
        
    End With
        
    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("(GetDefineVal)" & Err.Number & ":" & Err.Description)
    GetDefineVal = ""
End Function

'***************************************
' 選択項目グループIDの取得
'***************************************
Public Function GetGrpId(ByVal SheetNo As Integer _
                       , ByVal Target As Range) As Integer
On Error GoTo ErrHandler
    GetGrpId = 0
    Dim grpId As String
    grpId = GetDefineVal(SheetNo, Target, Define_ColNo_GrpId)
    
    If grpId <> "" Then
        GetGrpId = CInt(grpId)
    End If

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetGrpId()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' セル種類の取得
'***************************************
Public Function GetCellType(ByVal SheetNo As Integer _
                          , ByVal Target As Range) As String
On Error GoTo ErrHandler
    GetCellType = CellType_None
    Dim cellType As String
    
    GetCellType = True
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        GetCellType = False
        Exit Function
    End If

    cellType = GetDefineVal(SheetNo, Target, Define_ColNo_Type)
    
    If cellType <> "" Then
        GetCellType = cellType
    End If

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetCellType()" & Err.Number & ":" & Err.Description)
    GetCellType = ""
End Function

'***************************************
' 選択ID値格納先列番号の取得
'***************************************
Public Function GetValColNo(ByVal SheetNo As Integer _
                          , ByVal Target As Range) As Long
On Error GoTo ErrHandler
    GetValColNo = -1
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Function
    End If

    Dim ColNo As String
    ColNo = GetDefineVal(SheetNo, Target, Define_ColNo_Val)
    
    If ColNo <> "" Then
        GetValColNo = CLng(ColNo)
    End If


    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetValColNo()" & Err.Number & ":" & Err.Description)
    GetValColNo = -1
End Function

'***************************************
' 連動元列番号の取得
'***************************************
Public Function GetLinkColNo(ByVal SheetNo As Integer _
                           , ByVal Target As Range) As Long
On Error GoTo ErrHandler
    GetLinkColNo = -1
    Dim ColNo As String
    ColNo = GetDefineVal(SheetNo, Target, Define_ColNo_Link)
    
    If ColNo <> "" Then
        GetLinkColNo = CLng(ColNo)
    End If

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetLinkColNo()" & Err.Number & ":" & Err.Description)
    GetLinkColNo = -1
End Function

'***************************************
' コンボボックスの表示
'***************************************
Public Sub ShowComboBox(ByVal SheetNo As Integer _
                      , ByVal Target As Range _
                      , ByVal ComboBox As MSForms.ComboBox _
                      , ByVal iType As Integer _
                      , Optional sCellType As String = "")
On Error GoTo ErrHandler

    '定義情報を取得
    Dim valColNo As Long
    Dim grpId As Long
    Dim linkColNo As Long
    Dim FactoryIdNo As Long
    Dim iIdx As Long
    Dim lastRow As Long
    Dim i As Long

    '選択ID値格納先列番号
    valColNo = GetValColNo(SheetNo, Target)
    If valColNo <= 0 Then
        '選択ID値格納先列番号なし
        GoTo CONTINUE2:
    End If
    
    '選択項目グループID
    grpId = GetGrpId(SheetNo, Target)
    '連動元列番号
    linkColNo = GetLinkColNo(SheetNo, Target)
    
    '表示前の設定値を保持
    Dim prevValue As String
    prevValue = Worksheets(SheetNo_Input).Cells(Target.row, valColNo)
    
    Dim linkVal As String
    If linkColNo > 0 Then
        linkVal = Worksheets(SheetNo_Input).Cells(Target.row, linkColNo)
    End If
    
    Dim selectedIdx As Long
    selectedIdx = -1

    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)
    
    
    '項目名称が標準と自分の工場と複数存在する場合、自分の工場の名称を使用する。
    Dim DicA
    Dim DicId
    Dim DicFactoryId
    Dim DicItem
    lastRow = itemWs.Cells(itemWs.Rows.Count, 1).End(xlUp).row
    
    'ディクショナリーを作成
    Set DicA = CreateObject("Scripting.Dictionary")
    Set DicId = CreateObject("Scripting.Dictionary")
    Set DicFactoryId = CreateObject("Scripting.Dictionary")
    DicItem = itemWs.Range("A" & Item_RowNo_Start & ":" & GetColNum2Txt(Item_ColNo_FactoryId) & lastRow)
    
    '一時的にエラーを無視
    On Error Resume Next

    '「ID+|+工場ID」を登録
    For i = Item_RowNo_Start To UBound(DicItem) - 1
        If DicItem(i, Item_ColNo_GrpId) = grpId Then
            DicA.Add DicItem(i, Item_ColNo_Id) & DelimiterId & DicItem(i, Item_ColNo_FactoryId), DicItem(i, Item_ColNo_Text)
            
            DicId.Add DicItem(i, Item_ColNo_Id) & DelimiterId & DicItem(i, Item_ColNo_FactoryId), DicItem(i, Item_ColNo_Id)
            DicFactoryId.Add DicItem(i, Item_ColNo_Id) & DelimiterId & DicItem(i, Item_ColNo_FactoryId), DicItem(i, Item_ColNo_FactoryId)
        End If
    Next

    'エラーを表示する
    On Error GoTo 0

    '拡張項目設定時に使用する工場IDを初期化(標準工場ID「0」)
    G_FactoryId = G_DefaultID
    
    '工場ID取得
    Dim sFactoryId As String
    Dim lFactoryIdColNo As Long
    If G_FactoryIdCnt <= 0 Then
        '工場ID列取得
        Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)
    End If
    lFactoryIdColNo = G_FactoryIdData(SheetNo, 2)
    If lFactoryIdColNo > 0 Then
        sFactoryId = Worksheets(SheetNo_Input).Cells(Target.row, lFactoryIdColNo)
        
        '拡張項目を設定する際に使用する工場IDを設定
        G_FactoryId = sFactoryId
    End If
    
    'グループIDでフィルターを掛ける
    itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
    If linkColNo > 0 Then
        If linkVal <> "" Then
            '連動元値がある場合、連動元値と共通アイテムでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", linkVal, "=") _
                , Operator:=xlFilterValues
        Else
            '連動元値がない場合、共通アイテムでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", "=") _
                , Operator:=xlFilterValues
        End If
    End If

    If sFactoryId > "0" Then
        If CInt(lFactoryIdColNo) < valColNo Then
            '工場IDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", sFactoryId, "=") _
                , Operator:=xlFilterValues
        End If
    End If

    '■長期計画_対象機器別管理基準　用
    '対象シート番号を取得
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    Dim targetSheetNo  As Integer
    targetSheetNo = defineWs.Range(Define_Target_SheetNo).Value
    
    '長期計画_対象機器別管理基準：スケジュール文字列(選択項目グループID：2)は、指定文字(ID：1)のみ表示
    If targetSheetNo = SheetNo_ChokeiKanri Then
        If grpId = ChokeiKanri_Schedule_GrpId Then
            'スケジュール文字列は、使用するIDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Id, Criteria1:=Array(ChokeiKanri_Schedule_OnId, "=") _
                , Operator:=xlFilterValues
        End If
    End If

    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)
    Dim sName As String
    
    Dim RowNo As Long
    Dim ColNo As Long
    Dim sSearchId_default As String
    Dim sSearchId As String
    Dim sSearchId_me As String
    Dim lRow As Long
    Dim sFId As String
    Dim sId As String
    Dim sDefaultId As String
    Dim sMeId As String
    RowNo = 0
    Dim visibleRow As Range
    
    ' 標準アイテムの翻訳を工場アイテムの翻訳で上書きした回数
    Dim duplicationCnt As Integer
    duplicationCnt = 0
    
    '標準アイテム未使用工場ID格納用配列
    Dim unuseFactoryIdArray() As String
    Dim unuseFactoryIdCnt As Long
    unuseFactoryIdCnt = 0
    
    '可視セルの行をループ
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < (Item_RowNo_Start) Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If
        
        '標準アイテム未使用工場IDを「|」で分割する
        ReDim unuseFactoryIdArray(0)
        unuseFactoryIdArray = Split(visibleRow.Cells(1, Item_ColNo_UnuseFactoryId), DelimiterId)
        
        '標準アイテム未使用工場IDのうち､レコードの工場ID列の値と同一のものがあるか確認
        For unuseFactoryIdCnt = 0 To UBound(unuseFactoryIdArray)
            '未使用工場であればコンボボックスには追加しないのでここで終了
            If (unuseFactoryIdArray(unuseFactoryIdCnt) = sFactoryId) Then
                '余分な空白行を取り除くための変数を加算
                duplicationCnt = duplicationCnt + 1
                GoTo CONTINUE1:
            End If
        Next
        
        lRow = visibleRow.row

        '配列にすでに同じIDが登録済の場合、工場が選択済ならば名称を上書きする。
            sSearchId = visibleRow.Cells(1, Item_ColNo_Id) & DelimiterId & visibleRow.Cells(1, Item_ColNo_FactoryId)
            sSearchId_default = visibleRow.Cells(1, Item_ColNo_Id) & DelimiterId & G_DefaultID
            sSearchId_me = visibleRow.Cells(1, Item_ColNo_Id) & DelimiterId & sFactoryId


                'IDが同じ
                If DicId.Exists(sSearchId) Then
                    '発見
                     sId = DicId(sSearchId)
                     sDefaultId = DicId(sSearchId_default)
                     sFId = DicFactoryId(sSearchId)

                     If sFId = G_DefaultID Then
                         sMeId = DicFactoryId(sSearchId_me)
                         If sMeId > "" Then
                             '選択工場が別にある場合でも表示するためコメントアウト
                             'GoTo CONTINUE1
                         End If
                         '標準は表示
                         GoTo CONTINUE0
                     End If
                     
                     '標準ID取得
                     sDefaultId = DicId(sSearchId_default)
                     If sDefaultId = "" Then
                         If sFId <> G_DefaultID Then
                            '標準なしで自分の工場の場合、チェック無しで表示
                            GoTo CONTINUE0
                         End If
                     End If
                     
                     If RowNo = 0 Then
                        GoTo CONTINUE0
                     End If
                     '工場IDが選択済と同じ
                     If sFactoryId = sFId Then
                         sName = DicA(sSearchId)
                        
                        '標準翻訳を工場翻訳で上書き
                        filteredData(RowNo, Item_ColNo_Text) = sName
                        
                        '標準翻訳を工場翻訳で上書きしたら「CONTINUE0:」で翻訳がヒットしないためここで検索する
                        For ColNo = 1 To 4
                            If ColNo = 4 And filteredData(RowNo, ColNo) = Target Then
                            'クリック時の設定値のインデックス
                            selectedIdx = RowNo - 1
                            End If
                        Next
                        
                        '標準翻訳を工場翻訳で上書きしたのでカウントアップ
                        duplicationCnt = duplicationCnt + 1

                        '追加はしないのでループのEndへすすむ
                        GoTo CONTINUE1
                    End If
                End If

CONTINUE0:
   
        RowNo = RowNo + 1
        For ColNo = 1 To 4
            '可視セルの値を配列に入力
            filteredData(RowNo, ColNo) = visibleRow.Cells(1, ColNo)
            
            'コンボボックスのインデックスはアイテムのIDではなくアイテムの名称で一致するものにする
            'If ColNo = 2 And filteredData(RowNo, ColNo) = prevValue Then
            If ColNo = 4 And filteredData(RowNo, ColNo) = Target Then
                'クリック時の設定値のインデックス
                selectedIdx = RowNo - 1
            End If
        Next

CONTINUE1:
    Next

    'フィルタを解除
    itemWs.Range("A1").AutoFilter
    
    '抽出結果表示
    If sCellType = CellType_FormList Then
        '選択リスト表示用にシートへコピー
        Dim lstWs As Worksheet
        Set lstWs = Worksheets(SheetName_SelectList)
        lstWs.Rows.Clear   'コピー先シートクリア
        '選択リスト表示用にシートへコピー
        lstWs.Range("A1").Resize(UBound(filteredData, 1), UBound(filteredData, 2)) = filteredData
    Else
        'コンボボックスのプロパティ設定⇒表示
        With ComboBox
        
            '2次元配列の行列を入れ替えるための配列
            Dim transedArray()
            '行列入れ替え処理
            transedArray = WorksheetFunction.Transpose(filteredData)
            '2次元配列再作成(行は固定、列は元の要素数 - 標準翻訳を工場翻訳で上書きした回数)
            ReDim Preserve transedArray(1 To 4, 1 To UBound(filteredData, 1) - duplicationCnt)
            '行列入れ替え処理(この処理で元の2次元配列から不要なから行を除いた状態になる)
            filteredData = WorksheetFunction.Transpose(transedArray)
            
            .Clear
            .List = filteredData
            .Top = Target.Top
            .Left = Target.Left
            .ColumnCount = 4                '表示列数
            .TextColumn = Item_ColNo_Text   '表示列
            .BoundColumn = Item_ColNo_Id    '選択値列
            .ColumnWidths = "0;0;0;100"     '列幅(4列目のみ表示)
            .ListIndex = selectedIdx        '選択データインデックス
            .Width = Target.Width + 15      'コンボボックス表示幅(セル幅＋プルダウン矢印▼の幅)
            .Height = Target.Height         'コンボボックス表示高さ(セル高さ)
            .MatchEntry = fmMatchEntryComplete  '入力したテキストと全て一致する項目を検索
            
            '.Style = fmStyleDropDownList    '手入力不可
            .Style = fmStyleDropDownCombo    '手入力可
            .Visible = True
        End With
    End If

CONTINUE2:
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ShowComboBox()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 複数選択リストボックスの表示
'***************************************
Public Sub ShowMultiListBox(ByVal SheetNo As Integer _
                          , ByVal Target As Range _
                          , ByVal ListBox As MSForms.ListBox)
On Error GoTo ErrHandler
    Dim valColNo As Long
    Dim grpId As Long
    Dim linkColNo As Long
    Dim FactoryIdNo As Long
    Dim iIdx As Long
    '選択ID値格納先列番号
    valColNo = GetValColNo(SheetNo, Target)
    If valColNo <= 0 Then
        '選択ID値格納先列番号なし
        GoTo CONTINUE2:
    End If
    
     'この処理開始時の複数選択リストの文字列(引数のTargetの値)を対比しておく
    Dim itemNames As String
    itemNames = Target

    '選択項目グループIDを取得
    grpId = GetGrpId(SheetNo, Target)
    '連動元列番号
    linkColNo = GetLinkColNo(SheetNo, Target)
    
    '表示前の設定値を保持、カンマで分割
    Dim prevValue As String
    prevValue = Worksheets(SheetNo_Input).Cells(Target.row, valColNo)
    Dim prevValArray As Variant
    prevValArray = Split(prevValue, DelimiterId)
    
    Dim linkVal As String
    If linkColNo > 0 Then
        linkVal = Worksheets(SheetNo_Input).Cells(Target.row, linkColNo)
    End If
    
    Dim selectedIdx As Long
    selectedIdx = -1

    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)

    '工場ID取得
    Dim sFactoryId As String
    Dim lFactoryIdColNo As Long
    If G_FactoryIdCnt <= 0 Then
        '工場ID列取得
        Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)
    End If
    lFactoryIdColNo = G_FactoryIdData(SheetNo, 2)
    If lFactoryIdColNo > 0 Then
        sFactoryId = Worksheets(SheetNo_Input).Cells(Target.row, lFactoryIdColNo)
    End If

    '選択項目グループIDでフィルターを掛ける
    itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
    If linkColNo > 0 Then
        If linkVal <> "" Then
            '連動元値がある場合、連動元値と共通アイテムでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", linkVal, "=") _
                , Operator:=xlFilterValues
        Else
            '連動元値がない場合、共通アイテムでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", "=") _
                , Operator:=xlFilterValues
        End If
    End If

    If sFactoryId > "0" Then
        If CInt(lFactoryIdColNo) < valColNo Then
            '工場IDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", sFactoryId, "=") _
                , Operator:=xlFilterValues
        End If
    End If


    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)
    
    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    
    ' 標準アイテムの翻訳を工場アイテムの翻訳で上書きした回数
    Dim duplicationCnt As Integer
    duplicationCnt = 0
    
    '標準アイテム未使用工場ID格納用配列
    Dim unuseFactoryIdArray() As String
    Dim unuseFactoryIdCnt As Long
    unuseFactoryIdCnt = 0

    '可視セルの行をループ
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Item_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If
        
        '標準アイテム未使用工場IDを「|」で分割する
        ReDim unuseFactoryIdArray(0)
        unuseFactoryIdArray = Split(visibleRow.Cells(1, Item_ColNo_UnuseFactoryId), DelimiterId)
        
        '標準アイテム未使用工場IDのうち､レコードの工場ID列の値と同一のものがあるか確認
        For unuseFactoryIdCnt = 0 To UBound(unuseFactoryIdArray)
            '未使用工場であればリストボックスには追加しないのでここで終了
            If (unuseFactoryIdArray(unuseFactoryIdCnt) = sFactoryId) Then
                '余分な空白行を取り除くための変数を加算
                duplicationCnt = duplicationCnt + 1
                GoTo CONTINUE1:
            End If
        Next

        '配列にすでに同じIDが登録済の場合、工場が選択済ならば名称を上書きする。
        If RowNo > 0 Then
            For iIdx = 1 To RowNo
                'IDが同じ
                If filteredData(iIdx, Item_ColNo_Id) = visibleRow.Cells(1, Item_ColNo_Id) Then
                    '工場IDが選択済と同じ
                    If sFactoryId = visibleRow.Cells(1, Item_ColNo_FactoryId) Then
                       '名称を上書き
                       filteredData(iIdx, Item_ColNo_Text) = visibleRow.Cells(1, Item_ColNo_Text)
                       
                        '標準翻訳を工場翻訳で上書きしたのでカウントアップ
                        duplicationCnt = duplicationCnt + 1
                        
                       '追加はしないのでメインループのEndへすすむ
                       GoTo CONTINUE1
                    End If
                End If
            Next
        End If

        RowNo = RowNo + 1
        For ColNo = 1 To 4
          '可視セルの値を配列に入力
          filteredData(RowNo, ColNo) = visibleRow.Cells(1, ColNo)
        Next
CONTINUE1:
    Next
    
    'フィルタを解除
    itemWs.Range("A1").AutoFilter

    'リストボックスのプロパティ設定⇒表示
    With ListBox
        
        '2次元配列の行列を入れ替えるための配列
        Dim transedArray()
        '行列入れ替え処理
        transedArray = WorksheetFunction.Transpose(filteredData)
        '2次元配列再作成(行は固定、列は元の要素数 - 標準翻訳を工場翻訳で上書きした回数)
        ReDim Preserve transedArray(1 To 4, 1 To UBound(filteredData, 1) - duplicationCnt)
        '行列入れ替え処理(この処理で元の2次元配列から不要なから行を除いた状態になる)
        filteredData = WorksheetFunction.Transpose(transedArray)
            
        .List = filteredData
        .ColumnCount = 4                '表示列数
        .TextColumn = Item_ColNo_Text   '表示列
        .BoundColumn = Item_ColNo_Id    '選択値列
        .ColumnWidths = "0;0;0;100"     '列幅(4列目のみ表示)
        .Top = Target.Top
        .Left = Target.Left
        .Width = Target.Width + 15
        
        '表示前の選択値を選択
        Dim i As Long
        Dim j As Long
        Dim Val As String
        If UBound(prevValArray) >= 0 Then
            For i = 0 To .ListCount - 1
                Val = .List(i, Item_ColNo_Id - 1)
                If Val <> "" Then
                    For j = 0 To UBound(prevValArray)
                        If prevValArray(j) = Val Then
                            '選択済
                            .Selected(i) = True
                        End If
                    Next j
                End If
            Next i
        End If
        
        '処理開始時の値をセルに設定する
        ActiveSheet.Cells(ActiveCell.row, valColNo) = prevValue          'ID列
        ActiveSheet.Cells(ActiveCell.row, ActiveCell.Column) = itemNames 'アイテム列
           
        .Visible = True
    End With

CONTINUE2:
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ShowMultiListBox()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' コピーアドレスの取得
'***************************************
Public Function GetCopyAddress(ByVal SheetName As String) As String
On Error GoTo ErrHandler
     
    Dim i As Long
    Dim Format As Long
    Dim Data() As Byte
    Dim Address As String
#If VBA7 And Win64 Then
    Dim hMem As LongPtr
    Dim Size As LongPtr
    Dim p As LongPtr
#Else
    Dim hMem As Long
    Dim Size As Long
    Dim p As Long
#End If
     
    Call OpenClipboard(0)
    hMem = GetClipboardData(RegisterClipboardFormat("Link"))
    If hMem = 0 Then
        Call CloseClipboard
        Exit Function
    End If
     
    Size = GlobalSize(hMem)
    p = GlobalLock(hMem)
    ReDim Data(0 To CLng(Size) - CLng(1))
#If VBA7 And Win64 Then
    Call MoveMemory(Data(0), ByVal p, Size)
#Else
    Call MoveMemory(CLng(VarPtr(Data(0))), p, Size)
#End If
    Call GlobalUnlock(hMem)
     
    Call CloseClipboard
     
    For i = 0 To CLng(Size) - CLng(1)
        If Data(i) = 0 Then
            Data(i) = Asc(" ")
        End If
    Next i
     
    Address = StrConv(Data, vbUnicode)
    If InStr(Address, "]" & SheetName) <> 0 Then
        GetCopyAddress = Trim(Replace(Mid(Address, InStr(Address, "]" & SheetName)), "]" & SheetName, ""))
    Else
        GetCopyAddress = ""
    End If
    Exit Function
     
ErrHandler:
    Call CloseClipboard
    '異常時、表示の設定を行う
    Call ErrorExit("GetCopyAddress()" & Err.Number & ":" & Err.Description)
    GetCopyAddress = ""
End Function

'***************************************
' コンボボックス変更時
'***************************************
Public Function ComboBox_Change(ByVal valColNo As Long _
                              , ByVal vComboBox As Object _
                              , ByVal vSelected As Range)
On Error GoTo ErrHandler
    
    If valColNo < 0 Then
       Exit Function
    End If
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Function
    End If

    Dim sBefore As String  '変更前のコード
    Dim lParentId As Long  '親ID
    Dim SheetNo As Integer
    SheetNo = ActiveSheet.Index
    
    With vComboBox
        If .ListIndex >= 0 Then
            sBefore = ActiveSheet.Cells(vSelected.row, valColNo)
            ActiveSheet.Cells(vSelected.row, valColNo) = .Value
            ActiveSheet.Cells(vSelected.row, vSelected.Column) = .text
            If sBefore <> ActiveSheet.Cells(vSelected.row, vSelected.Column) Then
                '変更されました。
                '紐づく項目をクリア
                If sBefore > "" Then
                    '変更前が選択済ならば、紐づくコンボをクリア
                    Dim bRtn As Boolean
                    bRtn = SetInputLinkClear(SheetNo, vSelected, valColNo)
                End If
            End If
        Else
            'ActiveSheet.Cells(vSelected.row, valColNo) = ""
            'ActiveSheet.Cells(vSelected.row, vSelected.Column) = ""
       End If
    End With

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ComboBox_Change()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' リストボックス変更時
'***************************************
Public Sub ListBox_Change(ByVal valColNo As Long _
                        , ByVal vListBox As Object _
                        , ByVal vSelected As Object)
On Error GoTo ErrHandler
    Dim i As Long
    Dim text As String
    Dim Val As String
    Dim iCheckLen As Integer
    Dim sRight As String
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    With vListBox
        For i = 0 To .ListCount - 1
          If .Selected(i) = True Then
            If text <> "" Then
                Val = Val + DelimiterId
                text = text + DelimiterName
            End If
            Val = Val & .List(i, Item_ColNo_Id - 1)
            text = text & .List(i, Item_ColNo_Text - 1)
          End If
        Next i
    End With
    If valColNo > 0 Then
        Application.EnableEvents = False '一時的にイベントを無効化
        
        '最後に区切り文字がある場合、消去し表示
        sRight = Right(Val, 1)
        If sRight = DelimiterId Then
            iCheckLen = Len(Val)
            Val = Left(Val, iCheckLen - 1)
        End If
        ActiveSheet.Cells(vSelected.row, valColNo) = Val
        
        sRight = Right(text, 1)
        If sRight = DelimiterName Then
            iCheckLen = Len(text)
            text = Left(text, iCheckLen - 1)
        End If
        ActiveSheet.Cells(vSelected.row, vSelected.Column) = text
        
        Application.EnableEvents = True 'イベントを有効に戻す
    End If
    Exit Sub
    
ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ListBox_Change()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート変更時、未選択時初期化
'***************************************
Public Sub SetComboBox(ByVal cellType As String _
                     , ByVal valColNo As Long _
                     , ByVal Target As Range _
                     , ByVal vIndex As Integer _
                     , ByVal vSelected As Object _
                     , ByVal sName As String)
On Error GoTo ErrHandler
    Dim i As Long
    Dim targetRange As Range
    Dim text As String
    Dim Val As String
    Dim copyCells As Variant
    Dim copyRange As Range
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(sName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    Select Case cellType
        Case CellType_Text
            '文字列の場合

        Case CellType_Number
            '数値の場合

        Case CellType_Date
            '日付の場合

        Case CellType_Time
            '時刻の場合

        Case CellType_ComboBox
            'コンボボックスの場合
            If Target.text = "" Then
                For i = 0 To vSelected.Count - 1
                    Set targetRange = vSelected(i)
                Next
            Else
                If Application.CutCopyMode = xlCopy Then
                    'セルを編集するため、一時的にイベント無効化
                    'Me.Application.EnableEvents = False
                    Application.EnableEvents = False

                    Dim lBakRow As Long
                    Dim lAddRow As Long
                    lBakRow = 0
                    lAddRow = 0
                    
                    'コピーの場合、コピー元のセル範囲を取得
                    copyCells = GetCopyAddress(sName)
                    Set copyRange = Range(Application.ConvertFormula(copyCells, xlR1C1, xlA1))
                     
                    'コピー元のセル個数分ループ
                    'コピー元の選択ID値格納先列番号が０以上ならば、IDをコピーする。
                    For i = 1 To copyRange.Count
                        Set targetRange = copyRange(i)
                        valColNo = GetValColNo(vIndex, targetRange)
                        If valColNo > 0 Then
                            text = ActiveSheet.Cells(targetRange.row, targetRange.Column)
                            Val = ActiveSheet.Cells(targetRange.row, valColNo)
                            
                            '１つ前のセルより下の行か？
                            If lBakRow > 0 And targetRange.row > 0 Then
                                If lBakRow < targetRange.row Then
                                    lAddRow = lAddRow + 1
                                End If
                            End If
                                                                             
                            'コピー先へIDをセット
                            ActiveSheet.Cells(Target.row + lAddRow, valColNo) = Val

                            lBakRow = targetRange.row
                        End If
                    Next
                    Application.EnableEvents = True
                End If
            End If

        Case CellType_MultiListBox
            '複数選択リストボックスの場合
            If Target.text = "" Then
                For i = 1 To vSelected.Count
                    Set targetRange = vSelected(i)
                    ActiveSheet.Cells(vSelected.row + i - 1, valColNo) = ""
                Next
            Else
                If Application.CutCopyMode = xlCopy Then
                    'セルを編集するため、一時的にイベント無効化
                    Application.EnableEvents = False
                    
                    lBakRow = 0
                    lAddRow = 0

                    'コピーの場合、コピー元のセル範囲を取得
                    copyCells = GetCopyAddress(sName)
                    Set copyRange = Range(Application.ConvertFormula(copyCells, xlR1C1, xlA1))
                    For i = 1 To copyRange.Count
                        Set targetRange = copyRange(i)
                        valColNo = GetValColNo(vIndex, targetRange)
                        If valColNo > 0 Then
                            text = ActiveSheet.Cells(targetRange.row, targetRange.Column)
                            Val = ActiveSheet.Cells(targetRange.row, valColNo)
                            
                            '１つ前のセルより下の行か？
                            If lBakRow > 0 And targetRange.row > 0 Then
                                If lBakRow < targetRange.row Then
                                    lAddRow = lAddRow + 1
                                End If
                            End If
                             'コピー先へIDをセット
                            ActiveSheet.Cells(Target.row + lAddRow, valColNo) = Val
                            lBakRow = targetRange.row
                        End If
                    Next

                    Application.EnableEvents = True
                End If
            End If

        Case CellType_CheckBox
            'チェックボックスの場合
            Worksheets(SheetNo_Input).Cells(vSelected.row, valColNo) = ""

        Case CellType_FormList
            '選択リスト画面の場合
            
        Case CellType_Text_NewLine
            '文字列の場合（改行あり

    End Select

    'イベントを有効に戻す
    Application.EnableEvents = True

    Exit Sub
    
ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetComboBox()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート選択時、タイプ毎にセルの設定を行う
'***************************************
Public Sub SetCellType(ByVal cellType As String _
                     , ByVal ComboBox1 As Object _
                     , ByVal ListBox1 As Object _
                     , ByVal Target As Range _
                     , ByVal vIndex As Integer _
                     , ByVal vSelected As Range)
On Error GoTo ErrHandler
    ComboBox1.Visible = False
    ListBox1.Visible = False
    cellType = GetCellType(vIndex, Target)
    Dim Target_Id As Range
    Dim copyCells As String
    Dim copyRange As Range
    Dim bFormFlg As Boolean
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    Select Case cellType
        Case CellType_Text
            '文字列の場合
            
        Case CellType_Number
            '数値の場合
        
        Case CellType_Date
            '日時の場合
            Set vSelected = Target
            ' カレンダーフォームを起動する
            Call modCalendar5.ShowCalendarFromRange2(Target)
            
        Case CellType_Time
            '時刻の場合
        
        Case CellType_ComboBox
            'コンボボックスの場合
            Set vSelected = Target
            
            If Target.Count = 1 And Application.CutCopyMode < xlCopy Then
                '1行選択＆コピーでない場合、コンボボックスを表示
                bFormFlg = False
                Call ShowComboBox(vIndex, Target, ComboBox1, 1, CellType_ComboBox)
            End If
                 
        Case CellType_MultiListBox
            '複数選択リストボックスの場合
            Set vSelected = Target
            
            If Target.Count = 1 And Application.CutCopyMode < xlCopy Then
                '1行選択＆コピーでない場合、コンボボックスを表示
                Call ShowMultiListBox(vIndex, Target, ListBox1)
            End If
                        
        Case CellType_CheckBox
            'チェックボックスの場合
            Set vSelected = Target
        
        Case CellType_FormList
            '選択リスト画面の場合
            Set vSelected = Target
            '選択リスト画面表示データ取得
            If Target.Count = 1 And Application.CutCopyMode < xlCopy Then
                '1行選択＆コピーでない場合、コンボボックスを表示
                Call ShowComboBox(vIndex, Target, ComboBox1, 0, CellType_FormList)
            End If

            'セル編集、一時的にイベント無効化
            Application.EnableEvents = False

            '選択ID値格納先列番号
            Dim valColNo As Long
            valColNo = GetValColNo(vIndex, vSelected)
            If valColNo <= 0 Then
            Else
                Dim sSelectName As String
                Dim lstWs As Worksheet
                Set lstWs = Worksheets(SheetNo_Input)
                sSelectName = lstWs.Cells((Input_RowNo_Start - 1), Target.Column).Value
    
                ' 選択リスト画面を起動する
                Call modFormList.ShowFormListRange2(Target, valColNo, sSelectName)
            End If

            'セル編集終了、イベント有効化
            Application.EnableEvents = True
            
        Case CellType_Text_NewLine
            '文字列の場合（改行あり）

    End Select

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetCellType()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' エラー情報表示
'***************************************
Function SetErrorInfo(tErrorInfo As typeErrorInfo _
                    , dataWs As Worksheet _
                    , ByVal ErrorUmuNo As Long) As Boolean
    On Error GoTo ErrHandler

    Dim errorWs As Worksheet
    
    With tErrorInfo
    
        '[エラー情報シート]
        
        '出力シート設定
        Set errorWs = Worksheets(.sOutName)
        'シート名
        errorWs.Cells(.lOutRow, ErrorInfo_SheetNo) = .sErrName
        errorWs.Cells(.lOutRow, ErrorInfo_SheetNo).Borders.LineStyle = xlContinuous '罫線
        
        '行
        errorWs.Cells(.lOutRow, ErrorInfo_Row) = .lErrRow
        errorWs.Cells(.lOutRow, ErrorInfo_Row).Borders.LineStyle = xlContinuous '罫線
        
        '列
        errorWs.Cells(.lOutRow, ErrorInfo_Col) = GetColNum2Txt(.lErrCol)
        errorWs.Cells(.lOutRow, ErrorInfo_Col).Borders.LineStyle = xlContinuous '罫線
        
        '処理区分
        errorWs.Cells(.lOutRow, ErrorInfo_Kubun) = .sErrKubun
        errorWs.Cells(.lOutRow, ErrorInfo_Kubun).Borders.LineStyle = xlContinuous '罫線
        
        'セルへのハイパーリンク設定
        '    リンク元：エラー情報シートのエラー情報列
        '    リンク先：エラーが発生しているシートのセル
        errorWs.Hyperlinks.Add Anchor:=errorWs.Cells(.lOutRow, ErrorInfo_Info), Address:="", SubAddress:="'" & .sErrName & "'" & "!" & .sErrRange, TextToDisplay:=.sErrMsg
        
        '「エラー情報」列のフォントを「Meiryo UI」に変更
        errorWs.Cells(.lOutRow, ErrorInfo_Info).Font.Name = "Meiryo UI"
        errorWs.Cells(.lOutRow, ErrorInfo_Info).Borders.LineStyle = xlContinuous '罫線
        
        'エラー行の高さを自動設定する
        errorWs.Cells.EntireRow.AutoFit
        
        

        '[入力シート]
        
        'セル背景色色替え（薄い赤）
        dataWs.Cells(.lErrRow, .lErrCol).Interior.Color = RGB(BackColorNg.Red, BackColorNg.Green, BackColorNg.Blue)

        'メモ
        Dim rMemo As Range
        Set rMemo = Worksheets(.sErrName).Range(.sErrRange)
        '追加済の場合、さらに追加するとエラーになる為、無ければ新規追加
        If TypeName(rMemo.Comment) = "Comment" Then
            'メモあり、後ろに追加
             rMemo.Comment.text text:=rMemo.Comment.text & Chr(10) & .sErrMsg
        Else
            Call Sheet_UnProtect(1)
            'メモなし、新規追加
             rMemo.AddComment
             rMemo.Comment.text text:=.sErrMsg
        End If
        rMemo.Comment.Visible = False '閉じる。右上の赤い三角のみ。
    
        'エラー有無列にエラー字の文言を表示
        dataWs.Cells(.lErrRow, ErrorUmuNo) = Error_Umu_Ari
        
        'メモのサイズを自動調整にする
        rMemo.Comment.Shape.TextFrame.AutoSize = True
        
        'メモのフォントを「Meiryo UI」に変更
        rMemo.Comment.Shape.TextFrame.Characters.Font.Name = "Meiryo UI"

    End With
    
    SetErrorInfo = True
    Exit Function
    
ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetErrorInfo()" & Err.Number & ":" & Err.Description)
    SetErrorInfo = False
End Function

'***************************************
' チェック処理メイン
'***************************************
Sub CheckMain()
On Error GoTo ErrHandler
    Dim i As Long
    Dim j As Long
    Dim lCheckCnt As Long       'チェック項目件数
    Dim WhereData() As Variant  'チェック用
    Dim iSheetNo As Integer     '入力シートNo
    Dim idColno As Long         '送信時処理IDの列
    Dim ErrorUmuNo As Long      'エラー有無の列
    Dim FactoryIdNo As Long     '工場IDの列
    Dim ColumnCnt As Long       '定義シート項目数
    Dim lRtn As Long            'チェック処理戻り値
    Dim lErrorCnt As Long       'エラー件数
    Dim iKubun As Integer       'チェック区分 1:最大桁数と数値チェック, 2:通常の入力チェック
    
    '画面描画停止
    Application.ScreenUpdating = False

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If


    'ブック保護解除
    Call Book_UnProtect
    'シート保護解除
    Call Sheet_UnProtect(Worksheets(SheetNo_Input).Index)
    'エラー情報シート表示
    Call Sheet_Visible_on(SheetName_ErrorInfo)
    Call Sheet_UnProtect_Error
    
    'フィルタを解除
    Call Sheet_AutoFilterOff(Worksheets(SheetNo_Input).Name)
    
    'オープン時に取得済のKEY情報が無い場合、再取得
    If G_SyoriIdCnt <= 0 Then
        '各種KEY情報取得
        Call modCommon.GetKeyInfo
    End If

    If G_SyoriIdCnt <= 0 Then
       '入力シートなし
        GoTo CONTINUE1:
    End If
    
    If G_FactoryIdCnt <= 0 Then
        '工場ID列取得
        Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)
    End If
    
    '対象シート番号を取得
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    Dim targetSheetNo  As Integer
    targetSheetNo = defineWs.Range(Define_Target_SheetNo).Value
    
    '、対象シート番号により、チェックが２パターンある為どちらか判断
    iKubun = 0
    Select Case targetSheetNo
        Case SheetNo_Bashokaiso _
           , SheetNo_ShokushuKishu _
           , SheetNo_Bumon _
           , SheetNo_YobiLocation
               ' 20：場所階層
               ' 56：職種・機種
               ' 62：予備ロケーション
               ' 63：部門（工場・部門）               '
            iKubun = 1
        Case Else
            iKubun = 2
    End Select
        
    'エラー有無-文言(NG時に表示）取得
    Error_Umu_Ari = GetMessage("111010021") 'あり
    
    If G_SyoriIdCnt <> G_ErrorUmuCnt Then
        '送信時処理ID列とエラー有無列が揃っていない
        MsgBox GetMessage("141190003") '定義情報が不正です。
        Exit Sub
    End If
    
    'エラー情報シート初期化
    Call SetErrorSheetClear
    lErrorCnt = 0

    '入力シートの件数分ループ
    For i = 1 To G_SyoriIdCnt
        
        ColumnCnt = 0
        iSheetNo = 0
        idColno = 0
        ErrorUmuNo = 0
        FactoryIdNo = 0
        
        '定義情報シートの項目数取得
        If i <= UBound(G_SendIdData) Then
            iSheetNo = G_SendIdData(i, 1)
            idColno = G_SendIdData(i, 2)
        End If
        If i <= UBound(G_ErrorUmuData) Then
            ErrorUmuNo = G_ErrorUmuData(i, 2)
        End If
        If i <= UBound(G_FactoryIdData) Then
            FactoryIdNo = G_FactoryIdData(i, 2)
        End If

        Call GetInputColmunCount(iSheetNo, idColno, ErrorUmuNo, ColumnCnt)

        '定義情報シートより取得
        lCheckCnt = 0
        Erase WhereData
        Call GetCheckColmun(iSheetNo, idColno, ErrorUmuNo, lCheckCnt, WhereData)
        If lCheckCnt <= 0 Then
            'チェック項目なし
            GoTo CONTINUE1:
        End If

        '入力シートチェック
        lRtn = 0
        If iKubun = 1 Then
            '最大桁数のみチェック
            lRtn = GetInputSheetCheck_Keta(iSheetNo, idColno, ErrorUmuNo, FactoryIdNo, ColumnCnt, lCheckCnt, WhereData, targetSheetNo, lErrorCnt)
        Else
            '最大桁数、最小値、最大値、書式　チェック
            lRtn = GetInputSheetCheck(iSheetNo, idColno, ErrorUmuNo, FactoryIdNo, ColumnCnt, lCheckCnt, WhereData, targetSheetNo)
        End If
        
        If lRtn = -1 Then
            'エラー発生
            GoTo CONTINUE1:
        ElseIf lRtn > 0 Then
            '入力エラーあり
            lErrorCnt = lErrorCnt + lRtn
        End If
        
    Next i

    If iKubun <> 1 Then
        'フィルタをかける(エラー有無列まで)
        Call Sheet_AutoFilterOn(Worksheets(1).Name, (Input_RowNo_Start - 1), ErrorUmuNo)

    End If
    
CONTINUE1:
    
    'レイアウト定義シート 非表示
    Call Sheet_Visible_off(SheetName_Define)
    '選択アイテムシート 非表示
    Call Sheet_Visible_off(SheetName_Item)
    
    'エラーがある場合、エラー情報を表示
    If lErrorCnt > 0 Then
        '入力エラーあり
        Worksheets(SheetName_ErrorInfo).Select
        Worksheets(SheetName_ErrorInfo).Range("A1").Select
        MsgBox GetMessage("941220005") '入力エラーがあります
    Else
        If G_SyoriIdCnt <= 0 Then
            '定義情報シートにに送信時処理ID列が存在しない
            MsgBox GetMessage("141190003") '定義情報が不正です。
        Else
            'エラーなし
            'エラー情報シートを非表示
            Call Sheet_Visible_off(SheetName_ErrorInfo)
            Worksheets(SheetNo_Input).Select
            '正常メッセージ
            MsgBox GetMessage("141220006") '入力チェックが完了しました。
        End If
    End If
    
    '画面描画再開
    Application.ScreenUpdating = True

    'シート保護
    Call Sheet_Protect(1)       '入力用シート
    Call Sheet_Protect_Error    'エラー情報シート
    
    'ブック保護
    Call Book_Protect

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("CheckMain()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 定義シートの列区分より、検索文字列の取得
'***************************************
Sub GetDefineFindString(ByRef iSheetCnt As Integer _
                      , ByRef filteredData() As Variant _
                      , ByVal strFind As String _
                      , Optional ByVal iWSheetNo As Integer = 0)
On Error GoTo ErrHandler

    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    iSheetCnt = 0   'シート件数
    
    'フィルタを解除
    Call Sheet_AutoFilterOff(SheetName_Define)
    
    '列区分でフィルターを掛ける
    defineWs.Range("$A$5:$M$77").AutoFilter Field:=Define_ColNo_ColumnDivision, Criteria1:=strFind
    If strFind = ColumnDivision_Key And iWSheetNo > 0 Then
        'KEY列の場合、シートNoでフィルターを掛ける
        defineWs.Range("$A$5:$M$77").AutoFilter Field:=Define_ColNo_SheetNo, Criteria1:=iWSheetNo
    End If
    
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = defineWs.Range("A5").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    ReDim filteredData(1 To rowCnt - 1, 1 To Define_ColNo_Name)
        
    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    '可視セルの行をループ
    For Each visibleRow In defineWs.Range("A5").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Define_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

        RowNo = RowNo + 1
        For ColNo = 1 To Define_ColNo_Name
          '可視セルの値を配列に入力
          filteredData(RowNo, ColNo) = visibleRow.Cells(1, ColNo)
          
        Next
CONTINUE1:
    Next
    
    'フィルタを解除
    defineWs.Range("A1").AutoFilter
    iSheetCnt = RowNo

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetDefineFindString()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' Sheet_Defineの入力チェック関連項目取得
'    ・必須項目フラグ
'    ・最大桁数
'    ・最小値
'    ・最大値
'    ・書式
'***************************************
Sub GetCheckColmun(ByVal iSheetNo As Integer _
                 , ByVal idColno As Long _
                 , ByVal ErrorUmuNo As Long _
                 , ByRef lCheckCnt As Long _
                 , ByRef filteredData() As Variant)
On Error GoTo ErrHandler
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
        
    'フィルターを掛ける
    '  Sheet_Defineより、シートNo、列区分が空白　で絞り込み
    '  マスタ関連は並列で複数の送信時処理ID列が存在する場合がある為、
    '  列番号で「送信時処理ID」列～「エラー有無」列でフィルターを掛ける
    With defineWs.Range("A5")
        .AutoFilter Define_ColNo_SheetNo, iSheetNo
        .AutoFilter Define_ColNo_ColumnDivision, "=" '列区分が空白のもの
        .AutoFilter Define_ColNo_ColNo, Criteria1:=">=" & idColno, _
            Operator:=xlAnd, Criteria2:="<=" & ErrorUmuNo
    End With
   
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = defineWs.Range("A5").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
       
    ReDim filteredData(1 To rowCnt, 1 To (Define_Max + 1))
    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    Dim sData As String
    Dim bCheckFlg As Boolean
    lCheckCnt = 0
    
    '可視セルの行をループ
    For Each visibleRow In defineWs.Range("A5").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows

        If visibleRow.row < Define_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If
          
        RowNo = RowNo + 1
        lCheckCnt = lCheckCnt + 1
        For ColNo = 1 To Define_Max

            '可視セルの値を配列に入力
            filteredData(lCheckCnt, ColNo) = visibleRow.Cells(1, ColNo)
        Next
CONTINUE1:
    Next

    'フィルタを解除
    defineWs.Range("A1").AutoFilter

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetCheckColmun()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 入力シートの項目数取得
'***************************************
Sub GetInputColmunCount(ByVal iSheetNo As Integer _
                      , ByVal idColno As Long _
                      , ByVal ErrorUmuNo As Long _
                      , ByRef rowCnt As Long)

On Error GoTo ErrHandler
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
        
    'シート番号でフィルターを掛ける
    defineWs.Range("A5").AutoFilter Field:=Define_ColNo_SheetNo, Criteria1:=iSheetNo
    '列番号で「送信時処理ID」列～「エラー有無」列でフィルターを掛ける
    defineWs.Range("A5").AutoFilter Field:=Define_ColNo_ColNo, Criteria1:=">=" & idColno, _
        Operator:=xlAnd, Criteria2:="<=" & ErrorUmuNo
        
    '可視セルの行数を取得
    'Dim rowCnt As Long
    rowCnt = defineWs.Range("A5").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    rowCnt = rowCnt - 2
    
    'フィルタを解除
    defineWs.Range("A1").AutoFilter
    
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetInputColmunCount()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' エラー情報シート　初期化
'***************************************
Sub SetErrorSheetClear()

On Error GoTo ErrHandler
    Dim dataWs As Worksheet
    Set dataWs = Worksheets(SheetName_ErrorInfo)

    Dim lastRow As Long
    lastRow = dataWs.Cells(dataWs.Rows.Count, ErrorInfo_SheetNo).End(xlUp).row
    If lastRow >= ErrorInfo_RowNo_Start Then
        '開始行～最終行をクリア
        dataWs.Range("A" & ErrorInfo_RowNo_Start & ":I" & lastRow).ClearContents
        '罫線を削除
        dataWs.Range("A" & ErrorInfo_RowNo_Start & ":I" & lastRow).Borders.LineStyle = xlLineStyleNone
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetErrorSheetClear()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
'***************************************
' 入力シート　指定行の背景色・メモ・エラー有無を初期化
'***************************************
Sub SetInputRowClear(ByVal SheetNo As Integer _
                   , ByVal lRow As Long, ErrorUmuNo As Long)
On Error GoTo ErrHandler
    Dim ColNo As Long
    Dim dataWs As Worksheet
    Set dataWs = Worksheets(SheetNo)
    Dim rRange As Range
    
    '「 A列～エラー有無列まで 」ループ
    For ColNo = 1 To ErrorUmuNo
        Set rRange = dataWs.Range(GetColNum2Txt(ColNo) & lRow)
        
        '背景色　クリア
        rRange.Interior.ColorIndex = 0
        
        'メモ消去
        If TypeName(rRange.Comment) = "Comment" Then
            'メモあり、消去
            rRange.ClearComments
        End If
    Next
    
    'エラー有無　クリア
    rRange.Value = ""

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetInputRowClear()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 入力シート　指定セルの背景色・メモ・エラー有無を初期化
'***************************************
Sub SetInputCellClear(ByVal SheetNo As Integer _
                   , ByVal lRow As Long, ByVal lCol As Long, ErrorUmuNo As Long)
On Error GoTo ErrHandler
    Dim ColNo As Long
    Dim dataWs As Worksheet
    Set dataWs = Worksheets(SheetNo)
    Dim rRange As Range
    
    Set rRange = dataWs.Range(GetColNum2Txt(lCol) & lRow)
        
    '背景色　クリア
    rRange.Interior.ColorIndex = 0
    
    'メモ消去
    If TypeName(rRange.Comment) = "Comment" Then
        'メモあり、消去
        rRange.ClearComments
    End If
    
    'エラー有無　クリア
'    rRange.Value = ""

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetInputCellClear()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 行毎に入力チェックを行う
'***************************************
Function GetInputSheetCheck(ByVal SheetNo As Integer _
                          , ByVal idColno As Long _
                          , ByVal ErrorUmuNo As Long _
                          , ByVal FactoryIdNo As Long _
                          , ByVal ColumnCnt As Long _
                          , ByVal lCheckCnt As Long _
                          , WhereData() As Variant _
                          , ByVal targetSheetNo As Long) As Long
  On Error GoTo ErrHandler

    Dim dataWs As Worksheet
    Set dataWs = Worksheets(SheetNo)
    Dim lastRow As Long
    Dim i As Long
    Dim j As Long
    Dim k As Long
    Dim sCol As String
    Dim sColName As String
    Dim sRange As String
    Dim sSheetName As String
    
    '送信時処理は非表示するシート用
    Dim idColnoWork As Integer
    idColnoWork = idColno
    If targetSheetNo = SheetNo_MasterItemSort Then
        '送信時処理は非表示にする為
        idColnoWork = idColnoWork + 1
    End If
    
    '該当件数取得
    lastRow = dataWs.Cells(dataWs.Rows.Count, idColno).End(xlUp).row
    sSheetName = dataWs.Name
    
    '列数をアルファベットに変換
    sCol = GetColNum2Txt(idColno)
    
    '非表示列があると、フィルターの行取得でエラーになる為、
    '送信時処理より左の列を再表示する
    For i = 1 To (idColnoWork)
        If dataWs.Columns(i).Hidden = True Then
            dataWs.Columns(i).Hidden = False  '列を再表示
        End If
    Next
    
    sRange = sCol & (Input_RowNo_Start - 1)
            
    '★2023/03/01
    '送信時処理IDの右隣の送信時処理でフィルターを掛ける
    dataWs.Range("A" & (Input_RowNo_Start - 1)).AutoFilter Field:=(idColno + 1), Criteria1:="<>"
    
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = dataWs.Range("C" & (Input_RowNo_Start - 2)).CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count

    Dim filteredData
    ReDim filteredData(1 To rowCnt, 1 To 5)
    
    Dim lLoopCnt As Long, ColNo As Long
    Dim sId As String
    Dim sFactoryId As String
    Dim sData As String
    Dim lCol As Long
    Dim sValue As String
    Dim sValueOrg As String
    Dim iMaxLen As Integer
    Dim iCheckLen As Integer
    Dim dMinValue As Double
    Dim dMaxValue As Double
    Dim dCheckValue As Double
    Dim lErrorCnt As Long
    Dim tErrorInfo As typeErrorInfo       'エラー情報
    Dim tErrorInfoClear As typeErrorInfo  'エラー情報 初期用
    Dim sType As String                   'Sheet_Define：列タイプ
    Dim sFormat As String                 'Sheet_Define：書式
    Dim bErrorFlg As Boolean              '必須入力エラー時のデフォルト値表示エラーフラグ True:エラーあり
    Dim iKeyCnt As Integer

    lLoopCnt = 0
    lErrorCnt = 0
    iKeyCnt = 0
    Dim visibleRow As Range
    Dim lCheckRowNo As Long
    Dim KeyData() As Variant '定義情報シートのキー情報
    Dim sKeyVal As String    '
    Dim bFlg As Boolean
    Dim sWhValue As String
    Dim arrDt As Variant
    Dim bMinMaxFlg As Boolean
    Dim sErrMsgNo As String
    Dim rRange As Range
    Dim sDefauleId As String
    Dim sDefauleVal As String
    Dim lvalColNo As Long
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    
    '可視セルの行をループ
    Dim sColStart As String
    Dim sColEnd As String
    sColStart = GetColNum2Txt(idColno)  'チェック開始列
    sColEnd = GetColNum2Txt(ErrorUmuNo) 'チェック入力列

    lastRow = dataWs.Cells(dataWs.Rows.Count, 4).End(xlUp).row

    With Worksheets(SheetNo_Input).Range("A3:" & sColEnd & lastRow)
    For Each visibleRow In Worksheets(SheetNo_Input).Range("A3").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible)
        
        lLoopCnt = lLoopCnt + 1
        If lLoopCnt > rowCnt Then
            GoTo CONTINUE2:
        End If
        If visibleRow.row < Input_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

        lCheckRowNo = visibleRow.row
        sKeyVal = ""
        
        '送信時処理ID取得
        'sId = visibleRow.Cells(1, idColno)
        
        '送信時処理名称より、送信時処理IDを取得する(ラベルを正とする)
        sId = GetSendProcIdByName(visibleRow.Cells(1, (idColno + 1)).text)
        
        '送信時処理IDをセルに値を設定
        If sId = SendId_Error Then
            'エラーの場合は空
            visibleRow.Cells(1, idColno) = ""
        Else
            '正常の場合は取得した値
            visibleRow.Cells(1, idColno) = sId
        End If

        If sId = "" Then
            'チェック対象行を再取得
            Set visibleRow = Worksheets(SheetNo_Input).Range("A" & lCheckRowNo & ":A" & lCheckRowNo)
            sId = visibleRow.Cells(1, idColno)
            
            '★2023/03/01 Start
            '送信字処理IDから送信時処理をループの前に取得（コピー字、IDが空になる為）
            Dim bReturn As Boolean
            
            'フィルタを解除
            Call Sheet_AutoFilterOff(SheetName_Define)
            '選択ID値格納先列番号でフィルターを掛ける
            defineWs.Range("A" & Define_RowNo_Start).AutoFilter Field:=Define_ColNo_Val, Criteria1:=idColno
            '可視セルの行をループ
            Dim visibleSendProcessRow As Range
            For Each visibleSendProcessRow In defineWs.Range("A5").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
                If visibleSendProcessRow.row < Define_RowNo_Start Then
                    'ヘッダー行はスキップ
                    GoTo ContinueRow:
                End If
                
                '送信時処理の列タイプ取得
                sType = visibleSendProcessRow.Cells(1, Define_ColNo_Type)
                Exit For
ContinueRow:
            Next
            'フィルタを解除
            Call Sheet_AutoFilterOff(SheetName_Define)
            
            '入力値取得
            sValue = visibleRow.Cells(1, (idColno + 1)).text
            Set rRange = dataWs.Range(GetColNum2Txt(idColno + 1) & CStr(lCheckRowNo))
            sDefauleId = ""
            Dim errMsg As String
            Call ShowComboBox_Check(SheetNo, rRange, sValue, sType, "0", lvalColNo, sDefauleId, sDefauleVal, bReturn, errMsg)
            If sDefauleId > "" Then
                '送信字処理ID表示
                dataWs.Cells(lCheckRowNo, idColno) = sDefauleId
                sId = sDefauleId
            End If
            '★2023/03/01 End
        End If

        '工場ID取得
        If FactoryIdNo > 0 Then
            sFactoryId = visibleRow.Cells(1, FactoryIdNo)

            If sFactoryId = "" Then
                '工場IDが取得できない場合、翻訳からIDを取得する
                Dim itemWs As Worksheet
                Set itemWs = Worksheets(SheetName_Item)
                
                'フィルタを解除
                Call Sheet_AutoFilterOff(SheetName_Define)
                
                'シートNoでフィルターを掛ける
                defineWs.Range("A" & Define_RowNo_Start).AutoFilter Field:=Define_ColNo_SheetNo, Criteria1:=SheetNo
                '選択ID値格納先列番号でフィルターを掛ける
                defineWs.Range("A" & Define_RowNo_Start).AutoFilter Field:=Define_ColNo_Val, Criteria1:=FactoryIdNo
                
                '可視セルの行をループ
                Dim visibleDefineRow As Range
                For Each visibleDefineRow In defineWs.Range("A5").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
                    If visibleDefineRow.row < Define_RowNo_Start Then
                        'ヘッダー行はスキップ
                        GoTo ContinueDefine:
                    End If
                    
                    '工場（翻訳）列番号
                    Dim sFactoryNameColNo As Long
                    sFactoryNameColNo = visibleDefineRow.Cells(1, Define_ColNo_ColNo)
                    '工場（翻訳）取得
                    Dim sFactoryName As String
                    sFactoryName = visibleRow.Cells(1, sFactoryNameColNo)
                    
                    '選択項目グループID
                    Dim grpId As Long
                    grpId = visibleDefineRow.Cells(1, Define_ColNo_GrpId)
                    
                    'フィルタを解除
                    itemWs.Range("A1").AutoFilter
                    
                    'グループIDでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
                    '表示文字列でフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Item_ColNo_Text, sFactoryName
                    '可視セルの行をループ
                    Dim visibleItemRow As Range
                    For Each visibleItemRow In itemWs.Range("A5").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
                        If visibleItemRow.row < Item_RowNo_Start Then
                            'ヘッダー行はスキップ
                            GoTo ContinueItem:
                        End If
                        sFactoryId = visibleItemRow.Cells(1, Item_ColNo_Id)
                        Exit For

ContinueItem:
                    Next
                    
                    If sFactoryId <> "" Then
                        Exit For
                    End If

ContinueDefine:
                Next
                
                'フィルタを解除
                Call Sheet_AutoFilterOff(SheetName_Define)
                Call Sheet_AutoFilterOff(SheetName_Item)
            End If
        End If

        Select Case sId
            Case SendId_Insert, SendId_Update, SendId_Delete, SendId_Error   '登録, 更新, 削除, エラー(左記３項目以外)
            Case Else
                'IDが空
                GoTo CONTINUE1:
        End Select

        '送信時処理列のヘッダ表示名
        sColName = dataWs.Cells(Input_RowNo_Start - 1, (idColno + 1))

        'チェックする行の背景色・メモ・エラー有無をクリア
        Call SetInputRowClear(SheetNo, lCheckRowNo, ErrorUmuNo)
        
        'エラー情報構造体初期化
        tErrorInfo = tErrorInfoClear

        'エラー情報の共通部分を格納
        With tErrorInfo
            .sOutName = SheetName_ErrorInfo
            .sErrName = sSheetName
            .lErrRow = lCheckRowNo
            .sErrColName = sColName
            .sErrKubun = visibleRow.Cells(1, (idColno + 1))
        End With

        'KEY列取得
        Call GetDefineFindString(iKeyCnt, KeyData, ColumnDivision_Key, SheetNo)
                
        
        'KEY項目列をチェック
        For j = 1 To iKeyCnt
            sKeyVal = visibleRow.Cells(1, KeyData(j, Define_ColNo_ColNo))
            
            '送信時処理により、KEY項目をチェック
            bFlg = True
            Select Case sId
                Case SendId_Insert  '登録
                    If sKeyVal <> "" Then
                        '登録時、設定ありはエラー
                        bFlg = False
                        sErrMsgNo = "141070006"  '既存データに対して登録は指定できません。
                    End If
                Case SendId_Update, SendId_Delete  '更新, 削除
                    If sKeyVal = "" Then
                        '更新・ 削除時、設定なしはエラー
                        bFlg = False
                        sErrMsgNo = "141120014"  '新規追加データに対して内容更新・削除は指定できません。
                    End If
                Case SendId_Error ' エラー(上記３項目以外)
                    bFlg = False
                    sErrMsgNo = "141140004"  '選択内容が不正です。
            End Select
            
            If bFlg = False Then
                lErrorCnt = lErrorCnt + 1
                'エラー情報格納
                With tErrorInfo
                
                    .lErrCol = idColno + 1
                    .sErrRange = GetColNum2Txt(idColno + 1) & lCheckRowNo
                    .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
                    .sErrColName = dataWs.Cells(Input_RowNo_Start - 1, (idColno + 1))
                    .sErrMsg = GetMessage(sErrMsgNo)
                End With
                'エラー情報表示
                If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                    GoTo CONTINUE2:
                End If
                Exit For
            End If
        Next j

        '-----------------------------------------------
        '入力チェック
        '-----------------------------------------------
        Select Case sId
            Case SendId_Insert, SendId_Update  '登録, 更新
              
                '項目の件数分ループ
                For i = 1 To lCheckCnt

                    '対象列番号取得
                    lCol = WhereData(i, Define_ColNo_ColNo)

                    'ヘッダ表示名
                    sColName = dataWs.Cells(Input_RowNo_Start - 1, lCol)

                    '列タイプ取得
                    sType = WhereData(i, Define_ColNo_Type)

                    '入力値取得
                    sValue = visibleRow.Cells(1, lCol).text
                    
                    '入力セル情報格納
                    With tErrorInfo
                        .lErrCol = lCol
                        .sErrRange = GetColNum2Txt(lCol) & lCheckRowNo
                        .sErrColName = sColName
                    End With
                    
                    '数値の場合
                    If sType = CellType_Number Then
                                        
                        '後ろの空白を除去
                        sValue = Trim(sValue)
                        'カンマを除去
                        sValue = Trim(Replace(sValue, ",", ""))
                        
                        '数値入力時に、セルの列幅以上の桁数の場合に「#####」と表示された場合は、セルのValue値を使用する
                        If sValue > "" Then
                            arrDt = Split(sValue, "#")
                            If UBound(arrDt) > 0 Then
                                If UBound(arrDt) = Len(sValue) Then
                                    'ALL # なので、.Valueに実際の値が入っているのでそちらを使用
                                    sValue = visibleRow.Cells(1, lCol).Value
                                End If
                            End If
                        End If

                    End If

                    '列タイプ毎のチェック　及び、テキストの文字列に改行コードがあれば消去
                    sValueOrg = sValue
                    If lCol > (idColno + 1) And sValue > "" Then
                        If CheckType(lErrorCnt, tErrorInfo, SheetNo, dataWs, sType, sFactoryId, sValue, lCheckRowNo, lCol, ErrorUmuNo) = False Then
                            '次の項目のチェックへ
                            GoTo ContinueNextCol:
                        End If
                    End If
                    
                    If sType = CellType_Text Or sType = CellType_Text Then
                        If sValueOrg <> sValue Then
                            '改行コード消去後置換
                            dataWs.Cells(lCheckRowNo, lCol) = sValue
                        End If
                    End If
                    
                    '-----------------------------------------------
                    '必須項目フラグ　チェック
                    '-----------------------------------------------
                    If WhereData(i, Define_Hissu_Flg) <> "" Then

                        '必須項目フラグ
                        If sValue = "" Then
                            '入力必須エラー
                            bErrorFlg = True
                            
                            'コンボボックスのリストが１件の場合、リストの１件目をデフォルト値として表示する
                            Select Case sType
                            Case CellType_ComboBox, CellType_MultiListBox, CellType_FormList
                                'コンボボックス、複数選択リストボックス、選択リスト画面

                                Set rRange = Worksheets(SheetNo).Range(GetColNum2Txt(lCol) & CStr(lCheckRowNo))
                                Call ShowComboBox_Hidden(SheetNo, rRange, sType, sFactoryId, lvalColNo, sDefauleId, sDefauleVal)
                                If sDefauleVal > "" Then
                                    'デフォルト値　名称表示
                                    dataWs.Cells(lCheckRowNo, lCol) = sDefauleVal
                                    bErrorFlg = False 'エラーではない
                                End If
                                If sDefauleId > "" And lvalColNo > 0 Then
                                    'デフォルト値　ID表示
                                    dataWs.Cells(lCheckRowNo, lvalColNo) = sDefauleId
                                End If
                            Case Else
                            
                            End Select


                            '入力必須エラー
                            If bErrorFlg = True Then
                                lErrorCnt = lErrorCnt + 1
                                'エラー情報格納
                                With tErrorInfo
                                    .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
                                    .sErrMsg = GetMessage("941270001") '必須項目です。入力してください。
                                End With
                                'エラー情報表示
                                If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                                    GoTo CONTINUE2:
                                End If
                                '次の項目のチェックへ
                                GoTo ContinueNextCol:
                            End If
                        End If
                    End If
                    
                    '-----------------------------------------------
                    '最大桁数　チェック
                    '-----------------------------------------------
                    If WhereData(i, Define_Max_Length) <> "" Then
                    
                        '最大桁数
                        iMaxLen = WhereData(i, Define_Max_Length)
                    
                        '入力値あり かつ 最大桁数が０以上の場合にチェックする
                        If sValue <> "" And iMaxLen > 0 Then
                        
                            '入力値の桁数取得（半角全角考慮しない）
                            iCheckLen = Len(sValue)

                            If iCheckLen > iMaxLen Then

                                '最大桁数エラー
                                lErrorCnt = lErrorCnt + 1
                                'エラー情報格納
                                With tErrorInfo
                                    .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
                                    .sErrMsg = GetMessage("941060018", CStr(iMaxLen)) '{0}文字以内で入力して下さい。
                                End With

                                'エラー情報表示
                                If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                                    GoTo CONTINUE2:
                                End If
                                '次の項目のチェックへ
                                GoTo ContinueNextCol:
                            End If
                        End If

                    End If

                    bMinMaxFlg = True

                    '-----------------------------------------------
                    '最小値　チェック
                    '-----------------------------------------------
                    If WhereData(i, Define_Min_Value) <> "" Then

                        '最小値自体が数値ならばチェック
                        sWhValue = WhereData(i, Define_Min_Value)
                        If IsNumeric(sWhValue) = True Then
                            
                            '最小値
                            dMinValue = StringToDouble(sWhValue)
                            If sValue <> "" Then
                                '入力値の取得
                                dCheckValue = StringToDouble(sValue)
    
                                If dCheckValue < dMinValue Then
                                    '最小値エラー
                                    bMinMaxFlg = False
                                End If
                            End If
                        End If
                    End If

                    '-----------------------------------------------
                    '最大値　チェック
                    '-----------------------------------------------
                    If WhereData(i, Define_Max_Value) <> "" Then

                        '最大値自体が数値ならばチェック
                        sWhValue = WhereData(i, Define_Max_Value)
                        If IsNumeric(sWhValue) = True Then

                            dMaxValue = StringToDouble(sWhValue)
                            If sValue <> "" Then
                                '入力値の取得
                                dCheckValue = StringToDouble(sValue)
    
                                If dCheckValue > dMaxValue Then
                                    '最大値エラー
                                    bMinMaxFlg = False
                                End If
                            End If
                        End If
                    End If
                    
                    If bMinMaxFlg = False Then
    
                        '範囲エラー
                        lErrorCnt = lErrorCnt + 1
                        'エラー情報格納
                        With tErrorInfo
                            .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
                            .sErrMsg = GetMessage("941060015", CStr(dMinValue), CStr(dMaxValue)) '{0}から{1}の範囲で入力して下さい。
                        End With

                        'エラー情報表示
                        If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                            GoTo CONTINUE2:
                        End If
                        '次の項目のチェックへ
                        GoTo ContinueNextCol:
                    End If
    
                    '-----------------------------------------------
                    '書式　チェック
                    '-----------------------------------------------
                    If WhereData(i, Define_Format) <> "" Then
                        '列タイプ、書式
                        sType = WhereData(i, Define_ColNo_Type)
                        sFormat = WhereData(i, Define_Format)
                        If CheckFormat(lErrorCnt, tErrorInfo, dataWs, sType, sFormat, sValue, ErrorUmuNo, lCheckRowNo, lCol) = False Then
                        End If
                    End If
                    
ContinueNextCol:
                Next

            Case Else
                '入力チェックなし

        End Select
'    Next


CONTINUE1:
    Next

End With

CONTINUE2:
    'フィルタを解除
    Worksheets(SheetNo).Range("A1").AutoFilter
    
    '送信時処理より左の列を非表示にする
    For i = 1 To (idColnoWork)
        Worksheets(SheetNo).Columns(i).Hidden = True  '列を非表示
    Next
    
    GetInputSheetCheck = lErrorCnt
    
    Exit Function

ErrHandler:

    '送信時処理より左の列を非表示にする
    If idColnoWork > 0 Then
        For i = 1 To (idColnoWork)
            Worksheets(SheetNo).Columns(i).Hidden = True  '列を非表示
        Next
    End If
    
    GetInputSheetCheck = -1
    Call Sheet_AutoFilterOff(Worksheets(SheetNo).Name)
    '異常時、表示の設定を行う
    Call ErrorExit("GetInputSheetCheck()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' 行毎に入力チェックを行う：桁数チェック、数値型チェック
'***************************************
Function GetInputSheetCheck_Keta(ByVal SheetNo As Integer _
                          , ByVal idColno As Long _
                          , ByVal ErrorUmuNo As Long _
                          , ByVal FactoryIdNo As Long _
                          , ByVal ColumnCnt As Long _
                          , ByVal lCheckCnt As Long _
                          , WhereData() As Variant _
                          , ByVal targetSheetNo As Long _
                          , ByVal lErrorStartCnt As Long) As Long
  On Error GoTo ErrHandler

    Dim dataWs As Worksheet
    Set dataWs = Worksheets(SheetNo)
    Dim lastRow As Long
    Dim lRow As Long
    Dim i As Long
    Dim sCol As String
    Dim sColName As String
    Dim sRange As String
    Dim sSheetName As String
    Dim sId As String
    Dim lCol As Long
    Dim sValue As String
    Dim lCheckRowNo As Long
    Dim iMaxLen As Integer
    Dim iCheckLen As Integer
    Dim lLoopCnt As Long
    Dim lSubErrorCnt As Long
    Dim lErrorCnt As Long
    Dim tErrorInfo As typeErrorInfo       'エラー情報
    Dim tErrorInfoClear As typeErrorInfo  'エラー情報 初期用
    Dim rRange As Range
    Dim sType As String                   'Sheet_Define：列タイプ
    Dim arrDt As Variant
    Dim sValueOrg As String

    lErrorCnt = 0
    lLoopCnt = 0
    lSubErrorCnt = 0
    lCheckRowNo = 4
        
    'シート名取得
    sSheetName = dataWs.Name
    
    '列数をアルファベットに変換
    sCol = GetColNum2Txt(idColno)
    
    '入力最大件数取得
    lastRow = 0
    lastRow = dataWs.Cells(dataWs.Rows.Count, 1).End(xlUp).row
    For i = idColno To ErrorUmuNo
        If lastRow < dataWs.Cells(dataWs.Rows.Count, i).End(xlUp).row Then lastRow = dataWs.Cells(dataWs.Rows.Count, i).End(xlUp).row
    Next i

    sRange = sCol & (Input_RowNo_Start - 1)
            

    If Input_RowNo_Start > lastRow Then
        GoTo CONTINUE2:
    End If
    
    '初期処理
    '送信時処理列のヘッダ表示名
    sColName = dataWs.Cells(Input_RowNo_Start - 1, (idColno + 1))

    'エラー情報構造体初期化
    tErrorInfo = tErrorInfoClear

    'エラー情報の共通部分を格納
    With tErrorInfo
        .sOutName = SheetName_ErrorInfo
        .sErrName = sSheetName
        .lErrRow = lCheckRowNo
        .sErrColName = sColName
    End With
    
        
    '入力チェック
    '　列：送信時処理IDの右の列～エラー有無列の１つ手前の列まで
    '　行：入力開始行～最大行まで
        
    For lCol = (idColno + 1) To (ErrorUmuNo - 1)
        lLoopCnt = lLoopCnt + 1
        
        '列タイプ取得
        sType = WhereData(lLoopCnt, Define_ColNo_Type)
        'ヘッダ表示名
        sColName = dataWs.Cells(Input_RowNo_Start - 1, lCol)

        '入力セル情報格納
        With tErrorInfo
            .lErrCol = lCol
            .sErrColName = sColName
        End With

        For lRow = Input_RowNo_Start To lastRow
    
            lCheckRowNo = lRow
            
            'ループの１回目に、初期処理を行う
            If lCol = (idColno + 1) Then
                'エラー有無クリア
                If ErrorUmuNo > 0 Then
                    If ErrorUmuNo > idColno Then
                        Set rRange = dataWs.Range(GetColNum2Txt(ErrorUmuNo) & lCheckRowNo)
                        rRange = ""
                    End If
                End If
            End If
            'チェックするセルの背景色・メモ・エラー有無をクリア
            Call SetInputCellClear(SheetNo, lCheckRowNo, lCol, ErrorUmuNo)
        
        
            '入力値取得
            sValue = dataWs.Cells(lCheckRowNo, lCol).text
            If sValue = "" Then
                GoTo CONTINUE1:
            End If
            
            'コンボボックスの項目の場合以下の入力チェックを実施
            If sType = CellType_ComboBox And lCol > (idColno + 1) And sValue > "" Then
            
                With tErrorInfo
                    .sErrRange = GetColNum2Txt(lCol) & lCheckRowNo
                    .lErrRow = lCheckRowNo
                    .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorStartCnt + lSubErrorCnt
                End With
          
                If CheckType(lErrorCnt, tErrorInfo, SheetNo, dataWs, sType, 0, sValue, lCheckRowNo, lCol, ErrorUmuNo) = False Then
                ' エラーの場合は処理全体のエラー件数を加算
                    lSubErrorCnt = lSubErrorCnt + 1
                End If
            End If
            
            '-----------------------------------------------
            '最大桁数　チェック
            '-----------------------------------------------
            If WhereData(lLoopCnt, Define_Max_Length) <> "" Then
            
                '最大桁数
                iMaxLen = WhereData(lLoopCnt, Define_Max_Length)
            
                '入力値あり かつ 最大桁数が０以上の場合にチェックする
                If sValue <> "" And iMaxLen > 0 Then
                
                    '入力値の桁数取得（半角全角考慮しない）
                    iCheckLen = Len(sValue)

                    If iCheckLen > iMaxLen Then

                        '最大桁数エラー
                        lSubErrorCnt = lSubErrorCnt + 1
                        lErrorCnt = lErrorCnt + 1
                        
                        'エラー情報格納
                        With tErrorInfo
                            .lErrRow = lCheckRowNo
                            .sErrRange = GetColNum2Txt(lCol) & lCheckRowNo
                            .sErrKubun = dataWs.Cells(lCheckRowNo, (idColno + 1))
                            .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorStartCnt + lSubErrorCnt
                            .sErrMsg = GetMessage("941060018", CStr(iMaxLen)) '{0}文字以内で入力して下さい。
                        End With

                        'エラー情報表示
                        If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                            GoTo CONTINUE2:
                        End If
                    End If
                End If

            End If

            '-----------------------------------------------
            '数値　チェック
            '-----------------------------------------------
            '数値の場合
            If sType = CellType_Number Then
                                
                '後ろの空白を除去
                sValue = Trim(sValue)
                'カンマを除去
                sValue = Trim(Replace(sValue, ",", ""))
                
                '数値入力時に、セルの列幅以上の桁数の場合に「#####」と表示された場合は、セルのValue値を使用する
                If sValue > "" Then
                    arrDt = Split(sValue, "#")
                    If UBound(arrDt) > 0 Then
                        If UBound(arrDt) = Len(sValue) Then
                            'ALL # なので、.Valueに実際の値が入っているのでそちらを使用
                            sValue = dataWs.Cells(lCheckRowNo, lCol).Value
                        End If
                    End If
                End If
                
                '全角があるか
                Dim sValue2 As String: sValue2 = StrConv(sValue, vbFromUnicode)

                '数値型以外、全角あり　は、エラー
                If IsNumeric(sValue) = False Or Len(sValue) <> LenB(sValue2) Then
                    '数値エラー
                    lSubErrorCnt = lSubErrorCnt + 1
                    lErrorCnt = lErrorCnt + 1
                    
                    'エラー情報格納
                    With tErrorInfo
                        .lErrRow = lCheckRowNo
                        .sErrRange = GetColNum2Txt(lCol) & lCheckRowNo
                        .sErrKubun = dataWs.Cells(lCheckRowNo, (idColno + 1))
                        .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorStartCnt + lSubErrorCnt
                        .sErrMsg = GetMessage("941130004", "", "") '941130004：数値で入力してください。
                        
                    End With

                    'エラー情報表示
                    If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
                        GoTo CONTINUE2:
                    End If

                End If

            End If
                    
CONTINUE1:
            
        Next lRow

    Next lCol

CONTINUE2:
        
    GetInputSheetCheck_Keta = lSubErrorCnt
    
    Exit Function

ErrHandler:
    
    GetInputSheetCheck_Keta = -1
'    Call Sheet_AutoFilterOff(Worksheets(SheetNo).Name)
    '異常時、表示の設定を行う
    Call ErrorExit("GetInputSheetCheck_Keta()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' 入力チェック時、コンボボックスのリストが１件の場合、リストの１件目をデフォルト値として表示する用
' 条件：下記の場合は、デフォルト値を表示する
'    ①入力必須項目：前処理でチェック済
'    ②未入力　　　：前処理でチェック済
'    ③列タイプ：5:コンボ, 6:リスト, 8:選択リスト画面
'    ④リストが１件のみ（この値を表示）
'***************************************

Public Sub ShowComboBox_Hidden(ByVal SheetNo As Integer _
                             , ByVal Target As Range _
                             , ByVal sCellType As String _
                             , ByVal sFactoryId As String _
                             , ByRef p_valColNo As Long _
                             , ByRef sDefaultId As String _
                             , ByRef sDefaultVal As String)
On Error GoTo ErrHandler
    sDefaultId = ""   'デフォルト値ID
    sDefaultVal = ""  'デフォルト値名称
    p_valColNo = 0    '選択ID値格納先列番号
    
    '定義情報を取得
    Dim valColNo As Long
    Dim grpId As Long
    Dim linkColNo As Long
    Dim arr() As String

    '工場ID取得
    Dim lFactoryIdColNo As Long
    If G_FactoryIdCnt <= 0 Then
        '工場ID列取得
        Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)
    End If
    lFactoryIdColNo = G_FactoryIdData(SheetNo, 2)
    
    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)


    Select Case sCellType
        Case CellType_ComboBox, CellType_FormList
            'コンボボックス、選択リスト画面
        
            '選択ID値格納先列番号
            valColNo = GetValColNo(SheetNo, Target)
            If valColNo <= 0 Then
                GoTo CONTINUE2:
            End If
            
            '選択項目グループID
            grpId = GetGrpId(SheetNo, Target)
            '連動元列番号
            linkColNo = GetLinkColNo(SheetNo, Target)
                       
            Dim linkVal As String
            If linkColNo > 0 Then
                linkVal = Worksheets(SheetNo_Input).Cells(Target.row, linkColNo)
            End If
            
            'グループIDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
            If linkColNo > 0 Then
                If linkVal <> "" Then

                    '連動元値がある場合、連動元値と共通アイテムでフィルターを掛ける
                    '「0 or 空 or 選択ID」でフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", linkVal, "=") _
                        , Operator:=xlFilterValues
                
                Else
                    '連動元値がない場合、共通アイテムでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", "=") _
                        , Operator:=xlFilterValues
                End If
            End If
        
            If sFactoryId > "0" Then
                If CInt(lFactoryIdColNo) < valColNo Then
                    '工場IDでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", sFactoryId, "=") _
                        , Operator:=xlFilterValues
                End If
            End If
            
        Case CellType_MultiListBox
            '複数選択リストボックス

            '選択項目グループIDを取得
            grpId = GetGrpId(SheetNo, Target)
            
            '選択項目グループIDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId

        Case Else
            Exit Sub
    End Select
    

    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)

    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    '可視セルの行をループ
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Item_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

      RowNo = RowNo + 1
      For ColNo = 1 To 4
        '可視セルの値を配列に入力
        filteredData(RowNo, ColNo) = visibleRow.Cells(1, ColNo)
      Next
CONTINUE1:
    Next

    'フィルタを解除
    itemWs.Range("A1").AutoFilter

    'リスト件数が１件の場合、「入力必須項目かつ未入力の項目」のデフォルト値となる
    If RowNo = 1 Then
        'デフォルト値を変数にセットして返す
        If filteredData(1, Item_ColNo_Text) > "" Then
            '選択ID値格納先列番号
            p_valColNo = valColNo
            'ID値
            sDefaultId = filteredData(1, Item_ColNo_Id)
            '名称
            sDefaultVal = filteredData(1, Item_ColNo_Text)
    ElseIf RowNo > 1 Then
        'デフォルト値を変数にセットして返す
        If filteredData(1, Item_ColNo_Text) > "" Then
            '選択ID値格納先列番号
            p_valColNo = valColNo
            'ID値
            sDefaultId = filteredData(1, Item_ColNo_Id)
            '名称
            sDefaultVal = filteredData(1, Item_ColNo_Text)
        End If
        End If
    End If

CONTINUE2:
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ShowComboBox_Hidden()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 整合性チェック用
' 5:コンボ・8:検索UI・6:複数選択：IDと選択文字列の整合性チェック
'       未選択時、IDが設定済みの場合、IDをクリア
'       選択時、選択アイテムシートに該当ありかチェックする。該当なしの場合、エラーとする。
'       選択時、IDが設定済みの場合、選択アイテムシートのIDと同じかチェック。異なる場合、正しいIDを設定する。
'       選択時、IDが未設定の場合、選択アイテムシートのIDを設定
'***************************************

Public Sub ShowComboBox_Check(ByVal SheetNo As Integer _
                            , ByVal Target As Range _
                            , ByVal sValue As String _
                            , ByVal sCellType As String _
                            , ByVal sFactoryId As String _
                            , ByRef p_valColNo As Long _
                            , ByRef sDefaultId As String _
                            , ByRef sDefaultVal As String _
                            , ByRef bReturn As Boolean _
                            , ByRef errMsg As String)
On Error GoTo ErrHandler
    sDefaultId = ""   'デフォルト値ID
    sDefaultVal = ""  'デフォルト値名称
    p_valColNo = 0    '選択ID値格納先列番号
    bReturn = False   'チェック結果 True:正常, False:異常
    
    '定義情報を取得
    Dim valColNo As Long
    Dim grpId As Long
    Dim linkColNo As Long
    Dim arr() As String
    
    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)

    Dim lFactoryIdColNo As Long
    If G_FactoryIdCnt <= 0 Then
        '工場ID列取得
        Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)
    End If
    lFactoryIdColNo = G_FactoryIdData(SheetNo, 2)
    
    '標準アイテム未使用工場ID格納用配列
    Dim unuseFactoryIdArray() As String
    Dim unuseFactoryIdCnt As Long
    unuseFactoryIdCnt = 0
    
    '標準アイテム未使用チェック用エラーフラグ
    Dim isUnuseError As Boolean
    isUnuseError = False
    
    '複数選択リストで選択可能なアイテム件数(非表示シートにフィルタをかけた後のレコード数)
    Dim multiItemCnt As Long
    multiItemCnt = 0
    
    '複数選択リストで選択されているアイテムのID(パイプ区切り)
    Dim multiItemId As String

    Select Case sCellType
        Case CellType_ComboBox, CellType_FormList
            'コンボボックス、選択リスト画面
        
            '選択ID値格納先列番号
            valColNo = GetValColNo(SheetNo, Target)
            If valColNo <= 0 Then
                GoTo CONTINUE2:
            End If

            '選択項目グループID
            grpId = GetGrpId(SheetNo, Target)
            '連動元列番号
            linkColNo = GetLinkColNo(SheetNo, Target)
                       
            Dim linkVal As String
            If linkColNo > 0 Then
                linkVal = Worksheets(SheetNo_Input).Cells(Target.row, linkColNo)
            End If
            
            'グループIDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
            If linkColNo > 0 Then
                If linkVal <> "" Then
                    '連動元値がある場合、連動元値と共通アイテムでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", linkVal, "=") _
                        , Operator:=xlFilterValues

                Else
                    '連動元値がない場合、共通アイテムでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_Parent, Criteria1:=Array("0", "=") _
                        , Operator:=xlFilterValues
                End If
            End If
            If sFactoryId > "0" Then
                If CInt(lFactoryIdColNo) < valColNo Then
                    '工場IDでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", sFactoryId, "=") _
                        , Operator:=xlFilterValues
                End If
            Else
                'コンボボックスの場合
                If sCellType = CellType_ComboBox Then
                    '共通工場=0でフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", "=") _
                        , Operator:=xlFilterValues
                End If
            End If

            '選択名称でフィルターを掛ける
            itemWs.Range("A1").AutoFilter Item_ColNo_Text, sValue
        
        Case CellType_MultiListBox
            '複数選択リストボックス
            
            '選択ID値格納先列番号
            valColNo = GetValColNo(SheetNo, Target)
            If valColNo <= 0 Then
                GoTo CONTINUE2:
            End If

            '選択項目グループIDを取得
            grpId = GetGrpId(SheetNo, Target)
            
            '選択項目グループIDでフィルターを掛ける
            itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
            If sFactoryId > "0" Then
                If CInt(lFactoryIdColNo) < valColNo Then
                    '工場IDでフィルターを掛ける
                    itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", sFactoryId, "=") _
                        , Operator:=xlFilterValues
                End If
            Else
                '共通工場=0でフィルターを掛ける
                itemWs.Range("A1").AutoFilter Field:=Item_ColNo_FactoryId, Criteria1:=Array("0", "=") _
                    , Operator:=xlFilterValues
            End If
            '選択名称でフィルターを掛ける
            'itemWs.Range("A1").AutoFilter Item_ColNo_Text, sValue
            arr = Split(sValue, DelimiterName)
            itemWs.Range("A1").AutoFilter Item_ColNo_Text, Array(arr) _
                    , Operator:=xlFilterValues

        Case Else
            Exit Sub
    End Select
    
    ' 入力チェック開始時のIDの値を取得
    Dim selectedItemId As Long
    selectedItemId = Worksheets(SheetNo).Cells(Target.row, valColNo).Value
    ' 入力チェック開始時に選択されているアイテムの行番号
    Dim selectedItemRowNo As Long
    selectedItemRowNo = 0
    
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)

    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    '可視セルの行をループ
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Item_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If
        
        '標準アイテム未使用工場IDを「|」で分割する
        ReDim unuseFactoryIdArray(0)
        unuseFactoryIdArray = Split(visibleRow.Cells(1, Item_ColNo_UnuseFactoryId), DelimiterId)

        '標準アイテム未使用工場IDのうち､レコードの工場ID列の値と同一のものがあるか確認
        For unuseFactoryIdCnt = 0 To UBound(unuseFactoryIdArray)
            '未使用工場の場合はスキップ
            If (unuseFactoryIdArray(unuseFactoryIdCnt) = sFactoryId) Then
                isUnuseError = True
                GoTo CONTINUE1:
            End If
        Next
        
        '未使用アイテムが選択されている場合
        If isUnuseError = True Then
            RowNo = 0
            GoTo CONTINUE1:
        End If
        
        '複数選択リストの場合は選択可能なアイテム数を退避
        If sCellType = CellType_MultiListBox Then
          multiItemCnt = multiItemCnt + 1
        End If
        
      RowNo = RowNo + 1
      For ColNo = 1 To 4
        '可視セルの値を配列に入力
        filteredData(RowNo, ColNo) = visibleRow.Cells(1, ColNo)
        
        '複数選択リストの場合
        If sCellType = CellType_MultiListBox And ColNo = 2 Then
            'IDをパイプ区切りで設定
            multiItemId = multiItemId + Str(visibleRow.Cells(1, ColNo)) + DelimiterId
        End If
        
        ' IDの値が入力チェック開始時に選択されているアイテムのIDと同一の場合
        If ColNo = 2 And selectedItemId = visibleRow.Cells(1, ColNo) Then
            selectedItemRowNo = RowNo
        End If
      Next
CONTINUE1:
    Next

    'フィルタを解除
    itemWs.Range("A1").AutoFilter

    '該当なし
    If RowNo = 0 Then
        '選択ID値格納先列番号
        p_valColNo = valColNo
        'エラー
        bReturn = False
    
    '該当あり
    ElseIf RowNo = 1 Then
        'デフォルト値を変数にセットして返す
        If filteredData(1, Item_ColNo_Text) > "" Then
            '選択ID値格納先列番号
            p_valColNo = valColNo
            'ID値
            sDefaultId = filteredData(1, Item_ColNo_Id)
            '名称
            sDefaultVal = filteredData(1, Item_ColNo_Text)
            '該当あり
            bReturn = True
        End If
    ElseIf RowNo > 1 Then
        'デフォルト値を変数にセットして返す
        If filteredData(1, Item_ColNo_Text) > "" Then
            
            '選択ID値格納先列番号
            p_valColNo = valColNo
            
            '選択されているアイテムが正常な場合、選択されているアイテムにする
            If selectedItemRowNo > 0 Then
                'ID値
                sDefaultId = filteredData(selectedItemRowNo, Item_ColNo_Id)
                '名称
                sDefaultVal = filteredData(selectedItemRowNo, Item_ColNo_Text)
            Else
            '選択されているアイテムが不正な場合は選択可能リストの１番目にする
                'ID値
                sDefaultId = filteredData(1, Item_ColNo_Id)
                '名称
                sDefaultVal = filteredData(1, Item_ColNo_Text)
            End If
            
            '該当あり
            bReturn = True
        End If
    End If
    
    'セルタイプを判定
    If sCellType = CellType_MultiListBox Then
    '複数選択リストの場合、①選択されているアイテム数と非表示シートでフィルタをかけたレコード数が異なる、②未使用アイテムが選択されている場合エラーとする
        If Not multiItemCnt = UBound(arr) + 1 Or isUnuseError = True Then
            '選択ID値格納先列番号
            p_valColNo = valColNo
            'ID値
            sDefaultId = ""
            '名称
            sDefaultVal = ""
            '該当なし
            bReturn = False
        Else
        'エラーではない場合
        sDefaultId = Replace(multiItemId, " ", "")
        End If
    ElseIf sCellType = CellType_FormList Then
        '選択画面の場合、工場IDが空で選択対象が複数存在する場合エラーとする
        If sFactoryId = "" And RowNo > 1 Then
            '選択ID値格納先列番号
            p_valColNo = 0
            'ID値
            sDefaultId = ""
            '名称
            sDefaultVal = ""
            '該当なし
            bReturn = False
            '返り値のエラーメッセージを設定(名称により項目を絞り込むことができません。画面より項目を指定してください。)
            errMsg = GetMessage("141340001")
        End If
    End If

CONTINUE2:
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("ShowComboBox_Check()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 列数をアルファベットに変換
'***************************************
Function GetColNum2Txt(ByVal lngColNum As Long) As String
    On Error GoTo ErrHandler
    
    Dim strAddr As String
    
    strAddr = Cells(1, lngColNum).Address(False, False)
    GetColNum2Txt = Left(strAddr, Len(strAddr) - 1)
    
    Exit Function

ErrHandler:

    GetColNum2Txt = ""
    '異常時、表示の設定を行う
    Call ErrorExit("GetColNum2Txt()" & Err.Number & ":" & Err.Description)

End Function

'***************************************
' メッセージ翻訳シートより、メッセージ翻訳取得
'    sHonyakuID：翻訳ID
'    sReplace1 ：{0}の置換文字列（省略可）
'    sReplace2 ：{1}の置換文字列（省略可）
'    sReplace3 ：{2}の置換文字列（省略可）
'***************************************
Function GetMessage(ByVal sHonyakuID As String _
                  , Optional ByVal sReplace1 As String = "" _
                  , Optional ByVal sReplace2 As String = "" _
                  , Optional ByVal sReplace3 As String = "") As String

    On Error GoTo ErrHandler
    Dim rng As Range, searchRng As Range
    Dim sHonyaku As String
    sHonyaku = ""
    
    '検索
    Set searchRng = Worksheets(SheetName_Message).Range("A:C")
    Set rng = searchRng.Find(sHonyakuID)
    
    If rng Is Nothing Then
       ' 該当なし
       GetMessage = ""
       Exit Function
    End If
        
    'メッセージ翻訳
    sHonyaku = Worksheets(SheetName_Message).Cells(rng.row, 3)
        
    '１つ目置き換え
    If sReplace1 <> "" Then
        sHonyaku = Replace(sHonyaku, "{0}", sReplace1)
    End If
    
    '２つ目置き換え
    If sReplace2 <> "" Then
        sHonyaku = Replace(sHonyaku, "{1}", sReplace2)
    End If
    
    '３つ目置き換え
    If sReplace3 <> "" Then
        sHonyaku = Replace(sHonyaku, "{2}", sReplace3)
    End If

    GetMessage = sHonyaku

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetMessage()" & Err.Number & ":" & Err.Description)
    GetMessage = ""
End Function

'***************************************
' コンボボックス変更時、紐づく項目をクリアする
'
'***************************************
Function SetInputLinkClear(ByVal SheetNo As Integer _
                         , ByVal Target As Range _
                         , ByVal lColNo As Long) As Boolean
    On Error GoTo ErrHandler

    Dim rng As Range, searchRng As Range
    Dim sKakunouId As String
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    Dim inputWs As Worksheet
    Set inputWs = Worksheets(SheetNo)
    Dim lastRow As Long
    Dim lRowNo As Long
    lastRow = defineWs.Cells(defineWs.Rows.Count, Define_ColNo_SheetNo).End(xlUp).row
    lRowNo = Target.row
    
    '検索
    Set searchRng = defineWs.Range(defineWs.Cells(Define_RowNo_Start, Define_ColNo_Link), _
            defineWs.Cells(lastRow, Define_ColNo_Link))
    Set rng = searchRng.Find(lColNo)

    If rng Is Nothing Then
       ' 該当なし
       SetInputLinkClear = False
       Exit Function
    End If

    Dim msg As String
    Dim myCell As Range
    Set myCell = rng
    Dim lParentId As Long  '親ID
    Do
        
        '親ID取得
        lParentId = GetItemInfo(SheetNo, Target, lColNo)
        If lParentId = -1 Then
            '
        ElseIf lParentId = Item_ColNo_Parent_Default Then
            '標準アイテム、紐づく項目をクリアしない
        Else
        
            '標準アイテム以外
            '列番号セルクリア
            inputWs.Cells(lRowNo, Worksheets(SheetName_Define).Cells(rng.row, Define_ColNo_ColNo)) = ""
            '選択ID値格納先列番号セルクリア
            inputWs.Cells(lRowNo, Worksheets(SheetName_Define).Cells(rng.row, Define_ColNo_Val)) = ""
            
            '選択ID値格納先列番号に紐づく項目をさらにクリア（自モジュールを呼ぶ）
            If SetInputLinkClear(SheetNo, Target, Worksheets(SheetName_Define).Cells(rng.row, Define_ColNo_Val)) = -1 Then
                SetInputLinkClear = False
                Exit Function
            End If
       End If
        
        '次の選択ID値格納先列番号へ
        Set myCell = rng.FindNext(myCell)
    Loop While myCell.row <> rng.row

    '選択ID値格納先列番号
    sKakunouId = Worksheets(SheetName_Define).Cells(rng.row, Define_ColNo_Val)

    SetInputLinkClear = True
    Exit Function
    
ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetInputLinkClear()" & Err.Number & ":" & Err.Description)
    SetInputLinkClear = False
End Function

'***************************************
' 選択アイテムシートの値の取得
'***************************************
Function GetItemInfo(ByVal SheetNo As Integer _
                   , ByVal Target As Range _
                   , ByVal ColNo As Long) As Long
On Error GoTo ErrHandler
    '定義情報を取得
    Dim valColNo As Long
    Dim linkColNo As Long
    Dim ParentId As Long
    ParentId = -1
    GetItemInfo = -1
    '選択ID値格納先列番号の列
    valColNo = GetValColNo(SheetNo, Target)
    If valColNo <= 0 Then
        '選択ID値格納先列番号なし
        GoTo CONTINUE2:
    End If
    '表示前の設定値を保持
    Dim prevIDValue As String
    prevIDValue = Worksheets(SheetNo_Input).Cells(Target.row, valColNo)
    
    '連動元列番号
    linkColNo = GetLinkColNo(SheetNo, Target)
    
    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)

    '連動元列番号、機器IDでフィルターを掛ける
    itemWs.Range("A1").AutoFilter Define_ColNo_Link, linkColNo
    itemWs.Range("A1").AutoFilter Item_ColNo_Id, prevIDValue, xlOr, 0
        
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)
    
    Dim RowNo As Long
    RowNo = 0
    Dim visibleRow As Range
    Dim kDat As String
    
    '可視セルの行をループ（１行のはず）
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Item_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

        RowNo = RowNo + 1
        ParentId = visibleRow.Cells(1, Item_ColNo_Parent)
CONTINUE1:
    Next
    
    'フィルタを解除
    itemWs.Range("A1").AutoFilter
    '親IDを返す
    GetItemInfo = ParentId

CONTINUE2:
    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetItemInfo()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' 列タイプ毎のチェック
'     型と入力値が合っているかチェックする
'     テキストの文字列に改行コードがあれば消去
'     選択項目(コンボ、検索UI、複数選択)のIDと文字列が正しい組み合わせかチェックする
'***************************************
Function CheckType(ByRef lErrorCnt As Long _
                 , ByRef tErrorInfo As typeErrorInfo _
                 , ByVal SheetNo As Integer _
                 , ByVal dataWs As Worksheet _
                 , ByVal sType As String _
                 , ByVal sFactoryId As String _
                 , ByRef sValue As String _
                 , ByVal lRow As Long _
                 , ByVal lCol As Long _
                 , ByVal ErrorUmuNo As Long) As Boolean

On Error GoTo ErrHandler
    Dim sErrMsg As String
    Dim sReplaceString As String
    sErrMsg = ""
    sReplaceString = ""        '置換文字列：消去する
    
    If sValue = "" Then
        Select Case sType
            Case CellType_None, CellType_Text, CellType_Text_NewLine, CellType_Number, CellType_Date, CellType_Time
                '未入力時、チェックは行わない
                CheckType = True
                Exit Function
        End Select
    End If

    '入力チェック
    Select Case sType
        Case CellType_Text           '文字列（改行なし）
            
            '改行コード「\r,\n,\r\n」を除去
            If InStr(sValue, vbLf) > 0 Then
                sValue = Replace(sValue, vbLf, sReplaceString)
            End If
            If InStr(sValue, vbCr) > 0 Then
                sValue = Replace(sValue, vbCr, sReplaceString)
            End If
            If InStr(sValue, vbCrLf) > 0 Then
                sValue = Replace(sValue, vbCrLf, sReplaceString)
            End If
            
        Case CellType_Text_NewLine   '文字列（改行あり）
        
            '改行コード「\r」を除去
            If InStr(sValue, vbCr) > 0 Then
                sValue = Replace(sValue, vbCr, sReplaceString)
            End If
        
        Case CellType_Number         '数値
            Dim sValue2 As String: sValue2 = StrConv(sValue, vbFromUnicode)
            If IsNumeric(sValue) = False Or Len(sValue) <> LenB(sValue2) Then
                '数値エラー
                '941130004：数値で入力してください。
                sErrMsg = GetMessage("941130004")
            End If
            
        Case CellType_Date           '年月日
            If CheckTypeDate(sValue) = False Then
                '日付エラー
                '941270003：日付で入力して下さい。
                sErrMsg = GetMessage("941270003")
            End If
            
        Case CellType_Time           '時刻
            If CheckTypeTime(lRow, lCol) = False Then
                '時刻エラー
                '941120015：時刻で入力してください。
                sErrMsg = GetMessage("941120015")
            End If
            
        Case CellType_ComboBox, CellType_MultiListBox, CellType_FormList
            'コンボボックス、複数選択リストボックス、選択リスト画面
            Dim rRange As Range
            Dim sDefauleId As String
            Dim sDefauleVal As String
            Dim lvalColNo As Long
            Dim bReturn As Boolean
            Dim errMsg As String
            Set rRange = dataWs.Range(GetColNum2Txt(lCol) & CStr(lRow))
            Call ShowComboBox_Check(SheetNo, rRange, sValue, sType, sFactoryId, lvalColNo, sDefauleId, sDefauleVal, bReturn, errMsg)
            If bReturn = False Then
                '選択エラー
                'IDクリア
                If lvalColNo > 0 Then
                    dataWs.Cells(lRow, lvalColNo) = ""
                End If
                
                ' チェック後にエラーメッセージが設定されている場合は設定されているものをセット
                If Not errMsg = "" Then
                    sErrMsg = errMsg
                Else
                '何も設定されていない場合
                    '141140004：選択内容が不正です。
                    sErrMsg = GetMessage("141140004")
                End If
                
            Else
                If lvalColNo > 0 Then
                    dataWs.Cells(lRow, lvalColNo) = sDefauleId
                    
                    'チェック正常終了後、拡張項目を設定する
                    If Not ActiveCell Is Nothing Then
                        '拡張項目があれば表示
                        Call modCommon.GetKakuchoCheck(rRange, rRange.row, 1)
                    End If
                End If
            End If
            
        Case Else
        
    End Select
    
    'エラー出力
    If sErrMsg <> "" Then
        lErrorCnt = lErrorCnt + 1
        'エラー情報格納
        With tErrorInfo
            .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
            .sErrMsg = sErrMsg
        End With
        
        'エラー情報表示
        If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
            CheckType = False
            Exit Function
        End If
        CheckType = False
    Else
        CheckType = True
    End If

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("CheckType()" & Err.Number & ":" & Err.Description)
    CheckType = False
                        
End Function

'***************************************
' 日付型チェック
'***************************************
Function CheckTypeDate(ByVal sValue As String) As Boolean
On Error GoTo ErrHandler

    Dim xDate1 As String, xDate2 As String
    Dim arr() As String
    Dim iCnt As Integer
    
    sValue = Replace(sValue, "-", "/")
    sValue = Replace(sValue, ".", "/")
   
    xDate1 = ""
    '「/」の個数により、年のみ・年月のみの場合も、日付になるように調整し、日付の型チェックを行う
    arr = Split(sValue, "/")
    iCnt = UBound(arr)
    Select Case iCnt
    Case 0
        xDate1 = sValue & "/01/01"
    Case 1
        xDate1 = sValue & "/01"
    Case 2
        xDate1 = sValue
    End Select
        
    If IsDate(xDate1) = True Then
        CheckTypeDate = True

    Else
        '日付エラー
        CheckTypeDate = False

    End If

Exit Function

ErrHandler:

    '異常時、表示の設定を行う
    Call ErrorExit("CheckTypeDate()" & Err.Number & ":" & Err.Description)
    CheckTypeDate = False
  
End Function

'***************************************
' 時刻型チェック
'***************************************
Function CheckTypeTime(ByVal lRow As Long, ByVal lCol As Long) As Boolean
On Error GoTo ErrHandler

    '時刻か？
    If IsDate(ActiveWorkbook.Worksheets(SheetNo_Input).Cells(lRow, lCol).text) = True Then
        CheckTypeTime = True
    Else
        '時刻エラー
        CheckTypeTime = False
    End If

    Exit Function

ErrHandler:

    '異常時、表示の設定を行う
    Call ErrorExit("CheckTypeTime()" & Err.Number & ":" & Err.Description)
    CheckTypeTime = False
  
End Function

'***************************************
' 書式チェック
'***************************************
Function CheckFormat(ByRef lErrorCnt As Long _
                   , ByRef tErrorInfo As typeErrorInfo _
                   , ByVal dataWs As Worksheet _
                   , ByVal sType As String _
                   , ByVal sFormat As String _
                   , ByVal sValue As String _
                   , ByVal ErrorUmuNo As Long _
                   , ByVal lRow As Long _
                   , ByVal lCol As Long) As Boolean

On Error GoTo ErrHandler
    Dim sErrMsg As String
    sErrMsg = ""
    If sValue = "" Then
        '未入力時、チェックは行わない
        CheckFormat = True
        Exit Function
    End If
    
    '列タイプにより、書式チェック
    Select Case sType
        Case CellType_Number         '数値
            If CheckFormatNumeric(sValue, sFormat) = False Then
                '数値：書式エラー
                '941190002：{0}で入力してください。。
                sErrMsg = GetMessage("941190002", sFormat)
            End If
            
        Case CellType_Date           '年月日
            If CheckFormatDate(sValue, sFormat, lRow, lCol) = False Then
                '日付：書式エラー
                '941190002：{0}で入力してください。。
                sErrMsg = GetMessage("941190002", sFormat)
            End If
            
        Case CellType_Time           '時刻
            If CheckFormatTime(sValue, sFormat, lRow, lCol) = False Then
                '時刻：書式エラー
                '941190002：{0}で入力してください。。
                sErrMsg = GetMessage("941190002", sFormat)
            End If
            
        Case Else
        
    End Select
    
    'エラー出力
    If sErrMsg <> "" Then
        lErrorCnt = lErrorCnt + 1
        'エラー情報格納
        With tErrorInfo
            .lOutRow = (ErrorInfo_RowNo_Start - 1) + lErrorCnt
            .sErrMsg = sErrMsg
        End With
        
        'エラー情報表示
        If SetErrorInfo(tErrorInfo, dataWs, ErrorUmuNo) = False Then
            CheckFormat = False
            Exit Function
        End If
        CheckFormat = False
    Else
        CheckFormat = True
    End If

    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("CheckFormat()" & Err.Number & ":" & Err.Description)
    CheckFormat = True
End Function

'***************************************
' 数値：書式チェック
'***************************************
Function CheckFormatNumeric(ByVal sValue As String _
                          , ByVal sFormat As String) As Boolean
On Error GoTo ErrHandler

    Dim arrFormat() As String
    Dim arrData() As String
    Dim iCntFormat As Integer
    Dim iCntData As Integer
    Dim sFormatSeisu As String
    Dim sDataSeisu As String
    
    If sFormat = "" Then
        CheckFormatNumeric = True
        Exit Function
    End If
    
    '整数部・小数部 分割
    arrFormat = Split(sFormat, ".")
    iCntFormat = UBound(arrFormat)
    arrData = Split(sValue, ".")
    iCntData = UBound(arrData)
    
    If iCntFormat <> iCntData Then
        'エラー （入力値を書式で小数点の存在が異なる）
        CheckFormatNumeric = False
        GoTo GOTO_END:
    End If
    
    If iCntFormat = 0 Then
        '整数部のみ
        sFormatSeisu = sFormat
        sDataSeisu = sValue
    ElseIf iCntFormat > 0 Then
        '小数点あり、整数部 取得
        sFormatSeisu = arrFormat(0)
        sDataSeisu = arrData(0)
    End If
    
    'カンマ消去
    sFormatSeisu = Replace(sFormatSeisu, ",", "")
    sDataSeisu = Replace(sDataSeisu, ",", "")
    If Len(sFormatSeisu) < Len(sDataSeisu) Then
        '整数部の桁数が大きくてもエラーにしない（入力桁数に関わらず「#,##0」
        ''整数部の桁数が大きい
        'CheckFormatNumeric = False
        'GoTo GOTO_END:
        
    End If
    
    If iCntFormat > 0 Then
        '書式は小数点以下なし
        If iCntData = 0 Then
            If Len(arrFormat(1)) > 0 Then
                '書式は小数点以下なし、データは小数点以下入力あり
                CheckFormatNumeric = False
                GoTo GOTO_END:

            End If
        
        '書式は小数点以下あり
        ElseIf Len(arrFormat(1)) < Len(arrData(1)) Then
            '小数点以下の桁数が大きい
            CheckFormatNumeric = False
            GoTo GOTO_END:
            
        End If
    End If

    CheckFormatNumeric = True
    
GOTO_END:
    Exit Function

ErrHandler:

    '異常時、表示の設定を行う
    Call ErrorExit("CheckFormatNumeric()" & Err.Number & ":" & Err.Description)
    CheckFormatNumeric = False
    
End Function

'***************************************
' 日付：書式チェック
'***************************************
Function CheckFormatDate(ByVal sValue As String _
                       , ByVal sFormat As String _
                       , ByVal lRow As Long _
                       , ByVal lCol As Long) As Boolean
On Error GoTo ErrHandler

    Dim arrFormat() As String
    Dim arrData() As String
    Dim iCntFormat As Integer
    Dim iCntData As Integer
    Dim sFormat1 As String
       
    If sFormat = "" Then
        CheckFormatDate = True
        Exit Function
    End If
    
    sFormat1 = sFormat

    '「/」の個数により、判断
    arrFormat = Split(sFormat1, "/")
    iCntFormat = UBound(arrFormat)
    arrData = Split(sValue, "/")
    iCntData = UBound(arrData)
    
    If iCntFormat = iCntData Then
        CheckFormatDate = True
    Else
        'セルの書式とフォーマット後と同じかチェックする
        '        （セルの書式無しの場合、「2022/12」と入力しても、勝手に「2022/12/01」となってしまうExcelの標準機能によるNG判定を回避する為）
        '        （書式「YYYY/MM」の場合、「2022/12/01」はイコールではない為NGですよね。しかしセルの見た目は「2022/12」となっていて、OKなのにNGになっちゃう…）
        '        再度、セルの書式でフォーマットし、定義の書式と比較を行う
        Dim sLocalLoval As String
        sLocalLoval = StrConv(Worksheets(SheetNo_Input).Cells(lRow, lCol).NumberFormatLocal, vbUpperCase) '大文字へ変換
        'セルの書式が日付の場合、再度フォーマットをかける
        If InStr(sLocalLoval, "YY") > 0 Or InStr(sLocalLoval, "MM") > 0 Or InStr(sLocalLoval, "DD") > 0 Then
            sValue = Format(sValue, sLocalLoval)

            arrData = Split(sValue, "/")
            iCntData = UBound(arrData)
            If iCntFormat = iCntData Then
                CheckFormatDate = True
            Else
                '日付：書式エラー
                CheckFormatDate = False
            End If
        Else
            '日付：書式エラー
            CheckFormatDate = False
        End If
    End If

Exit Function

ErrHandler:

    '異常時、表示の設定を行う
    Call ErrorExit("CheckFormatDate()" & Err.Number & ":" & Err.Description)
    CheckFormatDate = False
  
End Function

'***************************************
' 時刻：書式チェック
'***************************************
Function CheckFormatTime(ByVal sValue As String _
                       , ByVal sFormat As String _
                       , ByVal lRow As Long _
                       , ByVal lCol As Long) As Boolean
On Error GoTo ErrHandler

    Dim arrFormat() As String
    Dim arrData() As String
    Dim iCntFormat As Integer
    Dim iCntData As Integer
       
    If sFormat = "" Then
        CheckFormatTime = True
        Exit Function
    End If
    
    '「:」の個数により、判断
    arrFormat = Split(sFormat, ":")
    iCntFormat = UBound(arrFormat)
    arrData = Split(sValue, ":")
    iCntData = UBound(arrData)
    
    If iCntFormat = iCntData Then
        CheckFormatTime = True
    Else
        'セルの書式とフォーマット後と同じかチェックする
        '        セルの書式でフォーマットし、定義の書式と比較を行う
        Dim sLocalLoval As String
        sLocalLoval = StrConv(Worksheets(SheetNo_Input).Cells(lRow, lCol).NumberFormatLocal, vbUpperCase) '大文字へ変換
        'セルの書式が日付の場合、再度フォーマットをかける
        If InStr(sLocalLoval, "h") > 0 Or InStr(sLocalLoval, "n") > 0 Or InStr(sLocalLoval, "s") > 0 Or InStr(sLocalLoval, "t") > 0 Then
            sValue = Format(sValue, sLocalLoval)

            arrData = Split(sValue, "/")
            iCntData = UBound(arrData)
            If iCntFormat = iCntData Then
                CheckFormatTime = True
            Else
                '時刻：書式エラー
                CheckFormatTime = False
            End If
        Else
            '時億：書式エラー
            CheckFormatTime = False
        End If

        CheckFormatTime = False
    End If

Exit Function

ErrHandler:

    '異常時、表示の設定を行う
    Call ErrorExit("CheckFormatTime()" & Err.Number & ":" & Err.Description)
    CheckFormatTime = False
    
End Function

'***************************************
' 行コピー時、KEY項目をクリア
'***************************************
Public Sub SetKeyClear(ByVal SheetNo As Integer _
                     , ByVal Target As Range)
On Error GoTo ErrHandler
    Dim rng_addr As String
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    With Application
        If .CutCopyMode = xlCopy Then
            'ペースト後、ここにくる
            '行が選択されているか判定
            rng_addr = Selection.Address(False, False)
            rng_addr = Replace(rng_addr, ":", "")
            If IsNumeric(rng_addr) Then
                'ペースト後に行選択されている場合、行コピーとみなす
                '定義情報シートよりKEY列を取得し、クリアする
                Call SetKeyColNoClear(SheetNo, Target)
                
            End If
        End If
    End With

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetKeyClear()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' KEY項目を取得
'***************************************
Public Sub SetKeyColNoClear(ByVal SheetNo As Integer _
                          , ByVal Target As Range)
    
On Error GoTo ErrHandler
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    Dim inputWs As Worksheet
    Set inputWs = Worksheets(SheetNo)
    Dim lRowNo As Long
    lRowNo = Target.row
    
    '列区分でフィルターを掛ける
    defineWs.Range("A1").AutoFilter Define_ColNo_SheetNo, SheetNo
    defineWs.Range("A1").AutoFilter Define_ColNo_ColumnDivision, ColumnDivision_Key
    
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = defineWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)
    
    'セルを編集するため、一時的にイベント無効化
    Application.EnableEvents = False

    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    '可視セルの行をループ
    For Each visibleRow In defineWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Define_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

        inputWs.Cells(lRowNo, visibleRow.Cells(1, Define_ColNo_ColNo)) = ""
CONTINUE1:
    Next
    
    'イベント有効化
    Application.EnableEvents = True

    'フィルタを解除
    defineWs.Range("A1").AutoFilter
    
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetKeyColNoClear()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' 例外エラー発生時
'***************************************
Sub ErrorExit(ByVal sErrInfo As String)

On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If


    '保護解除
    Call Book_UnProtect
    Call Sheet_UnProtect(1) '１シート目　保護解除
    
    '非表示
    Call Sheet_Visible_off(SheetName_Define)
    Call Sheet_Visible_off(SheetName_Item)
    
    '保護設定
    Call Sheet_Protect(1)  '１シート目　保護
    Call Book_Protect

    'イベント有効化
    Application.EnableEvents = True
    
    'メッセージ
    MsgBox ("システムエラーが発生しました。\n" & sErrInfo)
    
    Exit Sub

ErrHandler:
    Exit Sub
End Sub

'***************************************
' オートフィルター
'***************************************
Sub Sheet_AutoFilterOn(ByVal SheetName As String, ByVal RowNo As Integer, ByVal ErrorUmuNo As Integer)
On Error GoTo ErrHandler
    Dim sColStart As String
    Dim sColEnd As String

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If


    If ErrorUmuNo <= 1 Then
        'エラー有無列なし、オートフィルタが設定されていれば、オートフィルターを行にかける。
        If (Worksheets(SheetName).AutoFilterMode = False) Then
            Worksheets(SheetName).Range(RowNo & ":" & RowNo).AutoFilter
        End If
    Else
        sColStart = GetColNum2Txt(1)
        sColEnd = GetColNum2Txt(ErrorUmuNo)
        'オートフィルタが設定されていれば、エラー有無までオートフィルターをかける。
        If (Worksheets(SheetName).AutoFilterMode = False) Then
            Worksheets(SheetName).Range(sColStart & RowNo & ":" & sColEnd & RowNo).AutoFilter
        End If

    End If
    
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_AutoFilterOn()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' オートフィルター解除
'***************************************
Sub Sheet_AutoFilterOff(ByVal SheetName As String)
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'オートフィルタが設定されていれば、オートフィルター解除する。
    If (Worksheets(SheetName).AutoFilterMode = True) Then
        Worksheets(SheetName).Range("A1").AutoFilter
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_AutoFilterOff()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' ブック保護
'***************************************
Sub Book_Protect()
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'パスワードを設定して、ブックを保護
    If ActiveWorkbook.ProtectWindows = False Then
        ActiveWorkbook.Protect Password:=ProtectPassword, Structure:=True, Windows:=True
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Book_Protect()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' ブック保護解除
'***************************************
Sub Book_UnProtect()
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'パスワードを設定して、ブック保護解除
    'If ActiveWorkbook.ProtectWindows = True Then
        ActiveWorkbook.Unprotect Password:=ProtectPassword
    'End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Book_UnProtect()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート保護
'***************************************
Function Sheet_Protect(ByVal SheetNo As Integer)
On Error GoTo ErrHandler

    Sheet_Protect = True
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Sheet_Protect = False
        Exit Function
    End If

    'シートを保護
    '＜許可するもの＞
    '  ・マクロによる操作
    '  ・ロックされたセル範囲の選択
    '  ・ロックされていないセル範囲の選択
    '  ・セルの書式設定
    '  ・列の書式設定
    '  ・オートフィルターの使用
    '  ・オブジェクトの編集
    If Worksheets(SheetNo).ProtectContents = False Then
        Worksheets(SheetNo).Protect UserInterfaceOnly:=True, DrawingObjects:=False, Contents:=True, _
            Scenarios:=True, AllowFormattingCells:=True, AllowFormattingColumns:=True, _
            AllowFiltering:=True
    End If
    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_Protect()" & Err.Number & ":" & Err.Description)
End Function
Sub Sheet_Protect_Name(ByVal SheetName As String)
On Error GoTo ErrHandler


    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'シートを保護
    '＜許可するもの＞
    '  ・マクロによる操作
    '  ・ロックされたセル範囲の選択
    '  ・ロックされていないセル範囲の選択
    '  ・セルの書式設定
    '  ・列の書式設定
    '  ・オートフィルターの使用
    '  ・オブジェクトの編集
    If Worksheets(SheetName).ProtectContents = False Then
        Worksheets(SheetName).Protect UserInterfaceOnly:=True, DrawingObjects:=False, Contents:=True, _
            Scenarios:=True, AllowFormattingCells:=True, AllowFormattingColumns:=True, _
            AllowFiltering:=True
    End If
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_Protect()" & Err.Number & ":" & Err.Description)
End Sub
Sub Sheet_Protect_Error()
On Error GoTo ErrHandler
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_ErrorInfo) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'パスワードを設定して、エラー情報シートを保護
    If Worksheets(SheetName_ErrorInfo).ProtectContents = False Then
        Worksheets(SheetName_ErrorInfo).Protect
    End If
    
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート保護解除
'***************************************
Sub Sheet_UnProtect(SheetNo As Integer)
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If
    
    'パスワードを設定して、シート保護解除
    If Worksheets(SheetNo).ProtectContents = True Then
        Worksheets(SheetNo).Unprotect
    End If
  
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_UnProtect()" & Err.Number & ":" & Err.Description)
End Sub
Sub Sheet_UnProtect_Name(SheetName As String)
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If
    
    'パスワードを設定して、シート保護解除
    If Worksheets(SheetName).ProtectContents = True Then
        Worksheets(SheetName).Unprotect
    End If
  
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_UnProtect()" & Err.Number & ":" & Err.Description)
End Sub
Sub Sheet_UnProtect_Error()
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_ErrorInfo) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    'パスワードを設定して、エラー情報シート保護解除
    If Worksheets(SheetName_ErrorInfo).ProtectContents = True Then
        Worksheets(SheetName_ErrorInfo).Unprotect Password:=ProtectPassword
    End If
    
    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_UnProtect_Error()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート再表示
'***************************************
Sub Sheet_Visible_on(ByVal SheetName As String)
On Error GoTo ErrHandler

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If

    '非表示ならば、再表示
    If Worksheets(SheetName).Visible = False Then
        '再表示する
        Worksheets(SheetName).Visible = True
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_Visible_on()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート非表示
'***************************************
Sub Sheet_Visible_off(ByVal SheetName As String)
On Error GoTo ErrHandler
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If
    
    '表示ならば、非表示
    If Worksheets(SheetName).Visible = True Then
        '非表示
        Worksheets(SheetName).Visible = False
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_Visible_off()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' シート存在チェック
'     セル入力中に別ブックをクリックすると無限ループが発生する為、
'     アクティブブックに入力中のシートが存在するかチェックする
'***************************************
Function Sheet_Name_Check(strSheetName As String)
On Error GoTo ErrHandler
    Dim blnFileExists As Boolean
    Dim objWorksheet As Worksheet
    
    blnFileExists = False
   
    '全てのシートをループする。
    For Each objWorksheet In ThisWorkbook.Worksheets
    
        If objWorksheet.Name = ActiveSheet.Name Then
            blnFileExists = True
            Exit For
        End If
    Next
    
    Sheet_Name_Check = blnFileExists
    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("Sheet_Name_Check()" & Err.Number & ":" & Err.Description)
End Function

'***************************************
' KEY情報取得
'***************************************
Sub GetKeyInfo()
On Error GoTo ErrHandler

    G_ErrorUmuCnt = 0
    G_FactoryIdCnt = 0
    
    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(SheetName_Define) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If
    

    'フィルタを解除
    Call modCommon.Sheet_AutoFilterOff(Worksheets(SheetNo_Input).Name)

    '送信時処理ID列取得
    Call modCommon.GetDefineFindString(G_SyoriIdCnt, G_SendIdData, ColumnDivision_SnedId)

    'エラー有無列取得
    Call GetDefineFindString(G_ErrorUmuCnt, G_ErrorUmuData, ColumnDivision_ErrorUmu)

    '工場ID列取得
    Call modCommon.GetDefineFindString(G_FactoryIdCnt, G_FactoryIdData, ColumnDivision_FactoryId)

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetKeyInfo()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' ：選択アイテムシートより拡張項目取得の前処理
'***************************************
Sub GetKakuchoCheck(ByVal Target As Range, ByVal RowNo As Long, ByVal SheetNo As Integer)
On Error GoTo ErrHandler
    Dim valRowNo As Long
    Dim lKakuchoNo As Long
    Dim valgrpId As Long
    Dim valMinNo As Long
    Dim valMaxNo As Long
    Dim valColNo2 As Long

    'シート名が存在するか（別ブックをクリックしてないか？）
    If Sheet_Name_Check(Worksheets(SheetNo).Name) = False Then
    '存在しない。別ブックがアクティブなので、処理中止。
        Exit Sub
    End If
    
    '一時的にイベントを無効化(値を設定すると無限ループになるため)
    Application.EnableEvents = False

    '変更有りならば、拡張項目を表示
    valRowNo = RowNo
    '定義情報を取得
    '選択ID値格納先列番号
    valColNo2 = GetValColNo(SheetNo, Target)
    Dim sNo As String
    If valColNo2 > 0 Then
        sNo = Worksheets(SheetNo_Input).Cells(valRowNo, valColNo2)
        
        If sNo = "" Then
            lKakuchoNo = 0
        Else
            If IsNumeric(sNo) = True Then
                lKakuchoNo = CLng(sNo)
            Else
                lKakuchoNo = 0
            End If
        End If
        
        '選択項目グループID取得
        valgrpId = GetGrpId(SheetNo, Target)
        '拡張情報表示
        Call GetKakuchoInfo(Target, lKakuchoNo, valRowNo, valColNo2, valgrpId, valMinNo, valMaxNo)
        
        'コンボボックスが空になったら、拡張項目をクリアする
        Dim i As Long
        If sNo = "" And valRowNo > 0 Then
            If valMinNo > 0 And valMaxNo > 0 Then
                '拡張項目クリア
                For i = valMinNo To valMaxNo
                    Worksheets(SheetNo_Input).Cells(valRowNo, i) = ""
                Next
            End If
            Worksheets(SheetNo_Input).Cells(valRowNo, valColNo2) = ""
        End If
    End If

    'イベントを有効に戻す
    Application.EnableEvents = True

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetKakuchoCheck()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' ：選択アイテムシートより拡張項目取得
'***************************************
Sub GetKakuchoInfo(ByVal Target As Range, ByVal KikiNo As Long, ByVal RowNo As Long, ByVal valColId As Long, ByVal valgrpId As Long, ByRef lMinNo As Long, ByRef lMaxNo As Long)
On Error GoTo ErrHandler
    Dim lRow, lastRow As Long
    Dim defineWs As Worksheet
    Set defineWs = Worksheets(SheetName_Define)
    lMinNo = 0
    lMaxNo = 0
    lRow = Target.row
    lastRow = defineWs.Cells(defineWs.Rows.Count, Define_ColNo_SheetNo).End(xlUp).row

    Dim rData As Range
    Set rData = defineWs.Range("A" & Define_RowNo_Start & ":" & modCommon.GetColNum2Txt(Define_ColNo_GrpId) & lastRow)
    
    Dim vData As Variant
    vData = rData.Value ' (1 To Rows.Count, 1 To Columns.Count) の二次元配列で値を取得
    
    Dim Kakucho() As String    '選択アイテムシート：拡張項目
    Erase Kakucho
    Dim row As Long
    Dim lCnt As Long
    Dim vCellType As Variant     '列タイプ
    Dim vLinkNo As Variant       '連動元列番号
    Dim vAutoNo As Variant       '自動表示拡張列番号
        
    '列初期用
    lMinNo = 99999
    lMaxNo = -99999
    lCnt = 0

    '定義情報シートより、下記条件に該当する項目を取得し配列へ格納
    '  条件１：列タイプが「1：文字列」
    '  条件２：連動元番号が設定が設定されている
    ReDim Kakucho(1 To 3, 1 To rData.Rows.Count + 1)
    For row = 1 To rData.Rows.Count
            
        ' セル種類-文字列
        vCellType = vData(row, Define_ColNo_Type)
        If vCellType = CellType_Text Then
            vLinkNo = vData(row, Define_ColNo_Link)
            '連動元番号が同じ場合
            If vLinkNo = valColId Then
                '自動表示拡張列番号 退避
                vAutoNo = vData(row, Define_ColNo_AutoNo)
                '自動表示拡張列番号が設定時のみ格納
                If vAutoNo <> "" Then
                    lCnt = lCnt + 1
                    Kakucho(1, lCnt) = CStr(row)                              '行
                    Kakucho(2, lCnt) = CStr(vData(row, Define_ColNo_ColNo))   '列
                    Kakucho(3, lCnt) = CStr(vAutoNo)                          '連動元列番号
    
                    '初期の為、列の最小値取得
                    If lMinNo > CLng(Kakucho(2, lCnt)) Then
                        lMinNo = CLng(Kakucho(2, lCnt))
                    End If
                    '初期の為、列の最大値取得
                    If lMaxNo < CLng(Kakucho(2, lCnt)) Then
                        lMaxNo = CLng(Kakucho(2, lCnt))
                    End If
                End If
            End If
        End If
        
    Next

    '拡張項目ありの場合、表示
    If lCnt > 0 Then
        '拡張項目、表示
        Call SetKakuchoInfo(valgrpId, KikiNo, lCnt, Kakucho, RowNo)
    End If

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("GetKakuchoInfo()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' ：選択アイテムシートより拡張項目表示
'***************************************
Public Sub SetKakuchoInfo(grpId As Long, KikiNo As Long, KakuchoCnt As Long, Kakucho() As String, outRowNo As Long)
On Error GoTo ErrHandler
    
    Dim itemWs As Worksheet
    Set itemWs = Worksheets(SheetName_Item)

    'グループID、機器IDでフィルターを掛ける
    itemWs.Range("A1").AutoFilter Item_ColNo_GrpId, grpId
    itemWs.Range("A1").AutoFilter Item_ColNo_Id, CLng(KikiNo), xlOr, 0
    '可視セルの行数を取得
    Dim rowCnt As Long
    rowCnt = itemWs.Range("A1").CurrentRegion.Resize(, 1).SpecialCells(xlCellTypeVisible).Count
    Dim filteredData
    ReDim filteredData(1 To rowCnt - 1, 1 To 4)
    
    Dim RowNo As Long, ColNo As Long
    RowNo = 0
    Dim visibleRow As Range
    Dim kDat As String
    
    '表示する拡張項目の値(標準工場翻訳)
    Dim exValue As String
    '表示する拡張項目の値(工場個別翻訳)
    Dim exValueFactory As String
    
    '可視セルの行をループ（１行のはず）
    For Each visibleRow In itemWs.Range("A1").CurrentRegion.SpecialCells(xlCellTypeVisible).Rows
        If visibleRow.row < Item_RowNo_Start Then
            'ヘッダー行はスキップ
            GoTo CONTINUE1:
        End If

      RowNo = RowNo + 1
      For ColNo = 1 To KakuchoCnt
      
      '表示する拡張項目の工場IDを判定
      If visibleRow.Cells(1, Item_ColNo_FactoryId) = G_DefaultID Then
        '標準工場の翻訳の場合
        exValue = visibleRow.Cells(1, (Item_ColNo_Kakucho01 + (Kakucho(3, ColNo) - 1)))
      ElseIf visibleRow.Cells(1, Item_ColNo_FactoryId) = G_FactoryId Then
        '工場個別翻訳の場合
        exValueFactory = visibleRow.Cells(1, (Item_ColNo_Kakucho01 + (Kakucho(3, ColNo) - 1)))
      ElseIf G_FactoryId = "" Then
        '工場IDが空(コンボボックス表示処理が実行されていない)の場合、値の表示のみ行う
        Worksheets(SheetNo_Input).Cells(outRowNo, CLng(Kakucho(2, ColNo))) = visibleRow.Cells(1, (Item_ColNo_Kakucho01 + (Kakucho(3, ColNo) - 1)))
        GoTo CONTINUE2:
      Else
        '対象外の翻訳の場合は何もしない
        GoTo CONTINUE2:
      End If
      
      '工場個別翻訳が設定されていれば優先して表示
      If Not exValueFactory = "" Then
        exValue = exValueFactory
      End If
      
      '拡張項目をセルに表示
      Worksheets(SheetNo_Input).Cells(outRowNo, CLng(Kakucho(2, ColNo))) = exValue
      exValue = ""        '標準工場翻訳を初期化
      exValueFactory = "" '工場個別翻訳を初期化
      
      '可視セルの拡張項目の値を表示（拡張項目１つ目～）
      'Worksheets(SheetNo_Input).Cells(outRowNo, CLng(Kakucho(2, ColNo))) = visibleRow.Cells(1, (Item_ColNo_Kakucho01 + (Kakucho(3, ColNo) - 1)))
CONTINUE2:
      Next
CONTINUE1:
    Next
    
    'フィルタを解除
    itemWs.Range("A1").AutoFilter

    Exit Sub

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("SetKakuchoInfo()" & Err.Number & ":" & Err.Description)
End Sub

'***************************************
' String型からDouble型へ変換
'    Double最小値以下の場合、Double最小値に置換する
'    Double最大値以上の場合、Double最大値に置換する
'***************************************
Function StringToDouble(ByVal sValue As String) As Double
    StringToDouble = 0
On Error GoTo ErrHandler
    Dim lMaxlen As Long

    lMaxlen = Len(Trim(Str(G_DoubleMax))) 'Trimが必要

    'チェック
    If sValue <> "" Then
        If Len(sValue) < lMaxlen Then
            '桁数が最大値の桁数以下なら、Long正常範囲内なので、チェックなしで変換
            StringToDouble = Val(sValue)
            Exit Function
        End If
        
        '桁数が最大値の桁数と同じか、それ以上の場合に、範囲チェックを行い、オーバーする場合はMaxまたはMinに置換
        
        '最大値チェック
        If sValue > Trim(Str(G_DoubleMax)) Then
                StringToDouble = G_DoubleMax
        Else
            '先頭が‐でなければ、範囲内
            If Left(sValue, 1) <> "-" Then
                StringToDouble = Val(sValue)
                Exit Function
            End If
            
            '最小値チェック （文字で比較する為、最小値より大きいか？となる）
            If sValue > Str(G_DoubleMin) Then
                '最小値以下は、最小値に置換
                StringToDouble = G_DoubleMin
            Else
                '正常範囲なので、変換
                StringToDouble = Val(sValue)
            
            End If
        End If
        
    End If
    
    Exit Function

ErrHandler:
    '異常時、表示の設定を行う
    Call ErrorExit("StringToDouble()" & Err.Number & ":" & Err.Description)
End Function

'===========================================================================
'===========================================================================
Sub メンテナンス用_イベント_有効化_on()
    'セル編集、イベント有効化
    Application.EnableEvents = True
End Sub
'===========================================================================
'===========================================================================
Sub メンテナンス用_イベント_無効化_off()
    'セル編集、イベント無効化
    Application.EnableEvents = False
End Sub

'===========================================================================
'===========================================================================
Sub メンテナンス用_部品_非表示()
    ActiveSheet.ComboBox1.Visible = False  'コンボボックス非表示
    ActiveSheet.ListBox1.Visible = False   'リスト非表示
End Sub

'===========================================================================
'===========================================================================
Sub メンテナンス用_部品_表示()
    ActiveSheet.ComboBox1.Visible = True  'コンボボックス表示
    ActiveSheet.ListBox1.Visible = True   'リスト表示
End Sub

'===========================================================================
'===========================================================================
Sub メンテナンス用_保護()
    Call Sheet_Protect(1)  '１シート目　保護
    Call Book_Protect
End Sub

'===========================================================================
'===========================================================================
Sub メンテナンス用_保護解除()
    Call Book_UnProtect
    Call Sheet_UnProtect(1) '１シート目　保護解除
End Sub


'===========================================================================
'===========================================================================
Sub メンテナンス用_部品_全シート_表示()

    '全入力用シートのコンボボックス・リストを表示
    Call メンテナンス用_部品_全シート_設定(True)
End Sub

Sub メンテナンス用_部品_全シート_非表示()

    '全入力用シートのコンボボックス・リストを非表示
    Call メンテナンス用_部品_全シート_設定(False)
End Sub

'①コンボボックス・リストの表示切替
'②タイトル行（１行目・３行目）をロックする
Sub メンテナンス用_部品_全シート_設定(ByVal bValue As Boolean)
    Dim i As Long
    Dim j As Long
    Dim SheetsCnt As Long
    Dim sName As String
 On Error GoTo ErrHandler

Debug.Print "メンテナンス用_部品_全シート_設定 bValue:" & bValue

    '更新停止
    Application.ScreenUpdating = False

    Call メンテナンス用_イベント_無効化_off
    'ブック保護解除
    Call Book_UnProtect
    
    SheetsCnt = ThisWorkbook.Sheets.Count
    
    '一時的にエラーを無視
    On Error Resume Next
 
    For i = 1 To SheetsCnt
        sName = Sheets(i).Name

        'シート名がSheet+数値(Sheet1…)の場合、コンボボックス・リストが存在する
        If Left(sName, 5) = "Sheet" Then
            If IsNumeric(Mid(sName, 6, 1)) = True Then
Debug.Print "sName:" & sName
                'シート保護解除
                Call Sheet_UnProtect_Name(sName)
                
                '部品の表示／非表示をセット
                Worksheets(i).ComboBox1.Visible = bValue  'コンボボックス
                Worksheets(i).ListBox1.Visible = bValue   'リスト
                
                'タイトル行（１行目・３行目）にロックをかける
                Worksheets(i).Range("1:1,3:3").Select
                Selection.Locked = True
    
                'シート保護
                Call Sheet_Protect_Name(sName)
    
            Else
                '入力シート以外。Sheet_Item 等
Debug.Print "   ---> sName:" & sName
            End If
        End If
        
    Next i
    
    'エラーを表示する
    On Error GoTo 0


    'ブック保護
    Call Book_Protect
    Call メンテナンス用_イベント_有効化_on

    '更新再開
    Application.ScreenUpdating = True
    
    MsgBox "部品の表示切替が完了しました。"

ErrHandler:
   '
    Call メンテナンス用_イベント_有効化_on
    '更新再開
    Application.ScreenUpdating = True
End Sub


'***************************************
' 指定されたファイルのレコード数を取得
'***************************************
Function GetLineCount(a_sFilePath) As Long
    Dim oFS As New FileSystemObject
    Dim oTS As TextStream
    Dim iLine
    
    '// 引数のファイルが存在しない場合は処理を終了する
    If (oFS.FileExists(a_sFilePath) = False) Then
        GetLineCount = -1
        Exit Function
    End If
    
    '// 追加モードで開く
    Set oTS = oFS.OpenTextFile(a_sFilePath, ForAppending)
    
    GetLineCount = oTS.Line - 1
End Function

'***************************************
' 送信時処理名称より、送信時処理IDを取得する
'***************************************
Function GetSendProcIdByName(sendProcName) As String

    '送信時処理名称を判定
    If sendProcName = SendName_Insert Then
        '「登録」の場合「1」
        GetSendProcIdByName = SendId_Insert
        
    ElseIf sendProcName = SendName_Update Then
        '「内容更新」の場合「2」
        GetSendProcIdByName = SendId_Update
        
    ElseIf sendProcName = SendName_Delete Then
        '「削除」の場合「9」
        GetSendProcIdByName = SendId_Delete
        
    Else
        '該当しない場合「-1」
        GetSendProcIdByName = SendId_Error
    End If
End Function


