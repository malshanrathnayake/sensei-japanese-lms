using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.SystemService.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Country>> GetContries();
        Task<IEnumerable<State>> GetStates(int countryId = 0);
        Task<IEnumerable<City>> GetCities(int stateId = 0);
        Task<IEnumerable<Branch>> GetBranches();
        Task<IEnumerable<StudentLearningMode>> GetStudentLearningModes();

    }
}
