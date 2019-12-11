using AdvertApi.Entitiess;
using AdvertApi.Interfaces;
using AdvertApi.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public class DynamoDbAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper mapper;

        public DynamoDbAdvertStorage(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<string> Add(AdvertModel model)
        {
            var entity = mapper.Map<AdvertEntity>(model);
            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(entity);
                }
            }
            return entity.Id;
        }

        public async Task<bool> CheckHealthAsync()
        {
            using (var client = new AmazonDynamoDBClient())
            {
                var status = await client.DescribeTableAsync("Adverts");
                return status.Table.TableStatus == TableStatus.ACTIVE;
            }
        }

        public async Task Confirm(ConfirmAdvertModel model)
        {
            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var entity = await context.LoadAsync<AdvertEntity>(model.Id);
                    if (entity == null)
                    {
                        throw new KeyNotFoundException("Id not found");
                    }
                    if (model.Status == AdvertStatus.Active)
                    {
                        entity.Status = AdvertStatus.Active;
                        await context.SaveAsync(entity);
                    }
                    else
                    {
                        await context.DeleteAsync(model);
                    }
                }
            }
        }
    }
}
