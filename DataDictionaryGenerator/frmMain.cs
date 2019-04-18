using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Extractor;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace DataDictionaryGenerator
{
    public partial class frmMain : Form
    {
        #region 私有域

        private DataTable dtInfo;

        #endregion

        #region 构造方法

        public frmMain()
        {
            InitializeComponent();
            dtInfo = new DataTable();
        }

        #endregion

        #region 自定义方法

        public ReturnMessage CheckCnnString(string cnnString)
        {
            ReturnMessage retMsg = new ReturnMessage(string.Empty, true);
            SqlConnection cnn = new SqlConnection(cnnString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                retMsg.isSuccess = false;
                retMsg.Messages = ex.Message;
                return retMsg;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Dispose();
            }
            return retMsg;
        }

        public ReturnMessage GetInfo(string cnnString)
        {
            ReturnMessage retMsg = new ReturnMessage(string.Empty, true);
            dtInfo.Rows.Clear();
            string strQry = @"SELECT TOP (100) PERCENT 
                CASE WHEN a.colorder = 1 THEN d .name ELSE '' END AS 表名, CASE WHEN a.colorder = 1 THEN isnull(f.value, '') 
                ELSE '' END AS 表说明, a.colorder AS 字段序号, a.name AS 字段名, CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') 
                = 1 THEN '√' ELSE '' END AS 标识, CASE WHEN EXISTS 
                (SELECT 1 FROM dbo.sysindexes si INNER JOIN 
                dbo.sysindexkeys sik ON si.id = sik.id AND si.indid = sik.indid INNER JOIN 
                dbo.syscolumns sc ON sc.id = sik.id AND sc.colid = sik.colid INNER JOIN 
                dbo.sysobjects so ON so.name = so.name AND so.xtype = 'PK' 
                WHERE sc.id = a.id AND sc.colid = a.colid) THEN '√' ELSE '' END AS 主键, b.name AS 类型, a.length AS 长度, COLUMNPROPERTY(a.id, a.name, 
                'PRECISION') AS 精度, ISNULL(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0) AS 小数位数, 
                CASE WHEN a.isnullable = 1 THEN '√' ELSE '' END AS 允许空, ISNULL(e.text, '') AS 默认值, ISNULL(g.value, '') AS 字段说明, d.crdate AS 创建时间, 
                CASE WHEN a.colorder = 1 THEN d .refdate ELSE NULL END AS 更改时间 
                FROM dbo.syscolumns AS a LEFT OUTER JOIN 
                dbo.systypes AS b ON a.xtype = b.xusertype INNER JOIN 
                dbo.sysobjects AS d ON a.id = d.id AND d.xtype = 'U'  AND d.status >= 0 LEFT OUTER JOIN 
                dbo.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN 
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN 
                sys.extended_properties AS f ON d.id = f.major_id AND f.minor_id = 0 
                ORDER BY d.name, 字段序号";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(strQry, cnnString);
                da.Fill(dtInfo);
                dgvData.DataSource = dtInfo;
                return retMsg;
            }
            catch (Exception ex)
            {
                retMsg.isSuccess = false;
                retMsg.Messages = ex.Message;
                return retMsg;
            }
        }

        private ReturnMessage WriteDoc()
        {
            ReturnMessage retMsg = new ReturnMessage(string.Empty, true);
            FileStream fs = null;
            try
            {
                XWPFDocument doc = new XWPFDocument();
                XWPFTable table = null;
                int index = 1;
                //把内存中的DataTable写到Docx文件里
                foreach (DataRow dr in dtInfo.Rows)
                {
                    if (dr["表名"] != DBNull.Value && !string.IsNullOrEmpty(dr["表名"].ToString()))
                    {
                        //表名，以段落表示
                        CT_P ctp = doc.Document.body.AddNewP();
                        //XWPFParagraph p = doc.CreateParagraph();
                        XWPFParagraph p = new XWPFParagraph(ctp, doc);
                        XWPFRun r = p.CreateRun();
                        //设置字体
                        r.GetCTR().AddNewRPr().AddNewRFonts().ascii = "宋体";
                        r.GetCTR().AddNewRPr().AddNewRFonts().eastAsia = "宋体";
                        r.GetCTR().AddNewRPr().AddNewRFonts().hint = ST_Hint.eastAsia;
                        r.GetCTR().AddNewRPr().AddNewSz().val = (ulong)32;//3号字体;
                        r.GetCTR().AddNewRPr().AddNewSzCs().val = (ulong)32;
                        //设置行间距
                        //单倍为默认值（240twip）不需设置，1.5倍=240X1.5=360twip，2倍=240X2=480twip
                        ctp.AddNewPPr().AddNewSpacing().line = "720";
                        //ctp.AddNewPPr().AddNewSpacing().lineRule = ST_LineSpacingRule.exact;
                        //设置段落文本
                        r.SetText(index.ToString() + "." + dr["表名"].ToString());

                        //表结构，以表格显示
                        CT_Tbl m_CTTbl = doc.Document.body.AddNewTbl();
                        table = doc.CreateTable(1, 9);
                        //标题行(固定)
                        //列宽
                        CT_TcPr mPr = table.GetRow(0).GetCell(0).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "900";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(1).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "1500";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(2).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "500";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(3).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "1000";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(4).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "500";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(6).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "900";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(7).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "800";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        mPr = table.GetRow(0).GetCell(8).GetCTTc().AddNewTcPr();
                        mPr.tcW = new CT_TblWidth();
                        mPr.tcW.w = "1500";
                        mPr.tcW.type = ST_TblWidth.dxa;
                        //填充文字
                        table.GetRow(0).GetCell(0).SetText("字段序号");
                        table.GetRow(0).GetCell(1).SetText("字段名");
                        table.GetRow(0).GetCell(2).SetText("主键");
                        table.GetRow(0).GetCell(3).SetText("类型");
                        table.GetRow(0).GetCell(4).SetText("长度");
                        table.GetRow(0).GetCell(5).SetText("精度");
                        table.GetRow(0).GetCell(6).SetText("小数位数");
                        table.GetRow(0).GetCell(7).SetText("允许空");
                        table.GetRow(0).GetCell(8).SetText("字段说明");
                        //内容行
                        XWPFTableRow row = table.CreateRow();
                        row.GetCell(0).SetText(dr["字段序号"].ToString());
                        row.GetCell(1).SetText(dr["字段名"].ToString());
                        row.GetCell(2).SetText(dr["主键"].ToString());
                        row.GetCell(3).SetText(dr["类型"].ToString());
                        row.GetCell(4).SetText(dr["长度"].ToString());
                        row.GetCell(5).SetText(dr["精度"].ToString());
                        row.GetCell(6).SetText(dr["小数位数"].ToString());
                        row.GetCell(7).SetText(dr["允许空"].ToString());
                        row.GetCell(8).SetText(dr["字段说明"].ToString());
                        //
                        index++;
                    }
                    else
                    {
                        if (table != null)
                        {
                            //内容行
                            XWPFTableRow row = table.CreateRow();
                            row.GetCell(0).SetText(dr["字段序号"].ToString());
                            row.GetCell(1).SetText(dr["字段名"].ToString());
                            row.GetCell(2).SetText(dr["主键"].ToString());
                            row.GetCell(3).SetText(dr["类型"].ToString());
                            row.GetCell(4).SetText(dr["长度"].ToString());
                            row.GetCell(5).SetText(dr["精度"].ToString());
                            row.GetCell(6).SetText(dr["小数位数"].ToString());
                            row.GetCell(7).SetText(dr["允许空"].ToString());
                            row.GetCell(8).SetText(dr["字段说明"].ToString());
                        }
                    }
                }
                //输出保存
                string docAllPath = Application.StartupPath + "\\SqlDBDicFile.docx";
                if (File.Exists(docAllPath))
                {
                    File.Delete(docAllPath);
                }
                fs = File.OpenWrite(docAllPath);
                doc.Write(fs);
                doc.Close();
                return retMsg;
            }
            catch (Exception ex)
            {
                retMsg.isSuccess = false;
                retMsg.Messages = ex.Message;
                return retMsg;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

        }

        #endregion

        #region 委托方法

        private void btnBulid_Click(object sender, EventArgs e)
        {
            Cursor currCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            ReturnMessage retMsg = CheckCnnString(txtCnnString.Text.Trim());
            if (!retMsg.isSuccess)
            {
                MessageBox.Show("数据库连接字符串错误，信息为：" + retMsg.Messages);
                this.Cursor = currCursor;
                return;
            }
            retMsg = GetInfo(txtCnnString.Text.Trim());
            if (!retMsg.isSuccess)
            {
                MessageBox.Show("读取数据库表结构错误，信息为：" + retMsg.Messages);
                this.Cursor = currCursor;
                return;
            }
            retMsg = WriteDoc();
            if (retMsg.isSuccess)
            {
                MessageBox.Show("文档生成成功!\n请在程序根目录查找文档!");
            }
            else
            {
                MessageBox.Show("文档生成失败，信息为：" + retMsg.Messages);
            }

            this.Cursor = currCursor;
        }

        #endregion
    }
}
