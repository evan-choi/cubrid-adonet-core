using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CUBRID.Data
{
    internal sealed class Win64Runtime : IRuntime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_get_db_version(int con_handle, StringBuilder out_buf, int capacity)
            => Win64Interop.cci_get_db_version(con_handle, out_buf, capacity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_size(int mapped_conn_id, string oid_str, string col_attr, ref int col_size, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_size(mapped_conn_id, oid_str, col_attr, ref col_size, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_set_add(int mapped_conn_id, string oid_str, string col_attr, string value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_set_add(mapped_conn_id, oid_str, col_attr, value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_set_drop(int mapped_conn_id, string oid_str, string col_attr, string value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_set_drop(mapped_conn_id, oid_str, col_attr, value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_seq_put(int mapped_conn_id, string oid_str, string col_attr, int index, string value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_seq_put(mapped_conn_id, oid_str, col_attr, index, value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_seq_insert(int mapped_conn_id, string oid_str, string col_attr, int index, string value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_seq_insert(mapped_conn_id, oid_str, col_attr, index, value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_col_seq_drop(int mapped_conn_id, string oid_str, string col_attr, int index, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_col_seq_drop(mapped_conn_id, oid_str, col_attr, index, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_connect_with_url_ex(string url, string db_user, string db_password, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_connect_with_url_ex(url, db_user, db_password, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_oid_get_class_name(int mapped_conn_id, string oid_str, byte[] out_buf, int out_buf_size, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_oid_get_class_name(mapped_conn_id, oid_str, out_buf, out_buf_size, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_set_holdability(int mapped_conn_id, int holdable)
            => Win64Interop.cci_set_holdability(mapped_conn_id, holdable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_set_charset(int mapped_conn_id, string charset)
            => Win64Interop.cci_set_charset(mapped_conn_id, charset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_disconnect(int mapped_conn_id, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_disconnect(mapped_conn_id, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_close_req_handle(int req_id)
            => Win64Interop.cci_close_req_handle(req_id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_prepare(int conn_handle, byte[] sql_stmt, char flag, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_prepare(conn_handle, sql_stmt, flag, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_execute(int req_handle, char flag, int max_col_size, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_execute(req_handle, flag, max_col_size, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr cci_get_result_info_internal(int req_handle, ref int stmt_type, ref int col_num)
            => Win64Interop.cci_get_result_info_internal(req_handle, ref stmt_type, ref col_num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_cursor(int mapped_stmt_id, int offset, CCICursorPosition origin, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_cursor(mapped_stmt_id, offset, origin, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_fetch(int mapped_stmt_id, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_fetch(mapped_stmt_id, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_get_value(CUBRIDConnection con_handle, int col_no, int type, ref IntPtr value)
            => Win64Interop.cci_get_value(con_handle, col_no, type, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_get_data(int req_handle, int col_no, int type, ref IntPtr value, ref int indicator)
            => Win64Interop.cci_get_data(req_handle, col_no, type, ref value, ref indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_get_data(int req_handle, int col_no, int type, ref T_CCI_BIT value, ref int indicator)
            => Win64Interop.cci_get_data(req_handle, col_no, type, ref value, ref indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_set_make(ref IntPtr set, CUBRIDDataType u_type, int size, string[] value, int[] indicator)
            => Win64Interop.cci_set_make(ref set, u_type, size, value, indicator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void cci_set_free(IntPtr set)
            => Win64Interop.cci_set_free(set);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_blob_new(int con_h_id, ref IntPtr blob, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_blob_new(con_h_id, ref blob, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64 cci_blob_size(IntPtr blob)
            => Win64Interop.cci_blob_size(blob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_blob_write(int con_h_id, IntPtr blob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_blob_write(con_h_id, blob, start_pos, length, buf, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_blob_read(int con_h_id, IntPtr blob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_blob_read(con_h_id, blob, start_pos, length, buf, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_blob_free(IntPtr blob)
            => Win64Interop.cci_blob_free(blob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_clob_new(int con_h_id, ref IntPtr clob, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_clob_new(con_h_id, ref clob, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64 cci_clob_size(IntPtr clob)
            => Win64Interop.cci_clob_size(clob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_clob_write(int con_h_id, IntPtr clob, UInt64 start_pos, int length, string buf, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_clob_write(con_h_id, clob, start_pos, length, buf, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_clob_read(int con_h_id, IntPtr clob, UInt64 start_pos, int length, byte[] buf, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_clob_read(con_h_id, clob, start_pos, length, buf, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_clob_free(IntPtr clob)
            => Win64Interop.cci_clob_free(clob);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_set_autocommit(int mapped_conn_id, CCI_AUTOCOMMIT_MODE mode)
            => Win64Interop.cci_set_autocommit(mapped_conn_id, mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CCI_AUTOCOMMIT_MODE cci_get_autocommit(int mapped_conn_id)
            => Win64Interop.cci_get_autocommit(mapped_conn_id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_get_db_parameter(int con_handle, T_CCI_DB_PARAM param_name, ref int value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_get_db_parameter(con_handle, param_name, ref value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_set_db_parameter(int con_handle, T_CCI_DB_PARAM param_name, ref int value, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_set_db_parameter(con_handle, param_name, ref value, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_end_tran(int con_handle, char type, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_end_tran(con_handle, type, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int bind_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type, byte[] value, CUBRIDDataType u_type, char flag)
            => Win64Interop.bind_param(mapped_stmt_id, index, a_type, value, u_type, flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int bind_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type, IntPtr value, CUBRIDDataType u_type, char flag)
            => Win64Interop.bind_param(mapped_stmt_id, index, a_type, value, u_type, flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_register_out_param(int mapped_stmt_id, int index, T_CCI_A_TYPE a_type)
            => Win64Interop.cci_register_out_param(mapped_stmt_id, index, a_type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_execute_batch_internal(int conn_handle, int num_sql_stmt, string[] sql_stmt, ref IntPtr query_result, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_execute_batch_internal(conn_handle, num_sql_stmt, sql_stmt, ref query_result, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_query_result_free(IntPtr query_result, int num_query)
            => Win64Interop.cci_query_result_free(query_result, num_query);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_next_result(int req_handle, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_next_result(req_handle, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int get_query_plan(int req_handle, ref IntPtr out_buf_p)
            => Win64Interop.get_query_plan(req_handle, ref out_buf_p);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_schema_info(int conn_handle, T_CCI_SCH_TYPE type, string class_name, string attr_name, char flag, ref T_CCI_ERROR err_buf)
            => Win64Interop.cci_schema_info(conn_handle, type, class_name, attr_name, flag, ref err_buf);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int cci_query_info_free(IntPtr out_buf)
            => Win64Interop.cci_query_info_free(out_buf);
    }
}
