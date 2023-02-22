using AutoMapper;
using DAL;
using DAL.Entities;
using Deedle;
using Deedle.Internal;
using FSharp.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.FSharp.Core;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.Drawing;
using System.Reflection.PortableExecutable;
using WebApi_CSV.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace WebApi_CSV.Services
{
    public class DataService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DataService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task DataProcessing(IFormFile files)
        {
            var df = await ConvertCSVtoFrame(files);
            df.Print();

            // подсчет количества колонок
            int count_colomns = df.ColumnCount;
            // получение количества строк
            int count_rows = df.RowCount;

            // Проверка на количество строк и столбцов!!

 
            ResultModel resultModel = await CalculationOfResults(df);
            resultModel.FileName = files.FileName;

            Console.WriteLine("!!!! " + resultModel);




            Console.WriteLine($@"FileName = {resultModel.FileName} " );
            Console.WriteLine($@"AllTime = {resultModel.AllTime} ");
            Console.WriteLine($@"AllTime = {resultModel.AllTime.Ticks} ");



            var dbResult = _mapper.Map<DAL.Entities.Results>(resultModel);
            var t = await _context.Results.AddAsync(dbResult);
            await _context.SaveChangesAsync();
        }


        private async Task<ResultModel> CalculationOfResults(Frame<int, string> df)
        {
           
            // получение названия  столбца
            string[] colomn_names_array = df.ColumnKeys.ToArray();

            int count_rows = df.RowCount;
            //var Column1 = df.GetColumn<string>(column: colomn_names_array[0]).GetAllValues().ToList();


            var MaxValue = Stats.max(df.GetColumn<string>(column: colomn_names_array[2]));
            Console.WriteLine($@"MaxValue= {MaxValue}");

            var MinValue = Stats.min(df.GetColumn<string>(column: colomn_names_array[2]));
            Console.WriteLine($@"MinValue= {MinValue}");

            var MedianValue = Stats.median(df.GetColumn<string>(column: colomn_names_array[2]));
            Console.WriteLine($@"MedianValue= {MedianValue}");

            var AverageValue = Stats.mean(df.GetColumn<string>(column: colomn_names_array[2]));
            Console.WriteLine($@"AverageValue= {AverageValue}");


            var timeFrame = df.GetColumn<string>(column: colomn_names_array[0])
                .GetAllValues()
                .ToList()
                .Select(i => DateTimeOffset.ParseExact(i.Value.ToString(), "yyyy-MM-dd_HH-mm-ss", null));

            var MinDateTime = timeFrame.Min();
            Console.WriteLine($@"MinDateTime= {MinDateTime}");

            TimeSpan AllTime = timeFrame.Max() - timeFrame.Min();
            Console.WriteLine($@"AllTime= {AllTime}  and {AllTime.GetType()}");

            var AverageTimeWork = Stats.mean(df.GetColumn<string>(column: colomn_names_array[1]));
            Console.WriteLine($@"AverageTimeWork= {AverageTimeWork}");

            ResultModel res = new ResultModel{
                AllTime = AllTime,
                MinDateTime = MinDateTime,
                AverageTimeWork = AverageTimeWork,
                AverageValue = AverageValue,
                MedianValue = MedianValue,
                MaxValue = MaxValue,
                MinValue = MinValue,
                CountRows = count_rows
            };


           return  res;
            /*
            var newfR = df.GetColumn<string>(column: colomn_names_array[0])
                 .Select(i => new KeyValuePair<int, DateTimeOffset>(i.Key, DateTimeOffset.ParseExact(i.Value.ToString(), "yyyy-MM-dd_HH-mm-ss", null))
                );
            newfR.Print();
            */

            // ретурнуть модеть? или сразу сохранить...

        }


        // Парсинг пришедшего файла в фрейм
        private async Task<Frame<int, string>> ConvertCSVtoFrame(IFormFile files)
        {
            using (var ms = new MemoryStream())
            {
                await files.CopyToAsync(ms);
                //files.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var msftRaw = Frame.ReadCsv(ms, separators: ";", hasHeaders: false, inferTypes: false);

                return msftRaw;
            }
        }




        /*
        public async Task datas1(IFormFile files)
        {
            var dt = ConvertCSVtoDataTable(files);
            foreach (DataRow dataRow in dt.Rows)
            {
                Console.WriteLine(dataRow);
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

        }
        private static DataTable ConvertCSVtoDataTable(IFormFile file)
        {
            DataTable dt = new DataTable();
            using (var stream = file.OpenReadStream())
            using (StreamReader sr = new StreamReader(stream))
            {
                string[] headers = sr.ReadLine().Split(';');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        */


    }
}
