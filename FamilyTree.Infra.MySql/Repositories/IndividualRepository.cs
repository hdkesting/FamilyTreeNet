﻿using Dapper;

using FamilyTree.Infra.Models;

using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTree.Infra.MySql.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly string connStr;

        public IndividualRepository(string connStr)
        {
            this.connStr = connStr;
        }

        /// <summary>Adds or updates the individual.</summary>
        /// <param name="individual">The individual.</param>
        /// <returns></returns>
        public async Task AddOrUpdate(IndividualDto individual)
        {
            if (individual == null)
            {
                throw new ArgumentNullException(nameof(individual));
            }

            var mdl = Map(individual);

            using (var conn = new MySqlConnection(this.connStr))
            {
                conn.Open();
                string sql;
                // ID is not autonumber, so read db to check whether it already exists
                sql = "SELECT count(1) FROM individual WHERE id = @Id";
                int count = await conn.ExecuteScalarAsync<int>(sql, new {  mdl.Id }).ConfigureAwait(false);

                if (count == 0)
                {
                    sql = @"INSERT INTO individual (id, firstnames, lastname, sex, birth_date, birth_place, death_date, death_place, is_deleted)
                            VALUES (@Id, @Firstnames, @Lastname, @BirthDateInt, @Sex, @BirthDateInt, @BirthPlace, @DeathDateInt, @DeathPlace, 0)";
                }
                else
                {
                    sql = @"UPDATE individual SET
                                firstnames = @Firstnames,
                                lastname = @Lastname,
                                sex = @Sex,
                                birth_date = @BirthDateInt,
                                birth_place = @BirthPlace,
                                death_date = @DeathDateInt,
                                death_place = @DeathPlace,
                                is_deleted = 0
                            WHERE id = @Id";
                }

                await conn.ExecuteAsync(sql, mdl).ConfigureAwait(false);
            }
        }

        /// <summary>Counts all individuals, possibly including deleted ones.</summary>
        /// <param name="includeDeleted">if set to <c>true</c>, include deleted records.</param>
        /// <returns>The total count.</returns>
        public async Task<int> Count(bool includeDeleted)
        {
            var sql = "SELECT count(1) FROM individual";
            if (!includeDeleted)
            {
                sql += " WHERE is_deleted = 0";
            }

            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql).ConfigureAwait(false);
            }
        }

        /// <summary>Deletes all individuals.</summary>
        public async Task DeleteAll()
        {
            var sql = "DELETE FROM individual";
            using (var conn = new MySqlConnection(this.connStr))
            {
                await conn.ExecuteAsync(sql).ConfigureAwait(false);
            }
        }

        /// <summary>Gets the by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        public async Task<IndividualDto> GetById(long id, bool includeDeleted)
        {
            var sql = "SELECT * FROM individual WHERE id=@Id";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var indi = await conn.QuerySingleOrDefaultAsync<Individual>(sql, new { id }).ConfigureAwait(false);

                return Map(indi);
            }
        }

        public async Task<List<IndividualDto>> GetIndividualsByLastname(string name)
        {
            var sql = "SELECT * FROM individual WHERE lastname = @Name";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var list = await conn.QueryAsync<Individual>(sql, new { name }).ConfigureAwait(false);

                return list.Select(Map).ToList();
            }
        }

        public async Task<List<NameCount>> GetLastNames()
        {
            var sql = "SELECT lastname, count(1) as [Count] FROM individual GROUP BY lastname";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var list = await conn.QueryAsync<NameCount>(sql).ConfigureAwait(false);

                return list.ToList();
            }
        }

        public async Task<int> GetTotalChildrenCount()
        {
            var sql = @"SELECT count(1) FROM individuals ind
INNER JOIN children chil ON chil.individual_id = ind.id
WHERE ind.is_deleted = 0";
            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<int> GetTotalSpouseCount()
        {
            var sql = @"SELECT count(1) FROM individuals ind
INNER JOIN spouses sp ON chil.individual_spouse_id = ind.id
WHERE ind.is_deleted = 0";
            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public Task MarkIndividualAsDeleted(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }

        private IndividualDto Map(Individual indi)
        {
            if (indi is null)
            {
                return null;
            }

            return new IndividualDto
            {
                Id = indi.Id,
                Firstnames = indi.Firstnames,
                Lastname = indi.Lastname,
                BirthDate = indi.BirthDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(indi.BirthDateInt.Value),
                BirthPlace = indi.BirthPlace,
                DeathDate = indi.DeathDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(indi.DeathDateInt.Value),
                DeathPlace = indi.DeathPlace,
                Sex = indi.Sex == 'M' ? FamilyTreeNet.Core.Support.Sex.Male
                    : indi.Sex == 'F' ? FamilyTreeNet.Core.Support.Sex.Female
                    : FamilyTreeNet.Core.Support.Sex.Unknown,
            };
        }

        private Individual Map(IndividualDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Individual
            {
                Id = dto.Id,
                Firstnames = dto.Firstnames,
                Lastname = dto.Lastname,
                BirthDateInt = dto.BirthDate?.ToInt32(),
                BirthPlace = dto.BirthPlace,
                DeathDateInt = dto.DeathDate?.ToInt32(),
                DeathPlace = dto.DeathPlace,
                Sex = dto.Sex == FamilyTreeNet.Core.Support.Sex.Female ? 'F'
                    : dto.Sex == FamilyTreeNet.Core.Support.Sex.Male ? 'M'
                    : '?',
            };
        }
    }
}