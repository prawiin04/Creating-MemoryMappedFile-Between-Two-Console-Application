using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
namespace ConsoleApp1
{
    internal struct DataExchange1
    {
        internal double wrt1;
        internal double wrt2;
    }
    internal struct DataExchange2
    {
        internal double rd1;
        internal double rd2;

        internal double a;
        internal double b;
        internal double c;
        internal double d;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.Write("Console Application 2\n");
            Console.Out.Write("Enter 'q' to quit \n");
            DataExchange1 DataExchangeWrite;
            DataExchange2 DataExchangeRead;

            DataExchangeWrite.wrt1 = 0;
            DataExchangeWrite.wrt2 = 0;
            DataExchangeRead.rd1 = 0;
            DataExchangeRead.rd2 = 0;
            DataExchangeRead.a = 0;
            DataExchangeRead.b = 0;
            DataExchangeRead.c = 0;
            DataExchangeRead.d = 0; 



            int DataSize = Marshal.SizeOf(typeof(DataExchange2));

            using (var mmf_Read = MemoryMappedFile.CreateOrOpen("file", DataSize))
            using (var mmf_Write = MemoryMappedFile.CreateOrOpen("file", DataSize))
            {
                bool quit = false;
                while (!quit)
                {
                    using (var accessorRead = mmf_Read.CreateViewAccessor(0, DataSize, MemoryMappedFileAccess.Read))
                    using (var accessorWrite = mmf_Write.CreateViewAccessor(0, DataSize, MemoryMappedFileAccess.Write))
                    {
                        try
                        {

                            accessorRead.Read(10, out DataExchangeRead.rd1);
                            accessorRead.Read(11, out DataExchangeRead.rd2);

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.ToString());
                        }

                        accessorWrite.Write(0, ref DataExchangeWrite.wrt1);
                        accessorWrite.Write(1, ref DataExchangeWrite.wrt2);

                        Random rand = new Random();
                        DataExchangeWrite.wrt1 = rand.NextDouble();
                        DataExchangeWrite.wrt2 = rand.NextDouble();
                        
                        Console.Out.Write("Read rd1 : {0} , rd2 : {1}      \r",
                                           DataExchangeRead.rd1,
                                           DataExchangeRead.rd2);
                        
                        Thread.Sleep(2000);
                        
                        if (Console.KeyAvailable)
                            if (Console.ReadKey().KeyChar == 'q')
                                quit = true;
                                
                    }
                }
            }
        }
    }
}
