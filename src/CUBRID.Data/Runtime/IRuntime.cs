using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CUBRID.Data
{
    internal interface IRuntime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_get_db_version(int con_handle, StringBuilder out_buf, int capacity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_size(int mapped_conn_id, string oid_str, string col_attr, ref int col_size, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_set_add(int mapped_conn_id, string oid_str, string col_attr, string value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_set_drop(int mapped_conn_id, string oid_str, string col_attr, string value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_seq_put(int mapped_conn_id, string oid_str, string col_attr, int index, string value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_seq_insert(int mapped_conn_id, string oid_str, string col_attr, int index, string value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_col_seq_drop(int mapped_conn_id, string oid_str, string col_attr, int index, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_connect_with_url_ex(string url, string db_user, string db_password, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_oid_get_class_name(int mapped_conn_id, string oid_str, byte[] out_buf, int out_buf_size, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_set_holdability(int mapped_conn_id, int holdable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_set_charset(int mapped_conn_id, string charset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_disconnect(int mapped_conn_id, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_close_req_handle(int req_id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_prepare(int conn_handle, byte[] sql_stmt, char flag, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_execute(int req_handle, char flag, int max_col_size, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IntPtr cci_get_result_info_internal(int req_handle, ref int stmt_type, ref int col_num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_cursor(int mapped_stmt_id, int offset, CCICursorPosition origin, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_fetch(int mapped_stmt_id, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_get_value(CUBRIDConnection con_handle, int col_no, int type, ref IntPtr value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_get_data(int req_handle, int col_no, int type, ref IntPtr value, ref int indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_get_data(int req_handle, int col_no, int type, ref T_CCI_BIT value, ref int indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_set_make(ref IntPtr set, CUBRIDDataType u_type, int size, string[] value, int[] indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void cci_set_free(IntPtr set);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_blob_new(int con_h_id, ref IntPtr blob, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        UInt64 cci_blob_size(IntPtr blob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_blob_write(int con_h_id, IntPtr blob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_blob_read(int con_h_id, IntPtr blob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_blob_free(IntPtr blob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_clob_new(int con_h_id, ref IntPtr clob, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        UInt64 cci_clob_size(IntPtr clob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_clob_write(int con_h_id, IntPtr clob, UInt64 start_pos, int length, string buf, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_clob_read(int con_h_id, IntPtr clob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_clob_free(IntPtr clob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_set_autocommit(int mapped_conn_id, CCI_AUTOCOMMIT_MODE mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        CCI_AUTOCOMMIT_MODE cci_get_autocommit(int mapped_conn_id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_get_db_parameter(int con_handle, T_CCI_DB_PARAM param_name, ref int value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_set_db_parameter(int con_handle, T_CCI_DB_PARAM param_name, ref int value, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_end_tran(int con_handle, char type, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int bind_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type, byte[] value, CUBRIDDataType u_type, char flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int bind_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type, IntPtr value, CUBRIDDataType u_type, char flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_register_out_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_execute_batch_internal(int conn_handle, int num_sql_stmt, string[] sql_stmt, ref IntPtr query_result, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_query_result_free(IntPtr query_result, int num_query);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_next_result(int req_handle, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int get_query_plan(int req_handle, ref IntPtr out_buf_p);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_schema_info(int conn_handle, T_CCI_SCH_TYPE type, string class_name, string attr_name, char flag, ref T_CCI_ERROR err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int cci_query_info_free(IntPtr out_buf);
    }
}
