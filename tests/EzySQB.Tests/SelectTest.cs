using System;
using System.Collections.Generic;
using EzySQB;
using EzySQB.Clauses;
using EzySQB.Statements;
using Xunit;

namespace EzySQB.Tests
{
    public class SelectTest : TestBase
    {

        [Fact]
        public void TestDefaultSimplifiedQuery()
        {
            Assert.Equal(
                "select * from categories",
                GetQueryBuilder().Select("categories").ToString(),
                true
            );
        }

        [Fact]
        public void TestWithJoinsAndWhereClauses()
        {
            Assert.Equal(
                "select * from categories left join category_types on categories.id = category_types.category_id where category_types.name = @value and categories.level > 2",
                GetQueryBuilder()
                    .Select("categories")
                    .Join("category_types", "categories.id = category_types.category_id")
                    .Where("category_types.name = @value")
                    .AndWhere("categories.level > 2")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestSubGroupedWhereClauses()
        {
            Assert.Equal(
                "select * from categories left join category_types on categories.id = category_types.category_id where (category_types.name = @value and categories.level > 2) or category_types = @value2",
                GetQueryBuilder()
                    .Select("categories")
                    .Join("category_types", "categories.id = category_types.category_id")
                    .Where((query) => {
                        return query.Where("category_types.name = @value")
                                    .AndWhere("categories.level > 2");
                    })
                    .OrWhere("category_types = @value2")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestWithExists()
        {
            Assert.Equal(
                "select * from categories where exists (select * from category_types where category_types.category_id = @value and category_types.type = @value2)",
                GetQueryBuilder()
                    .Select("categories")
                    .Where(String.Format(
                        @"EXISTS ({0})",
                        GetQueryBuilder()
                            .Select("category_types")
                            .Where("category_types.category_id = @value")
                            .AndWhere("category_types.type = @value2")
                    ))
                    .ToString(),
                true
            );
        }


        [Fact]
        public void TestExpressionsWithOrdering()
        {
            Assert.Equal(
                "select * from categories where id = 1 and price = 10.55 order by id asc",
                GetQueryBuilder()
                    .Select("categories")
                    .Where("id = 1")
                    .AndWhere("price = 10.55")
                    .OrderBy("id")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestExpressionsWithMultipleOrdering()
        {
            Assert.Equal(
                "select * from categories where id = 1 and price = 10.55 order by id asc, name desc",
                GetQueryBuilder()
                    .Select("categories")
                    .Where("id = 1")
                    .AndWhere("price = 10.55")
                    .OrderBy(new Dictionary<string, OrderDirection>() {
                        { "id", OrderDirection.Asc },
                        { "name", OrderDirection.Desc }
                     })
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestExpressionsWithGroupBy()
        {
            Assert.Equal(
                "select * from categories where id = 1 and price = 10.55 group by id",
                GetQueryBuilder()
                    .Select("categories")
                    .Where("id = 1")
                    .AndWhere("price = 10.55")
                    .GroupBy("id")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestExpressionsWithMultipleGroupBy()
        {
            Assert.Equal(
                "select * from categories where id = 1 and price = 10.55 group by id, name",
                GetQueryBuilder()
                    .Select("categories")
                    .Where("id = 1")
                    .AndWhere("price = 10.55")
                    .GroupBy(new List<string>() { "id", "name" })
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestKindOfComplexQuery()
        {
            Assert.Equal(
                "select distinct categories.id as id, products_categories.cms_header as cms_header from categories " +
                "left join products_categories on categories.id = products_categories.category_id "  +
                "where categories.level = 1 " +
                "group by categories.id " +
                "order by categories.name asc " +
                "offset 0 " +
                "limit 100",
                GetQueryBuilder()
                    .Select("categories", new string[] { "categories.id as id", "products_categories.cms_header as cms_header" })
                    .Distinct()
                    .Join("products_categories", "categories.id = products_categories.category_id")
                    .Where("categories.level = 1")
                    .GroupBy("categories.id")
                    .OrderBy("categories.name")
                    .Offset(0)
                    .Limit(100)
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestComplexPivotsQuery()
        {
            Assert.Equal(
                "select products.*, array_to_string(array_agg(distinct products_categories.category_id), ',') as category_ids, " +
                "array_to_string(array_agg(distinct products_assets.asset_id), ',') as image_ids " +
                "from products " +
                "left join products_categories on products.id = products_categories.product_id " +
                "left join products_assets on products.id = products_assets.asset_id " +
                "where products.name ilike 'iphone%' group by products.id",
                GetQueryBuilder()
                    .Select("products")
                    .Columns(new string[] {
                        "products.*",
                        "array_to_string(array_agg(distinct products_categories.category_id), ',') as category_ids",
                        "array_to_string(array_agg(distinct products_assets.asset_id), ',') as image_ids"
                    })
                    .Join("products_categories", "products.id = products_categories.product_id")
                    .Join("products_assets", "products.id = products_assets.asset_id")
                    .Where("products.name ilike 'iphone%'")
                    .GroupBy("products.id")
                    .ToString(),
                true
            );
        }
    }
}