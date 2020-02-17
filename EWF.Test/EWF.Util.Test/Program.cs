using System;

namespace EWF.Util.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("代码生成开始!");
            Console.WriteLine("代码生成中...");
            GeneratorTest.GeneratorModelForSqlServer();
            Console.WriteLine("代码生成成功！");
            Console.WriteLine("按任意键退出！");
            Console.ReadKey();
        }
    }
}
