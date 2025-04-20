using RestProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface INasaApiService
    {
        Task<List<ApodEntry>> GetApodDataAsync(DateTime startDate, DateTime endDate);
    }
}