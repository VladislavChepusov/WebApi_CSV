namespace WebApi_CSV.Exceptions
{
    public class CustomExceptions
    {
        public class NotFoundException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"{Model} не найден!";
        }

        public class CSVException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"Необходим файл разрешения .csv";
        }



        public class testNotFoundException : NotFoundException
        {
            public testNotFoundException()
            {
                Model = "test";
            }
        }




    }
}
