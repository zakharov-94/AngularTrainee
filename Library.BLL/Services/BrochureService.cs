﻿using AutoMapper;
using Library.DAL.Context;
using Library.DAL.Repositories;
using Library.Entities.Entities;
using Library.ViewModels;
using Library.ViewModels.BrochureViewModel;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL.Services
{
    public class BrochureService
    {

        private UnitOfWork _unitOfWork;

        public BrochureService(ApplicationContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public GetBrochureViewModel GetAll()
        {
            IEnumerable<Brochure> brochuresList = _unitOfWork.Brochure.GetAll();
            return Mapper.Map<IEnumerable<Brochure>, GetBrochureViewModel>(brochuresList);
        }

        public void Add(PostBrochureViewItem brochureViewModel)
        {
            _unitOfWork.Brochure.Add(ToBrochure(brochureViewModel));
        }

        public GetBrochureViewItem GetById(int id)
        {
            Brochure brochure = _unitOfWork.Brochure.FindById(id);
            return Mapper.Map<Brochure, GetBrochureViewItem>(brochure);
        }

        public void Edit(PostBrochureViewItem brochureViewModel)
        {
            _unitOfWork.Brochure.Update(ToBrochure(brochureViewModel));
        }

        public void Delete(int id)
        {
            _unitOfWork.Brochure.Remove(id);
        }
        private Brochure ToBrochure(PostBrochureViewItem brochureViewModel)
        {
            return Mapper.Map<PostBrochureViewItem, Brochure>(brochureViewModel);
        }
    }
}
