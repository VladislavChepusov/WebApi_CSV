using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Уточнения для создания моделей БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
        }


        // Переопределел метод конфигурации
        // Указывает где у нас будут прописываться миграции
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(b => b.MigrationsAssembly("WebApi_CSV"));


        // Оповещаем об появлении новых таблиц
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<dataInfo> dataInfos { get; set; } = null!;

    }
}
