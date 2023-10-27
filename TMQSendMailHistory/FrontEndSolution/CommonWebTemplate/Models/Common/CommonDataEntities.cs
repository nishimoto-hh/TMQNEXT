using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CommonWebTemplate.Models.Common {
    public partial class CommonDataEntities : DbContext {
        public CommonDataEntities() : base()
        {
        }

        public CommonDataEntities(DbContextOptions options)
            : base(options) {
        }

        public virtual DbSet<COM_CONDUCT_MST> COM_CONDUCT_MST { get; set; }
        public virtual DbSet<COM_FORM_DEFINE> COM_FORM_DEFINE { get; set; }
        //TMQカスタマイズ start====================================
        public virtual DbSet<COM_LISTITEM_DEFINE> COM_LISTITEM_DEFINE { get; set; }
        public virtual DbSet<COM_FORM_CONTROL_DEFINE> COM_FORM_CONTROL_DEFINE { get; set; }
        public virtual DbSet<COM_CONTROL_DEFINE> COM_CONTROL_DEFINE { get; set; }
        public virtual DbSet<COM_CONTROL_UNUSED> COM_CONTROL_UNUSED { get; set; }
        //TMQカスタマイズ end====================================
        public virtual DbSet<COM_LISTITEM_USER> COM_LISTITEM_USER { get; set; }
        //public virtual DbSet<COM_TMPTBL_DATA> COM_TMPTBL_DATA { get; set; }
        public virtual DbSet<COM_USER_AUTH> COM_USER_AUTH { get; set; }
        public virtual DbSet<COM_USER_MST> COM_USER_MST { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            if (base.Database.IsNpgsql())
            {
                //※Postgresの場合、有効にする
                modelBuilder.HasDefaultSchema("public");

                //テーブル名、カラム名を小文字に変更
                NamesToLower(modelBuilder);

                modelBuilder.Entity<COM_USER_AUTH>()
                    .Property(e => e.USERID)
                    .IsFixedLength()
                    .IsUnicode(false);

            }

            if (base.Database.IsSqlServer())
            {
                //※SQL Serverの場合
                //テーブル名、カラム名を小文字に変更
                NamesToLower(modelBuilder);

                modelBuilder.Entity<COM_USER_AUTH>()
                    .Property(e => e.USERID)
                    .IsFixedLength()
                    .IsUnicode(false);
            }

            if (base.Database.IsOracle())
            {
                //※Oracleの場合
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(base.Database.GetConnectionString());
                string UserID = builder.UserID;
                modelBuilder.HasDefaultSchema(UserID);

            }

            // EF Coreでは複合キーは各テーブルクラスの[Key]属性ではなくHasKeyで設定
            modelBuilder.Entity<COM_FORM_DEFINE>().HasKey(e => new {e.PGMID, e.FORMNO, e.CTRLID });
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().HasKey(e => new { e.LOCATION_LAYER_ID, e.PGMID, e.FORMNO, e.CTRLID, e.DEFINETYPE, e.CONTROL_NO });
            modelBuilder.Entity<COM_CONTROL_DEFINE>().HasKey(e => new { e.CONTROL_ID, e.CONTROL_TYPE });
            modelBuilder.Entity<COM_CONTROL_UNUSED>().HasKey(e => new { e.LOCATION_LAYER_ID, e.CONTROL_ID, e.CONTROL_TYPE });
            modelBuilder.Entity<COM_LISTITEM_DEFINE>().HasKey(e => new { e.PGMID, e.FORMNO, e.CTRLID, e.DEFINETYPE, e.ITEMNO });
            modelBuilder.Entity<COM_LISTITEM_USER>().HasKey(e => new { e.USERID, e.PGMID, e.FORMNO, e.CTRLID, e.DEFINETYPE, e.ITEMNO });
            modelBuilder.Entity<COM_USER_AUTH>().HasKey(e => new { e.USERID, e.CONDUCTID, e.BUSHOCODE, e.AUTHKBN });
            modelBuilder.Entity<COM_USER_MST>().HasKey(e => new { e.USERID });

        }

        /// <summary>
        /// データベースのテーブル名、カラム名を小文字にする
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void NamesToLower(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COM_CONDUCT_MST>().ToTable("cm_conduct");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.CONDUCTID).HasColumnName("conduct_id");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.CONDUCTGRP).HasColumnName("conduct_group_id");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.NAME).HasColumnName("conduct_name");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.RYAKU).HasColumnName("conduct_name_ryaku");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.PTN).HasColumnName("process_pattern");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.MENUORDER).HasColumnName("menu_order");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.MENUDISP).HasColumnName("menu_division");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.PGMID).HasColumnName("program_id");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.BOOTPARAM).HasColumnName("start_up_parameters");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.CM_CONDUCTID).HasColumnName("common_conduct_id");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.VERSION).HasColumnName("version");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.CHANGELOG).HasColumnName("update_information");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.DELFLG).HasColumnName("delete_flg");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.INSERT_DATETIME).HasColumnName("insert_datetime");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.INSERT_USER_ID).HasColumnName("insert_user_id");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.UPDATE_DATETIME).HasColumnName("update_datetime");
            modelBuilder.Entity<COM_CONDUCT_MST>().Property(entity => entity.UPDATE_USER_ID).HasColumnName("update_user_id");

            modelBuilder.Entity<COM_FORM_DEFINE>().ToTable("cm_form_define");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.PGMID).HasColumnName("program_id");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.FORMNO).HasColumnName("form_no");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTRLID).HasColumnName("control_group_id");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTRLNAME).HasColumnName("control_group_name");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTRLTYPE).HasColumnName("control_group_type");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.AREAKBN).HasColumnName("area_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DISPORDER).HasColumnName("display_order");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_FORMTITLE).HasColumnName("dat_form_title");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTRLGRPNO).HasColumnName("group_no");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTRLGRPNAME).HasColumnName("group_name");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.TABNO).HasColumnName("tab_no");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.TABNAME).HasColumnName("tab_name");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TRANSPTN).HasColumnName("dat_transition_pattern");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TRANSDISPPTN).HasColumnName("dat_transition_display_pattern");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TRANSTARGET).HasColumnName("dat_transition_target");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TRANSPARAM).HasColumnName("dat_transition_parameters");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_EDITPTN).HasColumnName("dat_edit_pattern");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_DIRECTION).HasColumnName("dat_direction");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_HEADERDISPKBN).HasColumnName("dat_header_display_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_PAGEROWS).HasColumnName("dat_page_rows");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_MAXROWS).HasColumnName("dat_maximum_rows");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TITLE).HasColumnName("dat_title");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_SWITCHKBN).HasColumnName("dat_switch_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_ROWADDKBN).HasColumnName("dat_row_add_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_ROWDELKBN).HasColumnName("dat_row_delete_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_ROWSELKBN).HasColumnName("dat_row_select_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_COLSELKBN).HasColumnName("dat_column_select_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_ROWSORTKBN).HasColumnName("dat_row_sort_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_TRANSICONKBN).HasColumnName("dat_rowno_pattern");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DAT_HEIGHT).HasColumnName("dat_height");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTR_RELATIONCTRLID).HasColumnName("control_relation_control_id");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CTR_POSITIONKBN).HasColumnName("control_position_division");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.CSSNAME).HasColumnName("css_name");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.TOOLTIP).HasColumnName("tooltip");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.REFERENCE_MODE).HasColumnName("reference_mode_flg");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.COMM_FORMNO).HasColumnName("common_form_no");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.DELFLG).HasColumnName("delete_flg");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.INSERT_DATETIME).HasColumnName("insert_datetime");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.INSERT_USER_ID).HasColumnName("insert_user_id");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.UPDATE_DATETIME).HasColumnName("update_datetime");
            modelBuilder.Entity<COM_FORM_DEFINE>().Property(entity => entity.UPDATE_USER_ID).HasColumnName("update_user_id");

            //TMQカスタマイズ start====================================
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().ToTable("com_listitem_define");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.PGMID).HasColumnName("pgmid");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.FORMNO).HasColumnName("formno");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.CTRLID).HasColumnName("ctrlid");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.DEFINETYPE).HasColumnName("definetype");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.ITEMNO).HasColumnName("itemno");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.ITEMNAME).HasColumnName("itemname");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.DISPKBN).HasColumnName("dispkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.ROWNO).HasColumnName("rowno");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.COLNO).HasColumnName("colno");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.ROWSPAN).HasColumnName("rowspan");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.COLSPAN).HasColumnName("colspan");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.HEADER_ROWSPAN).HasColumnName("header_rowspan");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.HEADER_COLSPAN).HasColumnName("header_colspan");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.HEADER_ALIGN).HasColumnName("header_align");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.CELLTYPE).HasColumnName("celltype");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.COLWIDTH).HasColumnName("colwidth");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.FROMTOKBN).HasColumnName("fromtokbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.ITEM_CNT).HasColumnName("item_cnt");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.INITVAL).HasColumnName("initval");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.NULLCHKKBN).HasColumnName("nullchkkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.MINVAL).HasColumnName("minval");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.MAXVAL).HasColumnName("maxval");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.FORMAT).HasColumnName("format");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.MAXLENGTH).HasColumnName("maxlength");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.TXT_AUTOCOMPKBN).HasColumnName("txt_autocompkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.BTN_CTRLID).HasColumnName("btn_ctrlid");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.BTN_ACTIONKBN).HasColumnName("btn_actionkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.BTN_AUTHCONTROLKBN).HasColumnName("btn_authcontrolkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.BTN_AFTEREXECKBN).HasColumnName("btn_afterexeckbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.BTN_MESSAGE).HasColumnName("btn_message");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.RELATIONID).HasColumnName("relationid");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.RELATIONPARAM).HasColumnName("relationparam");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.OPTIONINFO).HasColumnName("optioninfo");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.UNCHANGEABLEKBN).HasColumnName("unchangeablekbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.COLFIXKBN).HasColumnName("colfixkbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.FILTERUSEKBN).HasColumnName("filterusekbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.TXT_PLACEHOLDER).HasColumnName("txt_placeholder");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.CSSNAME).HasColumnName("cssname");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.TOOLTIP).HasColumnName("tooltip");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_KEY_NAME).HasColumnName("exp_key_name");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_TABLE_NAME).HasColumnName("exp_table_name");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_COL_NAME).HasColumnName("exp_col_name");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_PARAM_NAME).HasColumnName("exp_param_name");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_ALIAS_NAME).HasColumnName("exp_alias_name");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_LIKE_PATTERN).HasColumnName("exp_like_pattern");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_IN_CLAUSE_KBN).HasColumnName("exp_in_clause_kbn");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.EXP_LOCK_TYPE).HasColumnName("exp_lock_type");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.DELFLG).HasColumnName("delflg");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.UPDYMD).HasColumnName("updymd");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.UPDHMIS).HasColumnName("updhmis");
            //modelBuilder.Entity<COM_LISTITEM_DEFINE>().Property(entity => entity.UPDID).HasColumnName("updid");

            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().ToTable("cm_form_control_define");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.LOCATION_LAYER_ID).HasColumnName("location_structure_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.PGMID).HasColumnName("program_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.FORMNO).HasColumnName("form_no");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.CTRLID).HasColumnName("control_group_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DEFINETYPE).HasColumnName("define_type");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.CONTROL_NO).HasColumnName("control_no");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.CONTROL_TYPE).HasColumnName("control_type");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.CONTROL_ID).HasColumnName("control_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DISPKBN).HasColumnName("display_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.ROWNO).HasColumnName("row_no");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.COLNO).HasColumnName("column_no");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.ROWSPAN).HasColumnName("row_span");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.COLSPAN).HasColumnName("column_span");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.HEADER_ROWSPAN).HasColumnName("header_row_span");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.HEADER_COLSPAN).HasColumnName("header_column_span");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.POSITION).HasColumnName("position");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.COLWIDTH).HasColumnName("column_width");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.FROMTOKBN).HasColumnName("from_to_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.ITEM_CNT).HasColumnName("control_count");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.INITVAL).HasColumnName("initial_value");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.NULLCHKKBN).HasColumnName("required_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.TXT_AUTOCOMPKBN).HasColumnName("text_auto_complete_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.BTN_CTRLID).HasColumnName("button_control_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.BTN_ACTIONKBN).HasColumnName("button_action_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.BTN_AUTHCONTROLKBN).HasColumnName("button_authority_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.BTN_AFTEREXECKBN).HasColumnName("button_after_execution_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.BTN_MESSAGE).HasColumnName("button_message");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.RELATIONID).HasColumnName("relation_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.RELATIONPARAM).HasColumnName("relation_parameters");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.OPTIONINFO).HasColumnName("option_information");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.UNCHANGEABLEKBN).HasColumnName("unchangeable_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.COLFIXKBN).HasColumnName("column_fixed_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.FILTERUSEKBN).HasColumnName("filter_use_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.SORT_DIVISION).HasColumnName("sort_division");
            //modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DETAILED_SEARCH_FLG).HasColumnName("detailed_search_flg");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DETAILED_SEARCH_DIVISION).HasColumnName("detailed_search_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DETAILED_SEARCH_CONTROL_TYPE).HasColumnName("detailed_search_control_type");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.ITEM_CUSTOMIZE_FLG).HasColumnName("control_customize_flg");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.CSSNAME).HasColumnName("css_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_KEY_NAME).HasColumnName("expansion_key_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_TABLE_NAME).HasColumnName("expansion_table_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_COL_NAME).HasColumnName("expansion_column_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_PARAM_NAME).HasColumnName("expansion_parameters_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_ALIAS_NAME).HasColumnName("expansion_alias_name");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_LIKE_PATTERN).HasColumnName("expansion_like_pattern");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_IN_CLAUSE_KBN).HasColumnName("expansion_in_clause_division");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.EXP_LOCK_TYPE).HasColumnName("expansion_lock_type");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.DELFLG).HasColumnName("delete_flg");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.INSERT_DATETIME).HasColumnName("insert_datetime");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.INSERT_USER_ID).HasColumnName("insert_user_id");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.UPDATE_DATETIME).HasColumnName("update_datetime");
            modelBuilder.Entity<COM_FORM_CONTROL_DEFINE>().Property(entity => entity.UPDATE_USER_ID).HasColumnName("update_user_id");

            modelBuilder.Entity<COM_CONTROL_DEFINE>().ToTable("cm_control_define");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.CONTROL_ID).HasColumnName("control_id");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.CONTROL_TYPE).HasColumnName("control_type");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.MINVAL).HasColumnName("minimum_value");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.MAXVAL).HasColumnName("maximum_value");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.FORMAT_TRANSLATION_ID).HasColumnName("format_translation_id");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.MAXLENGTH).HasColumnName("maximum_length");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.TEXT_PLACEHOLDER_TRANSLATION_ID).HasColumnName("text_placeholder_translation_id");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.TOOLTIP_TRANSLATION_ID).HasColumnName("tooltip_translation_id");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.DELFLG).HasColumnName("delete_flg");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.INSERT_DATETIME).HasColumnName("insert_datetime");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.INSERT_USER_ID).HasColumnName("insert_user_id");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.UPDATE_DATETIME).HasColumnName("update_datetime");
            modelBuilder.Entity<COM_CONTROL_DEFINE>().Property(entity => entity.UPDATE_USER_ID).HasColumnName("update_user_id");

            modelBuilder.Entity<COM_CONTROL_UNUSED>().ToTable("cm_control_unused");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.LOCATION_LAYER_ID).HasColumnName("location_structure_id");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.CONTROL_ID).HasColumnName("control_id");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.CONTROL_TYPE).HasColumnName("control_type");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.DELFLG).HasColumnName("delete_flg");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.INSERT_DATETIME).HasColumnName("insert_datetime");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.INSERT_USER_ID).HasColumnName("insert_user_id");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.UPDATE_DATETIME).HasColumnName("update_datetime");
            modelBuilder.Entity<COM_CONTROL_UNUSED>().Property(entity => entity.UPDATE_USER_ID).HasColumnName("update_user_id");
            //TMQカスタマイズ end====================================

            modelBuilder.Entity<COM_LISTITEM_USER>().ToTable("com_listitem_user");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.USERID).HasColumnName("userid");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.PGMID).HasColumnName("pgmid");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.FORMNO).HasColumnName("formno");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.CTRLID).HasColumnName("ctrlid");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.DEFINETYPE).HasColumnName("definetype");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.ITEMNO).HasColumnName("itemno");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.UPDYMD).HasColumnName("updymd");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.UPDHMIS).HasColumnName("updhmis");
            modelBuilder.Entity<COM_LISTITEM_USER>().Property(entity => entity.UPDID).HasColumnName("updid");

            //modelBuilder.Entity<COM_TMPTBL_DATA>().ToTable("com_tmptbl_data");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.TERMINALNO).HasColumnName("terminalno");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.USERID).HasColumnName("userid");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.CONDUCTID).HasColumnName("conductid");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.FORMNO).HasColumnName("formno");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.REPORT_NO).HasColumnName("report_no");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.CTRLID).HasColumnName("ctrlid");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.DATATYPE).HasColumnName("datatype");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.ROWNO).HasColumnName("rowno");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.ROWSTATUS).HasColumnName("rowstatus");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.UPDTAG).HasColumnName("updtag");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL1).HasColumnName("val1");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL2).HasColumnName("val2");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL3).HasColumnName("val3");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL4).HasColumnName("val4");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL5).HasColumnName("val5");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL6).HasColumnName("val6");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL7).HasColumnName("val7");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL8).HasColumnName("val8");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL9).HasColumnName("val9");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL10).HasColumnName("val10");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL11).HasColumnName("val11");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL12).HasColumnName("val12");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL13).HasColumnName("val13");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL14).HasColumnName("val14");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL15).HasColumnName("val15");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL16).HasColumnName("val16");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL17).HasColumnName("val17");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL18).HasColumnName("val18");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL19).HasColumnName("val19");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL20).HasColumnName("val20");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL21).HasColumnName("val21");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL22).HasColumnName("val22");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL23).HasColumnName("val23");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL24).HasColumnName("val24");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL25).HasColumnName("val25");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL26).HasColumnName("val26");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL27).HasColumnName("val27");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL28).HasColumnName("val28");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL29).HasColumnName("val29");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL30).HasColumnName("val30");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL31).HasColumnName("val31");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL32).HasColumnName("val32");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL33).HasColumnName("val33");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL34).HasColumnName("val34");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL35).HasColumnName("val35");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL36).HasColumnName("val36");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL37).HasColumnName("val37");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL38).HasColumnName("val38");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL39).HasColumnName("val39");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL40).HasColumnName("val40");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL41).HasColumnName("val41");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL42).HasColumnName("val42");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL43).HasColumnName("val43");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL44).HasColumnName("val44");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL45).HasColumnName("val45");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL46).HasColumnName("val46");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL47).HasColumnName("val47");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL48).HasColumnName("val48");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL49).HasColumnName("val49");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL50).HasColumnName("val50");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL51).HasColumnName("val51");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL52).HasColumnName("val52");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL53).HasColumnName("val53");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL54).HasColumnName("val54");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL55).HasColumnName("val55");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL56).HasColumnName("val56");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL57).HasColumnName("val57");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL58).HasColumnName("val58");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL59).HasColumnName("val59");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL60).HasColumnName("val60");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL61).HasColumnName("val61");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL62).HasColumnName("val62");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL63).HasColumnName("val63");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL64).HasColumnName("val64");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL65).HasColumnName("val65");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL66).HasColumnName("val66");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL67).HasColumnName("val67");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL68).HasColumnName("val68");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL69).HasColumnName("val69");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL70).HasColumnName("val70");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL71).HasColumnName("val71");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL72).HasColumnName("val72");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL73).HasColumnName("val73");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL74).HasColumnName("val74");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL75).HasColumnName("val75");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL76).HasColumnName("val76");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL77).HasColumnName("val77");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL78).HasColumnName("val78");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL79).HasColumnName("val79");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL80).HasColumnName("val80");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL81).HasColumnName("val81");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL82).HasColumnName("val82");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL83).HasColumnName("val83");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL84).HasColumnName("val84");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL85).HasColumnName("val85");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL86).HasColumnName("val86");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL87).HasColumnName("val87");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL88).HasColumnName("val88");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL89).HasColumnName("val89");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL90).HasColumnName("val90");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL91).HasColumnName("val91");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL92).HasColumnName("val92");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL93).HasColumnName("val93");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL94).HasColumnName("val94");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL95).HasColumnName("val95");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL96).HasColumnName("val96");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL97).HasColumnName("val97");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL98).HasColumnName("val98");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL99).HasColumnName("val99");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL100).HasColumnName("val100");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL101).HasColumnName("val101");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL102).HasColumnName("val102");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL103).HasColumnName("val103");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL104).HasColumnName("val104");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL105).HasColumnName("val105");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL106).HasColumnName("val106");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL107).HasColumnName("val107");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL108).HasColumnName("val108");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL109).HasColumnName("val109");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL110).HasColumnName("val110");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL111).HasColumnName("val111");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL112).HasColumnName("val112");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL113).HasColumnName("val113");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL114).HasColumnName("val114");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL115).HasColumnName("val115");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL116).HasColumnName("val116");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL117).HasColumnName("val117");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL118).HasColumnName("val118");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL119).HasColumnName("val119");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL120).HasColumnName("val120");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL121).HasColumnName("val121");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL122).HasColumnName("val122");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL123).HasColumnName("val123");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL124).HasColumnName("val124");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL125).HasColumnName("val125");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL126).HasColumnName("val126");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL127).HasColumnName("val127");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL128).HasColumnName("val128");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL129).HasColumnName("val129");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL130).HasColumnName("val130");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL131).HasColumnName("val131");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL132).HasColumnName("val132");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL133).HasColumnName("val133");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL134).HasColumnName("val134");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL135).HasColumnName("val135");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL136).HasColumnName("val136");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL137).HasColumnName("val137");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL138).HasColumnName("val138");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL139).HasColumnName("val139");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL140).HasColumnName("val140");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL141).HasColumnName("val141");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL142).HasColumnName("val142");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL143).HasColumnName("val143");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL144).HasColumnName("val144");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL145).HasColumnName("val145");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL146).HasColumnName("val146");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL147).HasColumnName("val147");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL148).HasColumnName("val148");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL149).HasColumnName("val149");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL150).HasColumnName("val150");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL151).HasColumnName("val151");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL152).HasColumnName("val152");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL153).HasColumnName("val153");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL154).HasColumnName("val154");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL155).HasColumnName("val155");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL156).HasColumnName("val156");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL157).HasColumnName("val157");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL158).HasColumnName("val158");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL159).HasColumnName("val159");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL160).HasColumnName("val160");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL161).HasColumnName("val161");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL162).HasColumnName("val162");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL163).HasColumnName("val163");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL164).HasColumnName("val164");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL165).HasColumnName("val165");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL166).HasColumnName("val166");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL167).HasColumnName("val167");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL168).HasColumnName("val168");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL169).HasColumnName("val169");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL170).HasColumnName("val170");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL171).HasColumnName("val171");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL172).HasColumnName("val172");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL173).HasColumnName("val173");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL174).HasColumnName("val174");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL175).HasColumnName("val175");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL176).HasColumnName("val176");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL177).HasColumnName("val177");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL178).HasColumnName("val178");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL179).HasColumnName("val179");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL180).HasColumnName("val180");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL181).HasColumnName("val181");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL182).HasColumnName("val182");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL183).HasColumnName("val183");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL184).HasColumnName("val184");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL185).HasColumnName("val185");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL186).HasColumnName("val186");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL187).HasColumnName("val187");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL188).HasColumnName("val188");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL189).HasColumnName("val189");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL190).HasColumnName("val190");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL191).HasColumnName("val191");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL192).HasColumnName("val192");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL193).HasColumnName("val193");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL194).HasColumnName("val194");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL195).HasColumnName("val195");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL196).HasColumnName("val196");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL197).HasColumnName("val197");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL198).HasColumnName("val198");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL199).HasColumnName("val199");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.VAL200).HasColumnName("val200");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.INPYMD).HasColumnName("inpymd");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.INPHMIS).HasColumnName("inphmis");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.UPDYMD).HasColumnName("updymd");
            //modelBuilder.Entity<COM_TMPTBL_DATA>().Property(entity => entity.UPDHMIS).HasColumnName("updhmis");

            modelBuilder.Entity<COM_USER_AUTH>().ToTable("com_user_auth");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.USERID).HasColumnName("userid");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.CONDUCTID).HasColumnName("conductid");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.BUSHOCODE).HasColumnName("bushocode");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.AUTHKBN).HasColumnName("authkbn");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.INPYMD).HasColumnName("inpymd");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.INPHMIS).HasColumnName("inphmis");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.INPID).HasColumnName("inpid");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.INPTERMINAL).HasColumnName("inpterminal");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.INPCONDUCTID).HasColumnName("inpconductid");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.UPDYMD).HasColumnName("updymd");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.UPDHMIS).HasColumnName("updhmis");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.UPDID).HasColumnName("updid");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.UPDTERMINAL).HasColumnName("updterminal");
            modelBuilder.Entity<COM_USER_AUTH>().Property(entity => entity.UPDCONDUCTID).HasColumnName("updconductid");

            modelBuilder.Entity<COM_USER_MST>().ToTable("com_user_mst");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.USERID).HasColumnName("userid");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.USERNAME).HasColumnName("username");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.KANA).HasColumnName("kana");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.PASSWORD).HasColumnName("password");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.DELFLG).HasColumnName("delflg");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.INPYMD).HasColumnName("inpymd");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.INPHMIS).HasColumnName("inphmis");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.INPID).HasColumnName("inpid");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.INPTERMINAL).HasColumnName("inpterminal");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.INPCONDUCTID).HasColumnName("inpconductid");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.UPDYMD).HasColumnName("updymd");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.UPDHMIS).HasColumnName("updhmis");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.UPDID).HasColumnName("updid");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.UPDTERMINAL).HasColumnName("updterminal");
            modelBuilder.Entity<COM_USER_MST>().Property(entity => entity.UPDCONDUCTID).HasColumnName("updconductid");

        }



        //public virtual int COM_BUSSINESS_LOGIC_CALL(string x_TERM, string x_ID, string x_INPID, string x_PROC_NAME, ObjectParameter x_STATUS, ObjectParameter x_MSGID, ObjectParameter x_LOGNO) {
        //    var x_TERMParameter = x_TERM != null ?
        //        new ObjectParameter("X_TERM", x_TERM) :
        //        new ObjectParameter("X_TERM", typeof(string));

        //    var x_IDParameter = x_ID != null ?
        //        new ObjectParameter("X_ID", x_ID) :
        //        new ObjectParameter("X_ID", typeof(string));

        //    var x_INPIDParameter = x_INPID != null ?
        //        new ObjectParameter("X_INPID", x_INPID) :
        //        new ObjectParameter("X_INPID", typeof(string));

        //    var x_PROC_NAMEParameter = x_PROC_NAME != null ?
        //        new ObjectParameter("X_PROC_NAME", x_PROC_NAME) :
        //        new ObjectParameter("X_PROC_NAME", typeof(string));

        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("COM_BUSSINESS_LOGIC_CALL", x_TERMParameter, x_IDParameter, x_INPIDParameter, x_PROC_NAMEParameter, x_STATUS, x_MSGID, x_LOGNO);
        //}
    }
}
