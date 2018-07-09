using System;
using System.Collections.Generic;
using EzySQB;
using EzySQB.Clauses;
using EzySQB.Statements;
using Xunit;


namespace EzySQBTests
{
    public class DeleteTest : TestBase
    {

        [Fact]
        public void TestSimpleQuery()
        {
            Assert.Equal(
                "delete from categories where id = @id",
                GetQueryBuilder()
                    .Delete("categories")
                    .Where("id = @id")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestWithMultipleWhere()
        {
            Assert.Equal(
                "delete from categories where id = @id or id in (select category_id from products_categories)",
                GetQueryBuilder()
                    .Delete("categories")
                    .Where("id = @id")
                    .OrWhere($"id in ({(GetQueryBuilder().Select("products_categories", new string[] {"category_id"}))})")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestPrettyWithMultipleWhere()
        {
            Assert.Equal(
                "delete from categories\nwhere id = @id or\n\tid in (select category_id from products_categories)",
                GetQueryBuilder()
                    .Delete("categories")
                    .Where("id = @id")
                    .OrWhere($"id in ({(GetQueryBuilder().Select("products_categories", new string[] {"category_id"}))})")
                    .PrettyAssemble(),
                true
            );
        }

    }
}