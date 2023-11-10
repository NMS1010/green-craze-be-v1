﻿using AutoMapper;
using green_craze_be_v1.Application.Common.SignalR;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Notification;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<AppHub> _hub;

        public NotificationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, IHubContext<AppHub> hub)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _hub = hub;
        }

        public async Task<long> CreateNotification(CreateNotificationRequest request)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetById(_currentUserService.UserId)
                ?? throw new Exception("User not login");

            var notification = _mapper.Map<Notification>(request);
            notification.User = user;
            await _unitOfWork.Repository<Notification>().Insert(notification);

            var res = await _unitOfWork.Save() > 0;
            if (res)
            {
                await _hub.Clients.Group(user.Id).SendAsync("ReceiveNotification", _mapper.Map<NotificationDto>(notification));
            }
            return notification.Id;
        }

        public async Task<PaginatedResult<NotificationDto>> GetListNotification(GetNotificationPagingRequest request)
        {
            var notifications = await _unitOfWork.Repository<Notification>().ListAsync(new NotificationSpecification(request, isPaging: true));
            var total = await _unitOfWork.Repository<Notification>().CountAsync(new NotificationSpecification(request));

            return new PaginatedResult<NotificationDto>(notifications
                .Select(x => _mapper.Map<NotificationDto>(x)).ToList(),
                request.PageIndex, total, request.PageSize);
        }

        public async Task<bool> UpdateNotification(UpdateNotificationRequest request)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetEntityWithSpec(new NotificationSpecification(request.UserId, request.Id))
                ?? throw new Exception("Notification not found");

            notification.Status = true;

            _unitOfWork.Repository<Notification>().Update(notification);

            return await _unitOfWork.Save() > 0;
        }
    }
}