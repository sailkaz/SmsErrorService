﻿using SsmErrorService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsmErrorService.Core.Repositories
{
    public class ErrorRepository
    {
        public List<Error> GetLastErrors(int intervalInMinutes)
        {
            // pobieranie z bazy danych

            return new List<Error>
            {
                new Error {Message = "Błąd 1", Date = DateTime.Now},
                new Error {Message = "Błąd 2", Date = DateTime.Now},
            };
        }
    }
}
