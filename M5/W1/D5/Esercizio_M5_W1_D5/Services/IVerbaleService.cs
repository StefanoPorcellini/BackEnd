﻿using Esercizio_M5_W1_D5.Models;

namespace Esercizio_M5_W1_D5.Services
{
    public interface IVerbaleService
    {
        void AddVerbale(Verbale verbale);
        IEnumerable<Verbale> GetAllVerbali();
    }
}
