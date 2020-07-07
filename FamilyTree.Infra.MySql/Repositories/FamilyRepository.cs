using Dapper;

using FamilyTree.Infra.MySql.Models;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Infra.MySql.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly string connStr;
        private readonly IIndividualRepository individualRepository;

        public FamilyRepository(string connStr, IIndividualRepository individualRepository)
        {
            this.connStr = connStr;
            this.individualRepository = individualRepository;
        }

        /// <summary>Adds a child to a family.</summary>
        /// <param name="cfam">The ID of the child's family.</param>
        /// <param name="id">The ID of the child.</param>
        public async Task AddChild(long cfam, long id)
        {
            var sql = "INSERT INTO children (family_id, individual_id) VALUES (@Fam, @Child)";
            using (var conn = new MySqlConnection(this.connStr))
            {
                await conn.ExecuteAsync(sql, new { Fam = cfam, Child = id });
            }
        }

        /// <summary>
        /// Adds or updates the family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <exception cref="ArgumentNullException">family</exception>
        public async Task AddOrUpdate(FamilyDto family)
        {
            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }

            var mdl = Map(family);

            using (var conn = new MySqlConnection(this.connStr))
            {
                // open connection, so that all commands use the same one
                conn.Open();

                string sql;
                // ID is not autonumber, so read db to check whether it already exists
                sql = "SELECT count(1) FROM family WHERE id = @Id";
                int count = await conn.ExecuteScalarAsync<int>(sql, new { mdl.Id }).ConfigureAwait(false);

                if (count == 0)
                {
                    sql = "INSERT INTO family (id, marriage_date, marriage_place, divorce_date, divorce_place) " +
                            "VALUES (@Id, @MarriageDateInt, @MarriagePlace, @DivorceDateInt, @DivorcePlace)";
                }
                else
                {
                    sql = "UPDATE family SET id=@Id, marriage_date=@MarriageDateInt, marriage_place=@MarriagePlace, " +
                            "divorce_date=@DivorceDateInt, divorce_place=@DivorcePlace " +
                            "WHERE id = @Id";
                }

                await conn.ExecuteAsync(sql, mdl).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adds a spouse to a family.
        /// </summary>
        /// <param name="sfam">The ID of the spouse's family.</param>
        /// <param name="id">The ID of the spouse.</param>
        /// <returns></returns>
        public async Task AddSpouse(long sfam, long id)
        {
            var sql = "INSERT INTO spouses (family_spouse_id, individual_spouse_id) VALUES (@Fam, @Spouse)";
            using (var conn = new MySqlConnection(this.connStr))
            {
                await conn.ExecuteAsync(sql, new { Fam = sfam, Spouse = id });
            }
        }

        /// <summary>
        /// Counts all families, possibly including the deleted ones.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c>, also include soft-deleted ones.</param>
        /// <returns></returns>
        public async Task<int> Count(bool includeDeleted)
        {
            var sql = "SELECT count(1) FROM family";
            if (!includeDeleted)
            {
                sql += " WHERE is_deleted = 0";
            }

            using (var conn = new MySqlConnection(this.connStr))
            {
                return await conn.ExecuteScalarAsync<int>(sql).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deletes all families (hard delete).
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAll()
        {
            var sql = "DELETE FROM family";

            using (var conn = new MySqlConnection(this.connStr))
            {
                await conn.ExecuteAsync(sql).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the families where the individual (see id) is child.
        /// </summary>
        /// <param name="id">The child's identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c>, also include deleted.</param>
        /// <returns></returns>
        public async Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted)
        {
            var sql = @"SELECT * FROM family f
INNER JOIN children c ON c.family_id = f.id
WHERE c.individual_id = @Id";

            using (var conn = new MySqlConnection(this.connStr))
            {
                var l1 = await conn.QueryAsync<Family>(sql, new { id }).ConfigureAwait(false);
                var l2 = l1.Select(async (f) => await Map(f, includeDeleted));
                var l3 = await Task.WhenAll(l2);
                return l3.ToList();
            }
        }

        /// <summary>
        /// Gets the families where the individual (see id) is spouse.
        /// </summary>
        /// <param name="id">The spouse's identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c>, also include deleted.</param>
        /// <returns></returns>
        public async Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted)
        {
            var sql = @"SELECT * FROM family f
INNER JOIN spouses s ON s.family_spouse_id = f.id
WHERE c.individual_spouse_id = @Id";

            using (var conn = new MySqlConnection(this.connStr))
            {
                var l1 = await conn.QueryAsync<Family>(sql, new { id }).ConfigureAwait(false);
                var l2 = l1.Select(async (f) => await Map(f, includeDeleted));
                var l3 = await Task.WhenAll(l2);
                return l3.ToList();
            }
        }

        /// <summary>
        /// Updates the relations between the family and spouses and children.
        /// </summary>
        /// <param name="familyId">The family identifier.</param>
        /// <param name="spouses">The spouses.</param>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        public async Task UpdateRelations(long familyId, List<long> spouses, List<long> children)
        {
            if (spouses is null)
            {
                spouses = new List<long>();
            }

            if (children is null)
            {
                children = new List<long>();
            }

            using (var conn = new MySqlConnection(this.connStr))
            {
                // open connection, so that all commands use the same one
                conn.Open();

                // check that the family exists (ignore if not)
                var sql = "SELECT count(1) FROM family where id = @Id";

                // I expect that the count is usually 1, but might be 0. No other values.
                var cnt = await conn.ExecuteScalarAsync<int>(sql, new { Id = familyId });
                if (cnt != 0)
                {
                    // remove any existing child or spouse relations (not the individuals themselves)
                    sql = "DELETE FROM children WHERE family_id = @Id";
                    await conn.ExecuteAsync(sql, new { Id = familyId });
                    sql = "DELETE FROM spouses WHERE family_spouse_id = @Id";
                    await conn.ExecuteAsync(sql, new { Id = familyId });

                    // add the new *lists*
                    sql = "INSERT INTO children (family_id, individual_id) VALUES (@FamilyId, @ChildId)";
                    await conn.ExecuteAsync(sql, new { FamilyId = familyId, ChildId = children });
                    sql = "INSERT INTO spouses (family_spouse_id, individual_spouse_id) VALUES (@FamilyId, @SpouseId)";
                    await conn.ExecuteAsync(sql, new { FamilyId = familyId, SpouseId = spouses });
                }
            }
        }

        private Family Map(FamilyDto dto) =>
            new Family
            {
                Id = dto.Id,
                MarriageDateInt = dto.MarriageDate?.ToInt32(),
                MarriagePlace = dto.MarriagePlace,
                DivorceDateInt = dto.DivorceDate?.ToInt32(),
                DivorcePlace = dto.DivorcePlace,
            };

        private async Task<FamilyDto> Map(Family db, bool includeDeleted)
        {
            var res = new FamilyDto
            {
                Id = db.Id,
                MarriageDate = db.MarriageDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(db.MarriageDateInt.Value),
                MarriagePlace = db.MarriagePlace,
                DivorceDate = db.DivorceDateInt == null ? null : new FamilyTreeNet.Core.Support.GeneaDate(db.DivorceDateInt.Value),
                DivorcePlace = db.DivorcePlace,
            };

            foreach (var spouse in db.Spouses)
            {
                res.Spouses.Add(await this.individualRepository.GetById(spouse.SpouseId, includeDeleted).ConfigureAwait(false));
            }

            foreach (var child in db.Children)
            {
                res.Children.Add(await this.individualRepository.GetById(child.ChildId, includeDeleted).ConfigureAwait(false));
            }

            return res;
        }
    }
}
