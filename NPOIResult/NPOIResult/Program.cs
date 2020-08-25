using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.IO;
using System.Linq;

namespace NPOIResult
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now + " Start");
            //查詢的值
            var strEnter = Console.ReadLine();

            if (strEnter == "" && args[0] == "")
            {
                Console.WriteLine("請輸入值");
                return;
            }
            if (strEnter=="")
            {
                strEnter = args[0];
            }
            



            //var strEnter = "T0001";
            //連線字串
            SqlConnection conn = new SqlConnection();
            string strconn = ConfigurationManager.ConnectionStrings["PracticeConnectionString"].ConnectionString;
            conn.ConnectionString = strconn;

            //設定EXCEL
            IWorkbook workbook = new HSSFWorkbook();
            try
            {
                using (conn)
                {
                    //SP名稱
                    var procedure = "spSearch";
                    //SP參數
                    var values = new { Account = strEnter };
                    //執行結果
                    var results = conn.Query<Member>(procedure, values, commandType: CommandType.StoredProcedure).ToList();

                    if (results.Count==0)
                    {
                        Console.WriteLine("資料不存在");
                        return;
                    }

                    //EXCEL sheet名稱
                    ISheet sheet = workbook.CreateSheet("Member");

                    //設定欄位名稱的cell style
                    var style = workbook.CreateCellStyle();
                    var font = workbook.CreateFont();
                    font.Underline = FontUnderlineType.Single;
                    font.Color = HSSFColor.White.Index;
                    style.SetFont(font);
                    //style.FillForegroundColor = HSSFColor.BlueGrey.Index;
                    style.FillPattern = FillPattern.SolidForeground;

                    var i = 0;

                    //跑表頭，根據欄位屬性來設定
                    foreach (var item in results)
                    {
                        var j = 0;
                        var row = sheet.CreateRow(i);
                        if (i == 0)
                        {
                            foreach (var property in item.GetType().GetProperties())
                            {
                                var cell = row.CreateCell(j);
                                cell.SetCellValue(property.Name);
                                cell.CellStyle = style;
                                sheet.AutoSizeColumn(j);
                                j++;
                            }

                            i++;
                            j = 0;
                            row = sheet.CreateRow(i);
                        }

                        //跑內容
                        foreach (var property in item.GetType().GetProperties())
                        {
                            row.CreateCell(j).SetCellValue(property.GetValue(item, null)?.ToString());
                            j++;
                        }
                        i++;
                    }

                    //若有檔案刪除定並重寫
                    File.Delete("D:/tmp/test.xlsx");
                    FileStream file = new FileStream("D:/tmp/test.xls", FileMode.Create);

                    workbook.Write(file);

                    file.Close();

                }
            }
            catch (Exception)
            {

                throw;
            }
            Console.WriteLine(DateTime.Now+" 成功");
            Console.ReadKey();
        }
    }
}
