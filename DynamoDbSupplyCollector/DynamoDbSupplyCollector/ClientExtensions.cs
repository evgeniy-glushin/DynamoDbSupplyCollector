﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDbSupplyCollector
{
    internal static class ClientExtensions
    {
        public static async Task<List<string>> GetTables(this AmazonDynamoDBClient client)
        {
            var tableNames = new List<string>();
            string lastTableNameEvaluated = null;

            do
            {
                // needs to be in a loop because ListTablesAsync can return up to 100 records at a time
                var request = new ListTablesRequest
                {
                    Limit = 100,
                    ExclusiveStartTableName = lastTableNameEvaluated
                };

                var response = await client.ListTablesAsync(request);
                foreach (string name in response.TableNames)
                    tableNames.Add(name);

                lastTableNameEvaluated = response.LastEvaluatedTableName;
            } while (lastTableNameEvaluated != null);

            return tableNames;
        }
    }
}
