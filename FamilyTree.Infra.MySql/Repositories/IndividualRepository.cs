using Dapper;

using FamilyTree.Infra.MySql.Models;
using FamilyTree.Infra.MySql.Support;

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

        public IndividualRepository(MySqlDbOptions options)
        {
            this.connStr = options?.ConnectionString ?? throw new ArgumentException("No connection string configured", nameof(options));
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
                            VALUES (@Id, @Firstnames, @Lastname, @Sex, @BirthDateInt, @BirthPlace, @DeathDateInt, @DeathPlace, 0)";
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
            // 1) mention all columns explicitly (better sql); 2) alias the columns to match the entity (snake_case to PascalCase)
            var sql = @"SELECT id, firstnames, lastname, sex, 
birth_date BirthDateInt, birth_place BirthPlace,
death_date DeathDateInt, death_place DeathPlace,
is_deleted IsDeleted
FROM individual WHERE id=@Id";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var indi = await conn.QuerySingleOrDefaultAsync<Individual>(sql, new { id }).ConfigureAwait(false);

                return Map(indi);
            }
        }

        public async Task<List<IndividualDto>> GetIndividualsByLastname(string name)
        {
            var sql = @"SELECT id, firstnames, lastname, sex, 
birth_date BirthDateInt, birth_place BirthPlace,
death_date DeathDateInt, death_place DeathPlace,
is_deleted IsDeleted 
FROM individual WHERE lastname = @Name";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var list = await conn.QueryAsync<Individual>(sql, new { name }).ConfigureAwait(false);

                return list.Select(Map).ToList();
            }
        }

        public async Task<List<NameCount>> GetLastNames()
        {
            var sql = "SELECT lastname, count(1) as `Count` FROM individual GROUP BY lastname";
            using (var conn = new MySqlConnection(this.connStr))
            {
                var list = await conn.QueryAsync<NameCount>(sql).ConfigureAwait(false);

                return list.ToList();
            }
        }

        public async Task<int> GetTotalChildrenCount()
        {
            var sql = @"SELECT count(1) FROM individual ind
INNER JOIN children chil ON chil.individual_id = ind.id
WHERE ind.is_deleted = 0";
            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<int> GetTotalSpouseCount()
        {
            var sql = @"SELECT count(1) FROM individual ind
INNER JOIN spouses sp ON sp.individual_spouse_id = ind.id
WHERE ind.is_deleted = 0";
            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task MarkIndividualAsDeleted(long id)
        {
            var sql = "UPDATE individual SET is_deleted=1 WHERE id = @Id";
            using (var conn = new MySqlConnection(this.connStr))
            {
                await conn.ExecuteAsync(sql, new { id }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname)
        {
            var sql = @"SELECT  id, firstnames, lastname, sex, 
birth_date BirthDateInt, birth_place BirthPlace,
death_date DeathDateInt, death_place DeathPlace,
is_deleted IsDeleted
FROM individual WHERE 1 = 1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(lastname))
            {
                sql += " AND lastname LIKE '%' + @Lastname + '%'";
                parameters.Add("@Lastname", lastname);
            }

            // if multiple first names were given, match *all* parts in any order
            if (!string.IsNullOrWhiteSpace(firstname))
            {
                var sa = firstname.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sa.Length; i++)
                {
                    sql += $" AND firstname LIKE '%' + @First{i} + '%'";
                    parameters.Add("@First" + i, sa[i]);
                }
            }

            using (var conn = new MySqlConnection(this.connStr))
            {
                var list = await conn.QueryAsync<Individual>(sql, parameters).ConfigureAwait(false);

                return list.Select(Map).ToList();
            }
        }

        internal static IndividualDto Map(Individual indi)
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

        private static Individual Map(IndividualDto dto)
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
