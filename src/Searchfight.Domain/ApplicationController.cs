﻿using Searchfight.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Searchfight.Domain
{
    public class ApplicationController
    {
        private readonly IInputValidator inputValidator;
        private readonly ISearchStatisticsService searchStatisticsService;
        private readonly ISearchStatisticsPresenter searchStatisticsPresenter;

        public ApplicationController(IInputValidator validator, 
            ISearchStatisticsService statisticsService,
            ISearchStatisticsPresenter presenter)
        {
            inputValidator = validator;
            searchStatisticsService = statisticsService;
            searchStatisticsPresenter = presenter;
        }

        public async Task Run(string[] args)
        {
            try
            {
                //Validate input
                inputValidator.Validate(args);

                //run searches
                var result = await Task.WhenAll(searchStatisticsService.CollectStatistics(args)); //Get rid of Task.WhenAll, it is more infrastructure

                //format output
                searchStatisticsPresenter.ShowData(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TODO: add logging
            }
        }
    }
}
