using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;

namespace AirlineWeb.Profiles
{
    public class WebhookSubscriptionProfile:Profile
    {
        public WebhookSubscriptionProfile()
        {
            CreateMap<WebhookSubscription,WebhookSubscriptionReadDto>();
            CreateMap<WebhookSubscriptionCreateDto,WebhookSubscription>();
        }
    }
}