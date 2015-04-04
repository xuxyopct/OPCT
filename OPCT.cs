using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Xml;
using System.Timers;

namespace OPCT
{
    public partial class OPCT : Form
    {
        private string connOStr = "";
        private Form subForm = null;
        System.Timers.Timer aTimer = new System.Timers.Timer();

        public OPCT()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Version:       1.0 \r\n" + "Developer:   xuxiaoyi@gmail.com  \r\n" + "Copyright:    2012-2012");
            About ab = new About();
            ab.ShowDialog();
        }

        public void getInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument doc = new XmlDocument();
                //doc.Load("app.config");
                //XmlNode root = doc.SelectSingleNode("//configuration");
                //XmlNode node = root.SelectSingleNode("//appSettings/add[@name='ConnectionString']");
                //XmlElement el = node as XmlElement;
                //string connStr = el.GetAttribute("connectionString");
                OracleConnection conn = new OracleConnection(this.dataSourceTextBox.Text);
                conn.Open();
                OracleCommand connCount = conn.CreateCommand();
                connCount.CommandText = "select value from v$parameter  where name='processes'";
                this.connTextBox.Text = Convert.ToString(connCount.ExecuteScalar()).Trim();
                OracleCommand baseDbInfo = conn.CreateCommand();
                baseDbInfo.CommandText = "select a.instance_name,a.status,b.name,a.database_status,a.version,b.created,b.log_mode,b.open_mode,a.startup_time" +
                                                      " from gv$instance a,(select inst_id, name, created, log_mode, open_mode from gv$database) b where a.inst_id = b.inst_id";
                OracleDataReader readerDbInfo = baseDbInfo.ExecuteReader();
                while (readerDbInfo.Read())
                {
                    this.instanceTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(0));
                    label9.Text = this.instanceTextBox.Text.ToUpper();
                    this.statusTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(1));
                    this.versionTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(4));
                    this.dbCreateTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(5));
                    this.logModeTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(6));
                    this.openModeTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(7));
                    this.startupTextBox.Text = Convert.ToString(readerDbInfo.GetOracleValue(8)); 
                }

                if(sgaCheckBox.Checked)
                {
                    string viewTx =   "select round(sga, 0) \"SGA(MB)\", round(cache_size, 0) \"Cache(MB)\", round(coun / cache_size * 100, 0) || '%' \"Cache_Use(%)\",round(shared_size, 0) \"Share_Pool(MB)\","+
                                           "round(freemb / shared_size * 100, 0) || '%' \"Free_Share_Pool(%)\", round(java_size, 2) \"Java_Pool(MB)\",round(large_pool_size, 2) \"Large_Pool(MB)\", round(log, 0) \"Log_Buffer(MB)\""+
                                           "from (select value / 1024 / 1024 sga  from v$parameter where name = 'sga_max_size'), (select s.CURRENT_SIZE / 1024 / 1024 shared_size from v$sga_dynamic_components s where s.COMPONENT = 'shared pool'),"+
                                           "(select sum(s.CURRENT_SIZE) / 1024 / 1024 cache_size from v$sga_dynamic_components s where s.COMPONENT = 'DEFAULT buffer cache'), (select value / 1024 / 1024 java_size from v$parameter where name = 'java_pool_size'),"+
                                           "(select value / 1024 / 1024 large_pool_size from v$parameter where name = 'large_pool_size'), (select value / 1024 log from v$parameter where name = 'log_buffer'), (select bytes / 1024 / 1024 freemb from v$sgastat s"+
                                           " where s.pool = 'shared pool' and s.name = 'free memory'), (select sum(count(*)) * 8 / 1024 coun from v$bh where status <> 'free' group by status)";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaDataGridView.AutoGenerateColumns = true;
                    sgaDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaDataGridView.ReadOnly = true;
                    sgaDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaDataGridView.ColumnHeadersVisible = true;
                    sgaDataGridView.DataSource = ds;
                    sgaDataGridView.DataMember = "viewTx";
                }

                if(pgaCheckBox.Checked)
                {
                    string viewTx = "select round(total, 0) \"PGA(MB)\",round(inuse, 0) \"InUseTotal(MB)\",round(allocated, 0) \"AllocatedTotal(MB)\",mem, disk from (select value / 1024 / 1024 total"+
                                         " from v$pgastat where name like 'aggregate PGA t%'),(select value / 1024 / 1024 inuse from v$pgastat where name like 'total PGA i%'),"+
                                         " (select value / 1024 / 1024 allocated from v$pgastat where name like 'total PGA a%'), (select mem.value mem, disk.value disk from v$sysstat mem," +
                                         "v$sysstat disk where mem.name = 'sorts (memory)' and disk.name = 'sorts (disk)')";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    pgaDataGridView.AutoGenerateColumns = true;
                    pgaDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    pgaDataGridView.ReadOnly = true;
                    pgaDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    pgaDataGridView.ColumnHeadersVisible = true;
                    pgaDataGridView.DataSource = ds;
                    pgaDataGridView.DataMember = "viewTx";
                }

                if(useCheckBox.Checked)
                {
                    string viewTx = "select round(sum(totalmb / 1024), 1) \"Total(GB)\",round(sum(totalmb / 1024 - nvl(freemb / 1024, 0)), 1) \"Used(GB)\", round(sum(freemb / 1024), 1) \"Free(GB)\" " +
                                         "from (select sum(bytes) / 1024 / 1024 freemb, tablespace_name  from dba_free_space group by tablespace_name) a, " +
                                         "(select sum(bytes) / 1024 / 1024 totalmb, tablespace_name from dba_data_files group by tablespace_name) b where a.tablespace_name(+) " + 
                                         "= b.tablespace_name";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    useDataGridView.AutoGenerateColumns = true;
                    useDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    useDataGridView.ReadOnly = true;
                    useDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    useDataGridView.ColumnHeadersVisible = true;
                    useDataGridView.DataSource = ds;
                    useDataGridView.DataMember = "viewTx";
                }

                if(tableSpaceCheckBox.Checked)
                {
                    string viewTx = "select nvl(b.tablespace_name, nvl(a.tablespace_name, 'UNKNOWN')) TableSpaceName,c.segment_space_management,c.block_size,c.status,round(totalmb, 0) \"Total(MB)\","+
                                          " round(nvl(freemb, 0), 0) \"Free(MB)\", round(((totalmb - nvl(freemb, 0)) / totalmb) * 100, 0) || '%' \"Used(%)\"  from (select sum(bytes) / 1024 / 1024 freemb, tablespace_name"+
                                          " from dba_free_space group by tablespace_name) a, (select sum(bytes) / 1024 / 1024 totalmb, tablespace_name from dba_data_files group by tablespace_name) b, (select segment_space_management,"+
                                          " block_size, status, tablespace_name, CONTENTS from dba_tablespaces) c where a.tablespace_name(+) = b.tablespace_name and c.tablespace_name = b.tablespace_name order by \"Used(%)\" desc";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tableSpaceDataGridView.AutoGenerateColumns = true;
                    tableSpaceDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tableSpaceDataGridView.ReadOnly = true;
                    tableSpaceDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tableSpaceDataGridView.ColumnHeadersVisible = true;
                    tableSpaceDataGridView.DataSource = ds;
                    tableSpaceDataGridView.DataMember = "viewTx";
                }

                if(dbFileCheckBox.Checked)
                {
                    string viewTx = "select tname \"TableSpaceName\", file_name \"FileName\", nvl(totalmb,0) \"Total(MB)\", nvl(freemb, 0) \"Free(MB)\", round((1 - nvl(freemb, 0) / totalmb) * 100, 0) \"Used(%)\", autoextensible \"AutoExtensible\","+
                                          "status \" Status\" from (select round((bytes / 1024 / 1024), 0) totalmb, file_id, tablespace_name tname, file_id fid, file_name, autoextensible, status"+
                                          " from dba_data_files) a, (select round(sum(bytes / 1024 / 1024), 0) freemb, file_id from dba_free_space group by file_id) b where a.file_id = b.file_id(+)"+
                                          "order by tname desc";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    dbFileDataGridView.AutoGenerateColumns = true;
                    dbFileDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    dbFileDataGridView.ReadOnly = true;
                    dbFileDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dbFileDataGridView.ColumnHeadersVisible = true;
                    dbFileDataGridView.DataSource = ds;
                    dbFileDataGridView.DataMember = "viewTx";
                }

                if(tempDbCheckBox.Checked)
                {
                    string viewTx = "select te.file# \"File#\", te.BYTES / 1024 / 1024 \"Size(MB)\",te.status \"Status\", tablespace_name \"TableSpaceName\", name \"FileName\", dtf.autoextensible \"AutoExtensible\" from v$tempfile te"+
                                         " join dba_temp_files dtf on dtf.file_id = te.FILE#  order by file#";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tempDbDataGridView.AutoGenerateColumns = true;
                    tempDbDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tempDbDataGridView.ReadOnly = true;
                    tempDbDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tempDbDataGridView.ColumnHeadersVisible = true;
                    tempDbDataGridView.DataSource = ds;
                    tempDbDataGridView.DataMember = "viewTx";
                }

                if(tempDbUsedCheckBox.Checked)
                {
                    string viewTx = "select a.tablespace_name \"TableSpaceName\", b.totalMB \"Total(MB)\", a.usedmb \"Used(MB)\", round(b.totalmb - a.usedmb,2) \"Free(MB)\","+
                                         " round(a.usedmb / b.totalmb, 2) * 100 || '%' \"Used(%)\"  from (select tablespace_name, sum(bytes_used) / 1024 / 1024 usedMB from v$temp_space_header"+
                                         "  group by tablespace_name) a,(select tablespace_name, sum(bytes) / 1024 / 1024 totalMB from dba_temp_files group by tablespace_name) b"+
                                         " where a.tablespace_name = b.tablespace_name";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tempDbUsedDataGridView.AutoGenerateColumns = true;
                    tempDbUsedDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tempDbUsedDataGridView.ReadOnly = true;
                    tempDbUsedDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tempDbUsedDataGridView.ColumnHeadersVisible = true;
                    tempDbUsedDataGridView.DataSource = ds;
                    tempDbUsedDataGridView.DataMember = "viewTx";
                }

                if(tableLockCheckBox.Checked)
                {
                    string viewTx = "select object_name \"ObjectName\", object_type \"ObjectType\", sid \"Sid\", serial# \"Serial#\", a.process \"Process\", a.lockwait \"LockWait\"  from v$session a, v$locked_object b, all_objects c"+
                                         " where a.process = b.process and c.object_id = b.object_id";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tableLockDataGridView.AutoGenerateColumns = true;
                    tableLockDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tableLockDataGridView.ReadOnly = true;
                    tableLockDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tableLockDataGridView.ColumnHeadersVisible = true;
                    tableLockDataGridView.DataSource = ds;
                    tableLockDataGridView.DataMember = "viewTx";
                }

                if(tableLockCountCheckBox.Checked)
                {
                    string viewTx = "select c.owner \"Owner\",c.object_name \"ObjectName\",c.object_type \"ObjectType\",b.sid \"Sid\", b.serial# \"Serial#\",b.status \"Status\",b.osuser \"OSUser\",b.machine \"Machine\""+
                                         " from v$locked_object a, v$session b, dba_objects c where b.sid = a.session_id and a.object_id = c.object_id";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tableLockCountDataGridView.AutoGenerateColumns = true;
                    tableLockCountDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tableLockCountDataGridView.ReadOnly = true;
                    tableLockCountDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tableLockCountDataGridView.ColumnHeadersVisible = true;
                    tableLockCountDataGridView.DataSource = ds;
                    tableLockCountDataGridView.DataMember = "viewTx";
                }

                if(topSqlCheckBox.Checked)
                {
                    string viewTx =  "select opname \"OPName\",target \"Target\", to_char(start_time, 'yy-mm-dd:hh24:mi:ss') \"StartTime\", elapsed_seconds \"Elapsed(S)\", executions \"Executions\","+
                                          " round(buffer_gets / decode(executions, 0, 1, executions),2)  \"BufferGets\", module \"Module\",sql_text \"SQLText\"  from v$session_longops sl, v$sqlarea sa where sl.sql_hash_value "+ 
                                          "= sa.hash_value and upper(substr(module, 1, 4)) <> 'RMAN' and substr(opname, 1, 4) <> 'RMAN' and module <> 'SQL*Plus' and sl.start_time > trunc(sysdate) order by start_time";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    topSqlDataGridView.AutoGenerateColumns = true;
                    topSqlDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    topSqlDataGridView.ReadOnly = true;
                    topSqlDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    topSqlDataGridView.ColumnHeadersVisible = true;
                    topSqlDataGridView.DataSource = ds;
                    topSqlDataGridView.DataMember = "viewTx";
                }

                if(sessionWaitCheckBox.Checked)
                {
                    string viewTx = "select event \"Event\", sum(decode(wait_Time, 0, 0, 1)) \"Previously\", sum(decode(wait_Time, 0, 1, 0)) \"Current\", count(*) \"Total\" "+
                                         "from v$session_Wait group by event order by 4 desc ";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sessionWaitDataGridView.AutoGenerateColumns = true;
                    sessionWaitDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sessionWaitDataGridView.ReadOnly = true;
                    sessionWaitDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sessionWaitDataGridView.ColumnHeadersVisible = true;
                    sessionWaitDataGridView.DataSource = ds;
                    sessionWaitDataGridView.DataMember = "viewTx";
                }

                if(rollbackCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\", waits \"Waits\", gets \"Gets\", round(waits / gets,5) \"Ratio\"  from v$rollstat a, v$rollname b where a.usn = b.usn";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    rollbackDataGridView.AutoGenerateColumns = true;
                    rollbackDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    rollbackDataGridView.ReadOnly = true;
                    rollbackDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    rollbackDataGridView.ColumnHeadersVisible = true;
                    rollbackDataGridView.DataSource = ds;
                    rollbackDataGridView.DataMember = "viewTx";
                }

                if(tablespaceIOCheckBox.Checked)
                {
                    string viewTx = "select df.tablespace_name \"TablespaceName\",  df.file_name \"FileName\", f.phyrds  \"Phyrds\", f.phywrts  \"Phywrts\",round(f.phyrds / f.phywrts , 2) \"R/W\",f.phyblkrd  \"Phyblkrd\", f.phyblkwrt  \"Phyblkwrt\",round(f.phyblkrd / f.phyblkwrt , 2) \"BR/BW\" from v$filestat f, dba_data_files df" +
                                         " where f.file# = df.file_id order by df.tablespace_name";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tablespaceIODataGridView.AutoGenerateColumns = true;
                    tablespaceIODataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tablespaceIODataGridView.ReadOnly = true;
                    tablespaceIODataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tablespaceIODataGridView.ColumnHeadersVisible = true;
                    tablespaceIODataGridView.DataSource = ds;
                    tablespaceIODataGridView.DataMember = "viewTx";
                }

                if(fileSystemIOCheckBox.Checked)
                {
                    string viewTx = "select substr(a.file#, 1, 2) \"#\", a.name \"Name\", a.status \"Status\", round((a.bytes/1024/1024),2) \"Size(MB)\", b.phyrds \"Phyrds\", b.phywrts \"Phywrts\",round(b.phyrds/b.phywrts,2) \"R/W\"  from v$datafile a, v$filestat b"+
                                          " where a.file# = b.file#";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    fileSystemIODataGridView.AutoGenerateColumns = true;
                    fileSystemIODataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    fileSystemIODataGridView.ReadOnly = true;
                    fileSystemIODataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    fileSystemIODataGridView.ColumnHeadersVisible = true;
                    fileSystemIODataGridView.DataSource = ds;
                    fileSystemIODataGridView.DataMember = "viewTx";
                }

                if(userIndexCheckBox.Checked)
                {
                    string viewTx = "select user_indexes.table_name \"TableName\", user_indexes.index_name \"IndexName\", uniqueness \"UniqueNess\", column_name  \"ColumnName\"  from user_ind_columns, user_indexes where user_ind_columns.index_name = user_indexes.index_name"+
                                         " and user_ind_columns.table_name = user_indexes.table_name order by user_indexes.table_type, user_indexes.table_name, user_indexes.index_name, column_position";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    userIndexDataGridView.AutoGenerateColumns = true;
                    userIndexDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    userIndexDataGridView.ReadOnly = true;
                    userIndexDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    userIndexDataGridView.ColumnHeadersVisible = true;
                    userIndexDataGridView.DataSource = ds;
                    userIndexDataGridView.DataMember = "viewTx";
                }

                if(SGARatioCheckBox.Checked)
                {
                    string viewTx = "select a.value + b.value \"LogicalReads\",c.value \"PhysReads\", case when round(((a.value + b.value) - c.value) / (a.value + b.value),5) < 0 then 100 else 100 * round (((a.value + b.value) - c.value) / (a.value + b.value),5) end  \"BufferHitRatio\" from v$sysstat a, v$sysstat b, v$sysstat c" +
                                         " where a.statistic# = 38 and b.statistic# = 39 and c.statistic# = 40";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaRatioDataGridView.AutoGenerateColumns = true;
                    sgaRatioDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaRatioDataGridView.ReadOnly = true;
                    sgaRatioDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaRatioDataGridView.ColumnHeadersVisible = true;
                    sgaRatioDataGridView.DataSource = ds;
                    sgaRatioDataGridView.DataMember = "viewTx";
                }

                if(sgaDicRatioCheckBox.Checked)
                {
                    string viewTx =  "select parameter \"Parameter\", gets \"Gets\",Getmisses \"GetMisses\", round(getmisses / (gets + getmisses) * 100,2) \"MissRatio\","+
                                          " round((1 - (sum(getmisses) / (sum(gets) + sum(getmisses)))) * 100,2) \"HitRatio\" from v$rowcache where gets + getmisses <> 0 "+
                                          " group by parameter, gets, getmisses";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaDicRatioDataGridView.AutoGenerateColumns = true;
                    sgaDicRatioDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaDicRatioDataGridView.ReadOnly = true;
                    sgaDicRatioDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaDicRatioDataGridView.ColumnHeadersVisible = true;
                    sgaDicRatioDataGridView.DataSource = ds;
                    sgaDicRatioDataGridView.DataMember = "viewTx";
                }

                if(sgaShareRatioCheckBox.Checked)
                {
                    string viewTx = "select sum(pins) \"TotalPins\", sum(reloads) \"TotalReloads\", round(sum(reloads) / sum(pins) * 100,2) \"LibCache\", round(sum(pinhits - reloads) / sum(pins),2) \"HitRadio\","+
                                          " round(sum(reloads) / sum(pins),2) \"ReloadPercent\" from v$librarycache";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaShareRatioDataGridView.AutoGenerateColumns = true;
                    sgaShareRatioDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaShareRatioDataGridView.ReadOnly = true;
                    sgaShareRatioDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaShareRatioDataGridView.ColumnHeadersVisible = true;
                    sgaShareRatioDataGridView.DataSource = ds;
                    sgaShareRatioDataGridView.DataMember = "viewTx";
                }

                if(sgaRedoBufferRatioCheckBox.Checked)
                {
                    string viewTx = "SELECT name \"Name\",gets \"Gets\",misses \"Misses\",immediate_gets \"ImmediateGets\",immediate_misses \"ImmediateMisses\",round(Decode(gets, 0, 0, misses / gets * 100),2) \"Ratio1\","+
                                          " round(Decode(immediate_gets + immediate_misses,0,0,immediate_misses / (immediate_gets + immediate_misses) * 100),2) \"Ratio2\"  FROM v$latch WHERE name IN ('redo allocation', 'redo copy')";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaRedoBufferRatioDataGridView.AutoGenerateColumns = true;
                    sgaRedoBufferRatioDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaRedoBufferRatioDataGridView.ReadOnly = true;
                    sgaRedoBufferRatioDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaRedoBufferRatioDataGridView.ColumnHeadersVisible = true;
                    sgaRedoBufferRatioDataGridView.DataSource = ds;
                    sgaRedoBufferRatioDataGridView.DataMember = "viewTx";
                }

                if(memoryDiskRatioCheckBox.Checked)
                {
                    string viewTx = "SELECT name \"Name\", value \"Value\"  FROM v$sysstat WHERE name IN ('sorts (memory)', 'sorts (disk)')";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    memoryDiskRatioDataGridView.AutoGenerateColumns = true;
                    memoryDiskRatioDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    memoryDiskRatioDataGridView.ReadOnly = true;
                    memoryDiskRatioDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    memoryDiskRatioDataGridView.ColumnHeadersVisible = true;
                    memoryDiskRatioDataGridView.DataSource = ds;
                    memoryDiskRatioDataGridView.DataMember = "viewTx";
                }

                if(runSqlCheckBox.Checked)
                {
                    string viewTx = "SELECT osuser \"OSUser\", username \"UserName\", sql_text \"SQLText\"  from v$session a, v$sqltext b where a.sql_address = b.address order by address, piece";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    runSqlDataGridView.AutoGenerateColumns = true;
                    runSqlDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    runSqlDataGridView.ReadOnly = true;
                    runSqlDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    runSqlDataGridView.ColumnHeadersVisible = true;
                    runSqlDataGridView.DataSource = ds;
                    runSqlDataGridView.DataMember = "viewTx";
                }
                
                if(chSetCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\",value$ \"Value\",comment$ \"Comment\" from sys.props$ where name = 'NLS_CHARACTERSET'";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    chSetDataGridView.AutoGenerateColumns = true;
                    chSetDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    chSetDataGridView.ReadOnly = true;
                    chSetDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    chSetDataGridView.ColumnHeadersVisible = true;
                    chSetDataGridView.DataSource = ds;
                    chSetDataGridView.DataMember = "viewTx";
                }

                if(mtsCheckBox.Checked)
                {
                    string viewTx = "select round(busy / (busy + idle),5) \"SharedServersBusy\" from v$dispatcher";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    mtsDataGridView.AutoGenerateColumns = true;
                    mtsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    mtsDataGridView.ReadOnly = true;
                    mtsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    mtsDataGridView.ColumnHeadersVisible = true;
                    mtsDataGridView.DataSource = ds;
                    mtsDataGridView.DataMember = "viewTx";
                }

                if(tableDeepCheckBox.Checked)
                {
                    string viewTx = "SELECT segment_name \"TableName\", COUNT(*) \"Extents\" FROM dba_segments WHERE owner NOT IN ('SYS', 'SYSTEM') GROUP BY segment_name HAVING COUNT(*) = (SELECT MAX(COUNT(*))"+
                                         "  FROM dba_segments GROUP BY segment_name)";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    tableDeepDataGridView.AutoGenerateColumns = true;
                    tableDeepDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    tableDeepDataGridView.ReadOnly = true;
                    tableDeepDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    tableDeepDataGridView.ColumnHeadersVisible = true;
                    tableDeepDataGridView.DataSource = ds;
                    tableDeepDataGridView.DataMember = "viewTx";
                }

                if(usedSessionCpuStorageCheckBox.Checked)
                {
                    string viewTx = "";
                    if (this.allRadioButton.Checked == true)
                       viewTx = "select a.sid \"Sid\", a.Serial# \"Serial#\",spid \"Spid\", status \"Status\",a.program \"Program\", a.terminal \"Terminal\",osuser \"OSUser\",a.machine \"Machine\",round(value / 60 / 100,5) \"Value\"  from v$session a, v$process b, v$sesstat c"+
                                         " where c.statistic# = 12 and c.sid = a.sid and a.paddr = b.addr order by value desc";
                    if (this.activeRadioButton.Checked == true)
                        viewTx = "select a.sid \"Sid\", a.Serial# \"Serial#\",spid \"Spid\", status \"Status\",a.program \"Program\", a.terminal \"Terminal\",osuser \"OSUser\", a.machine \"Machine\",round(value / 60 / 100,5) \"Value\"  from v$session a, v$process b, v$sesstat c" +
                                          " where c.statistic# = 12 and c.sid = a.sid and a.paddr = b.addr and a.status = 'ACTIVE' order by value desc";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    usedSessionCpuDataGridView.AutoGenerateColumns = true;
                    usedSessionCpuDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    usedSessionCpuDataGridView.ReadOnly = true;
                    usedSessionCpuDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    usedSessionCpuDataGridView.ColumnHeadersVisible = true;
                    usedSessionCpuDataGridView.DataSource = ds;
                    usedSessionCpuDataGridView.DataMember = "viewTx";
                }

                if (userPrivsCheckBox.Checked)
                {
                    string currentUser = string.Empty;
                    if(!string.IsNullOrEmpty(dataSourceTextBox.Text))
                    {
                        currentUser = dataSourceTextBox.Text.Substring(dataSourceTextBox.Text.IndexOf("User ID=") + 8, (dataSourceTextBox.Text.IndexOf(";Password=") - dataSourceTextBox.Text.IndexOf("User ID=") - 8));
                    }
                    string viewTx = "SELECT t.grantee  \"Grantee\",t.privilege  \"Privilega\",t.admin_option  \"Admin_Option\" FROM DBA_SYS_PRIVS t where t.grantee = '" + currentUser + "'";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    userPrivsDataGridView.AutoGenerateColumns = true;
                    userPrivsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    userPrivsDataGridView.ReadOnly = true;
                    userPrivsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    userPrivsDataGridView.ColumnHeadersVisible = true;
                    userPrivsDataGridView.DataSource = ds;
                    userPrivsDataGridView.DataMember = "viewTx";
                }
                if (userSessionGroupCheckBox.Checked)
                {
                    string viewTx = "";
                    if (this.allRadioButton.Checked == true)
                        viewTx = "select osuser \"OSUser\",count(*) \"Count\" from v$session a, v$process b, v$sesstat c  where c.statistic# = 12 and c.sid = a.sid and a.paddr = b.addr group by osuser";
                    if (this.activeRadioButton.Checked == true)
                        viewTx = "select osuser \"OSUser\",count(*) \"Count\" from v$session a, v$process b, v$sesstat c  where c.statistic# = 12 and c.sid = a.sid and a.paddr = b.addr   and a.status = 'ACTIVE'  group by osuser";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    userSessionGroupDataGridView.AutoGenerateColumns = true;
                    userSessionGroupDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    userSessionGroupDataGridView.ReadOnly = true;
                    userSessionGroupDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    userSessionGroupDataGridView.ColumnHeadersVisible = true;
                    userSessionGroupDataGridView.DataSource = ds;
                    userSessionGroupDataGridView.DataMember = "viewTx";
                }
                if (systemCheckBox.Checked)
                {
                    string viewTx = "select * from v$parameter order by num";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    systemDataGridView .AutoGenerateColumns = true;
                    systemDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    systemDataGridView.ReadOnly = true;
                    systemDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    systemDataGridView.ColumnHeadersVisible = true;
                    systemDataGridView.DataSource = ds;
                    systemDataGridView.DataMember = "viewTx";
                }
                if (sysStatCheckBox.Checked)
                {
                    string viewTx = "select stat_name \"Stat_name\",value \"Value\", osstat_id \"OSStat_ID\", comments \"Comments\", cumulative \"Cumulative\" from v$osstat order by osstat_id";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sysStatDataGridView.AutoGenerateColumns = true;
                    sysStatDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sysStatDataGridView.ReadOnly = true;
                    sysStatDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sysStatDataGridView.ColumnHeadersVisible = true;
                    sysStatDataGridView.DataSource = ds;
                    sysStatDataGridView.DataMember = "viewTx";
                }
                if (rollStatCheckBox.Checked)
                {
                    string viewTx = "select d.username \"UserName\", c.name \"Name\", b.writes \"Writes\" from v$transaction a,v$rollstat b,v$rollname c,v$session d where d.taddr = a.addr and a.xidusn = b.usn and b.usn = c.usn order by d.username";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    rollStatDataGridView.AutoGenerateColumns = true;
                    rollStatDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    rollStatDataGridView.ReadOnly = true;
                    rollStatDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    rollStatDataGridView.ColumnHeadersVisible = true;
                    rollStatDataGridView.DataSource = ds;
                    rollStatDataGridView.DataMember = "viewTx";
                }
                if (compressCheckBox.Checked)
                {
                    string viewTx = "select table_name \"Table_name\",num_rows \"Num_rows\",blocks \"Blocks\",avg_row_len \"Avg_row_len\",avg_space \"Avg_space\",compression \"Compression\",compress_for \"Compress_for\" from user_tables where table_name like 'T_%'";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    compressDataGridView.AutoGenerateColumns = true;
                    compressDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    compressDataGridView.ReadOnly = true;
                    compressDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    compressDataGridView.ColumnHeadersVisible = true;
                    compressDataGridView.DataSource = ds;
                    compressDataGridView.DataMember = "viewTx";
                }
                if (sgaInfoCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\",bytes \"Bytes\", round(bytes/1024/1024,2) \"Size(MB)\",resizeable \"Resizeable\" from v$sgainfo";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaInfoDataGridView.AutoGenerateColumns = true;
                    sgaInfoDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaInfoDataGridView.ReadOnly = true;
                    sgaInfoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaInfoDataGridView.ColumnHeadersVisible = true;
                    sgaInfoDataGridView.DataSource = ds;
                    sgaInfoDataGridView.DataMember = "viewTx";
                }
                if (sgaMemUnitCheckBox.Checked)
                {
                    string viewTx = "select component \"Component\",current_size \"Current_size\",round(current_size/1024/1024,2) \"Size(MB)\",granule_size \"Granule_size\", round(granule_size/1024/1024,2) \"Size(MB)\" from v$sga_dynamic_components";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaMemUnitDataGridView.AutoGenerateColumns = true;
                    sgaMemUnitDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaMemUnitDataGridView.ReadOnly = true;
                    sgaMemUnitDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaMemUnitDataGridView.ColumnHeadersVisible = true;
                    sgaMemUnitDataGridView.DataSource = ds;
                    sgaMemUnitDataGridView.DataMember = "viewTx";
                }
                if (asynIOCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\",asynch_io \"Asynch_IO\" from v$datafile f, v$iostat_file i where f.file#=i.file_no and filetype_name='Data File' order by name";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    asynIODataGridView.AutoGenerateColumns = true;
                    asynIODataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    asynIODataGridView.ReadOnly = true;
                    asynIODataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    asynIODataGridView.ColumnHeadersVisible = true;
                    asynIODataGridView.DataSource = ds;
                    asynIODataGridView.DataMember = "viewTx";
                }
                if (sgaLibShareCheckBox.Checked)
                {
                    string viewTx = "select namespace \"NameSpace\", pins \"Pins\", pinhits \"PinHits\",reloads \"Reloads\",invalidations \"Invalidations\" from v$librarycache order by namespace";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaLibShareDataGridView.AutoGenerateColumns = true;
                    sgaLibShareDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaLibShareDataGridView.ReadOnly = true;
                    sgaLibShareDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaLibShareDataGridView.ColumnHeadersVisible = true;
                    sgaLibShareDataGridView.DataSource = ds;
                    sgaLibShareDataGridView.DataMember = "viewTx";
                }
                if (sgaLibShareFreeCheckBox.Checked)
                {
                    string viewTx = "select pool \"Pool\",name \"Name\",bytes \"Bytes\",round(bytes/1024/1024,2) \"Size(MB)\" from v$sgastat where name like '%free memory%'";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    sgaLibShareFreeDataGridView.AutoGenerateColumns = true;
                    sgaLibShareFreeDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    sgaLibShareFreeDataGridView.ReadOnly = true;
                    sgaLibShareFreeDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    sgaLibShareFreeDataGridView.ColumnHeadersVisible = true;
                    sgaLibShareFreeDataGridView.DataSource = ds;
                    sgaLibShareFreeDataGridView.DataMember = "viewTx";
                }
                if (pgaHitRateCheckBox.Checked)
                {
                    string viewTx = "select round(pga_target_for_estimate/1024/1024,2) \"Size(MB)\",estd_pga_cache_hit_percentage \"HitRate%\",estd_overalloc_count \"Estd_Overalloc_Count\",pga_target_factor \"PGA_Target_Factor\" from v$pga_target_advice order by 1";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    pgaHitRateDataGridView.AutoGenerateColumns = true;
                    pgaHitRateDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    pgaHitRateDataGridView.ReadOnly = true;
                    pgaHitRateDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    pgaHitRateDataGridView.ColumnHeadersVisible = true;
                    pgaHitRateDataGridView.DataSource = ds;
                    pgaHitRateDataGridView.DataMember = "viewTx";
                }
                if (norParaCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\",value \"Value\" from v$parameter where name in('processes','sessions','transactions')";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    norParaDataGridView.AutoGenerateColumns = true;
                    norParaDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    norParaDataGridView.ReadOnly = true;
                    norParaDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    norParaDataGridView.ColumnHeadersVisible = true;
                    norParaDataGridView.DataSource = ds;
                    norParaDataGridView.DataMember = "viewTx";
                }
                if (logGroupCheckBox.Checked)
                {
                    string viewTx = "select group# \"Group#\",member \"Member\" from v$logfile";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    logGroupDataGridView.AutoGenerateColumns = true;
                    logGroupDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    logGroupDataGridView.ReadOnly = true;
                    logGroupDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    logGroupDataGridView.ColumnHeadersVisible = true;
                    logGroupDataGridView.DataSource = ds;
                    logGroupDataGridView.DataMember = "viewTx";
                }
                if (databasePropCheckBox.Checked)
                {
                    string viewTx = "select property_name \"Property_Name\",property_value \"Property_Value\",description \"Description\" from database_properties";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    databasePropDataGridView.AutoGenerateColumns = true;
                    databasePropDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    databasePropDataGridView.ReadOnly = true;
                    databasePropDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    databasePropDataGridView.ColumnHeadersVisible = true;
                    databasePropDataGridView.DataSource = ds;
                    databasePropDataGridView.DataMember = "viewTx";
                }
                if (ctrlFileCheckBox.Checked)
                {
                    string viewTx = "select name \"Name\",is_recovery_dest_file \"IS_Recovery_Dest_File\", block_size \"Block_Size\",file_size_blks \"File_Size_Blks\" from v$controlfile";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    ctrlFileDataGridView.AutoGenerateColumns = true;
                    ctrlFileDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    ctrlFileDataGridView.ReadOnly = true;
                    ctrlFileDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    ctrlFileDataGridView.ColumnHeadersVisible = true;
                    ctrlFileDataGridView.DataSource = ds;
                    ctrlFileDataGridView.DataMember = "viewTx";
                }
                if (processCpuCheckBox.Checked)
                {
                    string viewTx = "select * from v$sys_time_model order by value desc";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    processCpuDataGridView.AutoGenerateColumns = true;
                    processCpuDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    processCpuDataGridView.ReadOnly = true;
                    processCpuDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    processCpuDataGridView.ColumnHeadersVisible = true;
                    processCpuDataGridView.DataSource = ds;
                    processCpuDataGridView.DataMember = "viewTx";
                }
                if (networkIOCheckBox.Checked)
                {
                    string viewTx = "select * from v$iostat_network";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    networkIODataGridView.AutoGenerateColumns = true;
                    networkIODataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    networkIODataGridView.ReadOnly = true;
                    networkIODataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    networkIODataGridView.ColumnHeadersVisible = true;
                    networkIODataGridView.DataSource = ds;
                    networkIODataGridView.DataMember = "viewTx";
                }
                if (systemStatCheckBox.Checked)
                {
                    string viewTx = "select statistics_name \"Statistics_Name\",activation_level \"Activation_Level\" from v$statistics_level order by activation_level";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    systemStatDataGridView.AutoGenerateColumns = true;
                    systemStatDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    systemStatDataGridView.ReadOnly = true;
                    systemStatDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    systemStatDataGridView.ColumnHeadersVisible = true;
                    systemStatDataGridView.DataSource = ds;
                    systemStatDataGridView.DataMember = "viewTx";
                }
                if (undoLogStatCheckBox.Checked)
                {
                    string viewTx = "select * from v$sysstat where name like '%redo log space requests%'";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    undoLogStatDataGridView.AutoGenerateColumns = true;
                    undoLogStatDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    undoLogStatDataGridView.ReadOnly = true;
                    undoLogStatDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    undoLogStatDataGridView.ColumnHeadersVisible = true;
                    undoLogStatDataGridView.DataSource = ds;
                    undoLogStatDataGridView.DataMember = "viewTx";
                }
                if (onceChAndGetCheckBox.Checked)
                {
                    string viewTx = "select * from v$sysstat where name in('consistent gets','consistent changes')";
                    Logger(viewTx);
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                    da.Fill(ds, "viewTx");
                    onceChAndGetDataGridView.AutoGenerateColumns = true;
                    onceChAndGetDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    onceChAndGetDataGridView.ReadOnly = true;
                    onceChAndGetDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    onceChAndGetDataGridView.ColumnHeadersVisible = true;
                    onceChAndGetDataGridView.DataSource = ds;
                    onceChAndGetDataGridView.DataMember = "viewTx";
                }
                conn.Close();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void infoListButton_Click(object sender, EventArgs e)
        {
             subForm = new SubForm();
             subForm.Show();
             //this.Dispose();
        }

        //private void getInfoButton_Click(object sender, EventArgs e)
        //{
        //    //subForm = new SubForm();
        //    //subForm.ShowDialog();
        //    try
        //    {
        //        if (dataSourceTextBox.Text != "" || dataSourceTextBox.Text != null)
        //        {
        //            ;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        public string ConnectionString
        {
            set
            {
                connOStr = value;
            }
            get
            {
                return connOStr;
            }
        }

        private void OPCT_Load(object sender, EventArgs e)
        {
            this.dataSourceTextBox.Text = connOStr;
           // label9.Text = connOStr.Substring(12,connOStr.IndexOf(";") - 12).ToUpper();
            this.loadRemoveTabPage();
        }

        private void checkOption(TabPage chTab)
        {
            try
            {
                if (this.chSetCheckBox.Checked && !this.ocptTabControl.TabPages.Contains(chTab))
                {
                    this.ocptTabControl.TabPages.Add(chTab);
                    this.ocptTabControl.SelectTab(chTab);
                }
                else
                {
                    this.ocptTabControl.TabPages.Remove(chTab);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pgaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(pgaTabPage);
        }

        private void useCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(useTabPage);
        }

        private void topSqlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(topSqlTabPage);
        }

        private void loadRemoveTabPage()
        {
            try
            {
                this.ocptTabControl.TabPages.Remove(pgaTabPage);
                this.ocptTabControl.TabPages.Remove(sgaTabPage);
                this.ocptTabControl.TabPages.Remove(dbFileTabPage);
                this.ocptTabControl.TabPages.Remove(tableSpaceTabPage);
                this.ocptTabControl.TabPages.Remove(useTabPage);
                this.ocptTabControl.TabPages.Remove(tempDbTabPage);
                this.ocptTabControl.TabPages.Remove(tempDbUsedTabPage);
                this.ocptTabControl.TabPages.Remove(tableLockTabPage);
                this.ocptTabControl.TabPages.Remove(tableLockCountTabPage);
                this.ocptTabControl.TabPages.Remove(topSqlTabPage);
                this.ocptTabControl.TabPages.Remove(sessionWaitTabPage);
                this.ocptTabControl.TabPages.Remove(rollbackTabPage);
                this.ocptTabControl.TabPages.Remove(tablespaceIOTabPage);
                this.ocptTabControl.TabPages.Remove(fileSystemIOTabPage);
                this.ocptTabControl.TabPages.Remove(userIndexTabPage);
                this.ocptTabControl.TabPages.Remove(SGARatioTabPage);
                this.ocptTabControl.TabPages.Remove(sgaDicRatioTabPage);
                this.ocptTabControl.TabPages.Remove(sgaShareRatioTabPage);
                this.ocptTabControl.TabPages.Remove(sgaRedoBufferRatioTabPage);
                this.ocptTabControl.TabPages.Remove(memoryDiskRatioTabPage);
                this.ocptTabControl.TabPages.Remove(runSqlTabPage);
                this.ocptTabControl.TabPages.Remove(mtsTabPage);
                this.ocptTabControl.TabPages.Remove(tableDeepTabPage);
                this.ocptTabControl.TabPages.Remove(usedSessionCpuStorageTabPage);
                this.ocptTabControl.TabPages.Remove(userSessionGroupTabPage);
                this.ocptTabControl.TabPages.Remove(userPrivsTabPage);
                this.ocptTabControl.TabPages.Remove(systemTabPage);
                this.ocptTabControl.TabPages.Remove(sysStatTabPage);
                this.ocptTabControl.TabPages.Remove(rollStatTabPage);
                this.ocptTabControl.TabPages.Remove(compressTabPage);
                this.ocptTabControl.TabPages.Remove(sgaInfoTabPage);
                this.ocptTabControl.TabPages.Remove(sgaMemUnitTabPage);
                this.ocptTabControl.TabPages.Remove(asynIOTabPage);
                this.ocptTabControl.TabPages.Remove(sgaLibShareTabPage);
                this.ocptTabControl.TabPages.Remove(sgaLibShareFreeTabPage);
                this.ocptTabControl.TabPages.Remove(pgaHitRateTabPage);
                this.ocptTabControl.TabPages.Remove(norParaTabPage);
                this.ocptTabControl.TabPages.Remove(logGroupTabPage);
                this.ocptTabControl.TabPages.Remove(databasePropTabPage);
                this.ocptTabControl.TabPages.Remove(ctrlFileTabPage);
                this.ocptTabControl.TabPages.Remove(processCpuTabPage);
                this.ocptTabControl.TabPages.Remove(networkIOTabPage);
                this.ocptTabControl.TabPages.Remove(systemStatTabPage);
                this.ocptTabControl.TabPages.Remove(undoLogStatTabPage);
                this.ocptTabControl.TabPages.Remove(onceChAndGetTabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tableSpaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tableSpaceTabPage);
        }

        private void dbFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(dbFileTabPage);
        }

        private void tempDbCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tempDbTabPage);
        }

        private void tempDbUsedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tempDbUsedTabPage);
        }

        private void tableLockCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tableLockTabPage);
        }

        private void tableLockCountCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tableLockCountTabPage);
        }

        private void sessionWaitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sessionWaitTabPage);
        }

        private void rollbackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(rollbackTabPage);
        }

        private void tablespaceIOCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tablespaceIOTabPage);
        }

        private void fileSystemIOCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(fileSystemIOTabPage);
        }

        private void SGARatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(SGARatioTabPage);
        }

        private void sgaDicRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaDicRatioTabPage);
        }

        private void sgaShareRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaShareRatioTabPage);
        }

        private void sgaRedoBufferRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaRedoBufferRatioTabPage);
        }

        private void runSqlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(runSqlTabPage);
        }

        private void usedSessionCpuStorageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(usedSessionCpuStorageTabPage);
        }

        private void memoryDiskRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(memoryDiskRatioTabPage);
        }

        private void chSetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(chSetTabPage);
        }

        private void mtsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(mtsTabPage);
        }

        private void tableDeepCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(tableDeepTabPage);
        }

        private void userIndexCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(userIndexTabPage);
        }

        private void sgaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaTabPage);
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserObject uo = new UserObject();
            uo.ConnectionString = dataSourceTextBox.Text;
            uo.ShowDialog();
        }

        //public string opctConnStr
        //{
        //    get
        //    {
        //        return connStr;
        //    }
        //}

        private void killSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillSession ks = new KillSession();
            ks.ConnectionString = dataSourceTextBox.Text;
            ks.ShowDialog();
        }

        private void userSessionGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(userSessionGroupTabPage);
        }

        private void userPrivsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(userPrivsTabPage);
        }

        private void systemCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(systemTabPage);
        }

        public void Logger(string msgLog)
        {
            if (!System.IO.Directory.Exists(@"c:\\OPCT"))
            {
                System.IO.Directory.CreateDirectory(@"c:\\OPCT");
            }
            string path = @"C:\\OPCT\\" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SQL.txt";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
            if (!fileInfo.Exists)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
                writer.WriteLine(msgLog);
                writer.Flush();
                writer.Close();
            }
        }

        private void sysStatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sysStatTabPage);
        }

        private void rollStatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(rollStatTabPage);
        }

        private void compressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(compressTabPage);
        }

        private void sgaInfocheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaInfoTabPage);
        }

        private void sgaMemUnitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaMemUnitTabPage);
        }

        private void asynIOCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(asynIOTabPage);
        }

        private void sgaLibShareCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaLibShareTabPage);
        }

        private void sgaLibShareFreeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(sgaLibShareFreeTabPage);
        }

        private void pgaHitRateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(pgaHitRateTabPage);
        }

        private void norParaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(norParaTabPage);
        }

        private void logGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(logGroupTabPage);
        }

        private void databasePropCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(databasePropTabPage);
        }

        private void ctrlFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(ctrlFileTabPage);
        }

        private void processCpuCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(processCpuTabPage);
        }

        private void networkIOCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(networkIOTabPage);
        }

        private void systemStatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(systemStatTabPage);
        }

        private void undoLogStatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(undoLogStatTabPage);
        }

        private void onceChAndGetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.checkOption(onceChAndGetTabPage);
        }

        //private void refreshCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.refreshCheckBox.Checked)
        //    {
        //        //定义aTimer
        //        //New aTimer_Elapsed构造函数
        //        this.aTimer.Elapsed += new ElapsedEventHandler(this.getInfoButton_Click);
        //        //设置5秒执行一次
        //        this.aTimer.Interval = 5000;
        //        this.aTimer.Enabled = true;
        //        CheckForIllegalCrossThreadCalls = false;
        //        GC.KeepAlive(aTimer);
        //    }
        //}
    }
}
