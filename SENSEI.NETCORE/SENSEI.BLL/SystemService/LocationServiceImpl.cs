using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.SystemService
{
    public class LocationServiceImpl : ILocationService
    {
        private readonly IDatabaseService _databaseService;

        public LocationServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Country>> GetContries()
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var countries = await dataTransactionManager.CountryDataManager.RetrieveData("GetCountry");
            return countries;
        }

        public async Task<IEnumerable<State>> GetStates(int countryId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var states = await dataTransactionManager.StateDataManager.RetrieveData("GetState", [
                new SqlParameter("@countryId", countryId)
            ]);
            return states;
        }

        public async Task<IEnumerable<City>> GetCities(int stateId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var cities = await dataTransactionManager.CityDataManager.RetrieveData("GetCity", [
                new SqlParameter("@stateId", stateId)
            ]);
            return cities;
        }

        public async Task<IEnumerable<Branch>> GetBranches()
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var branches = await dataTransactionManager.BranchDataManager.RetrieveData("GetBranch");
            return branches;
        }

        public async Task<IEnumerable<StudentLearningMode>> GetStudentLearningModes()
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var learningModes = await dataTransactionManager.StudentLearningModeDataManager.RetrieveData("GetStudentLearningMode");
            return learningModes;
        }


    }
}
