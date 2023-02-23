﻿using AutoMapper;
using DAL;
using Deedle;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using WebApi_CSV.Models;
using static WebApi_CSV.Exceptions.CustomExceptions;

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
            // Проверка на пустоту файла
            using (var reader = new StreamReader(files.OpenReadStream()))
            {
                if (reader.ReadToEnd().Trim().Length < 1)
                    throw new RowSizeValidationException();
            }

            var df = await ConvertCSVtoFrame(files);
            df.Print();

            // Валидация данных 
            await Validate(df);

            // Расчет показателей и в модель
            ResultModel resultModel = await CalculationOfResults(df);
            resultModel.FileName = files.FileName;

            
            Console.WriteLine("!!!! " + resultModel);
            Console.WriteLine($@"FileName = {resultModel.FileName} ");
            Console.WriteLine($@"AllTime = {resultModel.AllTime} ");
            Console.WriteLine($@"AllTime = {resultModel.AllTime.Ticks} ");
            Console.WriteLine($@"CountRows = {resultModel.CountRows} ");



            // Сохранение данных в БД
            // await SavingDb(resultModel);
        }


        // Проверка файла
        private Task Validate(Frame<int, string> df)
        {
            //Количество строк не может быть меньше 1 и больше 10 000
            if (df.RowCount < 1 || df.RowCount > 10000)
                throw new RowSizeValidationException();
            // Проверка на количество стобцов 
            if (df.ColumnCount != 3)
                throw new ColSizeValidationException();

            // получение названия  столбцов
            string[] colomn_names_array = df.ColumnKeys.ToArray();

            // проверка на парсинг через регулярки
            Regex regexDouble = new Regex(@"[+-]?([0-9]*[.])?[0-9]+");
            Regex regexInt = new Regex(@"^[0-9]+$");//^[1-9]\d*$
            //Regex regexDate = new Regex(@"\d{4}-\d{2}-\d{2}_(?:0?\d|1\d|2[0-3])-[0-5]\d-[0-5]\d$");
            Regex regexDate = new Regex(@"(19|20)\d\d-((0[1-9]|1[012])-(0[1-9]|[12]\d)|(0[13-9]|1[012])-30|(0[13578]|1[02])-31)_(?:0?\d|1\d|2[0-3])-[0-5]\d-[0-5]\d$");

            var df1 = df.Where(r =>
            !regexInt.IsMatch(r.Value.TryGetAs<string>(column: colomn_names_array[1]).ValueOrDefault)
            ||
            !regexDate.IsMatch(r.Value.TryGetAs<string>(column: colomn_names_array[0]).ValueOrDefault)
            ||
            !regexDouble.IsMatch(r.Value.TryGetAs<string>(column: colomn_names_array[2]).ValueOrDefault));

            if (df1.RowCount > 0)
                throw new DataTypeValidationException();


            DateTimeOffset CurrentMoment = DateTimeOffset.Now;
            DateTimeOffset InitialMoment = DateTimeOffset.Parse("01.01.2000");

            // Проверка значений на несоблюдение условий
            var df2 = df.Where(r =>
            r.Value.TryGetAs<double>(column: colomn_names_array[2]).ValueOrDefault < 0
            ||
            r.Value.TryGetAs<int>(column: colomn_names_array[1]).ValueOrDefault < 0
            ||
            DateTimeOffset
            .ParseExact(r.Value.TryGetAs<string>(column: colomn_names_array[0])
            .ValueOrDefault
            .ToString(), "yyyy-MM-dd_HH-mm-ss", null)
            .CompareTo(InitialMoment) < 0
            ||
            DateTimeOffset
            .ParseExact(r.Value.TryGetAs<string>(column: colomn_names_array[0])
            .ValueOrDefault
            .ToString(), "yyyy-MM-dd_HH-mm-ss", null)
            .CompareTo(CurrentMoment) > 0);

            if (df2.RowCount > 0)
                throw new ValueValidationException();
            return Task.CompletedTask;
        }

        private async Task SavingDb(ResultModel resultModel)
        {
            var dbOldResult = await _context.Results
                .Where(x => x.FileName == resultModel.FileName)
                .ToListAsync();

            if (dbOldResult != null)
                _context.Results.RemoveRange(dbOldResult);
            // await _context.SaveChangesAsync();


            // Добавление результатов
            var dbResult = _mapper.Map<DAL.Entities.Results>(resultModel);
            await _context.Results.AddAsync(dbResult);


            await _context.SaveChangesAsync();
        }




        // Расчет показателей 
        private Task<ResultModel> CalculationOfResults(Frame<int, string> df)
        {
            // получение названия  столбца
            string[] colomn_names_array = df.ColumnKeys.ToArray();
            // • Количество строк
            int count_rows = df.RowCount;
            // • Максимальное значение показателя
            var MaxValue = Stats.max(df.GetColumn<string>(column: colomn_names_array[2]));
            // • Минимальное значение показателя
            var MinValue = Stats.min(df.GetColumn<string>(column: colomn_names_array[2]));
            //• Медина по показателям
            var MedianValue = Stats.median(df.GetColumn<string>(column: colomn_names_array[2]));
            //•	Среднее значение по показателям
            var AverageValue = Stats.mean(df.GetColumn<string>(column: colomn_names_array[2]));

            var timeFrame = df.GetColumn<string>(column: colomn_names_array[0])
                .GetAllValues()
                .ToList()
                .Select(i => DateTimeOffset.ParseExact(i.Value.ToString(), "yyyy-MM-dd_HH-mm-ss", null));

            //•	Минимальное дата и время, как момент запуска первой операции
            var MinDateTime = timeFrame.Min();
            //•	Все время (максимальное значение времени минус минимальное значение времени)
            TimeSpan AllTime = timeFrame.Max() - timeFrame.Min();
            //•	Среднее время выполнения
            var AverageTimeWork = Stats.mean(df.GetColumn<string>(column: colomn_names_array[1]));

            ResultModel res = new ResultModel
            {
                AllTime = AllTime,
                MinDateTime = MinDateTime,
                AverageTimeWork = AverageTimeWork,
                AverageValue = AverageValue,
                MedianValue = MedianValue,
                MaxValue = MaxValue,
                MinValue = MinValue,
                CountRows = count_rows
            };
            return Task.FromResult(res);
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





    }
}
