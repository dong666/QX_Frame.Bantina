using QX_Frame.Bantina.Bankinate;
using QX_Frame.Bantina.BankinateAuto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<string, object> dataDic = new Dictionary<string, object>();
            //dataDic.Add("classid", "1");


            //using (var db = new DB_QX_Frame_Test())
            //{
            //    if (db.Insert("TB_People", dataDic))
            //    {
            //        Console.WriteLine(db.Message);
            //    }
            //    else
            //    {
            //        Console.WriteLine(db.Message);
            //    }
            //}

            using (var db = new DB_QX_Frame_Test())
            {
                List<TB_People> peoples = db.QueryEntities<TB_People>();

                foreach (var item in peoples)
                {
                    Console.WriteLine(item.Uid + " - " + item.TB_ClassName.ClassName);
                }
            }


            Console.WriteLine("any key to exit ...");
            Console.ReadKey();
        }
    }
    public class DB_QX_Frame_Test : Bankinate
    {
        public DB_QX_Frame_Test() : base("data source=.;initial catalog=DB_QX_Frame_Test;persist security info=True;user id=Sa;password=Sa123456;MultipleActiveResultSets=True;App=EntityFramework") { }
    }

    [Table(TableName = "TB_People")]
    public class TB_People
    {
        [Key]
        public Guid Uid { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int Age { get; set; }
        [Column]
        public int ClassId { get; set; }
        [ForeignTable(ForeignKeyFieldName = "ClassId")]
        public TB_ClassName TB_ClassName { get; set; }
    }

    [Table(TableName = "TB_ClassName")]
    public class TB_ClassName
    {
        // PK（identity）  
        [Key]
        public Int32 ClassId { get; set; }
        //
        [Column]
        public String ClassName { get; set; }
    }
}
