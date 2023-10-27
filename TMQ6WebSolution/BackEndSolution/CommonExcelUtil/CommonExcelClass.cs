using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExcelUtil
{
    //▼▼▼ 本ファイル上のクラス、インターフェースは実際のExcel操作用ライブラリに合わせて実装すること！ ▼▼▼

    //
    // 概要:
    //     /// ワークブックを表します。 ///
    public class Workbook : IWorkbook
    {
        //
        // 概要:
        //     /// ワークブックを作成します。 ///
        public Workbook()
        {

        }
        public Workbook(string licenseKey)
        {

        }

        //
        // 概要:
        //     /// アクティブなシートを取得します。 ///
        public IWorksheet ActiveSheet { get; }
        public string FullName { get; }
        public string Name { get; set; }
        //
        // 概要:
        //     /// 指定されたワークブック内のすべての名前（ワークシート固有の名前もすべて含む）を表す /// GrapeCity.Documents.Excel.INames
        //     コレクションを返します。 /// Names オブジェクトは読み取り専用です。 ///
        public INames Names { get; }
        public string Path { get; set; }
        //
        // 概要:
        //     /// ワークブックに含まれるワークシートのコレクションを取得します。 ///
        public IWorksheets Worksheets { get; }

        //
        // 概要:
        //     /// 指定された Excel ファイルストリームを開きます。 ///
        //
        // パラメーター:
        //   fileStream:
        //     ファイルストリーム。
        public void Open(Stream fileStream)
        {

        }
        //
        // 概要:
        //     /// 指定された Excel ファイルを開きます。 ///
        //
        // パラメーター:
        //   fileName:
        //     Excel ファイル。
        public void Open(string fileName)
        {

        }
        //
        // 概要:
        //     /// 指定されたファイル形式でファイルを開きます。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        //
        //   fileFormat:
        //     ファイルの形式。
        public void Open(string fileName, OpenFileFormat fileFormat)
        {

        }
        //
        // 概要:
        //     /// 指定されたファイル形式でストリームを開きます。 ///
        //
        // パラメーター:
        //   fileStream:
        //     指定するファイルストリーム。
        //
        //   fileFormat:
        //     ファイルストリームの形式。
        public void Open(Stream fileStream, OpenFileFormat fileFormat)
        {

        }
        //
        // 概要:
        //     /// 指定されたオプションを使用してファイルを開きます。開くオプションには、XlsxOpenOptions または CsvOpenOptions を指定できます。
        //     ///
        //
        // パラメーター:
        //   fileName:
        //     Excel ファイル。
        //
        //   options:
        //     ファイルを開く際のオプション。
        public void Open(string fileName, OpenOptionsBase options)
        {

        }
        //
        // 概要:
        //     /// 指定されたオプションを使用してストリームを開きます。開くオプションには、XlsxOpenOptions または CsvOpenOptions を指定できます。
        //     ///
        //
        // パラメーター:
        //   fileStream:
        //     ファイルストリーム。
        //
        //   options:
        //     ファイルストリームを開く際の形式。
        public void Open(Stream fileStream, OpenOptionsBase options)
        {

        }

        public void Protect(string password, bool structure = true, bool windows = false)
        {
        }

        //
        // 概要:
        //     /// 指定された Excel ファイルにデータを保存します。 ///
        //
        // パラメーター:
        //   fileName:
        //     Excel ファイル。
        public void Save(string fileName)
        {

        }
        //
        // 概要:
        //     /// 指定された Excel ファイルストリームにワークブックを保存します。 ///
        //
        // パラメーター:
        //   fileStream:
        //     ファイルストリーム。
        public void Save(Stream fileStream)
        {

        }
        //
        // 概要:
        //     /// 指定されたファイル形式でワークブックをストリームに保存します。 ///
        //
        // パラメーター:
        //   fileStream:
        //     指定するファイルストリーム。
        //
        //   fileFormat:
        //     ファイルストリームの形式。
        public void Save(Stream fileStream, SaveFileFormat fileFormat)
        {

        }
        //
        // 概要:
        //     /// 指定されたファイル形式でワークブックをファイルに保存します。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        //
        //   fileFormat:
        //     ファイルの形式。
        public void Save(string fileName, SaveFileFormat fileFormat)
        {

        }
    }

    //
    // 概要:
    //     /// IWorkbook オブジェクトを表します。 ///
    public interface IWorkbook
    {
        //
        // 概要:
        //     /// アクティブなワークブックまたは指定されたウィンドウまたはワークブック内のアクティブシート（前面にあるシート）を表すオブジェクトを返します。アクティブシートがない場合は、null
        //     を返します（読み取り専用）。 ///
        IWorksheet ActiveSheet { get; }
        //
        // 概要:
        //     /// 指定されたワークブック内のすべてのワークシートを表す GrapeCity.Documents.Excel.IWorksheets コレクションを返します。この
        //     Sheets オブジェクトは読み取り専用です。 ///
        IWorksheets Worksheets { get; }

        void Open(string fileName);
        void Open(Stream fileStream);
        //
        // 概要:
        //     /// 指定された形式のファイルを開きます。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        //
        //   fileFormat:
        //     ファイルの形式。
        void Open(string fileName, OpenFileFormat fileFormat);
        //
        // 概要:
        //     /// 指定された形式のファイルストリームを開きます。 ///
        //
        // パラメーター:
        //   fileStream:
        //     指定するファイルストリーム。
        //
        //   fileFormat:
        //     ファイルストリームの形式。
        void Open(Stream fileStream, OpenFileFormat fileFormat);
        void Save(Stream fileStream);
        void Save(string fileName);
        //
        // 概要:
        //     /// 指定された形式のファイルを保存します。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        //
        //   fileFormat:
        //     ファイルの形式。
        void Save(string fileName, SaveFileFormat fileFormat);
        //
        // 概要:
        //     /// 指定された形式のファイルストリームを保存します。 ///
        //
        // パラメーター:
        //   fileStream:
        //     指定するファイルストリーム。
        //
        //   fileFormat:
        //     ファイルストリームの形式。
        void Save(Stream fileStream, SaveFileFormat fileFormat);
    }

    //
    // 概要:
    //     /// ワークブック内のすべてのシートのコレクションを表します。 ///
    public interface IWorksheets : IEnumerable<IWorksheet>, IEnumerable
    {
        //
        // 概要:
        //     /// インデックスを使用してワークシートを取得します。 ///
        //
        // パラメーター:
        //   index:
        //     インデックス。
        IWorksheet this[int index] { get; }
        //
        // 概要:
        //     /// 名前を使用してワークシートを取得します。 ///
        //
        // パラメーター:
        //   name:
        //     ワークシートの名前。
        IWorksheet this[string name] { get; }
        IWorksheets this[string[] name] { get; }

        //
        // 概要:
        //     /// コレクション内のオブジェクトの数を返します（読み取り専用）。 ///
        int Count { get; }

        //
        // 概要:
        //     /// 新しいワークシートを作成します。新しいワークシートがアクティブシートになります。 ///
        //
        // 戻り値:
        //     新しいワークシートを返します。
        IWorksheet Add();
        //
        // 概要:
        //     /// 新しいワークシートを作成し、指定されたシートの後に挿入します。 ///
        //
        // パラメーター:
        //   sheet:
        //     挿入シート
        //
        // 戻り値:
        //     新しいワークシートを返します。
        IWorksheet AddAfter(IWorksheet sheet);
        //
        // 概要:
        //     /// 新しいワークシートを作成し、指定されたシートの前に挿入します。 ///
        //
        // パラメーター:
        //   sheet:
        //     指定するシートインデックス。
        //
        // 戻り値:
        //     新しいワークシートを返します。
        IWorksheet AddBefore(IWorksheet sheet);
        //
        // 概要:
        //     /// 指定されたワークシートがワークシートのコレクションに含まれている場合は、True を返します。含まれていない場合は、False を返します。 ///
        //
        // パラメーター:
        //   worksheet:
        //     /// IWorksheet オブジェクト。 ///
        //
        // 戻り値:
        //     指定されたワークシートがワークシートのコレクションに含まれているかどうかを返します。
        bool Contains(IWorksheet worksheet);
        //
        // 概要:
        //     /// コレクション内で指定されたワークシートの 0 から始まるインデックスを返します。 ///
        //
        // パラメーター:
        //   worksheet:
        //     /// IWorksheet オブジェクト。 ///
        //
        // 戻り値:
        //     コレクション内で指定されたワークシートの 0 から始まるインデックスを返します。
        int IndexOf(IWorksheet worksheet);
        void Select(bool replace = true);
    }

    //
    // 概要:
    //     /// ワークシートを表します。 ///
    public interface IWorksheet
    {
        //
        // 概要:
        //     /// アクティブなセルを取得します。 ///
        IRange ActiveCell { get; }
        //
        //
        // 概要:
        //     /// ワークシート内のすべてのセル（現在使用されているセル以外も含む）を表す GrapeCity.Documents.Excel.IRange オブジェクトを返します。このオブジェクトは読み取り専用です。
        //     ///
        IRange Cells { get; }
        //
        // 概要:
        //     /// 指定されたワークシート内のすべての列を表す GrapeCity.Documents.Excel.IRange オブジェクトを返します（読み取り専用）。
        //     ///
        IRange Columns { get; }
        object DataSource { get; set; }
        //
        // 概要:
        //     /// 類似のオブジェクトのコレクション内のオブジェクトのインデックス番号を取得または設定します（読み取り専用）。 ///
        int Index { get; set; }
        //
        // 概要:
        //     /// オブジェクトの名前を取得または設定します（読み取りまたは書き込み）。 ///
        string Name { get; set; }
        //
        // 概要:
        //     /// セル値、式、書式設定などのセルのプロパティとメソッドへのアクセスを提供する GrapeCity.Documents.Excel.IRange のインスタンスを返します。
        //     ///
        IRangeProvider Range { get; }
        //
        // 概要:
        //     /// 指定されたワークシート内のすべての行を表す GrapeCity.Documents.Excel.IRange オブジェクトを返します。これは、読み取り専用の
        //     Range オブジェクトです。 ///
        IRange Rows { get; }
        //
        // 概要:
        //     /// 選択範囲を取得します。 ///
        IRange Selection { get; }
        //
        // 概要:
        //     /// ワークブックを返します。 ///
        IWorkbook Workbook { get; }
        //
        // 概要:
        //     /// オブジェクトを表示するかどうかを決定します（GrapeCity.Documents.Excel.Visibility の読み取りまたは書き込み）。
        //     ///
        Visibility Visible { get; set; }

        //
        // 概要:
        //     /// 現在のシートをアクティブシートにします。結果は、シートのタブをクリックすることと同じです。 ///
        void Activate();
        IWorksheet Copy(IWorkbook workbook = null);
        IWorksheet CopyAfter(IWorksheet targetSheet);
        IWorksheet CopyBefore(IWorksheet targetSheet);
        //
        // 概要:
        //     /// オブジェクトを削除します。 ///
        void Delete();
        IWorksheet Move(IWorkbook workbook = null);
        IWorksheet MoveAfter(IWorksheet targetSheet);
        IWorksheet MoveBefore(IWorksheet targetSheet);
        //
        // 概要:
        //     /// 指定された形式のファイルに現在のワークシートを保存します。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        void Save(string fileName);
        //
        // 概要:
        //     /// 指定された形式のファイルストリームに現在のワークフローを保存します。 ///
        //
        // パラメーター:
        //   fileStream:
        //     指定するファイルストリーム。
        //
        //   fileFormat:
        //     ファイルストリームの形式。
        void Save(Stream fileStream, SaveFileFormat fileFormat);
        //
        // 概要:
        //     /// 指定された形式のファイルに現在のワークフローを保存します。 ///
        //
        // パラメーター:
        //   fileName:
        //     指定するファイル。
        //
        //   fileFormat:
        //     ファイルの形式。
        void Save(string fileName, SaveFileFormat fileFormat);
        void Select(bool replace = true);
    }

    //
    // 概要:
    //     /// ワークブック内のすべての GrapeCity.Documents.Excel.IName オブジェクトのコレクションを表します。各 Name オブジェクトは、セル範囲の定義名を表します。
    //     /// Name は、組み込み名（Database、Print_Area、Auto_Open など） /// — またはカスタム名のいずれかになります。
    //     ///
    public interface INames
    {
        //
        // 概要:
        //     /// コレクションから GrapeCity.Documents.Excel.IName オブジェクトを返します。 ///
        //
        // パラメーター:
        //   name:
        //     コレクション内の要素の名前を指定します。
        //
        // 戻り値:
        //     /// GrapeCity.Documents.Excel.IName を返します。 ///
        IName this[string name] { get; }
        //
        // 概要:
        //     /// コレクションから GrapeCity.Documents.Excel.IName オブジェクトを返します。 ///
        //
        // パラメーター:
        //   index:
        //     コレクション内の要素のインデックスを指定します。
        //
        // 戻り値:
        //     /// GrapeCity.Documents.Excel.IName を返します。 ///
        IName this[int index] { get; }

        //
        // 概要:
        //     /// コレクション内のオブジェクトの数を返します（読み取り専用）。 ///
        int Count { get; }

        //
        // 概要:
        //     /// 新しい名前を定義します。GrapeCity.Documents.Excel.IName オブジェクトを返します。 ///
        //
        // パラメーター:
        //   name:
        //     名前として使用するテキスト。名前にスペースを含めることはできず、セル参照に似た名前にすることもできません。
        //
        //   refersTo:
        //     /// 他の RefersTo 引数のいずれかが指定されていない場合は必須です。 /// 名前の参照先を記述します（A1 形式を使用）。 ///
        //
        // 戻り値:
        //     /// 新しい GrapeCity.Documents.Excel.IName オブジェクトを返します。 ///
        IName Add(string name, string refersTo);
        //
        // 概要:
        //     /// GrapeCity.Documents.Excel.IName コレクションをクリアします。 ///
        void Clear();
        //
        // 概要:
        //     /// 名前が GrapeCity.Documents.Excel.INames に含まれているかどうかを指定します。 ///
        //
        // パラメーター:
        //   name:
        //     名前。
        //
        // 戻り値:
        //     INames に名前が含まれているかどうか。
        bool Contains(string name);
    }

    //
    // 概要:
    //     /// セル範囲の定義名を表します。 ///
    public interface IName
    {
        //
        // 概要:
        //     /// 名前に関連付けられたコメントを取得または設定します（読み取りまたは書き込み）。 ///
        string Comment { get; set; }
        //
        // 概要:
        //     /// オブジェクトの名前を取得または設定します。 ///
        string Name { get; set; }
        //
        // 概要:
        //     /// この名前の参照先として定義されている式を取得または設定します。これは、等号で始まり、マクロの言語および A1 形式で記述されます。 ///
        string RefersTo { get; set; }
        //
        // 概要:
        //     /// この名前の参照先として定義されている式を取得または設定します。これは、等号で始まり、マクロの言語および R1C1 形式で記述されます。 ///
        string RefersToR1C1 { get; set; }

        //
        // 概要:
        //     /// オブジェクトを削除します。 ///
        void Delete();
    }

    //
    // 概要:
    //     /// IRange オブジェクトを表します。 ///
    public interface IRange
    {
        //
        // 概要:
        //     /// インデックスに基づいて、1 つのセルを表す GrapeCity.Documents.Excel.IRange の新しいインスタンスを返します。 ///
        //
        // パラメーター:
        //   index:
        IRange this[int index] { get; }
        //
        // 概要:
        //     /// この IRange からオフセットされた GrapeCity.Documents.Excel.IRange の新しいインスタンスを返します。 ///
        //
        // パラメーター:
        //   rowOffset:
        //     行のオフセット。
        //
        //   columnOffset:
        //     列のオフセット。
        //
        // 戻り値:
        //     IRange。
        IRange this[int rowOffset, int columnOffset] { get; }

        string Address { get; }
        void AutoFit();
        //
        // 概要:
        //     /// この IRange で表されるセルまたは範囲の境界線を表す GrapeCity.Documents.Excel.IBorders のインスタンスを返します。
        //     ///
        IBorders Borders { get; }
        //
        // 概要:
        //     /// 指定された範囲内のセルを表す GrapeCity.Documents.Excel.IRange オブジェクトを返します。 ///
        IRange Cells { get; }
        //
        // 概要:
        //     /// 現在の IRange から、式と値をクリアします。 ///
        void ClearContents();
        //
        // 概要:
        //     /// この IRange の最初の列の 0 から始まる列番号を返します。 ///
        int Column { get; }
        int ColumnCount { get; }
        //
        // 概要:
        //     /// 指定された範囲内の列を表す GrapeCity.Documents.Excel.IRange オブジェクトを返します。 ///
        IRange Columns { get; }
        //
        // 概要:
        //     /// この範囲内に表示される個別の列の幅を文字単位で取得または設定します。 ///
        double ColumnWidth { get; set; }
        //
        // 概要:
        //     /// コレクション内のオブジェクトの数を返します。 ///
        int Count { get; }
        //
        // 概要:
        //     /// 範囲の高さをポイント単位で取得します。 ///
        double Height { get; }
        //
        // 概要:
        //     /// 行または列を非表示にするかどうかを指定するプロパティを取得または設定します。 ///
        bool Hidden { get; set; }
        //
        // 概要:
        //     /// セルまたは範囲の GrapeCity.Documents.Excel.IRange.HorizontalAlignment を取得または設定します。
        //     ///
        HorizontalAlignment HorizontalAlignment { get; set; }
        //
        // 概要:
        //     /// セルまたはセル範囲をワークシートに挿入し、そのスペースを空けるために他のセルを移動します。 /// セルの移動方法を指定します。 ///
        void Insert(InsertShiftDirection shiftDirection = InsertShiftDirection.Auto);
        //
        // 概要:
        //     /// 表されている範囲内のセルを 1 つの結合セルにマージします。 ///
        //
        // パラメーター:
        //   isAcross:
        //     /// オプションのオブジェクト。指定された範囲の各行にあるセルをそれぞれ個別の結合セルとしてマージする場合は True に /// 設定します。デフォルト値は
        //     false です。 ///
        void Merge(bool isAcross = false);
        //
        // 概要:
        //     /// 範囲内のセルの数値書式を取得または設定します。 ///
        string NumberFormat { get; set; }
        //
        // 概要:
        //     /// 最初の行の 0 から始まる行番号を返します。 ///
        int Row { get; }
        int RowCount { get; }
        //
        // 概要:
        //     /// この範囲で表示される個別の行の高さをポイント単位で取得または設定します。 ///
        double RowHeight { get; set; }
        //
        // 概要:
        //     /// 指定された範囲内の行を表す GrapeCity.Documents.Excel.IRange オブジェクトを返します。 ///
        IRange Rows { get; }
        //
        // 概要:
        //     /// この IRange によって表されるセルの値を書式設定された文字列として取得します。 ///
        string Text { get; }
        //
        // 概要:
        //     /// 指定された範囲の値を文字列、double、ブール値、オブジェクト、または null として取得するか、 /// 指定されたセルの値を文字列、double、int32、int64、
        //     /// int16、ブール値、System.DateTime、オブジェクト、または null として設定します。 ///
        object Value { get; set; }
        //
        // 概要:
        //     /// セルまたは範囲の GrapeCity.Documents.Excel.IRange.VerticalAlignment を取得または設定します。
        //     ///
        VerticalAlignment VerticalAlignment { get; set; }
        //
        // 概要:
        //     /// 範囲の幅をポイント単位で取得します。 ///
        double Width { get; }
        //
        // 概要:
        //     /// この範囲の親 GrapeCity.Documents.Excel.IRange.Worksheet を返します。 ///
        IWorksheet Worksheet { get; }

        //
        // 概要:
        //     /// 現在の選択範囲内にある単一のセルをアクティブ化します。 ///
        void Activate();
        //
        // 概要:
        //     /// 現在の IRange から、式、値、およびすべての書式設定をクリアします。 ///
        void Clear();
        void Copy(IRange destination, PasteType pasteType = PasteType.Default);
        //
        // 概要:
        //     /// 指定された範囲に、範囲をカットします。 ///
        //
        // パラメーター:
        //   destination:
        //     指定する範囲の貼り付け先になる新しい範囲を指定します。
        void Cut(IRange destination);
        //
        // 概要:
        //     /// セルまたはセル範囲をワークシートから削除し、削除されたセルの位置に他のセルを移動します。 /// セルの移動方法を指定します。 ///
        void Delete(DeleteShiftDirection shiftDirection = DeleteShiftDirection.Auto);
        //
        // 概要:
        //     /// オブジェクトを選択します。 ///
        void Select();
        //
        // 概要:
        //     /// 表されている範囲内の結合セルを通常のセルに変換します。 ///
        void UnMerge();
    }

    //
    // 概要:
    //     /// ワークシート内にあるセルの GrapeCity.Documents.Excel.IRange オブジェクトへのアクセスを提供します。 ///
    public interface IRangeProvider
    {
        //
        // 概要:
        //     /// 指定された参照を使用して GrapeCity.Documents.Excel.IRange オブジェクトを取得します。 ///
        //
        // パラメーター:
        //   reference:
        //     参照。
        //
        // 戻り値:
        //     IRange。
        IRange this[string reference] { get; }
        //
        // 概要:
        //     /// 指定された行および列を使用して GrapeCity.Documents.Excel.IRange オブジェクトを取得します。 ///
        //
        // パラメーター:
        //   row:
        //     行。
        //
        //   column:
        //     列。
        //
        // 戻り値:
        //     IRange。
        IRange this[int row, int column] { get; }
        //
        // 概要:
        //     /// 指定された行および列を使用して GrapeCity.Documents.Excel.IRange オブジェクトを取得します。 ///
        //
        // パラメーター:
        //   row:
        //     行。
        //
        //   column:
        //     列。
        //
        //   rowCount:
        //     行数。
        //
        //   columnCount:
        //     列数。
        //
        // 戻り値:
        //     IRange。
        IRange this[int row, int column, int rowCount, int columnCount] { get; }
    }

    //
    // 概要:
    //     /// GrapeCity.Documents.Excel.IRange オブジェクトまたは GrapeCity.Documents.Excel.IStyle
    //     オブジェクトの 4 つの境界線を表す 4 つの GrapeCity.Documents.Excel.IBorder オブジェクトのコレクションを表します。
    //     ///
    public interface IBorders
    {
        //
        // 概要:
        //     /// コレクションから GrapeCity.Documents.Excel.IBorder オブジェクトを返します。 ///
        //
        // パラメーター:
        //   index:
        //     コレクション内の要素の位置を指定します。
        //
        // 戻り値:
        //     /// GrapeCity.Documents.Excel.IBorder を返します。 ///
        IBorder this[BordersIndex index] { get; }

        //
        // 概要:
        //     /// GrapeCity.Documents.Excel.IBorder の GrapeCity.Documents.Excel.IBorders.Color
        //     を取得または設定します。 ///
        Color Color { get; set; }
        //
        // 概要:
        //     /// 4 つのすべての境界線の色を取得または設定します。 ///
        int ColorIndex { get; set; }
        //
        // 概要:
        //     /// コレクション内のオブジェクトの数を返します。 ///
        int Count { get; }
        //
        // 概要:
        //     /// 境界線の線スタイルを取得または設定します。 ///
        BorderLineStyle LineStyle { get; set; }
        //
        // 概要:
        //     /// 色を明るくまたは暗くする値を取得または設定します。 ///
        double TintAndShade { get; set; }

        void Clear();
    }

    //
    // 概要:
    //     /// オブジェクトの境界線を表します。 ///
    public interface IBorder
    {
        //
        // 概要:
        //     /// この境界線の GrapeCity.Documents.Excel.IBorder.Color を取得または設定します。 ///
        Color Color { get; set; }
        //
        // 概要:
        //     /// 境界線の色を取得または設定します。 ///
        int ColorIndex { get; set; }
        //
        // 概要:
        //     /// 境界線の線スタイルを取得または設定します。 ///
        BorderLineStyle LineStyle { get; set; }
        //
        // 概要:
        //     /// 色を明るくまたは暗くする値を取得または設定します。 ///
        double TintAndShade { get; set; }

        void Clear();
    }

    //
    // 概要:
    //     /// ワークブックを開く形式を表します。 ///
    public enum OpenFileFormat
    {
        //
        // 概要:
        //     /// xlsx ファイルを表します。 ///
        Xlsx = 0,
        //
        // 概要:
        //     /// csv ファイルを表します。 ///
        Csv = 1,
        //
        // 概要:
        //     /// xlsm ファイルを表します。 ///
        Xlsm = 2
    }

    //
    // 概要:
    //     /// ワークブックを保存する形式を表します。 ///
    public enum SaveFileFormat
    {
        //
        // 概要:
        //     /// xlsx ファイルを表します。 ///
        Xlsx = 0,
        //
        // 概要:
        //     /// csv ファイルを表します。 ///
        Csv = 1,
        //
        // 概要:
        //     /// pdf ファイルを表します。 ///
        Pdf = 2,
        //
        // 概要:
        //     /// xlsm ファイルを表します。 ///
        Xlsm = 3,
        Html = 4
    }

    //
    // 概要:
    //     /// 貼り付けのタイプを指定します。 ///
    [Flags]
    public enum PasteType
    {
        //
        // 概要:
        //     /// セルの値または式だけの貼り付けを指定します。 ///
        Values = 1,
        Formulas = 2,
        //
        // 概要:
        //     /// 書式設定だけの貼り付けを指定します。 ///
        Formats = 4,
        Default = 6,
        NumberFormats = 8,
        ColumnWidths = 16,
        RowHeights = 32
    }

    //
    // 概要:
    //     /// セルを移動して削除したセルに置き換える方法を指定します。 ///
    public enum DeleteShiftDirection
    {
        //
        // 概要:
        //     /// 移動方向が範囲の形に基づくことを指定します。 ///
        Auto = 0,
        //
        // 概要:
        //     /// セルを上へ移動することを指定します。 ///
        Up = 1,
        //
        // 概要:
        //     /// セルを左へ移動することを指定します。 ///
        Left = 2
    }

    //
    // 概要:
    //     /// csv ファイルを開く際のオプションクラス。 ///
    public class CsvOpenOptions : OpenOptionsBase
    {
        //
        // 概要:
        //     /// コンストラクタ。 ///
        public CsvOpenOptions()
        {

        }

        public char CellSeparator { get; set; }
        public string ColumnSeparator { get; set; }
        //
        // 概要:
        //     /// テキストファイルの文字列を日付データに変換するかどうかを示す値を取得または設定します。 /// デフォルトは true です。 ///
        public bool ConvertDateTimeData { get; set; }
        //
        // 概要:
        //     /// テキストファイルの文字列を数値データに変換するかどうかを示す値を取得または設定します。 /// デフォルトは true です。 ///
        public bool ConvertNumericData { get; set; }
        //
        // 概要:
        //     /// デフォルトのエンコーディングを取得または設定します。デフォルトは utf-8 です。 ///
        public Encoding Encoding { get; set; }
        //
        // 概要:
        //     /// テキストが「=」で始まる場合に、それが式であるかどうかを示します。デフォルトは true です。 ///
        public bool HasFormula { get; set; }
        //
        // 概要:
        //     /// 文字列値を数値または日時に変換するときに、解析値にスタイルを適用する /// かどうかを指定します。デフォルトは true です。 ///
        public bool ParseStyle { get; set; }
        public string RowSeparator { get; set; }
        //
        // 概要:
        //     /// 区切りとして使用される文字列値を取得または設定します。 ///
        [Obsolete("SeparatorString is obsolete, use ColumnSeparator.")]
        public string SeparatorString { get; set; }
    }

    //
    // 概要:
    //     /// 開く処理の基本オプションクラス。 ///
    public abstract class OpenOptionsBase
    {
        protected OpenOptionsBase()
        {

        }

        //
        // 概要:
        //     /// ワークブックを開く形式を表します。 ///
        public OpenFileFormat FileFormat { get; protected set; }
    }

    //
    // 概要:
    //     /// オブジェクトが表示されるかどうかを指定します。 ///
    public enum Visibility
    {
        //
        // 概要:
        //     /// シートを表示することを指定します。 ///
        Visible = 0,
        //
        // 概要:
        //     /// ワークシートを非表示にすることを指定します（ユーザーは、シートを表示に切り替えることができます）。 ///
        Hidden = 1,
        //
        // 概要:
        //     /// オブジェクトを非表示にし、このプロパティを true に設定しない限り、再度表示できないように /// することを指定します（ユーザーがオブジェクトを表示に切り替えることはできません）。
        //     ///
        VeryHidden = 2
    }

    //
    // 概要:
    //     /// セルまたはオブジェクトの水平方向の配置を指定します。 ///
    public enum HorizontalAlignment
    {
        //
        // 概要:
        //     /// テキストが左揃え、数値（日付と時刻を含む）が右揃え、論理値が中央揃えになることを指定します。 ///
        General = 0,
        //
        // 概要:
        //     /// 左揃えを指定します。 ///
        Left = 1,
        //
        // 概要:
        //     /// テキストを中央揃えにすることを指定します。 ///
        Center = 2,
        //
        // 概要:
        //     /// 右揃えを指定します。 ///
        Right = 3,
        //
        // 概要:
        //     /// テキストをセル全体に繰り返して表示することを指定します。 ///
        Fill = 4,
        //
        // 概要:
        //     /// 最後の行は左揃えにすることを除いて、テキストを左端と右端がきれいに揃うように折り返して整列することを指定します。 ///
        Justify = 5,
        //
        // 概要:
        //     /// 複数のセルにまたがって水平方向の配置を中央揃えにすることを指定 /// します。またがるセルの数についての情報は、 /// 問題のセルの行のシート部分に表されます。
        //     /// この配置内の各セルについて、 /// centerContinuous 配置を参照する同じスタイル ID を /// 使用して、セル要素を ///
        //     書き出す必要があります。 ///
        CenterContinuous = 6,
        //
        // 概要:
        //     /// 最後の行も含めて、テキストを左端と右端がきれいに揃うように折り返して整列することを指定します。 ///
        Distributed = 7
    }

    //
    // 概要:
    //     /// テキストの垂直方向の配置を指定します。 ///
    public enum VerticalAlignment
    {
        //
        // 概要:
        //     /// テキストが最上部に配置されることを指定します。 ///
        Top = 0,
        //
        // 概要:
        //     /// テキストが垂直方向の中央に揃えられることを指定します。 ///
        Center = 1,
        //
        // 概要:
        //     /// テキストが最下部に配置されることを指定します。 ///
        Bottom = 2,
        //
        // 概要:
        //     /// 回転したテキストが垂直方向に均等配置されることを指定します。 ///
        Justify = 3,
        //
        // 概要:
        //     /// 最後の行も含めて、回転したテキストを左端と右端がきれいに揃うように折り返して整列することを指定します。 ///
        Distributed = 4
    }

    //
    // 概要:
    //     /// 取得する境界線を指定します。 ///
    public enum BordersIndex
    {
        //
        // 概要:
        //     /// 境界線が範囲内の各セルの左上隅から右下に引かれることを指定します。 ///
        DiagonalDown = 0,
        //
        // 概要:
        //     /// 境界線が範囲内の各セルの左下隅から右上に引かれることを指定します。 ///
        DiagonalUp = 1,
        //
        // 概要:
        //     /// 境界線が範囲の下端にあることを指定します。 ///
        EdgeBottom = 2,
        //
        // 概要:
        //     /// 境界線が範囲の左端にあることを指定します。 ///
        EdgeLeft = 3,
        //
        // 概要:
        //     /// 境界線が範囲の右端にあることを指定します。 ///
        EdgeRight = 4,
        //
        // 概要:
        //     /// 境界線が範囲の上端にあることを指定します。 ///
        EdgeTop = 5,
        //
        // 概要:
        //     /// 範囲の外側の境界線を除いて、範囲内のすべてのセルに水平境界線があることを指定します。 ///
        InsideHorizontal = 6,
        //
        // 概要:
        //     /// 範囲の外側の境界線を除いて、範囲内のすべてのセルに垂直境界線があることを指定します。 ///
        InsideVertical = 7
    }

    //
    // 概要:
    //     /// 境界線の線タイプを指定します。 ///
    public enum BorderLineStyle
    {
        //
        // 概要:
        //     /// 境界線なしを指定します。 ///
        None = 0,
        //
        // 概要:
        //     /// 極細の境界線を指定します。 ///
        Hair = 1,
        //
        // 概要:
        //     /// 2 点鎖線の境界線を指定します。 ///
        DashDotDot = 2,
        //
        // 概要:
        //     /// 1 点鎖線の境界線を指定します。 ///
        DashDot = 3,
        //
        // 概要:
        //     /// 点線の境界線を指定します。 ///
        Dotted = 4,
        //
        // 概要:
        //     /// 破線の境界線を指定します。 ///
        Dashed = 5,
        //
        // 概要:
        //     /// 細い境界線を指定します。 ///
        Thin = 6,
        //
        // 概要:
        //     /// 中細 2 点鎖線の境界線を指定します。 ///
        MediumDashDotDot = 7,
        //
        // 概要:
        //     /// 1 点斜線の境界線を指定します。 ///
        SlantDashDot = 8,
        //
        // 概要:
        //     /// 中細 1 点鎖線の境界線を指定します。 ///
        MediumDashDot = 9,
        //
        // 概要:
        //     /// 中細破線を指定します。 ///
        MediumDashed = 10,
        //
        // 概要:
        //     /// 中細の境界線を指定します。 ///
        Medium = 11,
        //
        // 概要:
        //     /// 太線の境界線を指定します。 ///
        Thick = 12,
        //
        // 概要:
        //     /// 二重線を指定します。 ///
        Double = 13
    }

    //
    // 概要:
    //     /// 挿入時に既存のセルを移動する方向を指定します。 ///
    public enum InsertShiftDirection
    {
        //
        // 概要:
        //     /// 移動方向が範囲の形に基づいて決定されることを指定します。 ///
        Auto = 0,
        //
        // 概要:
        //     /// セルを右へ移動することを指定します。 ///
        Right = 1,
        //
        // 概要:
        //     /// セルを下へ移動することを指定します。 ///
        Down = 2
    }
}
